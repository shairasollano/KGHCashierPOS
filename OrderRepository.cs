using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace KGHCashierPOS
{
    public static class OrderRepository
    {
        // ============ SAVE ORDER ============
        public static void SaveOrder(string orderNumber, decimal totalAmount, List<OrderItem> items)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Start transaction
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Step 1: Insert into orders table
                            string orderQuery = @"
                                INSERT INTO orders 
                                (order_number, customer_name, customer_age, customer_contact, total_amount, order_date, status)
                                VALUES 
                                (@orderNo, @name, @age, @contact, @total, @date, 'Pending')";

                            using (var cmd = new MySqlCommand(orderQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                                cmd.Parameters.AddWithValue("@name", "Walk-in Customer");
                                cmd.Parameters.AddWithValue("@age", DBNull.Value);
                                cmd.Parameters.AddWithValue("@contact", DBNull.Value);
                                cmd.Parameters.AddWithValue("@total", totalAmount);
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                System.Diagnostics.Debug.WriteLine($"Order saved: {rowsAffected} row(s) affected");
                            }

                            // Step 2: Insert order items
                            foreach (var item in items)
                            {
                                string itemQuery = @"
                                    INSERT INTO order_items 
                                    (order_number, game_name, duration_minutes, price, equipment_cost)
                                    VALUES 
                                    (@orderNo, @game, @duration, @price, @equipCost)";

                                using (var cmd = new MySqlCommand(itemQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                                    cmd.Parameters.AddWithValue("@game", item.GameName);
                                    cmd.Parameters.AddWithValue("@duration", item.Duration);
                                    cmd.Parameters.AddWithValue("@price", item.GamePrice);
                                    cmd.Parameters.AddWithValue("@equipCost", item.EquipmentCost);

                                    cmd.ExecuteNonQuery();
                                    System.Diagnostics.Debug.WriteLine($"Item saved: {item.GameName}");
                                }

                                // Optional: Save detailed equipment if needed
                                // SaveEquipmentDetails(item, conn, transaction);
                            }

                            // Commit transaction
                            transaction.Commit();
                            System.Diagnostics.Debug.WriteLine($"✓ Order {orderNumber} saved successfully!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception($"Transaction failed: {ex.Message}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ SaveOrder Error: {ex.Message}");
                throw new Exception($"Failed to save order: {ex.Message}", ex);
            }
        }

        // ============ LOAD ORDER ============
        public static List<OrderItemData> LoadOrder(string orderNumber)
        {
            List<OrderItemData> items = new List<OrderItemData>();

            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Step 1: Check if order exists and is pending
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM orders 
                        WHERE order_number = @orderNo AND status = 'Pending'";

                    using (var cmd = new MySqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        System.Diagnostics.Debug.WriteLine($"Order {orderNumber} check: {count} found");

                        if (count == 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"Order {orderNumber} not found or not pending");
                            return null; // Order not found or already processed
                        }
                    }

                    // Step 2: Load order items
                    string itemsQuery = @"
                        SELECT 
                            game_name, 
                            duration_minutes, 
                            price, 
                            IFNULL(equipment_cost, 0) as equipment_cost
                        FROM order_items 
                        WHERE order_number = @orderNo
                        ORDER BY item_id";

                    using (var cmd = new MySqlCommand(itemsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new OrderItemData
                                {
                                    GameName = reader.GetString("game_name"),
                                    Duration = reader.GetInt32("duration_minutes"),
                                    Price = reader.GetDecimal("price"),
                                    EquipmentCost = reader.GetDecimal("equipment_cost")
                                };

                                items.Add(item);
                                System.Diagnostics.Debug.WriteLine($"Loaded item: {item.GameName} - ₱{item.TotalPrice}");
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"✓ Order {orderNumber} loaded: {items.Count} items");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ LoadOrder Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return null;
            }

            return items;
        }

        // ============ UPDATE ORDER STATUS ============
        public static void UpdateOrderStatus(string orderNumber, string status)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE orders 
                        SET status = @status,
                            updated_at = CURRENT_TIMESTAMP
                        WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine($"Order {orderNumber} status updated to {status}: {rowsAffected} row(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ UpdateOrderStatus Error: {ex.Message}");
                throw;
            }
        }

        // ============ CHECK IF ORDER EXISTS ============
        public static bool OrderExists(string orderNumber)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT COUNT(*) 
                        FROM orders 
                        WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ OrderExists Error: {ex.Message}");
                return false;
            }
        }

        // ============ GET ORDER DETAILS ============
        public static OrderDetails GetOrderDetails(string orderNumber)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            order_number,
                            customer_name,
                            total_amount,
                            order_date,
                            status
                        FROM orders 
                        WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new OrderDetails
                                {
                                    OrderNumber = reader.GetString("order_number"),
                                    CustomerName = reader.GetString("customer_name"),
                                    TotalAmount = reader.GetDecimal("total_amount"),
                                    OrderDate = reader.GetDateTime("order_date"),
                                    Status = reader.GetString("status")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ GetOrderDetails Error: {ex.Message}");
            }

            return null;
        }

        // ============ DELETE ORDER ============
        public static void DeleteOrder(string orderNumber)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Items will be auto-deleted due to CASCADE
                    string query = "DELETE FROM orders WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        System.Diagnostics.Debug.WriteLine($"Order {orderNumber} deleted: {rowsAffected} row(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ DeleteOrder Error: {ex.Message}");
                throw;
            }
        }
    }

    // ============ ORDER DETAILS CLASS ============
    public class OrderDetails
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}