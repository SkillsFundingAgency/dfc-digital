using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class DigitalModelExtensions
    {
        public static string ToJson(this IDigitalDataModel model)
        {
            return JsonConvert.SerializeObject(model);
        }

        public static string ToJson(this IEnumerable<IDigitalDataModel> enumerableModel)
        {
            return JsonConvert.SerializeObject(enumerableModel.ToArray());
        }

        internal static string ToJson<T>(this DelegateResult<T> delegateResult)
        {
            var result = delegateResult.Result;
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