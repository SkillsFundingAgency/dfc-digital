using DFC.Digital.Data.Interfaces;
using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public abstract class DynamicModuleRepository : IRepository<DynamicContent>, IDisposable
    {
        private DynamicModuleManager dynamicModuleManager;

        protected DynamicModuleRepository(string dynamicModuleName)
        {
            //GSR had to add this as we were getting not results returned on instances where the jobprofiles
            //modue had been added as an addon - think it adds it with a diffrent provider.
            ProviderName = DynamicModuleManager.GetDefaultProviderName(dynamicModuleName);
            dynamicModuleManager = DynamicModuleManager.GetManager(ProviderName);
        }

        protected DynamicModuleRepository(string dynamicModuleName, string contentType) : this(dynamicModuleName)
        {
            SetContentType(contentType);
        }

        protected Type DynamicModuleContentType { get; set; }

        protected string ProviderName { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public DynamicContent Get(Expression<Func<DynamicContent, bool>> where)
        {
            return GetAll().FirstOrDefault(where);
        }

        public IQueryable<DynamicContent> GetAll()
        {
            return dynamicModuleManager.GetDataItems(DynamicModuleContentType);
        }

        public DynamicContent GetById(string id)
        {
            return dynamicModuleManager.GetDataItems(DynamicModuleContentType)
                .FirstOrDefault(item => item.Status == ContentLifecycleStatus.Live && item.Visible &&
                                        item.Id == new Guid(id));
        }

        public IQueryable<DynamicContent> GetMany(Expression<Func<DynamicContent, bool>> where)
        {
            return GetAll().Where(where);
        }

        #region NotImplemented

        public void Add(DynamicContent entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DynamicContent entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<DynamicContent, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Update(DynamicContent entity)
        {
            throw new NotImplementedException();
        }

        #endregion NotImplemented

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (dynamicModuleManager != null)
                {
                    dynamicModuleManager.Dispose();
                    dynamicModuleManager = null;
                }
            }
        }

        protected virtual void Initialise()
        {
        }

        protected void SetContentType(string contentType)
        {
            DynamicModuleContentType = TypeResolutionService.ResolveType(contentType);
        }
    }
}