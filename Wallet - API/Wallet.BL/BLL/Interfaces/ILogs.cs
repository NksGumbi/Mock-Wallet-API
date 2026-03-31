using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallet.BL.BLL.Interfaces
{
    public interface ILogs
    {
        void LogData<T>(string methodName, string username, string? message, bool isError, T obj);
    }
}
