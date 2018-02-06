using DFC.Digital.Data.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Core.Extensions
{
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
    }
}