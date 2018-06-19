using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IContentPropertyConverter<T>
    {
        T ConvertFrom(DynamicContent content);
    }
}
