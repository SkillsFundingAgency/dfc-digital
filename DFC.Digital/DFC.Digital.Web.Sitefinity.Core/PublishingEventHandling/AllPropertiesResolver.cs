using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class AllPropertiesResolver : CamelCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            property.PropertyName = property.PropertyName.ToPascalCase();

            if (member.DeclaringType.Name.Equals("DynamicContent", StringComparison.OrdinalIgnoreCase))
            {
                return property;
            }

            property.Ignored = false;

            return property;
        }
    }
}