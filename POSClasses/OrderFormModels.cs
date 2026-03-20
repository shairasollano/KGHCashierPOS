using System;
using System.Collections.Generic;

namespace KGHCashierPOS
{
    // ============ EQUIPMENT CLASS ============
    public class Equipment
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DefaultQuantity { get; set; }
        public int RentalQuantity { get; set; }
        public string Type { get; set; }  // "Rental", "Purchase", "Included"

        public decimal TotalCost => RentalQuantity * Price;
    }

    // ============ ORDER ITEM CLASS ============
    public class OrderItem
    {
        public string OrderNumber { get; set; }
        public string GameName { get; set; }
        public int Duration { get; set; }
        public decimal GamePrice { get; set; }
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();
        public decimal EquipmentCost { get; set; }
        public decimal TotalPrice => GamePrice + EquipmentCost;

        public string GetDurationText()
        {
            return Duration >= 60 ? $"{Duration / 60} hr" : $"{Duration} min";
        }
    }

    // ============ GAME PRICING CLASS ============
    public class GamePricing
    {
        public decimal Price30Min { get; set; }
        public decimal Price1Hour { get; set; }

        public decimal GetPrice(int minutes)
        {
            if (minutes == 30)
                return Price30Min;
            else if (minutes == 60)
                return Price1Hour;
            else
                return Price1Hour * (minutes / 60.0m);
        }
    }

    // ============ ORDER MANAGER CLASS ============
    public class OrderManager
    {
        // Current order data
        public string OrderNumber { get; private set; }
        public List<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public decimal TotalAmount => CalculateTotal();

        // Game pricing
        public Dictionary<string, GamePricing> PriceList { get; private set; }

        // Game equipment
        public Dictionary<string, List<Equipment>> EquipmentList { get; private set; }

        // Current selection state
        public string SelectedGame { get; set; } = "";
        public int SelectedDuration { get; set; } = 0;

        public OrderManager()
        {
            InitializePricing();
            InitializeEquipment();
            GenerateNewOrderNumber();
        }

        // ============ INITIALIZATION ============
        private void InitializePricing()
        {
            PriceList = new Dictionary<string, GamePricing>
            {
                { "Billiards", new GamePricing { Price30Min = 80, Price1Hour = 150 } },
                { "Scooter", new GamePricing { Price30Min = 100, Price1Hour = 150 } },
                { "Badminton", new GamePricing { Price30Min = 50, Price1Hour = 90 } },
                { "Table Tennis", new GamePricing { Price30Min = 40, Price1Hour = 75 } }
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

        // ============ ORDER NUMBER GENERATION ============
        public void GenerateNewOrderNumber()
        {
            OrderNumber = GetNextOrderNumber();
        }

        private string GetNextOrderNumber()
        {
            try
            {
                using (var conn = new MySql.Data.MySqlClient.MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT order_number 
                        FROM orders 
                        WHERE order_number REGEXP '^[0-9]+$'
                        ORDER BY CAST(order_number AS UNSIGNED) DESC 
                        LIMIT 1";

                    using (var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int lastNumber))
                        {
                            return (lastNumber + 1).ToString("D6");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting order number: {ex.Message}");
            }

            return "000001";
        }

        // ============ ORDER MANAGEMENT ============
        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }

        public void RemoveLastItem()
        {
            if (Items.Count > 0)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }

        public void ClearOrder()
        {
            Items.Clear();
            SelectedGame = "";
            SelectedDuration = 0;
        }

        public void ClearAll()
        {
            ClearOrder();
            GenerateNewOrderNumber();
        }

        // ============ CALCULATIONS ============
        private decimal CalculateTotal()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.TotalPrice;
            }
            return total;
        }

        public decimal CalculateGamePrice(string gameName, int minutes)
        {
            if (!PriceList.ContainsKey(gameName))
                return 0;

            return PriceList[gameName].GetPrice(minutes);
        }

        // ============ EQUIPMENT HELPERS ============
        public List<Equipment> GetEquipmentForGame(string gameName)
        {
            if (!EquipmentList.ContainsKey(gameName))
                return new List<Equipment>();

            // Return deep copy to avoid modifying original
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

        // ============ VALIDATION ============
        public bool IsGameSelected()
        {
            return !string.IsNullOrEmpty(SelectedGame);
        }

        public bool IsDurationSelected()
        {
            return SelectedDuration > 0;
        }

        public bool IsReadyToAdd()
        {
            return IsGameSelected() && IsDurationSelected();
        }

        public bool HasItems()
        {
            return Items.Count > 0;
        }

        // ============ RESET SELECTION ============
        public void ResetSelection()
        {
            SelectedGame = "";
            SelectedDuration = 0;
        }
    }
}