using System.Text;
using Wallet.BL.BLL.Interfaces;
using Wallet.DTO.Requests;
using Wallet.DTO.Responses;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Serilog;
using Microsoft.Data.SqlClient;
using System.Data;
using Wallet.BL.DAL.Queries;
using Dapper;
using Microsoft.Extensions.Options;
using Wallet.DTO.DataObjects;
using RestSharp;
using System.Net;

namespace Wallet.BL.BLL
{
    public class Transaction : ITransaction
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<AppSettings> _config;
        private readonly IOptions<ConnectionStrings> _con;
        private readonly ILogs _logs;

        public Transaction(HttpClient httpClient, IOptions<AppSettings> config, IOptions<ConnectionStrings> con, ILogs logs)
        {
            _httpClient = httpClient;
            _config = config;
            _con = con;
            _logs = logs;
        }

        private async Task<Guid> InsertTransaction(TransactionRequest request)
        {
            var result = Guid.Empty;

            try
            {
                using (IDbConnection db = new SqlConnection(_con.Value.DBConnection))
                {
                    result = await db.QuerySingleAsync<Guid>(Queries.InsertTransaction, new
                    {
                        @PlayerID = request.PlayerID,
                        @TransactionTypeID = request.TransactionTypeID,
                        @OriginTransactionID = request.OriginTransactionID,
                        @CorrelationID = request.CorrelationID,
                        @Stake = request.Stake,
                        @Odds = request.Odds,
                        @RoundId = request.RoundID,
                        @ProviderGameID = request.GameID,
                        @Currency = request.Currency,
                        @GameEnumId = request.GameEnumId

                    }, commandType: CommandType.StoredProcedure);
                }

                _logs.LogData( nameof(InsertTransaction), request.PlayerID.ToString(), "InsertTransaction: Successful", false, request);

                return result;
            }
            catch (Exception ex)
            {
                _logs.LogData(nameof(InsertTransaction), request.PlayerID.ToString(), ex.Message, true, request);
                return result;
            }
        }

        public async Task<DebitResponseDTO> Debit(DebitRequest request)
        {
            var response = new DebitResponseDTO
            {
                debitResponse = new DebitResponse(),
                baseResponse = new BaseResponse()
            };

            var transaction = new TransactionRequest
            {
                PlayerID = request.PlayerID,
                OriginTransactionID = request.TransactionID,
                CorrelationID = request.CorrelationID,
                Stake = request.Amount,
                Odds = request.Odds,
                RoundID = request.RoundID,
                TransactionTypeID = 2,
                GameID = request.GameID,
                Currency = request.CurrencyCode,
                GameEnumId = request.GameEnumId
            };

            try
            {
                var result = await InsertTransaction(transaction);
                if (result == Guid.Empty)
                {
                    SetErrorResponse(response.baseResponse, 500, "UNKNOWN_ERROR: General error, internal server error");
                    return response;
                }

                var debitRequest = new WalletDebitRequest
                {
                    TransactionID = result.ToString(),
                    CorrelationID = request.CorrelationID.ToString(),
                    OriginPlayerID = request.OriginPlayerID,
                    GameID = request.GameID,
                    GameName = request.GameName,
                    CurrencyCode = request.CurrencyCode,
                    Amount = request.Amount,
                    RoundID = request.RoundID,
                    Odds = request.Odds
                };

                var debitReq = await RestRequest(request.OperatorUrl, "DebitBalance", debitRequest);
                var res = JsonConvert.DeserializeObject<DebitResponse>(debitReq.Content);

                response.debitResponse = res;
                response.baseResponse = await SetResponseStatus(response.baseResponse, (int)debitReq.StatusCode, debitReq.IsSuccessStatusCode ? "Debit Successful" : "Debit Unsuccessful");

                return response;
            }
            catch (Exception ex)
            {
                SetErrorResponse(response.baseResponse, 500, $"UNKNOWN_ERROR: {ex.Message}");
                return response;
            }
        }
        public async Task<CreditResponseDTO> Credit(CreditRequest request)
        {
            var response = new CreditResponseDTO
            {
                creditResponse = new CreditResponse(),
                baseResponse = new BaseResponse()
            };

            var transaction = new TransactionRequest
            {
                PlayerID = request.PlayerID,
                OriginTransactionID = request.TransactionID,
                CorrelationID = request.CorrelationID,
                Stake = request.Amount,
                RoundID = request.RoundID,
                TransactionTypeID = 1,
                GameID = request.GameID,
                Currency = request.CurrencyCode,
                GameEnumId = request.GameEnumId,
                Odds = request.Odds,
            };

            try
            {
                var result = await InsertTransaction(transaction);

                if (result == Guid.Empty)
                {
                    SetErrorResponse(response.baseResponse, 409, "DUPLICATE_TRANSACTION: Duplicate transaction");
                    return response;
                }

                var creditRequest = new WalletCreditRequest
                {
                    TransactionID = result.ToString(),
                    CorrelationID = request.CorrelationID.ToString(),
                    OriginPlayerID = request.OriginPlayerID,
                    GameID = request.GameID,
                    GameName = request.GameName,
                    CurrencyCode = request.CurrencyCode,
                    Amount = request.Amount,
                    RoundID = request.RoundID,
                    Odds = request.Odds
                };

                var creditReq = await RestRequest(request.OperatorUrl, "CreditBalance", creditRequest);
                var res = JsonConvert.DeserializeObject<CreditResponse>(creditReq.Content);

                response.creditResponse = res;
                response.baseResponse = await SetResponseStatus(response.baseResponse, (int)creditReq.StatusCode, creditReq.IsSuccessStatusCode ? "Credit Successful" : "Credit Unsuccessful");

                return response;
            }
            catch (Exception ex)
            {
                response.baseResponse = await SetResponseStatus(response.baseResponse, 500, $"UNKNOWN_ERROR: {ex.Message}");
                return response;
            }
        }
        public async Task<RefundResponseDTO> Refund(RefundRequest request)
        {
            var response = new RefundResponseDTO
            {
                refundResponse = new RefundResponse(),
                baseResponse = new BaseResponse()
            };

            var transaction = new TransactionRequest
            {
                PlayerID = request.PlayerID,
                OriginTransactionID = request.TransactionID,
                CorrelationID = request.CorrelationID,
                Stake = request.Amount,
                Odds = request.Odds,
                RoundID = request.RoundID,
                TransactionTypeID = 3,
                GameID = request.GameID,
                Currency = request.CurrencyCode,
                GameEnumId = request.GameEnumId
            };

            try
            {
                var result = await InsertTransaction(transaction);

                if (result == Guid.Empty)
                {
                    SetErrorResponse(response.baseResponse, 409, "DUPLICATE_TRANSACTION: Duplicate transaction");
                    return response;
                }

                var refundRequest = new WalletRefundRequest
                {
                    TransactionID = result.ToString(),
                    CorrelationID = request.CorrelationID.ToString(),
                    OriginPlayerID = request.OriginPlayerID,
                    GameID = request.GameID,
                    GameName = request.GameName,
                    CurrencyCode = request.CurrencyCode,
                    Amount = request.Amount,
                    RoundID = request.RoundID,
                    Odds = request.Odds
                };

                var refundReq = await RestRequest(request.OperatorUrl, "RefundBalance", refundRequest);
                var res = JsonConvert.DeserializeObject<RefundResponse>(refundReq.Content);

                response.refundResponse = res;
                response.baseResponse = await SetResponseStatus(response.baseResponse, (int)refundReq.StatusCode, refundReq.IsSuccessStatusCode ? "Refund Successful" : "Refund Unsuccessful");

                return response;
            }
            catch (Exception ex)
            {
                response.baseResponse = await SetResponseStatus(response.baseResponse, 500, $"UNKNOWN_ERROR: {ex.Message}");
                return response;
            }
        }
        public async Task<BalanceResponseDTO> Balance(BalanceRequest request)
        {
            var response = new BalanceResponseDTO
            {
                balanceResponse = new BalanceResponse(),
                baseResponse = new BaseResponse()
            };

            var balanceRequest = new
            {
                OriginPlayerID = request.OriginPlayerID,
                GameID = request.GameID,
                GameName = request.GameName
            };

            try
            {
                StringContent jsonContent = new StringContent(JsonConvert.SerializeObject(balanceRequest), Encoding.UTF8, "application/json");
                var res = await _httpClient.PostAsync($"{request.OperatorUrl}/GetBalance", jsonContent);
                response.balanceResponse = await res.Content.ReadFromJsonAsync<BalanceResponse>();
                response.baseResponse = await SetResponseStatus(response.baseResponse, (int)res.StatusCode, res.IsSuccessStatusCode ? "Balance Retrieval Successful" : "Balance Retrieval Unsuccessful");

                return response;
            }
            catch (Exception ex)
            {
                response.baseResponse = await SetResponseStatus(response.baseResponse, 500, $"UNKNOWN_ERROR: {ex.Message}");
                return response;
            }
        }

        public async Task<RestResponse> RestRequest<T>(string api,string endpoint, T requestObj) where T : class
        {
            RestResponse res = new RestResponse();
            try
            {
                var client = new RestClient(api);

                var request = new RestRequest(endpoint, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(requestObj);

                res = await client.ExecuteAsync(request);

                return res;
            }
            catch (Exception ex)
            {
                return res;
            }
        }
        private async Task<BaseResponse> SetResponseStatus(BaseResponse baseResponse, int statusCode, string defaultMessage)
        {
            baseResponse.StatusCode = statusCode;
            baseResponse.StatusMessage = MapErrorCode(statusCode, defaultMessage);
            baseResponse.isError = (statusCode != 200 && statusCode != 201);
            return baseResponse;
        }

        private void SetErrorResponse(BaseResponse baseResponse, int statusCode, string message)
        {
            baseResponse.StatusCode = statusCode;
            baseResponse.StatusMessage = message;
            baseResponse.isError = true;
        }
        private string MapErrorCode(int statusCode, string defaultMessage)
        {
            switch (statusCode)
            {
                case 200:
                    return "OK: Successful transaction";
                case 400:
                    return "BAD_REQUEST: Missing/invalid parameters";
                case 401:
                    return "UNAUTHORIZED: Invalid signatureInvalid signature";
                case 402:
                    return "ACCOUNT_LOCKED: Player is locked/frozen";
                case 403:
                    return "INSUFFICIENT_BALANCE: Balance is insufficient";
                case 404:
                    return "PLAYER_NOT_FOUND: Player does not exist";
                case 409:
                    return "DUPLICATE_TRANSACTION: Duplicate transaction";
                case 500:
                    return "UNKNOWN_ERROR: General error, internal server error";
                default:
                    return defaultMessage;
            }
        }
    }
}