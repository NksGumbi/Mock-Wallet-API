using Newtonsoft.Json;
using Serilog.Context;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.BL.BLL.Interfaces;

namespace Wallet.BL.BLL
{
    public class Logs: ILogs
    {
        public void LogData<T>(string methodName, string username, string? message, bool isError, T obj)
        {
            // Serialized object as a JSON string
            var serializedObject = JsonConvert.SerializeObject(obj);

            LogContext.PushProperty("UserId", username);
            LogContext.PushProperty("MethodName", $"WalletAPI : {methodName}");
            LogContext.PushProperty("IsBO", false);
            LogContext.PushProperty("IsSignificant", false);

            // Logging with separate message and template
            if (isError)
            {
                Log.Error($"Wallet API Error - {methodName} : {serializedObject}");
            }
            else
            {
                Log.Information($"Wallet API Success - {methodName} : {serializedObject}");
            }
        }
    }
}
