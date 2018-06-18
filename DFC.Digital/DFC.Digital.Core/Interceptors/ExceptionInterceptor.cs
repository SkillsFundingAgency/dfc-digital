using Castle.DynamicProxy;
using DFC.Digital.Core.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DFC.Digital.Core.Interceptors
{
    public class ExceptionInterceptor : IInterceptor
    {
        public const string Name = "ExceptionPolicy";

        private static readonly MethodInfo HandleAsyncMethodInfo = typeof(ExceptionInterceptor).GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
        private IApplicationLogger loggingService;

        public ExceptionInterceptor(IApplicationLogger logService)
        {
            this.loggingService = logService;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation != null)
            {
                try
                {
                    var returnType = invocation.Method.ReturnType;
                    if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        InterceptAsyncFunc(invocation);
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
        }

        private async Task InterceptAsyncAction(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
                if (invocation.ReturnValue is Task task)
                {
                    await task.ConfigureAwait(false);
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

        private void InterceptAsyncFunc(IInvocation invocation)
        {
            invocation.Proceed();
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = HandleAsyncMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new[] { invocation });
        }

        private async Task<T> HandleAsyncWithResult<T>(IInvocation invocation)
        {
            try
            {
                return await ((Task<T>)invocation.ReturnValue).ConfigureAwait(false);
            }
            catch (LoggedException)
            {
                // Ignore already logged exceptions.
                // We would loose the stack trace to the callee is that an issue?
                throw;
            }
            catch (Exception ex)
            {
                // other exception policies as we go along.
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