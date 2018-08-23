using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
   public class SocSkillMatrixRepository : ISocSkillMatrixRepository
    {
        #region Fields
        private const string RelatedSkillField = "RelatedSkill";
        private const string RelatedSocField = "RelatedSoc";
        private const string UpdateComment = "Updated via the SkillsFramework import process";
        private const string UrlNameField = "UrlName";
        private readonly IDynamicModuleRepository<FrameworkSkill> frameworkSkillRepository;
        private readonly IDynamicModuleRepository<SocSkillMatrix> socMatrixRepository;
        private readonly IDynamicModuleRepository<SocCode> socCodeRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IDynamicModuleConverter<SocSkillMatrix> socSkillConverter;

        #endregion Fields

        #region Ctor
        public SocSkillMatrixRepository(IDynamicModuleRepository<FrameworkSkill> frameworkSkillRepository, IDynamicModuleRepository<SocSkillMatrix> socMatrixRepository, IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleRepository<SocCode> socCodeRepository, IDynamicModuleConverter<SocSkillMatrix> socSkillConverter)
        {
            this.frameworkSkillRepository = frameworkSkillRepository;
            this.socMatrixRepository = socMatrixRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.socCodeRepository = socCodeRepository;
            this.socSkillConverter = socSkillConverter;
        }

        #endregion

        #region Interface Implementations
        public void UpsertSocSkillMatrix(SocSkillMatrix socSkillMatrix)
        {
            var repoSocMatrix = socMatrixRepository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == socSkillMatrix.SfUrlName);
            if (repoSocMatrix != null)
            {
                var master = socMatrixRepository.GetMaster(repoSocMatrix);

                if (!string.IsNullOrWhiteSpace(socSkillMatrix.ONetElementId))
                {
                    dynamicContentExtensions.DeleteRelatedFieldValues(master, RelatedSkillField);
                    var relatedSkillItem = frameworkSkillRepository.Get(d =>
                        d.Status == ContentLifecycleStatus.Master &&
                        d.GetValue<string>(nameof(FrameworkSkill.ONetElementId)) == socSkillMatrix.ONetElementId);
                    if (relatedSkillItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(master, relatedSkillItem, RelatedSkillField);
                    }
                }

                if (!string.IsNullOrWhiteSpace(socSkillMatrix.SocCode))
                {
                    dynamicContentExtensions.DeleteRelatedFieldValues(master, RelatedSocField);
                    var relatedSocItem = socCodeRepository.Get(d =>
                        d.Status == ContentLifecycleStatus.Master &&
                        d.GetValue<string>(nameof(SocCode.SOCCode)) == socSkillMatrix.SocCode);
                    if (relatedSocItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(master, relatedSocItem, RelatedSocField);
                    }
                }

                // Save related on live version
                socMatrixRepository.Commit();

                var temp = socMatrixRepository.GetTemp(master);

                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.Title), socSkillMatrix.Title);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.Contextualised), socSkillMatrix.Contextualised);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.ONetAttributeType), socSkillMatrix.ONetAttributeType);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.ONetRank), socSkillMatrix.ONetRank);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.Rank), socSkillMatrix.Rank);

                var updatedMaster = socMatrixRepository.CheckinTemp(temp);

                socMatrixRepository.Publish(updatedMaster, UpdateComment);
                socMatrixRepository.Commit();
            }
            else
            {
                var newSocMatrix = socMatrixRepository.Create();
                dynamicContentExtensions.SetFieldValue(newSocMatrix, UrlNameField, (Lstring)socSkillMatrix.SfUrlName);
                socMatrixRepository.Add(newSocMatrix);

                socMatrixRepository.Commit();
                var newlyCreated = socMatrixRepository.Get(item =>
                    item.Visible && item.Status == ContentLifecycleStatus.Live &&
                    item.UrlName == socSkillMatrix.SfUrlName);

                if (newlyCreated != null)
                {
                    UpsertSocSkillMatrix(socSkillMatrix);
                }
            }
        }

        public IEnumerable<SocSkillMatrix> GetSocSkillMatrices()
        {
            var socSkillMatrices = socMatrixRepository.GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live).ToList();

            if (socSkillMatrices.Any())
            {
                return socSkillMatrices.Select(item => socSkillConverter.ConvertFrom(item));
            }

            return Enumerable.Empty<SocSkillMatrix>();
        }
        #endregion Interface Implementations
    }
}
