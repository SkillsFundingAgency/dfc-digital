using System;

namespace DFC.Digital.Core
{
    public class TransientFaultHandlingStrategy
    {
        public static readonly string RetryKey = $"{nameof(TransientFaultHandlingStrategy)}.{nameof(Retry)}";
        public static readonly string AllowedFaultsKey = $"{nameof(TransientFaultHandlingStrategy)}.{nameof(AllowedFaults)}";
        public static readonly string TimeoutKey = $"{nameof(TransientFaultHandlingStrategy)}.{nameof(Timeout)}";
        public static readonly string WaitKey = $"{nameof(TransientFaultHandlingStrategy)}.{nameof(Wait)}";
        public static readonly string BreaktimeKey = $"{nameof(TransientFaultHandlingStrategy)}.{nameof(Breaktime)}";

        private readonly IConfigurationProvider configuration;

        public TransientFaultHandlingStrategy(IConfigurationProvider configuration)
        {
            this.configuration = configuration;
        }

        public int Retry => configuration.Get(RetryKey, 2);

        public int AllowedFaults => configuration.Get(AllowedFaultsKey, 4);

        public TimeSpan Timeout => configuration.Get(TimeoutKey, TimeSpan.FromSeconds(3));

        public TimeSpan Wait => configuration.Get(WaitKey, TimeSpan.FromSeconds(2));

        public TimeSpan Breaktime => configuration.Get(BreaktimeKey, TimeSpan.FromSeconds(60));
    }
}