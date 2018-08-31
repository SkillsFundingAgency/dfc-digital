using Castle.DynamicProxy;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DFC.Digital.Core.Interceptors
{
    public class InstrumentationInterceptor : IInterceptor
    {
        public const string Name = "Instrumentation";

        private static readonly MethodInfo HandleAsyncMethodInfo = typeof(InstrumentationInterceptor).GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
        private IApplicationLogger loggingService;

        public InstrumentationInterceptor(IApplicationLogger logService)
        {
            this.loggingService = logService;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
            {
                throw new System.ArgumentNullException(nameof(invocation));
            }

            if (loggingService.IsTraceDisabled())
            {
                invocation.Proceed();
            }
            else
            {
                HandleInterception(invocation);
            }
        }

        private void HandleInterception(IInvocation invocation)
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

        private void InterceptSync(IInvocation invocation)
        {
            bool shouldIgnoreInput = invocation.Method.IsDefined(typeof(IgnoreInputInInterceptionAttribute), true) | invocation.MethodInvocationTarget.IsDefined(typeof(IgnoreInputInInterceptionAttribute), true);
            bool shouldIgnoreOutput = invocation.Method.IsDefined(typeof(IgnoreOutputInInterceptionAttribute), true) | invocation.MethodInvocationTarget.IsDefined(typeof(IgnoreOutputInInterceptionAttribute), true);
            loggingService.Trace($"BEGIN: Method '{invocation.Method.Name}' called from '{invocation.TargetType.FullName}' with parameters '{(shouldIgnoreInput ? "ignored" : JsonConvert.SerializeObject(invocation.Arguments))}'.");
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                invocation.Proceed();
            }
            finally
            {
                loggingService.Trace($"END: Method '{invocation.Method.Name}' from '{invocation.TargetType.FullName}' took '{watch.Elapsed}' to complete. And returned '{(shouldIgnoreOutput ? "ignored" : invocation.ReturnValueString())}'");
            }
        }

        private async Task InterceptAsyncAction(IInvocation invocation)
        {
            bool shouldIgnoreInput = invocation.Method.IsDefined(typeof(IgnoreInputInInterceptionAttribute), true) | invocation.MethodInvocationTarget.IsDefined(typeof(IgnoreInputInInterceptionAttribute), true);
            bool shouldIgnoreOutput = invocation.Method.IsDefined(typeof(IgnoreOutputInInterceptionAttribute), true) | invocation.MethodInvocationTarget.IsDefined(typeof(IgnoreOutputInInterceptionAttribute), true);
            loggingService.Trace($"BEGIN: Async action '{invocation.Method.Name}' called from '{invocation.TargetType.FullName}' with parameters '{(shouldIgnoreInput ? "ignored" : JsonConvert.SerializeObject(invocation.Arguments))}'.");
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                invocation.Proceed();
                if (invocation.ReturnValue is Task task)
                {
                    await task.ConfigureAwait(false);
                }
            }
            finally
            {
                loggingService.Trace($"END: Async action '{invocation.Method.Name}' from '{invocation.TargetType.FullName}' took '{watch.Elapsed}' to complete.");
            }
        }

        private void InterceptAsyncFunc(IInvocation invocation)
        {
            bool shouldIgnoreInput = invocation.Method.IsDefined(typeof(IgnoreInputInInterceptionAttribute), true) | invocation.MethodInvocationTarget.IsDefined(typeof(IgnoreInputInInterceptionAttribute), true);
            loggingService.Trace($"BEGIN: Async Func '{invocation.Method.Name}' called from '{invocation.TargetType.FullName}' with parameters '{(shouldIgnoreInput ? "ignored" : JsonConvert.SerializeObject(invocation.Arguments))}'.");
            invocation.Proceed();
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = HandleAsyncMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new[] { invocation });
        }

        private async Task<T> HandleAsyncWithResult<T>(IInvocation invocation)
        {
            bool shouldIgnoreOutput = invocation.Method.IsDefined(typeof(IgnoreOutputInInterceptionAttribute), true) | invocation.MethodInvocationTarget.IsDefined(typeof(IgnoreOutputInInterceptionAttribute), true);
            T result = default;
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                result = await ((Task<T>)invocation.ReturnValue).ConfigureAwait(false);
            }
            finally
            {
                // other exception policies as we go along.
                loggingService.Trace($"END: Async Func '{invocation.Method.Name}' from '{invocation.TargetType.FullName}' took '{watch.Elapsed}' to complete. And returned '{(shouldIgnoreOutput ? "ignored" : JsonConvert.SerializeObject(result))}'");
            }

            return result;
        }
    }
}