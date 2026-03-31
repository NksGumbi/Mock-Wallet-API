using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.DTO.Requests
{
    public class BalanceRequest
    {
        public string OriginPlayerID { get; set; }
        public string OperatorUrl { get; set; }
        public int GameID { get; set; }
        public string GameName { get; set; }
    }
}
