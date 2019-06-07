using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class LstringJsonConverter : JsonConverter
    {
        private static readonly Type LstringType = typeof(Lstring);

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var typedValue = (Lstring)value;

            writer.WriteValue(typedValue.Value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == LstringType;
        }
    }
}
