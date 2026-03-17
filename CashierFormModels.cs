using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace KGHCashierPOS
{
    // ============ CASHIER SESSION MANAGER ============
    public class CashierSessionManager
    {
        // Active sessions
        public Dictionary<string, GameSession> ActiveSessions { get; private set; }

        // Pricing
        public Dictionary<string, (decimal min30, decimal hour1)> PriceList { get; private set; }

        // Equipment
        public Dictionary<string, List<Equipment>> EquipmentList { get; private set; }

        // Current selection
        public string SelectedGame { get; set; } = "";
        public decimal TotalAmount { get; private set; } = 0;

        public CashierSessionManager()
        {
            ActiveSessions = new Dictionary<string, GameSession>();
            InitializePricing();
            InitializeEquipment();
        }

        // ============ INITIALIZATION ============
        private void InitializePricing()
        {
            PriceList = new Dictionary<string, (decimal, decimal)>
            {
                { "Billiards", (80, 150) },
                { "Scooter", (100, 150) },
                { "Badminton", (50, 90) },
                { "Table Tennis", (40, 75) }
            };
        }

        private void InitializeEquipment()
        {
            EquipmentList = new Dictionary<string, List<Equipment>>
            {
                {
                    "Billiards", new List<Equipment>
                    {
                        new Equipment
                        {
                            Name = "Billiard Stick",
                            Price = 20.00m,
                            DefaultQuantity = 2,
                            Type = "Rental"
                        }
                    }
                },
                {
                    "Badminton", new List<Equipment>
                    {
                        new Equipment
                        {
                            Name = "Badminton Racket",
                            Price = 50.00m,
                            DefaultQuantity = 0,
                            Type = "Rental"
                        },
                        new Equipment
                        {
                            Name = "Shuttlecock",
                            Price = 50.00m,
                            DefaultQuantity = 0,
                            Type = "Purchase"
                        }
                    }
                },
                {
                    "Table Tennis", new List<Equipment>
                    {
                        new Equipment
                        {
                            Name = "Table Tennis Paddle",
                            Price = 20.00m,
                            DefaultQuantity = 2,
                            Type = "Rental"
                        },
                        new Equipment
                        {
                            Name = "Table Tennis Ball",
                            Price = 0.00m,
                            DefaultQuantity = 1,
                            Type = "Included"
                        }
                    }
                },
                {
                    "Scooter", new List<Equipment>
                    {
                        new Equipment
                        {
                            Name = "Scooter",
                            Price = 0.00m,
                            DefaultQuantity = 1,
                            Type = "Included"
                        },
                        new Equipment
                        {
                            Name = "Safety Gear",
                            Price = 0.00m,
                            DefaultQuantity = 1,
                            Type = "Included"
                        }
                    }
                }
            };
        }

        // ============ SESSION MANAGEMENT ============
        public void AddOrExtendSession(string gameName, int minutes, List<Equipment> equipment, decimal equipmentCost)
        {
            decimal gamePrice = minutes == 30
                ? PriceList[gameName].min30
                : PriceList[gameName].hour1;

            if (ActiveSessions.ContainsKey(gameName))
            {
                // Extend existing session
                ActiveSessions[gameName].TotalMinutes += minutes;
                ActiveSessions[gameName].TotalPrice += gamePrice;

                // Add equipment cost
                if (equipmentCost > 0)
                {
                    ActiveSessions[gameName].EquipmentCost += equipmentCost;

                    // Merge equipment lists
                    if (ActiveSessions[gameName].Equipment == null)
                    {
                        ActiveSessions[gameName].Equipment = new List<Equipment>();
                    }
                    ActiveSessions[gameName].Equipment.AddRange(equipment);
                }
            }
            else
            {
                // Create new session
                ActiveSessions[gameName] = new GameSession
                {
                    GameName = gameName,
                    TotalMinutes = minutes,
                    TotalPrice = gamePrice,
                    Equipment = equipment,
                    EquipmentCost = equipmentCost,
                    StartTime = DateTime.Now
                };
            }

            CalculateTotal();
        }

        public void RemoveSession(string gameName)
        {
            ActiveSessions.Remove(gameName);
            CalculateTotal();
        }

        public void ClearAll()
        {
            ActiveSessions.Clear();
            SelectedGame = "";
            TotalAmount = 0;
        }

        // ============ CALCULATIONS ============
        private void CalculateTotal()
        {
            TotalAmount = 0;
            foreach (var session in ActiveSessions.Values)
            {
                TotalAmount += session.TotalPrice + session.EquipmentCost;
            }
        }

        // ============ EQUIPMENT HELPERS ============
        public List<Equipment> GetEquipmentForGame(string gameName)
        {
            if (!EquipmentList.ContainsKey(gameName))
                return new List<Equipment>();

            // Deep copy
            List<Equipment> copy = new List<Equipment>();
            foreach (var eq in EquipmentList[gameName])
            {
                copy.Add(new Equipment
                {
                    Name = eq.Name,
                    Price = eq.Price,
                    DefaultQuantity = eq.DefaultQuantity,
                    RentalQuantity = eq.RentalQuantity,
                    Type = eq.Type
                });
            }
            return copy;
        }

        public bool HasEquipment(string gameName)
        {
            return EquipmentList.ContainsKey(gameName);
        }

        // ============ FORMAT HELPERS ============
        public string FormatDuration(int totalMinutes)
        {
            if (totalMinutes < 60)
            {
                return $"{totalMinutes} min";
            }
            else
            {
                int hours = totalMinutes / 60;
                int minutes = totalMinutes % 60;

                if (minutes == 0)
                    return $"{hours} hr";
                else
                    return $"{hours} hr {minutes} min";
            }
        }

        // ============ ORDER LOADING ============
        public List<OrderItemData> LoadOrderFromDatabase(string orderNumber)
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
                            return null; // Order not found
                    }

                    // Load items
                    string itemsQuery = @"
                        SELECT game_name, duration_minutes, price, equipment_cost 
                        FROM order_items 
                        WHERE order_number = @orderNo";

                    using (var cmd = new MySqlCommand(itemsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@orderNo", orderNumber);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                items.Add(new OrderItemData
                                {
                                    GameName = reader.GetString("game_name"),
                                    Duration = reader.GetInt32("duration_minutes"),
                                    Price = reader.GetDecimal("price"),
                                    EquipmentCost = reader.IsDBNull(reader.GetOrdinal("equipment_cost"))
                                        ? 0
                                        : reader.GetDecimal("equipment_cost")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading order: {ex.Message}");
                return null;
            }

            return items;
        }
    }

    // ============ ORDER ITEM DATA ============
    public class OrderItemData
    {
        public string GameName { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public decimal EquipmentCost { get; set; }
        public decimal TotalPrice => Price + EquipmentCost;
    }
}