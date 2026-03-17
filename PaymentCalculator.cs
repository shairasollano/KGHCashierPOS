using System.Collections.Generic;

namespace KGHCashierPOS
{
    public class PaymentCalculator
    {
        public decimal Subtotal { get; private set; }
        public decimal DiscountAmount { get; private set; }
        public decimal FinalAmount { get; private set; }

        public void Calculate(Dictionary<string, GameSession> sessions, decimal discount)
        {
            Subtotal = CalculateSubtotal(sessions);
            DiscountAmount = discount;
            FinalAmount = Subtotal - DiscountAmount;
        }

        public decimal CalculateSubtotal(Dictionary<string, GameSession> sessions)
        {
            decimal subtotal = 0;

            if (sessions != null)
            {
                foreach (var session in sessions.Values)
                {
                    subtotal += session.TotalPrice;
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
            FinalAmount = 0;
        }
    }
}