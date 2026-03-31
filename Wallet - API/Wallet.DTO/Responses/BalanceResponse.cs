using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.DTO.Responses
{

    public class BalanceResponseDTO
    {
        public BalanceResponse balanceResponse { get; set; }
        public BaseResponse baseResponse { get; set; }
    }

    public class BalanceResponse
    {
        public string OriginPlayerID { get; set; }
        public string CurrencyCode { get; set; }
        public double Balance { get; set; }
    }
}
