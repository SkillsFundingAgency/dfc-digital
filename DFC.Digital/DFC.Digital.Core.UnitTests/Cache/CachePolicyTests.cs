using Xunit;
using DFC.Digital.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FakeItEasy;

namespace DFC.Digital.Core.Tests
{
    public class CachePolicyTests
    {
        private IApplicationLogger fakeLogger;

        public CachePolicyTests()
        {
            fakeLogger = A.Fake<IApplicationLogger>();
        }

        [Fact()]
        public void ExecuteTest()
        {
            var cachePolicy = new CachePolicy(fakeLogger);
            var key = Guid.NewGuid();
            var result1 = cachePolicy.Execute(() => MethodToExecute(), CachePolicyType.AbsoluteContext, nameof(CachePolicyTests), key.ToString());
            var result2 = cachePolicy.Execute(() => MethodToExecute(), CachePolicyType.AbsoluteContext, nameof(CachePolicyTests), key.ToString());

            result1.Should().Be(result2);
        }

        private double MethodToExecute()
        {
            return new Random().NextDouble();
        }
    }
}