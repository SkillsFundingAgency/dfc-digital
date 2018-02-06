using System;
using DFC.Digital.Data.Interfaces;
using NLog;

namespace DFC.Digital.Service.Logger
{
    public class Log : ILoggingService
    {
        private ILogger logger;

        public Log(ILogger logger)
        {
            this.logger = logger;
        }
        public void LogError(string error, Exception ex)
        {
            this.logger.Error(ex, error);
        }

        public void LogInfo(string information)
        {

        }

        public void LogVerbose (string verbose)
        {

        }
    }
}
