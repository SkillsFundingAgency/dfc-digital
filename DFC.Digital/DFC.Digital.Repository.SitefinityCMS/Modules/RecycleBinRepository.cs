using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.RecycleBin;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RecycleBinRepository : IRecyleBinRepository, IDisposable
    {
        private const string ApprenticeVacancyDeleteTypeName = "Telerik.Sitefinity.DynamicTypes.Model.JobProfile.ApprenticeshipVacancy";
        private IRecycleBinManager recycleBinItemsManager;

        public RecycleBinRepository()
        {
            recycleBinItemsManager = RecycleBinManagerFactory.GetManager();
        }

        public void DeleteVacanciesPermanently(int itemCount)
        {
            SystemManager.RunWithElevatedPrivilege(d =>
            {
                var recycleBinItems = recycleBinItemsManager.GetRecycleBinItems().Where(di => di.DeletedItemTypeName.Equals(ApprenticeVacancyDeleteTypeName)).Take(itemCount).ToList();

                var providerName = DynamicModuleManager.GetDefaultProviderName(DynamicTypes.JobProfileModuleName);
                var dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
                var dynamicModuleContentType = TypeResolutionService.ResolveType(ApprenticeVacancyDeleteTypeName);
                foreach (var recycleBinItem in recycleBinItems)
                {
                    var dataItem =
                            dynamicModuleManager.GetItem(dynamicModuleContentType, recycleBinItem.DeletedItemId) as IRecyclableDataItem;
                    dynamicModuleManager.RecycleBin.PermanentlyDeleteFromRecycleBin(dataItem);
                    dynamicModuleManager.SaveChanges();
                }
            });

            recycleBinItemsManager.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (recycleBinItemsManager != null)
                {
                    recycleBinItemsManager.Dispose();
                    recycleBinItemsManager = null;
                }
            }
        }
    }
}