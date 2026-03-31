namespace Wallet.API.Enums
{
    public enum WalletErrorCode
    {
        OK = 200,
        BAD_REQUEST = 400,
        UNAUTHORIZED = 401,
        ACCOUNT_LOCKED = 402,
        INSUFFICIENT_BALANCE = 403,
        PLAYER_NOT_FOUND = 404,
        DUPLICATE_TRANSACTION = 409,
        UNKNOWN_ERROR = 500
    }

    public static class WalletErrorCodeExtensions
    {
        public static string GetDescription(this WalletErrorCode errorCode)
        {
            return errorCode switch
            {
                WalletErrorCode.OK => "Successful transaction.",
                WalletErrorCode.BAD_REQUEST => "Missing/invalid parameters.",
                WalletErrorCode.UNAUTHORIZED => "Invalid signature.",
                WalletErrorCode.ACCOUNT_LOCKED => "Player is locked/frozen.",
                WalletErrorCode.INSUFFICIENT_BALANCE => "Balance is insufficient.",
                WalletErrorCode.PLAYER_NOT_FOUND => "Player does not exist.",
                WalletErrorCode.DUPLICATE_TRANSACTION => "Duplicate transaction.",
                WalletErrorCode.UNKNOWN_ERROR => "General error, internal server error.",
                _ => "Undefined error."
            };
        }
    }
}