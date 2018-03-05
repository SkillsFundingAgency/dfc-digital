using System;

namespace DFC.Digital.Core
{
    public class TransientFaultHandlingStrategy
    {
        public int Retry => 2;

        public int AllowedFaults => 4;

        //Increasing the timeout strategy at the back of application need and timeout handling, further stories in backlog related to this.
        public TimeSpan Timeout => TimeSpan.FromSeconds(4);

        public TimeSpan Wait => TimeSpan.FromSeconds(2);

        public TimeSpan Breaktime => TimeSpan.FromSeconds(60);
    }
}