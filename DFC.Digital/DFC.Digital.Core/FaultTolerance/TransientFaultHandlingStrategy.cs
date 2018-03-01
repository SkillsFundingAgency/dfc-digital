using System;

namespace DFC.Digital.Core
{
    public class TransientFaultHandlingStrategy
    {
        public int Retry => 2;

        public int AllowedFaults => 4;

        public TimeSpan Timeout => TimeSpan.FromSeconds(2);

        public TimeSpan Wait => TimeSpan.FromSeconds(2);

        public TimeSpan Breaktime => TimeSpan.FromSeconds(60);
    }
}