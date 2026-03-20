using iTextSharp.text;
using KGHCashierPOS;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace KGHCashierPOS
{
    public static class ValidationHelper
    {
        public static bool IsValidOrderNumber(string orderNumber)
        {
            return !string.IsNullOrWhiteSpace(orderNumber) &&
                   orderNumber.Length == 6 &&
                   orderNumber.All(char.IsDigit);
        }

        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string digits = new string(phone.Where(char.IsDigit).ToArray());
            return digits.Length >= 10 && digits.Length <= 11;
        }

        public static bool IsValidAge(string age)
        {
            if (string.IsNullOrWhiteSpace(age))
                return false;

            if (int.TryParse(age, out int ageValue))
            {
                return ageValue >= 1 && ageValue <= 120;
            }

            return false;
        }

        public static bool IsValidAmount(string amount)
        {
            if (string.IsNullOrWhiteSpace(amount))
                return false;

            return decimal.TryParse(amount, out decimal value) && value > 0;
        }

        public static bool IsValidGCashReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
                return false;

            string digits = new string(reference.Where(char.IsDigit).ToArray());
            return digits.Length >= 12 && digits.Length <= 14;
        }
    }
}