using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace KGHCashierPOS
{
    public static class PaymentRepository
    {
        // ============ SAVE SESSION WITH EQUIPMENT ============
        public static int SaveSession(GameSession session)
        {
            int sessionId = 0;

            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // ⭐ Step 1: Save session (including equipment_cost)
                            string query = @"
                                INSERT INTO sessions
                                (game_name, start_time, end_time, total_minutes, total_price, equipment_cost, status)
                                VALUES
                                (@game, @start, @end, @minutes, @price, @equipCost, 'Completed');
                                SELECT LAST_INSERT_ID();";

                            using (var cmd = new MySqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@game", session.GameName);
                                cmd.Parameters.AddWithValue("@start", session.StartTime);
                                cmd.Parameters.AddWithValue("@end", session.EndTime);
                                cmd.Parameters.AddWithValue("@minutes", session.TotalMinutes);
                                cmd.Parameters.AddWithValue("@price", session.TotalPrice);
                                cmd.Parameters.AddWithValue("@equipCost", session.EquipmentCost);

                                sessionId = Convert.ToInt32(cmd.ExecuteScalar());

                                System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                                System.Diagnostics.Debug.WriteLine($"✓ Session saved:");
                                System.Diagnostics.Debug.WriteLine($"  Session ID: {sessionId}");
                                System.Diagnostics.Debug.WriteLine($"  Game: {session.GameName}");
                                System.Diagnostics.Debug.WriteLine($"  Game Price: {session.TotalPrice:C}");
                                System.Diagnostics.Debug.WriteLine($"  Equipment Cost: {session.EquipmentCost:C}");
                                System.Diagnostics.Debug.WriteLine($"  Total: {(session.TotalPrice + session.EquipmentCost):C}");
                            }

                            // ⭐ Step 2: Save equipment details to session_equipment table
                            if (session.Equipment != null && session.Equipment.Count > 0)
                            {
                                System.Diagnostics.Debug.WriteLine("  Equipment Details:");

                                foreach (var equipment in session.Equipment)
                                {
                                    // Save both defaults (for tracking) and rentals
                                    if (equipment.DefaultQuantity > 0 || equipment.RentalQuantity > 0)
                                    {
                                        string equipQuery = @"
                                            INSERT INTO session_equipment 
                                            (session_id, equipment_name, quantity, price_per_unit, equipment_type, total_cost)
                                            VALUES 
                                            (@sessionId, @name, @qty, @price, @type, @totalCost)";

                                        using (var cmd = new MySqlCommand(equipQuery, conn, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@sessionId", sessionId);
                                            cmd.Parameters.AddWithValue("@name", equipment.Name);
                                            cmd.Parameters.AddWithValue("@qty", equipment.RentalQuantity);
                                            cmd.Parameters.AddWithValue("@price", equipment.Price);
                                            cmd.Parameters.AddWithValue("@type", equipment.Type);
                                            cmd.Parameters.AddWithValue("@totalCost", equipment.TotalCost);

                                            cmd.ExecuteNonQuery();

                                            System.Diagnostics.Debug.WriteLine(
                                                $"    • {equipment.Name} x{equipment.RentalQuantity} " +
                                                $"({equipment.Type}) = {equipment.TotalCost:C}"
                                            );
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                            System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            System.Diagnostics.Debug.WriteLine($"❌ Transaction rollback: {ex.Message}");
                            throw new Exception($"Session save failed: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SaveSession Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }

            return sessionId;
        }

        // ============ SAVE PAYMENT ============
        public static void SavePayment(PaymentData payment)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO payments
                        (session_id, payment_method, amount_paid, payment_date, 
                         amount_tendered, receipt_no, discount_type, discount_amount, final_amount)
                        VALUES
                        (@sid, @method, @amt, @date, @tendered, @rno, @dtype, @disc, @final)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", payment.SessionId);
                        cmd.Parameters.AddWithValue("@method", payment.PaymentMethod);
                        cmd.Parameters.AddWithValue("@amt", payment.AmountPaid);
                        cmd.Parameters.AddWithValue("@date", payment.PaymentDate);
                        cmd.Parameters.AddWithValue("@tendered", payment.Reference);
                        cmd.Parameters.AddWithValue("@rno", payment.ReceiptNo);
                        cmd.Parameters.AddWithValue("@dtype", payment.DiscountType);
                        cmd.Parameters.AddWithValue("@disc", payment.DiscountAmount);
                        cmd.Parameters.AddWithValue("@final", payment.FinalAmount);

                        cmd.ExecuteNonQuery();

                        System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                        System.Diagnostics.Debug.WriteLine($"✓ Payment saved:");
                        System.Diagnostics.Debug.WriteLine($"  Receipt: {payment.ReceiptNo}");
                        System.Diagnostics.Debug.WriteLine($"  Session ID: {payment.SessionId}");
                        System.Diagnostics.Debug.WriteLine($"  Method: {payment.PaymentMethod}");
                        System.Diagnostics.Debug.WriteLine($"  Amount Paid: {payment.AmountPaid:C}");
                        System.Diagnostics.Debug.WriteLine($"  Discount: {payment.DiscountAmount:C}");
                        System.Diagnostics.Debug.WriteLine($"  Final Amount: {payment.FinalAmount:C}");
                        System.Diagnostics.Debug.WriteLine($"  Reference: {payment.Reference}");
                        System.Diagnostics.Debug.WriteLine("════════════════════════════════════════");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SavePayment Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        // ============ CHECK DUPLICATE GCASH ============
        public static bool IsDuplicateGCashReference(string reference)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT COUNT(*) 
                        FROM payments 
                        WHERE payment_method = 'GCash' 
                        AND amount_tendered = @reference";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@reference", reference);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Duplicate check error: {ex.Message}");
                return false;
            }
        }

        // ============ GET SESSION WITH EQUIPMENT ============
        public static SessionWithEquipment GetSessionWithEquipment(int sessionId)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Get session
                    string sessionQuery = @"
                        SELECT session_id, game_name, start_time, end_time, 
                               total_minutes, total_price, equipment_cost, status
                        FROM sessions
                        WHERE session_id = @sessionId";

                    SessionWithEquipment session = null;

                    using (var cmd = new MySqlCommand(sessionQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@sessionId", sessionId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                session = new SessionWithEquipment
                                {
                                    SessionId = reader.GetInt32("session_id"),
                                    GameName = reader.GetString("game_name"),
                                    StartTime = reader.GetDateTime("start_time"),
                                    EndTime = reader.GetDateTime("end_time"),
                                    TotalMinutes = reader.GetInt32("total_minutes"),
                                    TotalPrice = reader.GetDecimal("total_price"),
                                    EquipmentCost = reader.GetDecimal("equipment_cost"),
                                    Status = reader.GetString("status"),
                                    Equipment = new List<Equipment>()
                                };
                            }
                        }
                    }

                    if (session != null)
                    {
                        // Get equipment
                        string equipQuery = @"
                            SELECT equipment_name, quantity, price_per_unit, 
                                   equipment_type, total_cost
                            FROM session_equipment
                            WHERE session_id = @sessionId";

                        using (var cmd = new MySqlCommand(equipQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@sessionId", sessionId);

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    session.Equipment.Add(new Equipment
                                    {
                                        Name = reader.GetString("equipment_name"),
                                        RentalQuantity = reader.GetInt32("quantity"),
                                        Price = reader.GetDecimal("price_per_unit"),
                                        Type = reader.GetString("equipment_type")
                                    });
                                }
                            }
                        }
                    }

                    return session;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ GetSessionWithEquipment Error: {ex.Message}");
                return null;
            }
        }
    }

    // ============ PAYMENT DATA CLASS ============
    public class PaymentData
    {
        public int SessionId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal AmountPaid { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string ReceiptNo { get; set; }
        public string Reference { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }

    // ============ SESSION WITH EQUIPMENT CLASS ============
    public class SessionWithEquipment
    {
        public int SessionId { get; set; }
        public string GameName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalMinutes { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal EquipmentCost { get; set; }
        public string Status { get; set; }
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();
    }
}