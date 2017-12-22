using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Logger
{
    public interface ILogger<T>
    {
        void Debug(object message, Exception exception = null);
        void Error(object message);
        void logError(string message, Exception ex = null);
        void logInfo(string message);
    }
}
