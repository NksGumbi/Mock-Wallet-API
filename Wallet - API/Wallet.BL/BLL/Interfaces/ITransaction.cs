using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.DTO.Requests;
using Wallet.DTO.Responses;

namespace Wallet.BL.BLL.Interfaces
{
    public interface ITransaction
    {
        Task<DebitResponseDTO> Debit(DebitRequest request);
        Task<CreditResponseDTO> Credit(CreditRequest request);
        Task<RefundResponseDTO> Refund(RefundRequest request);
        Task<BalanceResponseDTO> Balance(BalanceRequest request);
    }
}
