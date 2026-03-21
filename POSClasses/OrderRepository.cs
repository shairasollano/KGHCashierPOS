using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace KGHCashierPOS
{
    public static class OrderRepository
    {
        // ============ SAVE ORDER WITH EQUIPMENT ============
        public static void SaveOrder(string orderNumber, decimal totalAmount, List<OrderItem> items)
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Step 1: Insert main order
                            string orderQuery = @"
                                INSERT INTO orders 
                                (order_number, total_amount, order_date, status)
                                VALUES 
                                (@orderNo, @total, @date, 'Pending')";

                            using (var cmd = new MySqlCommand(orderQuery, conn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                                cmd.Parameters.AddWithValue("@total", totalAmount);
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);

                                cmd.ExecuteNonQuery();
                                System.Diagnostics.Debug.WriteLine($"✓ Order {orderNumber} saved");
                            }

                            // Step 2: Insert order items with equipment
                            foreach (var item in items)
                            {
                                string itemQuery = @"
                                    INSERT INTO order_items 
                                    (order_number, game_name, duration_minutes, price, equipment_cost)
                                    VALUES 
                                    (@orderNo, @game, @duration, @price, @equipCost);
                                    SELECT LAST_INSERT_ID();";

                                int itemId = 0;
                                using (var cmd = new MySqlCommand(itemQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                                    cmd.Parameters.AddWithValue("@game", item.GameName);
                                    cmd.Parameters.AddWithValue("@duration", item.Duration);
                                    cmd.Parameters.AddWithValue("@price", item.GamePrice);
                                    cmd.Parameters.AddWithValue("@equipCost", item.EquipmentCost);

                                    itemId = Convert.ToInt32(cmd.ExecuteScalar());
                                }

                                // ⭐ Step 3: Insert equipment details
                                if (item.Equipment != null)
                                {
                                    foreach (var equipment in item.Equipment)
                                    {
                                        if (equipment.RentalQuantity > 0 || equipment.DefaultQuantity > 0)
                                        {
                                            string equipQuery = @"
                                                INSERT INTO order_equipment 
                                                (item_id, equipment_name, quantity, price_per_unit, equipment_type, total_cost)
                                                VALUES 
                                                (@itemId, @name, @qty, @price, @type, @totalCost)";

                                            using (var cmd = new MySqlCommand(equipQuery, conn, transaction))
                                            {
                                                cmd.Parameters.AddWithValue("@itemId", itemId);
                                                cmd.Parameters.AddWithValue("@name", equipment.Name);
                                                cmd.Parameters.AddWithValue("@qty", equipment.RentalQuantity + equipment.DefaultQuantity);
                                                cmd.Parameters.AddWithValue("@price", equipment.Price);
                                                cmd.Parameters.AddWithValue("@type", equipment.Type);
                                                cmd.Parameters.AddWithValue("@totalCost", equipment.TotalCost);

                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }

                            transaction.Commit();
                            System.Diagnostics.Debug.WriteLine($"✓ Order {orderNumber} fully saved with equipment!");
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
                throw;
            }
        }

        // ============ LOAD ORDER WITH EQUIPMENT ============
        public static List<OrderItemData> LoadOrder(string orderNumber)
        {
            List<OrderItemData> items = new List<OrderItemData>();

            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    // Check if order exists
                    string checkQuery = @"
                        SELECT COUNT(*) 
                        FROM orders 
                        WHERE order_number = @orderNo AND status = 'Pending'";

                    using (var cmd = new MySqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        if (count == 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"Order {orderNumber} not found");
                            return null;
                        }
                    }

                    // Load order items
                    string itemsQuery = @"
                        SELECT 
                            item_id,
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
                                int itemId = reader.GetInt32("item_id");

                                var item = new OrderItemData
                                {
                                    ItemId = itemId,
                                    GameName = reader.GetString("game_name"),
                                    Duration = reader.GetInt32("duration_minutes"),
                                    Price = reader.GetDecimal("price"),
                                    EquipmentCost = reader.GetDecimal("equipment_cost"),
                                    Equipment = new List<Equipment>()
                                };

                                items.Add(item);
                            }
                        }
                    }

                    // ⭐ Load equipment for each item
                    foreach (var item in items)
                    {
                        string equipQuery = @"
                            SELECT 
                                equipment_name,
                                quantity,
                                price_per_unit,
                                equipment_type,
                                total_cost
                            FROM order_equipment
                            WHERE item_id = @itemId";

                        using (var cmd = new MySqlCommand(equipQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@itemId", item.ItemId);

                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var equipment = new Equipment
                                    {
                                        Name = reader.GetString("equipment_name"),
                                        RentalQuantity = reader.GetInt32("quantity"),
                                        Price = reader.GetDecimal("price_per_unit"),
                                        Type = reader.GetString("equipment_type")
                                    };

                                    item.Equipment.Add(equipment);

                                    System.Diagnostics.Debug.WriteLine(
                                        $"  Loaded equipment: {equipment.Name} x{equipment.RentalQuantity} ({equipment.Type})"
                                    );
                                }
                            }
                        }
                    }

                    System.Diagnostics.Debug.WriteLine($"✓ Order {orderNumber} loaded: {items.Count} items");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ LoadOrder Error: {ex.Message}");
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
                        System.Diagnostics.Debug.WriteLine($"Order {orderNumber} status → {status}");
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

                    string query = "SELECT COUNT(*) FROM orders WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0;
                    }
                }
            }
            catch
            {
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
                    // Removed customer_name from the SELECT query
                    string query = @"
                        SELECT 
                            order_number,
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
                                    CustomerName = "Walk-in Customer", // Set as static since not in DB
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