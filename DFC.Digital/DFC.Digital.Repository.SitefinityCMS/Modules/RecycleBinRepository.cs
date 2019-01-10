using DFC.Digital.Data.Interfaces;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.RecycleBin;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RecycleBinRepository : IRecyleBinRepository
    {
        private const string ApprenticeVacancyDeleteTypeName = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.ApprenticeshipVacancy";

        public void DeleteVacanciesPermanently(int itemCount)
        {
            var providerName = DynamicModuleManager.GetDefaultProviderName(DynamicTypes.JobProfileModuleName);
            var dynamicModuleContentType = TypeResolutionService.ResolveType(ApprenticeVacancyDeleteTypeName);
            using (var recycleBinItemsManager = RecycleBinManagerFactory.GetManager())
            {
                SystemManager.RunWithElevatedPrivilege(d =>
                {
                    var recycleBinItems = recycleBinItemsManager
                                            .GetRecycleBinItems()
                                            .Where(di => di.DeletedItemTypeName.Equals(ApprenticeVacancyDeleteTypeName))
                                            .Take(itemCount)
                                            .Select(r => r.DeletedItemId);

                    using (var dynamicModuleManager = DynamicModuleManager.GetManager(providerName))
                    {
                        foreach (var recycleBinItemId in recycleBinItems)
                        {
                            var dataItem = dynamicModuleManager.GetItem(dynamicModuleContentType, recycleBinItemId) as IRecyclableDataItem;
                            dynamicModuleManager.RecycleBin.PermanentlyDeleteFromRecycleBin(dataItem);
                        }

                        dynamicModuleManager.SaveChanges();
                    }
                });

                recycleBinItemsManager.SaveChanges();
            }
        }
    }
}