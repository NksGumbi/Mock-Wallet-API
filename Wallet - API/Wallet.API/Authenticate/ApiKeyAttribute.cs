using Microsoft.AspNetCore.Mvc;

namespace Wallet.API.Authenticate
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute()
            : base(typeof(ApiKeyAuthorizationFilter))
        {
        }
    }
}
