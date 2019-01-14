using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System.Linq;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.RecycleBin;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.SitefinityExceptions;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RecycleBinRepository : IRecycleBinRepository
    {
        private const string ApprenticeVacancyDeleteTypeName = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.ApprenticeshipVacancy";
        private readonly IApplicationLogger applicationLogger;

        public RecycleBinRepository(IApplicationLogger applicationLogger)
        {
            this.applicationLogger = applicationLogger;
        }

        public bool DeleteVacanciesPermanently(int itemCount)
        {
            var lastDeleteCall = false;
            using (var recycleBinItemsManager = RecycleBinManagerFactory.GetManager())
            {
                SystemManager.RunWithElevatedPrivilege(d =>
                {
                    var recycleBinItems = recycleBinItemsManager.GetRecycleBinItems()
                        .Where(di => di.DeletedItemTypeName.Equals(ApprenticeVacancyDeleteTypeName)).Take(itemCount).ToList();
                    var providerName = DynamicModuleManager.GetDefaultProviderName(DynamicTypes.JobProfileModuleName);

                    lastDeleteCall = itemCount > recycleBinItems.Count;

                    using (var dynamicModuleManager = DynamicModuleManager.GetManager(providerName))
                    {
                        var dynamicModuleContentType = TypeResolutionService.ResolveType(ApprenticeVacancyDeleteTypeName);
                        foreach (var recycleBinItem in recycleBinItems)
                        {
                            try
                            {
                                var dataItem =
                                    dynamicModuleManager.GetItem(dynamicModuleContentType, recycleBinItem.DeletedItemId) as
                                        IRecyclableDataItem;
                                dynamicModuleManager.RecycleBin.PermanentlyDeleteFromRecycleBin(dataItem);
                            }
                            catch (ItemNotFoundException exception)
                            {
                                recycleBinItemsManager.Delete(recycleBinItem);
                                applicationLogger.Info($"Could not delete item :  {recycleBinItem.DeletedItemTitle}, failed with error {exception.Message}");
                                recycleBinItemsManager.SaveChanges();
                            }
                        }

                        dynamicModuleManager.SaveChanges();
                    }
                });
            }

            return lastDeleteCall;
        }
    }
}