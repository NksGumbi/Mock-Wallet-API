using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.DTO.Requests
{
    public class TransactionRequest
    {
        public int PlayerID { get; set; }
        public string OriginTransactionID { get; set; }
        public string? CorrelationID { get; set; }
        public double Stake { get; set; }
        public decimal Odds { get; set; } = 0;
        public string RoundID { get; set; }
        public int TransactionTypeID { get; set; }
        public long? GameID { get; set; }
        public string? Currency { get; set; }
        public int GameEnumId { get; set; }
    }
}
