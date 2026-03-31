using Microsoft.Extensions.Options;
using Wallet.DTO.DataObjects;

namespace Wallet.API.Authenticate
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IOptions<AppSettings> _config;

        public ApiKeyValidator(IOptions<AppSettings> config)
        {
            _config = config;
        }
        public bool IsValid(string apiKey)
        {
            string storedApiKey = _config.Value.ApiKey;
            return apiKey == storedApiKey;
        }
    }

    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
}
