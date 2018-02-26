using Autofac.Extras.NLog;
using DFC.Digital.Data.Interfaces;
using System;

namespace DFC.Digital.Core.Logging
{
    public class DfcLogger : IApplicationLogger
    {
        private ILogger logService;

        public DfcLogger(ILogger logService)
        {
            this.logService = logService;
        }

        public void Error(string message, Exception ex)
        {
            if (ex is LoggedException)
            {
                throw ex;
            }
            else
            {
                logService.LogException(NLog.LogLevel.Error, message, ex);
                if (ex != null)
                {
                    throw new LoggedException($"Logged exception with message: {message}", ex);
                }
            }
        }

        public void ErrorJustLogIt(string message, Exception ex)
        {
            logService.LogException(NLog.LogLevel.Error, message, ex);
        }

        public void Info(string message)
        {
            logService.Info(message);
        }

        public void Trace(string message)
        {
            logService.Trace(message);
        }

        public void Warn(string message, Exception ex)
        {
            message = message ?? string.Empty;

            if (ex is LoggedException)
            {
                throw ex;
            }
            else
            {
                if (message.Contains("An exception of type 'DFC.Digital.Core.Logging.LoggedException'"))
                {
                    //This is an application exception of known type.
                    //This exception has already been logged by the application, hence can be ignored from sitefinity logs.
                }
                else
                {
                    logService.LogException(NLog.LogLevel.Warn, message, ex);
                    if (ex != null)
                    {
                        throw new LoggedException($"Logged exception with message: {message}", ex);
                    }
                }
            }
        }

        public void Warn(string message)
        {
            logService.Warn(message);
        }
    }
}