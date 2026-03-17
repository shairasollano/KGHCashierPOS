using System.Collections.Generic;
using System.Linq;

namespace KGHCashierPOS
{
    public static class PriceManager
    {
        public static Dictionary<string, (decimal min30, decimal hour1)> GamePrices { get; } =
            new Dictionary<string, (decimal, decimal)>
            {
                { "Billiards", (80, 150) },
                { "Scooter", (100, 150) },
                { "Badminton", (50, 90) },
                { "Table Tennis", (40, 75) }
            };

        public static decimal GetPrice(string gameName, int minutes)
        {
            if (!GamePrices.ContainsKey(gameName))
                return 0;

            if (minutes == 30)
                return GamePrices[gameName].min30;
            else if (minutes == 60)
                return GamePrices[gameName].hour1;
            else
                return GamePrices[gameName].hour1 * (minutes / 60.0m);
        }

        public static string[] GetAllGameNames()
        {
            return GamePrices.Keys.ToArray();
        }
    }
}