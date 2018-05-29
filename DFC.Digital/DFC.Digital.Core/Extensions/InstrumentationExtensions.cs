using Castle.DynamicProxy;
using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class InstrumentationExtensions
    {
        internal static string ReturnValueString(this IInvocation invocation)
        {
            var result = invocation.ReturnValue;
            if (result == null)
            {
                return "Null";
            }

            if (result is IDigitalDataModel model)
            {
                return model.ToJson();
            }
            else if (result is IEnumerable<IDigitalDataModel> enumerableModel)
            {
                return enumerableModel.ToJson();
            }
            else
            {
                try
                {
                    return JsonConvert.SerializeObject(result);
                }
                catch (Exception ex)
                {
                    return $"Failed to serialise '{result}' with exception: '{ex.Message}'";
                }
            }
        }
    }
}