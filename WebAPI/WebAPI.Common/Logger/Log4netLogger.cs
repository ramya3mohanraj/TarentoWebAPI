using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Common.Logger
{
    public class Log4netLogger<T> : ILogger<T>
    {
        private ILog _logger;
        public Log4netLogger()
        {
            _logger = LogManager.GetLogger(typeof(T));
        }
        public void Debug(object message, Exception exception = null)
        {
            _logger.Debug(message, exception);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void logError(string message, Exception ex = null)
        {
            _logger.Error(message, ex);
        }

        public void logInfo(string message)
        {
            _logger.Info(message);
        }
    }
}
