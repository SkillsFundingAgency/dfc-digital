using System;
using System.Reflection;

namespace DFC.Digital.Web.Sitefinity.BreadCrumbs
{
  
	public class ResourceHelper
	{
		public static string GetResourceLookup(Type resourceType, string resourceName)
		{
			if ((resourceType != null) && (resourceName != null))
			{
				var property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
				if (property == null)
				{
					return resourceName;
				}
				if (property.PropertyType != typeof(string))
				{
					return resourceName;
				}

				return (string)property.GetValue(null, null);
			}
         
            return resourceName;
        }
    }
}
