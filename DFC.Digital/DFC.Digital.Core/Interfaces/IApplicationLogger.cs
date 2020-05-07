using System;

namespace DFC.Digital.Core
{
    public interface IApplicationLogger
    {
        void Trace(string message);

        void Info(string message);

        void Warn(string message, Exception ex);

        void Warn(string message);

        void Error(string message, Exception ex);

        void Error(string message);

        void ErrorJustLogIt(string message, Exception ex);

        bool IsTraceDisabled();
    }
}