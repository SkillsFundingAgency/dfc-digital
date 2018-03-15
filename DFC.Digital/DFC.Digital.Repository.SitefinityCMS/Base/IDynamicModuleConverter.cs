using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IDynamicModuleConverter<T>
    {
        T ConvertFrom(DynamicContent content);

        //IQueryable<T> ConvertChildrenFrom(DynamicContent parentContent, string childrenType);
    }
}