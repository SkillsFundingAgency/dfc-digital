using System;
using System.Runtime.Serialization;

namespace DFC.Digital.Core.Logging
{
    [Serializable]
    public class LoggedException : Exception
    {
        public LoggedException()
        {
        }

        public LoggedException(string message) : base(message)
        {
        }

        public LoggedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LoggedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}