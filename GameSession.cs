using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHCashierPOS
{
    
    public class GameSession
    {
        public string GameName { get; set; }
        public int TotalMinutes { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime StartTime { get; set; }
    }

}
