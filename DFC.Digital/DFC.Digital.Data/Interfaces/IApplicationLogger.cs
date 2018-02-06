using System;

namespace DFC.Digital.Data.Interfaces
{
    public interface IApplicationLogger
    {
        void Trace(string message);

        void Info(string message);

        void Warn(string message, Exception ex);

        void Warn(string message);

        void Error(string message, Exception ex);

        void ErrorJustLogIt(string message, Exception ex);
    }
}