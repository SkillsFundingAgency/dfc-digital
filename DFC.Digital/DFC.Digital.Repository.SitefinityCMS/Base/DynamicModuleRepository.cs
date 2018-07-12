using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Versioning;

namespace DFC.Digital.Repository.SitefinityCMS
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public sealed class DynamicModuleRepository<T> : IDynamicModuleRepository<T>, IDisposable
    {
        private DynamicModuleManager dynamicModuleManager;

        private Type dynamicModuleContentType;

        private string providerName;

        #region NotImplemented

        public void Add(DynamicContent entity)
        {
            Add(entity, null);
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
            Publish(entity, null);
        }

        #endregion NotImplemented

        #region IRepository implementations

        public void Initialise(string contentType, string dynamicModuleName)
        {
            //GSR had to add this as we were getting not results returned on instances where the jobprofiles
            //modue had been added as an addon - think it adds it with a diffrent provider.
            providerName = DynamicModuleManager.GetDefaultProviderName(dynamicModuleName);
            dynamicModuleManager = DynamicModuleManager.GetManager(providerName);
            dynamicModuleContentType = TypeResolutionService.ResolveType(contentType);
        }

        public Type GetContentType()
        {
            return dynamicModuleContentType;
        }

        public string GetProviderName()
        {
            return providerName;
        }

        public DynamicContent Create()
        {
            return dynamicModuleManager.CreateDataItem(dynamicModuleContentType);
        }

        public void Add(DynamicContent entity, string changeComment)
        {
            entity.SetValue("IncludeInSitemap", false);
            entity.SetValue("Owner", SecurityManager.GetCurrentUserId());
            entity.SetValue("PublicationDate", DateTime.UtcNow);
            Publish(entity, changeComment);
        }

        public void Update(DynamicContent entity, string changeComment)
        {
            // Set a transaction name and get the version manager
            var transactionName = DateTime.Now.Ticks.ToString();
            var versionManager = VersionManager.GetManager(null, transactionName);
            CreateVersion(entity, changeComment, versionManager, "Draft");

            // Commit the transaction in order for the items to be actually persisted to data store
            TransactionManager.CommitTransaction(transactionName);
        }

        public void Publish(DynamicContent entity, string changeComment)
        {
            var transactionName = DateTime.Now.Ticks.ToString();
            var versionManager = VersionManager.GetManager(null, transactionName);

            // You need to set appropriate workflow status
            // Now the item is published and can be seen in the page
            CreateVersion(entity, changeComment, versionManager, "Published");

            // We can now call the following to publish the item
            dynamicModuleManager.Lifecycle.Publish(entity);

            // Commit the transaction in order for the items to be actually persisted to data store
            TransactionManager.CommitTransaction(transactionName);
        }

        public DynamicContent Checkout(string urlName)
        {
            var master = Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Master);
            return dynamicModuleManager.Lifecycle.CheckOut(master) as DynamicContent;
        }

        public DynamicContent Get(Expression<Func<DynamicContent, bool>> where)
        {
            return GetAll().FirstOrDefault(where);
        }

        public IQueryable<DynamicContent> GetAll()
        {
            return dynamicModuleManager.GetDataItems(dynamicModuleContentType);
        }

        public DynamicContent GetById(string id)
        {
            return dynamicModuleManager.GetDataItems(dynamicModuleContentType)
                .FirstOrDefault(item => item.Status == ContentLifecycleStatus.Live && item.Visible &&
                                        item.Id == new Guid(id));
        }

        public IQueryable<DynamicContent> GetMany(Expression<Func<DynamicContent, bool>> where)
        {
            return GetAll().Where(where);
        }

        public DynamicContent GetMaster(DynamicContent entity)
        {
            return dynamicModuleManager.Lifecycle.GetMaster(entity) as DynamicContent;
        }

        public DynamicContent GetTemp(DynamicContent entity)
        {
            return dynamicModuleManager.Lifecycle.CheckOut(entity) as DynamicContent;
        }

        public DynamicContent CheckinTemp(DynamicContent entity)
        {
           return dynamicModuleManager.Lifecycle.CheckIn(entity) as DynamicContent;
        }

        #endregion IRepository implementations

        public void Commit()
        {
            dynamicModuleManager.SaveChanges();
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
                if (dynamicModuleManager != null)
                {
                    dynamicModuleManager.Dispose();
                    dynamicModuleManager = null;
                }
            }
        }

        private void CreateVersion(DynamicContent entity, string changeComment, VersionManager versionManager, string status)
        {
            entity.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, status);

            // Create a version and commit the transaction in order changes to be persisted to data store
            var change = versionManager.CreateVersion(entity, false);
            if (changeComment != null)
            {
                change.Comment = changeComment;
            }
        }
    }
}