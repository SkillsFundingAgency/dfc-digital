using Castle.DynamicProxy;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Core.Interceptors
{
    public class ExceptionInterceptor : IInterceptor
    {
        public const string NAME = "ExceptionPolicy";

        private IApplicationLogger loggingService;

        public ExceptionInterceptor(IApplicationLogger logService)
        {
            this.loggingService = logService;
        }

        public void Intercept(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                invocation.Proceed();
            }
            else if (returnType == typeof(Task))
            {
                invocation.ReturnValue = InterceptAsyncAction(invocation);
            }
            else
            {
                InterceptSync(invocation);
            }
        }

        private async Task InterceptAsyncAction(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
                if (invocation.ReturnValue is Task task)
                {
                    await task;
                }
            }
            catch (LoggedException)
            {
                // Ignore already logged exceptions.
                // We would loose the stack trace to the callee is that an issue?
                throw;
            }

            // other exception policies as we go along.
            catch (Exception ex)
            {
                loggingService.Error($"Async Method '{invocation.Method.Name}' called from '{invocation.TargetType.FullName}' with parameters '{string.Join(", ", invocation.Arguments.Select(a => (a ?? string.Empty).ToString()).ToArray())}' failed with exception.", ex);
                throw;
            }
        }

        private async Task<TResult> InterceptAsyncFunc<TResult>(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
                return await (Task<TResult>)invocation.ReturnValue;
            }
            catch (LoggedException)
            {
                // Ignore already logged exceptions.
                // We would loose the stack trace to the callee is that an issue?
                throw;
            }

            // other exception policies as we go along.
            catch (Exception ex)
            {
                loggingService.Error($"Async Method '{invocation.Method.Name}' called from '{invocation.TargetType.FullName}' with parameters '{string.Join(", ", invocation.Arguments.Select(a => (a ?? string.Empty).ToString()).ToArray())}' failed with exception.", ex);
                throw;
            }
        }

        private void InterceptSync(IInvocation invocation)
        {
            try
            {
                invocation?.Proceed();
            }
            catch (LoggedException)
            {
                // Ignore already logged exceptions.
                // We would loose the stack trace to the callee is that an issue?
                throw;
            }

            // other exception policies as we go along.
            catch (Exception ex)
            {
                loggingService.Error($"Method '{invocation.Method.Name}' called from '{invocation.TargetType.FullName}' with parameters '{string.Join(", ", invocation.Arguments.Select(a => (a ?? string.Empty).ToString()).ToArray())}' failed with exception.", ex);
                throw;
            }
        }
    }
}