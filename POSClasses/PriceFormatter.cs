namespace KGHCashierPOS
{
    public static class PriceFormatter
    {
        public static string Format(decimal price)
        {
            return "₱" + price.ToString("N2");
        }

        public static string FormatSimple(decimal price)
        {
            return "₱" + price.ToString("0.00");
        }

        public static decimal Parse(string priceText)
        {
            decimal price = 0;

            if (priceText.StartsWith("₱"))
            {
                string cleanPrice = priceText.Replace("₱", "").Replace(",", "").Trim();
                decimal.TryParse(cleanPrice, out price);
            }

            return price;
        }

        public static bool TryParse(string priceText, out decimal price)
        {
            price = 0;

            if (string.IsNullOrWhiteSpace(priceText))
                return false;

            string cleanPrice = priceText.Replace("₱", "").Replace(",", "").Trim();
            return decimal.TryParse(cleanPrice, out price);
        }
    }
}