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

        public int Retry => configuration.GetConfig(RetryKey, 2);

        public int AllowedFaults => configuration.GetConfig(AllowedFaultsKey, 4);

        public TimeSpan Timeout => configuration.GetConfig(TimeoutKey, TimeSpan.FromSeconds(3));

        public TimeSpan Wait => configuration.GetConfig(WaitKey, TimeSpan.FromSeconds(2));

        public TimeSpan Breaktime => configuration.GetConfig(BreaktimeKey, TimeSpan.FromSeconds(60));
    }
}