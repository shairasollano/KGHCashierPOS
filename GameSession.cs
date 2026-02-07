using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGHCashierPOS
{
    
    public class GameSession
    {
        public string CashierName { get; set; }
        public string GameName { get; set; }
        public int TotalMinutes { get; set; }
        public decimal RatePerHour { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

        public TimeSpan RemainingTime { get; set; }
        public bool IsPaused { get; set; }

        public bool HasDiscount { get; set; }
        public string DiscountType { get; set; } // Senior / PWD
        public string DiscountID { get; set; }
        public decimal DiscountAmount { get; set; }

        // FOR ORDER NUMBER ADDITIONAL FUNCTION
        public int SessionId { get; set; }
        public string OrderNumber { get; set; }
        public string GameType { get; set; }

    }

}

