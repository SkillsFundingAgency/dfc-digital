using DFC.Digital.Repository.SitefinityCMS.Modules;
using System;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Lifecycle;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.RelatedData;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Versioning;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public sealed class DynamicModuleRepository<T> : IDynamicModuleRepository<T>, IDisposable
    {
        private DynamicModuleManager dynamicModuleManager;

        private Type dynamicModuleContentType;

        private string providerName;

        #region NotImplemented

        public void Delete(DynamicContent entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Expression<Func<DynamicContent, bool>> where)
        {
            throw new NotImplementedException();
        }

        public void Update(DynamicContent entity, string changeComment, bool publish = false)
        {
            // Set a transaction name and get the version manager
            var transactionName = DateTime.Now.Ticks.ToString();

            var versionManager = VersionManager.GetManager(null, transactionName);

            ILifecycleDataItem masterEntity = dynamicModuleManager.Lifecycle.GetMaster(entity);

            // Then we check it out
            // To get a temp version of the item, check out the master version. This is the version you must modify.

            // We can now modifiy any values of the item
            foreach (var propertyMapping in propertyMappings)
            {
                checkOutEntityItemTEMP.SetValue(propertyMapping.Key, bauJobProfile.GetPropertyValue(propertyMapping.Value));
            }

            // You need to set appropriate workflow status
            if (publish)
            {
                checkOutEntityItemTEMP.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Published");
            }
            else
            {
                checkOutEntityItemTEMP.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Draft");
            }

            // Check in the temp version to transfer the changes from TEMP to MASTER version.
            ILifecycleDataItem checkInEntityItem = dynamicModuleManager.Lifecycle.CheckIn(checkOutEntityItemTEMP);

            if (publish)
            {
                //Publish the master version to transfer the changes to the live version.
                dynamicModuleManager.Lifecycle.Publish(checkInEntityItem);

                // Create a version with a comment
                var change = versionManager.CreateVersion(checkInEntityItem, true);
                change.Comment = changeComment;
            }
            else
            {
                // Create a version with a comment
                var change = versionManager.CreateVersion(checkInEntityItem, false);
                change.Comment = changeComment;
            }

            dynamicModuleManager.SaveChanges();

            // commit the transaction in order changes to be persisted to data store
            TransactionManager.CommitTransaction(transactionName);
        }

        public string UpdateRelatedCareers(DynamicContent entity, JobProfileImporting bauJobProfile, string changeComment, bool enforcePublishing = false)
        {
            string actionTaken = string.Empty;

            // Set a transaction name and get the version manager
            var transactionName = DateTime.Now.Ticks.ToString();

            var versionManager = VersionManager.GetManager(null, transactionName);

            ILifecycleDataItem masterEntity = dynamicModuleManager.Lifecycle.GetMaster(entity);

            // To get a temp version of the item, check out the master version. This is the version you must modify.
            DynamicContent checkOutEntityItemTEMP = dynamicModuleManager.Lifecycle.CheckOut(masterEntity) as DynamicContent;

            // We can now modifiy any values of the item
            foreach (var relatedCareer in bauJobProfile.RelatedCareers)
            {
                actionTaken += "&nbsp;&nbsp;&nbsp; - Related JobProfile - '" + relatedCareer.Title + "' ( " + relatedCareer.UrlName + " )"; // does not exist in BETA and cannot be updated.<br />";

                var betaRelatedCareerMaster = Get(item => item.UrlName == relatedCareer.UrlName && item.Status == ContentLifecycleStatus.Master);

                if (betaRelatedCareerMaster == null)
                {
                    actionTaken += " does NOT exist in BETA and cannot be updated.<br />";
                }
                else
                {
                    checkOutEntityItemTEMP.CreateRelation(betaRelatedCareerMaster, "RelatedCareerProfiles");
                    actionTaken += " was updated in BETA.<br />";
                }
            }

            // You need to set appropriate workflow status
            if (enforcePublishing)
            {
                checkOutEntityItemTEMP.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Published");
            }
            else
            {
                checkOutEntityItemTEMP.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Draft");
            }

            // Check in the temp version to transfer the changes from TEMP to MASTER version.
            ILifecycleDataItem checkInEntityItem = dynamicModuleManager.Lifecycle.CheckIn(checkOutEntityItemTEMP);

            if (enforcePublishing)
            {
                // Publish the master version to transfer the changes to the live version.
                dynamicModuleManager.Lifecycle.Publish(checkInEntityItem);

                // Create a version with a comment
                var change = versionManager.CreateVersion(checkInEntityItem, true);
                change.Comment = changeComment;
            }
            else
            {
                // Create a version with a comment
                var change = versionManager.CreateVersion(checkInEntityItem, false);
                change.Comment = changeComment;
            }

            dynamicModuleManager.SaveChanges();

            // commit the transaction in order changes to be persisted to data store
            TransactionManager.CommitTransaction(transactionName);

            return actionTaken;
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

        public DynamicContent Create()
        {
            return dynamicModuleManager.CreateDataItem(dynamicModuleContentType);
        }

        public void Add(DynamicContent entity)
        {
            Add(entity, null);
        }

        public void Add(DynamicContent entity, string comment)
        {
            entity.SetValue("IncludeInSitemap", false);
            entity.SetValue("Owner", SecurityManager.GetCurrentUserId());
            entity.SetValue("PublicationDate", DateTime.UtcNow);
            Update(entity, comment);
        }

        public DynamicContent Checkout(string urlName)
        {
            var master = Get(item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Master);
            return dynamicModuleManager.Lifecycle.CheckOut(master) as DynamicContent;
        }

        public void Update(DynamicContent entity)
        {
            Update(entity, null);
        }

        public void Update(DynamicContent entity, string comment)
        {
            // Set a transaction name and get the version manager
            var transactionName = DateTime.Now.Ticks.ToString();
            var versionManager = VersionManager.GetManager(null, transactionName);
            CreateVersion(entity, comment, versionManager, "Draft");

            // Commit the transaction in order for the items to be actually persisted to data store
            TransactionManager.CommitTransaction(transactionName);
        }

        private void Publish(DynamicContent entity, string changeComment)
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

        #endregion IRepository implementations

        #region IUnitOfWork

        public void Commit()
        {
            dynamicModuleManager.SaveChanges();
        }

        #endregion IUnitOfWork

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable

        #region Private helpers

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

        #endregion Private helpers
    }
}