using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Polly.CircuitBreaker;
using Polly.Timeout;
using Polly.Utilities;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Core.Tests
{
    public class TolerancePolicyTests
    {
        private const int Precision = 1000;

        [Theory]
        [InlineData("timeout", FaultToleranceType.Timeout)]
        [InlineData("Retry", FaultToleranceType.Retry)]
        [InlineData("WaitRetry", FaultToleranceType.WaitRetry)]
        [InlineData("CircuitBreaker", FaultToleranceType.CircuitBreaker)]
        [InlineData("RetryWithCircuitBreaker", FaultToleranceType.RetryWithCircuitBreaker)]
        public void ExecuteTest(string dependencyName, FaultToleranceType toleranceType)
        {
            //Assign
            var strategy = new TransientFaultHandlingStrategy();
            var fakeLogger = A.Fake<IApplicationLogger>();

            //Act
            var actor = new TolerancePolicy(fakeLogger, strategy);

            //Assert
            switch (toleranceType)
            {
                case FaultToleranceType.Timeout:
                    Action result1 = () => actor.Execute(() =>
                    {
                        SystemClock.Sleep(TimeSpan.FromSeconds(strategy.Timeout.Seconds + 1), CancellationToken.None);
                        return dependencyName;
                    },
                    dependencyName,
                    toleranceType);

                    result1.Should().Throw<TimeoutRejectedException>();
                    break;

                case FaultToleranceType.Retry:
                    var executedNumberOfTimes = 0;
                    Action result2 = () => actor.Execute(() =>
                    {
                        executedNumberOfTimes++;
                        return ThrowEx();
                    },
                    dependencyName,
                    toleranceType);

                    result2.Should().Throw<NotImplementedException>();
                    executedNumberOfTimes.Should().Be(strategy.Retry + 1);
                    break;

                case FaultToleranceType.WaitRetry:
                    TimeSpan delayResult = default;
                    var exNumberOfTimes = 0;
                    Action resultWait = () =>
                    {
                        var delay = Stopwatch.StartNew();
                        try
                        {
                            actor.Execute(
                            () =>
                            {
                                exNumberOfTimes++;
                                return ThrowEx();
                            },
                            dependencyName,
                            toleranceType);
                        }
                        finally
                        {
                            delay.Stop();
                            delayResult = delay.Elapsed;
                        }
                    };

                    resultWait.Should().Throw<NotImplementedException>();
                    exNumberOfTimes.Should().Be(strategy.Retry + 1);
                    delayResult.Should().BeCloseTo(TimeSpan.FromSeconds(strategy.Wait.Seconds * strategy.Retry), Precision);
                    break;

                case FaultToleranceType.CircuitBreaker:
                    Action result3 = () =>
                    {
                        int idx = 0;
                        while (idx++ <= strategy.AllowedFaults)
                        {
                            try
                            {
                                actor.Execute(() => ThrowEx(), dependencyName, toleranceType);
                            }
                            catch (NotImplementedException)
                            { }
                        }
                    };

                    result3.Should().Throw<BrokenCircuitException>();
                    break;

                case FaultToleranceType.RetryWithCircuitBreaker:
                    var retriedBeforeBreakingCircuit = 0;
                    Action result4 = () =>
                    {
                        int idx = 0;
                        while (idx++ <= strategy.AllowedFaults)
                        {
                            try
                            {
                                actor.Execute(() =>
                                {
                                    retriedBeforeBreakingCircuit++;
                                    return ThrowEx();
                                },
                                dependencyName,
                                toleranceType);
                            }
                            catch (NotImplementedException)
                            { }
                        }
                    };

                    result4.Should().Throw<BrokenCircuitException>();
                    retriedBeforeBreakingCircuit.Should().Be(strategy.AllowedFaults * (strategy.Retry + 1));
                    break;

                default:
                    Assert.False(true, "Missing implementation");
                    break;
            }
        }

        [Theory]
        [InlineData("timeout", FaultToleranceType.Timeout)]
        [InlineData("Retry", FaultToleranceType.Retry)]
        [InlineData("WaitRetry", FaultToleranceType.WaitRetry)]
        [InlineData("CircuitBreaker", FaultToleranceType.CircuitBreaker)]
        [InlineData("RetryWithCircuitBreaker", FaultToleranceType.RetryWithCircuitBreaker)]
        public void ExecuteAsyncTest(string dependencyName, FaultToleranceType toleranceType)
        {
            //Assign
            var strategy = new TransientFaultHandlingStrategy();
            var fakeLogger = A.Fake<IApplicationLogger>();

            //Act
            var actor = new TolerancePolicy(fakeLogger, strategy);

            //Assert
            switch (toleranceType)
            {
                case FaultToleranceType.Timeout:
                    Func<Task> result1 = async () => await actor.ExecuteAsync(() => SystemClock.SleepAsync(TimeSpan.FromSeconds(strategy.Timeout.Seconds + 1), CancellationToken.None).ContinueWith(_ => dependencyName), dependencyName, toleranceType);
                    result1.Awaiting(async a => await a()).Should().Throw<TimeoutRejectedException>();
                    break;

                case FaultToleranceType.Retry:
                    var executedNumberOfTimes = 0;
                    Func<Task> result2 = async () => await actor.ExecuteAsync(() =>
                    {
                        executedNumberOfTimes++;
                        return ThrowEx();
                    },
                    dependencyName,
                    toleranceType);

                    result2.Awaiting(async a => await a()).Should().Throw<NotImplementedException>();
                    executedNumberOfTimes.Should().Be(strategy.Retry + 1);
                    break;

                case FaultToleranceType.CircuitBreaker:
                    Func<Task> result3 = async () =>
                    {
                        int idx = 0;
                        while (idx++ <= strategy.AllowedFaults)
                        {
                            try
                            {
                                await actor.ExecuteAsync(() => ThrowEx(), dependencyName, toleranceType);
                            }
                            catch (NotImplementedException)
                            { }
                        }
                    };

                    result3.Awaiting(async a => await a()).Should().Throw<BrokenCircuitException>();
                    break;

                case FaultToleranceType.RetryWithCircuitBreaker:
                    var retriedBeforeBreakingCircuit = 0;
                    Func<Task> result4 = async () =>
                    {
                        int idx = 0;
                        while (idx++ <= strategy.AllowedFaults)
                        {
                            try
                            {
                                await actor.ExecuteAsync(() =>
                                {
                                    retriedBeforeBreakingCircuit++;
                                    return ThrowEx();
                                },
                                dependencyName,
                                toleranceType);
                            }
                            catch (NotImplementedException)
                            { }
                        }
                    };

                    result4.Awaiting(async a => await a()).Should().Throw<BrokenCircuitException>();
                    retriedBeforeBreakingCircuit.Should().Be(strategy.AllowedFaults * (strategy.Retry + 1));
                    break;

                case FaultToleranceType.WaitRetry:
                    var exNumberOfTimes = 0;
                    Func<Task> result5 = async () =>
                    {
                        await actor.ExecuteAsync(
                        () =>
                        {
                            exNumberOfTimes++;
                            return ThrowEx();
                        },
                        dependencyName,
                        toleranceType);
                    };

                    result5.Awaiting(async a => await a()).Should().Throw<NotImplementedException>();
                    exNumberOfTimes.Should().Be(strategy.Retry + 1);
                    break;

                default:
                    Assert.False(true, "Missing implementation");
                    break;
            }
        }

        [Fact]
        public void ExecuteTestWithPredicate()
        {
            //Assign
            var dependencyName = "test";
            var toleranceType = FaultToleranceType.WaitRetry;
            var strategy = new TransientFaultHandlingStrategy();
            var fakeLogger = A.Fake<IApplicationLogger>();

            //Act
            var actor = new TolerancePolicy(fakeLogger, strategy);

            //Assert
            var executedNumberOfTimes = 0;
            Func<Task> result2 = async () =>
            {
                await actor.ExecuteAsync(
                () => Task.FromResult(executedNumberOfTimes++),
                a => a < strategy.Retry,
                dependencyName,
                toleranceType);
            };

            result2.Awaiting(async a => await a()).Should().NotThrow();
            executedNumberOfTimes.Should().Be(strategy.Retry + 1);
        }

        private Task<string> ThrowEx()
        {
            throw new NotImplementedException();
        }
    }
}