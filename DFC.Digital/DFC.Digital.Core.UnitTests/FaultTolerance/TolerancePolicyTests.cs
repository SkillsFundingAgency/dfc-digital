using Xunit;
using DFC.Digital.Core.FaultTolerance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using DFC.Digital.Data.Interfaces;
using Polly.CircuitBreaker;

namespace DFC.Digital.Core.FaultTolerance.Tests
{
    public class TolerancePolicyTests
    {
        [Fact]
        public async Task ExecuteWithCircuitBreakerTestAsync()
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var actor = new TolerancePolicy(fakeLogger);
            var result1 = await Assert.ThrowsAsync<BrokenCircuitException>(async () =>
            {
                int idx = 0;
                while (idx <= 4)
                {
                    try
                    {
                        idx++;
                        await actor.ExecuteWithCircuitBreaker(() => ThrowEx(), "test");
                    }
                    catch (NotImplementedException)
                    { }
                }
            });

            Assert.NotNull(result1);
            Assert.IsType<BrokenCircuitException>(result1);
        }

        private Task<string> ThrowEx()
        {
            throw new NotImplementedException();
        }
    }
}