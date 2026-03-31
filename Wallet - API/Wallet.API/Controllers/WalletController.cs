using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Wallet.API.Authenticate;
using Wallet.BL.BLL.Interfaces;
using Wallet.DTO.Requests;
using Wallet.DTO.Responses;

namespace Wallet.API.Controllers
{
    [ApiKey]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : Controller
    {
        private readonly ITransaction _transaction;
        private readonly ILogs _logs;

        public WalletController(ITransaction transaction, ILogs logs)
        {
            _transaction = transaction;
            _logs = logs;
        }

        [HttpPost("Debit")]
        public async Task<IActionResult> Debit([FromBody] DebitRequest request)
        {
            _logs.LogData("DebitRequest", request.OriginPlayerID, "Debit: Successful Request", false, request);

            var response = new DebitResponseDTO { baseResponse = new BaseResponse() };

            try
            {
                response = await _transaction.Debit(request);

                _logs.LogData(nameof(Debit), request.OriginPlayerID, response.baseResponse.StatusMessage, response.baseResponse.isError, response.baseResponse.isError ? $"{response.baseResponse.StatusMessage}, Request: {JsonConvert.SerializeObject(request)}, Response: {JsonConvert.SerializeObject(response)}" : response.baseResponse.StatusMessage);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.baseResponse = new BaseResponse { isError = true, StatusMessage = ex.Message, StatusCode = 500 };

                _logs.LogData(nameof(Refund), request.OriginPlayerID, ex.Message, response.baseResponse.isError, $"Error: {ex}, Request: {JsonConvert.SerializeObject(response)}");

                return StatusCode(500, response);
            }
        }


        [HttpPost("Credit")]
        public async Task<IActionResult> Credit([FromBody] CreditRequest request)
        {
            _logs.LogData("CreditRequest", request.OriginPlayerID, "Credit: Successful Request", false, request);

            var response = new CreditResponseDTO { baseResponse = new BaseResponse() };

            try
            {
                response = await _transaction.Credit(request);

                _logs.LogData(nameof(Credit), request.OriginPlayerID, response.baseResponse.StatusMessage, response.baseResponse.isError, response.baseResponse.isError ? $"{response.baseResponse.StatusMessage}, Request: {JsonConvert.SerializeObject(request)}, Response: { JsonConvert.SerializeObject(response) }" : response.baseResponse.StatusMessage);

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.baseResponse = new BaseResponse { isError = true, StatusMessage = ex.Message, StatusCode = 500 };

                _logs.LogData(nameof(Refund), request.OriginPlayerID, ex.Message, response.baseResponse.isError, $"Error: {ex}, Request: {JsonConvert.SerializeObject(response)}");

                return StatusCode(500, response);
            }
        }


        [HttpPost("Refund")]
        public async Task<IActionResult> Refund([FromBody] RefundRequest request)
        {
            _logs.LogData("RefundRequest", request.OriginPlayerID, "Refund: Successful Request", false, request);

            var response = new RefundResponseDTO { baseResponse = new BaseResponse() };

            try
            {
                response = await _transaction.Refund(request);

                _logs.LogData(nameof(Refund), request.OriginPlayerID, response.baseResponse.StatusMessage, response.baseResponse.isError, response.baseResponse.isError ? $"{response.baseResponse.StatusMessage}, Request: {JsonConvert.SerializeObject(request)}, Response: {JsonConvert.SerializeObject(response)}" : response.baseResponse.StatusMessage);

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.baseResponse = new BaseResponse { isError = true, StatusMessage = ex.Message, StatusCode = 500 };

                _logs.LogData(nameof(Refund), request.OriginPlayerID, ex.Message, response.baseResponse.isError, $"Error: {ex}, Request: {JsonConvert.SerializeObject(response)}");

                return StatusCode(500, response);
            }
        }


        [HttpPost("Balance")]
        public async Task<IActionResult> Balance([FromBody] BalanceRequest request)
        {
            _logs.LogData("BalanceRequest", request.OriginPlayerID, "Balance: Successful Request", false, request);

            var response = new BalanceResponseDTO { baseResponse = new BaseResponse() };

            try
            {
                response = await _transaction.Balance(request);

                _logs.LogData(nameof(Balance), request.OriginPlayerID, response.baseResponse.StatusMessage, response.baseResponse.isError, response.baseResponse.isError ? $"{response.baseResponse.StatusMessage}, Request: {JsonConvert.SerializeObject(request)}, Response: {JsonConvert.SerializeObject(response)}" : response.baseResponse.StatusMessage);

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.baseResponse = new BaseResponse { isError = true, StatusMessage = ex.Message, StatusCode = 500 };

                _logs.LogData(nameof(Refund), request.OriginPlayerID, ex.Message, response.baseResponse.isError, $"Error: {ex}, Request: {JsonConvert.SerializeObject(response)}");

                return StatusCode(500, response);
            }
        }
    }
}
