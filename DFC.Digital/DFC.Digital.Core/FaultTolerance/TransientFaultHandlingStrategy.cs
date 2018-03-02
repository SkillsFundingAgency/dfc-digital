using System;

namespace DFC.Digital.Core
{
    public class TransientFaultHandlingStrategy
    {
        private readonly IConfigurationProvider configuration;

        public TransientFaultHandlingStrategy(IConfigurationProvider configuration)
        {
            this.configuration = configuration;
        }

        public int Retry => configuration.Get($"{nameof(TransientFaultHandlingStrategy)}.{nameof(Retry)}", 2);

        public int AllowedFaults => configuration.Get($"{nameof(TransientFaultHandlingStrategy)}.{nameof(AllowedFaults)}", 4);

        public TimeSpan Timeout => configuration.Get($"{nameof(TransientFaultHandlingStrategy)}.{nameof(Timeout)}", TimeSpan.FromSeconds(3));

        public TimeSpan Wait => configuration.Get($"{nameof(TransientFaultHandlingStrategy)}.{nameof(Wait)}", TimeSpan.FromSeconds(2));

        public TimeSpan Breaktime => configuration.Get($"{nameof(TransientFaultHandlingStrategy)}.{nameof(Breaktime)}", TimeSpan.FromSeconds(60));
    }
}