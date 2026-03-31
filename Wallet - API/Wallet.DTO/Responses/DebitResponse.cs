using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.DTO.Responses
{
    public class DebitResponseDTO
    {
        public DebitResponse debitResponse { get; set; }
        public BaseResponse baseResponse { get; set; }
    }

    public class DebitResponse
    {
        public string TransactionID { get; set; }
        public string OriginPlayerID { get; set; }
        public string CurrencyCode { get; set; }
        public double Balance { get; set; }
    }
}
