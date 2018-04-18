using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Extensions
{
    public static class SitefinityDataExtensions
    {
        /// <summary>
        /// Gets the value of a DynamicContent item, after check whether the item exists
        /// </summary>
        /// <typeparam name="T">Type of the expected item</typeparam>
        /// <param name="item">The Dynamic Content item (e.g. FieldValidationSet, PageValidationLookup, etc.)</param>
        /// <param name="fieldName">The Name of the field we are querying</param>
        /// <returns>Returns the value of the DynamicContent item</returns>
        public static T GetValueOrDefault<T>(this DynamicContent item, string fieldName)
        {
            return item?.DoesFieldExist(fieldName) == true ? item.GetValue<T>(fieldName) : default(T);
        }
    }
}