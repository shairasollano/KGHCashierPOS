using System.Collections.Generic;

namespace KGHCashierPOS
{
    public class PaymentCalculator
    {
        private const decimal TAX_RATE = 0.12m; // 12% VAT

        public decimal Subtotal { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal SubtotalAfterDiscount { get; private set; }
        public decimal TaxableAmount { get; private set; }
        public decimal TaxAmount { get; private set; }
        public decimal FinalAmount { get; private set; }

        public void Calculate(Dictionary<string, GameSession> sessions, decimal discount)
        {
            // ⭐ Calculate subtotal (includes game price + equipment cost)
            Subtotal = CalculateSubtotal(sessions);

            // Apply discount
            DiscountAmount = discount;
            SubtotalAfterDiscount = Subtotal - DiscountAmount;

            // Calculate tax breakdown (VAT is already included in prices)
            TaxableAmount = SubtotalAfterDiscount / (1 + TAX_RATE);
            TaxAmount = SubtotalAfterDiscount - TaxableAmount;

            // Final amount (already includes VAT)
            FinalAmount = SubtotalAfterDiscount;

            System.Diagnostics.Debug.WriteLine($"=== PaymentCalculator ===");
            System.Diagnostics.Debug.WriteLine($"Subtotal (Game + Equipment): {Subtotal:C}");
            System.Diagnostics.Debug.WriteLine($"Discount: {DiscountAmount:C}");
            System.Diagnostics.Debug.WriteLine($"Final Amount: {FinalAmount:C}");
        }

        public decimal CalculateSubtotal(Dictionary<string, GameSession> sessions)
        {
            decimal subtotal = 0;

            if (sessions != null)
            {
                foreach (var session in sessions.Values)
                {
                    // ⭐ FIXED: Include both game price AND equipment cost
                    decimal sessionTotal = session.TotalPrice + session.EquipmentCost;
                    subtotal += sessionTotal;

                    System.Diagnostics.Debug.WriteLine($"  {session.GameName}: Game={session.TotalPrice:C}, Equipment={session.EquipmentCost:C}, Total={sessionTotal:C}");
                }
            }

            return subtotal;
        }

        public decimal GetFinalAmount()
        {
            return FinalAmount;
        }

        public void Clear()
        {
            Subtotal = 0;
            DiscountAmount = 0;
            SubtotalAfterDiscount = 0;
            TaxableAmount = 0;
            TaxAmount = 0;
            FinalAmount = 0;
        }

        // Helper methods for display
        public string GetTaxBreakdown()
        {
            return $"VATable Sale: {PriceFormatter.Format(TaxableAmount)}\n" +
                   $"VAT (12%): {PriceFormatter.Format(TaxAmount)}";
        }
    }
}