using DFC.Digital.Core;
using Microsoft.Extensions.Logging;
using System;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseSearchLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly IApplicationLogger applicationLogger;

        public CourseSearchLogger(IApplicationLogger applicationLogger)
        {
            this.applicationLogger = applicationLogger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return !applicationLogger.IsTraceDisabled();

                case LogLevel.Information:
                case LogLevel.Warning:
                case LogLevel.Error:
                case LogLevel.Critical:
                case LogLevel.None:
                default:
                    return true;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            switch (logLevel)
            {
                case LogLevel.Information:
                    applicationLogger.Info($"{nameof(CourseSearchLogger)}-EventId:{eventId}-Formatted:{formatter(state, exception)}");
                    break;

                case LogLevel.Warning:
                    applicationLogger.Warn($"{nameof(CourseSearchLogger)}-EventId:{eventId}-Formatted:{formatter(state, exception)}");
                    break;

                case LogLevel.Critical:
                case LogLevel.Error:
                    applicationLogger.ErrorJustLogIt($"{nameof(CourseSearchLogger)}-EventId:{eventId}-Formatted:{formatter(state, exception)}", exception);
                    break;

                case LogLevel.None:
                case LogLevel.Trace:
                case LogLevel.Debug:
                default:
                    applicationLogger.Trace($"{nameof(CourseSearchLogger)}-EventId:{eventId}-Formatted:{formatter(state, exception)}");
                    break;
            }
        }
    }
}