using DFC.Digital.Core;
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
        private const string IncludeInSitemapFieldName = "IncludeInSitemap";
        private const string OwnerFieldName = "Owner";
        private const string PublicationDateFieldName = "PublicationDate";
        private readonly IApplicationLogger applicationLogger;

        private DynamicModuleManager dynamicModuleManager;

        private Type dynamicModuleContentType;

        private string providerName;

        public DynamicModuleRepository(IApplicationLogger applicationLogger)
        {
            this.applicationLogger = applicationLogger;
        }

        #region NotImplemented

        public void Delete(DynamicContent entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<DynamicContent, bool>> where)
        {
            throw new NotImplementedException();
        }

        #endregion NotImplemented

        #region IRepository implementations

        public void Add(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Add(entity, null);
        }

        public void Update(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Update(entity, null);
        }

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
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.SetValue(IncludeInSitemapFieldName, false);
            entity.SetValue(OwnerFieldName, SecurityManager.GetCurrentUserId());
            entity.SetValue(PublicationDateFieldName, DateTime.UtcNow);
            Publish(entity, changeComment);
        }

        public void Update(DynamicContent entity, string changeComment)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Set a transaction name and get the version manager
            var transactionName = $"{entity.GetType().Name}-{DateTime.Now.Ticks}";

            applicationLogger.Info($"Updating entity with transaction name {transactionName} for {entity.UrlName}");
            var versionManager = VersionManager.GetManager(null, transactionName);
            CreateVersion(entity, changeComment, versionManager, WorkflowStatus.Draft);

            // Commit the transaction in order for the items to be actually persisted to data store
            TransactionManager.CommitTransaction(transactionName);
        }

        public void Publish(DynamicContent entity, string changeComment)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //CodeReview: Consider audit / log transaction name as well, might be an useful instrument for prd troubleshooting.
            var transactionName = $"{entity.GetType().Name}-{DateTime.Now.Ticks}";

            applicationLogger.Info($"Publishing entity with transaction name {transactionName} for {entity.UrlName}");

            var versionManager = VersionManager.GetManager(null, transactionName);

            // You need to set appropriate workflow status
            // Now the item is published and can be seen in the page
            CreateVersion(entity, changeComment, versionManager, WorkflowStatus.Published);

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
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return dynamicModuleManager.Lifecycle.GetMaster(entity) as DynamicContent;
        }

        public DynamicContent GetTemp(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return dynamicModuleManager.Lifecycle.CheckOut(entity) as DynamicContent;
        }

        public DynamicContent CheckinTemp(DynamicContent entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return dynamicModuleManager.Lifecycle.CheckIn(entity) as DynamicContent;
        }

        public bool IsCheckedOut(DynamicContent entity)
        {
            return entity.Status == ContentLifecycleStatus.Master ? dynamicModuleManager.Lifecycle.IsCheckedOut(entity) : dynamicModuleManager.Lifecycle.IsCheckedOut(dynamicModuleManager.Lifecycle.GetMaster(entity));
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

        private void CreateVersion(DynamicContent entity, string changeComment, VersionManager versionManager, WorkflowStatus status)
        {
            entity.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, status.ToString());

            var change = versionManager.CreateVersion(entity, status == WorkflowStatus.Published);
            if (changeComment != null)
            {
                change.Comment = changeComment;
            }
        }
    }
}