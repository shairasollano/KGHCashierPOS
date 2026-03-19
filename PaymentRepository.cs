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
                            // Step 1: Save session
                            string query = @"
                                INSERT INTO sessions
                                (game_name, start_time, end_time, total_minutes, total_price, status)
                                VALUES
                                (@game, @start, @end, @minutes, @price, 'Completed');
                                SELECT LAST_INSERT_ID();";

                            using (var cmd = new MySqlCommand(query, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@game", session.GameName);
                                cmd.Parameters.AddWithValue("@start", session.StartTime);
                                cmd.Parameters.AddWithValue("@end", session.EndTime);
                                cmd.Parameters.AddWithValue("@minutes", session.TotalMinutes);
                                cmd.Parameters.AddWithValue("@price", session.TotalPrice);

                                sessionId = Convert.ToInt32(cmd.ExecuteScalar());
                                System.Diagnostics.Debug.WriteLine($"✓ Session saved: {session.GameName} (ID: {sessionId})");
                            }

                            // ⭐ Step 2: Save equipment for this session
                            if (session.Equipment != null && session.Equipment.Count > 0)
                            {
                                foreach (var equipment in session.Equipment)
                                {
                                    if (equipment.RentalQuantity > 0 || equipment.DefaultQuantity > 0)
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
                                                $"  ✓ Session equipment: {equipment.Name} x{equipment.RentalQuantity} " +
                                                $"({equipment.Type}) = {equipment.TotalCost:C}"
                                            );
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Session save failed: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SaveSession Error: {ex.Message}");
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
                        (session_id, payment_method, amount_paid, discount_type,
                         discount_amount, final_amount, receipt_no, amount_tendered, payment_date)
                        VALUES
                        (@sid, @method, @amt, @dtype, @disc, @final, @rno, @ref, @date)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@sid", payment.SessionId);
                        cmd.Parameters.AddWithValue("@method", payment.PaymentMethod);
                        cmd.Parameters.AddWithValue("@amt", payment.AmountPaid);
                        cmd.Parameters.AddWithValue("@dtype", payment.DiscountType);
                        cmd.Parameters.AddWithValue("@disc", payment.DiscountAmount);
                        cmd.Parameters.AddWithValue("@final", payment.FinalAmount);
                        cmd.Parameters.AddWithValue("@rno", payment.ReceiptNo);
                        cmd.Parameters.AddWithValue("@ref", payment.Reference);
                        cmd.Parameters.AddWithValue("@date", payment.PaymentDate);

                        cmd.ExecuteNonQuery();
                    }
                }

                System.Diagnostics.Debug.WriteLine($"✓ Payment saved: {payment.ReceiptNo}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SavePayment Error: {ex.Message}");
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
}