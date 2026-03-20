using MySql.Data.MySqlClient;
using System;
using System.Linq;

namespace KGHCashierPOS
{
    public static class OrderNumberGenerator
    {
        public static string GenerateNext()
        {
            try
            {
                using (var conn = new MySqlConnection(Database.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT order_number 
                        FROM orders 
                        WHERE order_number REGEXP '^[0-9]+$'
                        ORDER BY CAST(order_number AS UNSIGNED) DESC 
                        LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
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
                System.Diagnostics.Debug.WriteLine($"Error generating order number: {ex.Message}");
            }

            return "000001";
        }

        public static string Format(string input)
        {
            // Remove non-digits
            string digits = new string(input.Where(char.IsDigit).ToArray());

            // Pad to 6 digits
            return digits.PadLeft(6, '0');
        }

        public static bool IsValid(string orderNumber)
        {
            return !string.IsNullOrEmpty(orderNumber) &&
                   orderNumber.Length == 6 &&
                   orderNumber.All(char.IsDigit);
        }
    }
}