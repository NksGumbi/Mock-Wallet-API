using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.DTO.Responses
{
    public class TokenDataDTO
    {
        public TokenData tokenData { get; set; }
        public BaseResponse baseResponse { get; set; }
    }

    public class TokenData
    {
        public string? OperatorKey {  get; set; }
        public string? OriginPlayerID { get; set; }
        public string? OperatorURL { get; set; }
        public string? CurrencyCode { get; set; }
    }
}
