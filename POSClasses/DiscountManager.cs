using System;

namespace KGHCashierPOS
{
    public class DiscountManager
    {
        public decimal SubtotalAmount { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public string DiscountType { get; private set; } = "None";

        public void SetSubtotal(decimal subtotal)
        {
            SubtotalAmount = subtotal;
            System.Diagnostics.Debug.WriteLine($"DiscountManager - Subtotal set: {subtotal:C}");
        }

        public void ClearDiscount()
        {
            DiscountAmount = 0;
            DiscountType = "None";
            System.Diagnostics.Debug.WriteLine("DiscountManager - Discount cleared");
        }

        public bool ApplyPercentageDiscount(decimal percentage, string discountName)
        {
            if (SubtotalAmount <= 0)
                return false;

            DiscountAmount = SubtotalAmount * percentage;
            DiscountType = discountName;

            System.Diagnostics.Debug.WriteLine($"DiscountManager - Percentage discount applied:");
            System.Diagnostics.Debug.WriteLine($"  Subtotal: {SubtotalAmount:C}");
            System.Diagnostics.Debug.WriteLine($"  Percentage: {percentage:P}");
            System.Diagnostics.Debug.WriteLine($"  Discount: {DiscountAmount:C}");

            return true;
        }

        public bool ApplyCustomDiscount(decimal amount, string discountName)
        {
            if (amount > SubtotalAmount)
                return false;

            DiscountAmount = amount;
            DiscountType = discountName;

            System.Diagnostics.Debug.WriteLine($"DiscountManager - Custom discount applied: {amount:C}");

            return true;
        }

        public DiscountResult ValidatePromoCode(string promoCode, decimal subtotal)
        {
            if (string.IsNullOrWhiteSpace(promoCode))
                return new DiscountResult { IsValid = false, Message = "Please enter a promo code" };

            decimal discount = 0;
            string message = "";

            switch (promoCode.ToUpper())
            {
                case "WELCOME10":
                    discount = subtotal * 0.10m;
                    message = "10% discount applied!";
                    break;

                case "NEWUSER20":
                    discount = subtotal * 0.20m;
                    message = "20% discount applied!";
                    break;

                case "FREEGAME":
                    discount = 100;
                    message = "₱100 discount applied!";
                    break;

                default:
                    return new DiscountResult { IsValid = false, Message = "Invalid promo code!" };
            }

            DiscountAmount = discount;
            DiscountType = $"Promo Code ({promoCode.ToUpper()})";

            return new DiscountResult { IsValid = true, Message = message, Discount = discount };
        }
    }

    public class DiscountResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public decimal Discount { get; set; }
    }
}