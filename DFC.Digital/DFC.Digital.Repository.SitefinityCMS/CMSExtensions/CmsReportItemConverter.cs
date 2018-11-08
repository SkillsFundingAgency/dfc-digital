using DFC.Digital.Data;
using DFC.Digital.Data.CMSExtension.Interfaces;
using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class CmsReportItemConverter : IDynamicModuleConverter<CmsReportItem>
    {
        private const string TitlePropertyName = "Title";
        private const string UrlNamePropertyName = "UrlName";
        private const string IdPropertyName = "Id";
        private const string LastModifiedByPropertyName = "LastModifiedBy";
        private const string LastModifiedPropertyName = "LastModified";
        private const string DateCreatedPropertyName = "DateCreated";
        private const string ApprovalWorkflowStatePropertyName = "ApprovalWorkflowState";
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IUserRepository userRepository;

        public CmsReportItemConverter(
            IDynamicContentExtensions dynamicContentExtensions,
            IUserRepository userRepository)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.userRepository = userRepository;
        }

        public CmsReportItem ConvertFrom(DynamicContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var reportItem = new CmsReportItem
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, TitlePropertyName),
                UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, UrlNamePropertyName),
                Id = dynamicContentExtensions.GetFieldValue<Guid>(content, IdPropertyName),
                LastModifiedBy = userRepository.GetUserNameById(dynamicContentExtensions.GetFieldValue<Guid>(content, LastModifiedByPropertyName)),
                LastModified = dynamicContentExtensions.GetFieldValue<DateTime>(content, LastModifiedPropertyName),
                DateCreated = dynamicContentExtensions.GetFieldValue<DateTime>(content, DateCreatedPropertyName),
                Status = GetContentStatus(dynamicContentExtensions.GetFieldValue<Lstring>(content, ApprovalWorkflowStatePropertyName))
            };

            return reportItem;
        }

        private WorkflowStatus GetContentStatus(string status)
        {
            switch (status.ToUpperInvariant())
            {
                case "PUBLISHED":
                    return WorkflowStatus.Published;
                case "UNPUBLISHED":
                    return WorkflowStatus.UnPublished;
                default:
                    return WorkflowStatus.Draft;
            }
        }
    }
}