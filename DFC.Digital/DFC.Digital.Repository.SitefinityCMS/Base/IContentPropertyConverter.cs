using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Base
{
    public interface IContentPropertyConverter<T>
    {
        T ConvertFrom(DynamicContent content);
    }
}
