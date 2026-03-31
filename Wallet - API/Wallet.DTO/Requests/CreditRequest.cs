using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Wallet.DTO.Requests
{
    public class CreditRequest
    {
        public string TransactionID { get; set; }
        public string? CorrelationID { get; set; }
        public string OriginPlayerID { get; set; }
        public int PlayerID { get; set; } = 0;
        public string OperatorUrl { get; set; }
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string CurrencyCode { get; set; }
        public double Amount { get; set; }
        public string RoundID { get; set; }
        public int GameEnumId { get; set; }
        public decimal Odds { get; set; } = 0;
    }

    public class WalletCreditRequest
    {
        [JsonPropertyName("TransactionID")]
        public string TransactionID { get; set; }
        [JsonPropertyName("CorrelationID")]
        public string? CorrelationID { get; set; }
        [JsonPropertyName("OriginPlayerID")]
        public string OriginPlayerID { get; set; }
        [JsonPropertyName("GameID")]
        public int GameID { get; set; }
        [JsonPropertyName("GameName")]
        public string GameName { get; set; }
        [JsonPropertyName("CurrencyCode")]
        public string CurrencyCode { get; set; }
        [JsonPropertyName("Amount")]
        public double Amount { get; set; }
        [JsonPropertyName("RoundID")]
        public string RoundID { get; set; }

        [JsonPropertyName("Odds")]
        public decimal Odds { get; set; } = 0;
    }
}
