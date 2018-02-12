﻿using Castle.DynamicProxy;
using DFC.Digital.Core.Extensions;
using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.Digital.Core.Interceptors
{
    public class InstrumentationInterceptor : IInterceptor
    {
        public const string NAME = "Instrumentation";

        private IApplicationLogger loggingService;

        public InstrumentationInterceptor(IApplicationLogger logService)
        {
            this.loggingService = logService;
        }

        public void Intercept(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                bool shouldIgnoreInput = invocation.Method.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreInputInInterceptionAttribute));
                loggingService.Trace($"Async Func '{invocation.Method.Name}' called with parameters '{(shouldIgnoreInput ? "ignored" : JsonConvert.SerializeObject(invocation.Arguments))}'.");
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

        private void InterceptSync(IInvocation invocation)
        {
            bool shouldIgnoreInput = invocation.Method.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreInputInInterceptionAttribute));
            bool shouldIgnoreOutput = invocation.Method.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreOutputInInterceptionAttribute));
            loggingService.Trace($"Method '{invocation.Method.Name}' called with parameters '{(shouldIgnoreInput ? "ignored" : JsonConvert.SerializeObject(invocation.Arguments))}'.");
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                invocation.Proceed();
            }
            finally
            {
                loggingService.Trace($"Method '{invocation.Method.Name}' took '{watch.Elapsed}' to complete. And returned '{(shouldIgnoreOutput ? "ignored" : invocation.ReturnValueString())}'");
            }
        }

        private async Task InterceptAsyncAction(IInvocation invocation)
        {
            bool shouldIgnoreInput = invocation.Method.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreInputInInterceptionAttribute));
            bool shouldIgnoreOutput = invocation.Method.CustomAttributes.Any(a => a.AttributeType == typeof(IgnoreOutputInInterceptionAttribute));
            loggingService.Trace($"Async action '{invocation.Method.Name}' called with parameters '{(shouldIgnoreInput ? "ignored" : JsonConvert.SerializeObject(invocation.Arguments))}'.");
            Stopwatch watch = Stopwatch.StartNew();
            try
            {
                invocation.Proceed();
                if (invocation.ReturnValue is Task task)
                {
                    await task;
                }
            }
            finally
            {
                loggingService.Trace($"Async action '{invocation.Method.Name}' took '{watch.Elapsed}' to complete.");
            }
        }
    }
}