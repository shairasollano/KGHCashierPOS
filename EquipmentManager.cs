using System.Collections.Generic;

namespace KGHCashierPOS
{
    public static class EquipmentManager
    {
        private static Dictionary<string, List<Equipment>> equipmentCatalog;

        static EquipmentManager()
        {
            InitializeEquipment();
        }

        private static void InitializeEquipment()
        {
            equipmentCatalog = new Dictionary<string, List<Equipment>>
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

        public static List<Equipment> GetEquipmentForGame(string gameName)
        {
            if (!equipmentCatalog.ContainsKey(gameName))
                return new List<Equipment>();

            // Return deep copy
            List<Equipment> copy = new List<Equipment>();
            foreach (var eq in equipmentCatalog[gameName])
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

        public static bool HasEquipment(string gameName)
        {
            return equipmentCatalog.ContainsKey(gameName) &&
                   equipmentCatalog[gameName].Count > 0;
        }

        public static decimal CalculateEquipmentCost(List<Equipment> equipment)
        {
            decimal total = 0;
            foreach (var eq in equipment)
            {
                total += eq.RentalQuantity * eq.Price;
            }
            return total;
        }
    }
}