using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.RecycleBin;

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

        public void DeleteVacanciesPermanently()
        {
            if (recycleBinItemsManager == null)
            {
                recycleBinItemsManager = RecycleBinManagerFactory.GetManager();
            }

           var recycleBinItems = recycleBinItemsManager.GetRecycleBinItems().ToList();

            foreach (var recycleBinItem in recycleBinItems.Where(di => di.DeletedItemTypeName.Equals(ApprenticeVacancyDeleteTypeName)))
            {
                recycleBinItemsManager.Delete(recycleBinItem);
            }

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