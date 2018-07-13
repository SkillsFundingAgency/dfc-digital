using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
   public class OnetRepository : IOnetRepository
    {
        private const string RelatedSkillField = "RelatedSkill";
        private const string RelatedSocField = "RelatedSoc";
        private readonly IDynamicModuleRepository<OnetSkill> onetSkillRepository;
        private readonly IDynamicModuleRepository<SocSkillMatrix> socMatrixRepository;
        private readonly IDynamicModuleRepository<SocCode> socCodeRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public OnetRepository(IDynamicModuleRepository<OnetSkill> onetSkillRepository, IDynamicModuleRepository<SocSkillMatrix> socMatrixRepository, IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleRepository<SocCode> socCodeRepository)
        {
            this.onetSkillRepository = onetSkillRepository;
            this.socMatrixRepository = socMatrixRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.socCodeRepository = socCodeRepository;
        }

        public RepoActionResult UpsertOnetSkill(OnetSkill onetSkill)
        {
            var repoSkill = onetSkillRepository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == onetSkill.SfUrlName);
            if (repoSkill != null)
            {
                var master = onetSkillRepository.GetMaster(repoSkill);

                var temp = onetSkillRepository.GetTemp(master);

                dynamicContentExtensions.SetFieldValue(temp, nameof(OnetSkill.Description), onetSkill.Description);
                dynamicContentExtensions.SetFieldValue(temp, nameof(OnetSkill.OnetElementId), onetSkill.OnetElementId);

                var updatedMaster = onetSkillRepository.CheckinTemp(temp);

                onetSkillRepository.Update(updatedMaster);
                onetSkillRepository.Commit();
            }
            else
            {
                var newRepoSkill = onetSkillRepository.Create();
                newRepoSkill.UrlName = onetSkill.SfUrlName;

                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(OnetSkill.Title), onetSkill.Title);
                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(OnetSkill.Description), onetSkill.Description);
                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(OnetSkill.OnetElementId), onetSkill.OnetElementId);

                onetSkillRepository.Add(newRepoSkill);
                onetSkillRepository.Commit();
            }

            return new RepoActionResult
            {
                Success = true
            };
        }

        public RepoActionResult UpsertSocSkillMatrix(SocSkillMatrix socSkillMatrix)
        {
            var repoSocMatrix = socMatrixRepository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == socSkillMatrix.SfUrlName);
            if (repoSocMatrix != null)
            {
                var master = socMatrixRepository.GetMaster(repoSocMatrix);

                var temp = socMatrixRepository.GetTemp(master);

                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.Title), socSkillMatrix.Title);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.Contextualised), socSkillMatrix.Contextualised);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.ONetAttributeType), socSkillMatrix.ONetAttributeType);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.ONetRank), socSkillMatrix.ONetRank);
                dynamicContentExtensions.SetFieldValue(temp, nameof(SocSkillMatrix.Rank), socSkillMatrix.Rank);

                if (!string.IsNullOrWhiteSpace(socSkillMatrix.Skill))
                {
                    dynamicContentExtensions.DeleteRelatedFieldValues(temp, RelatedSkillField);
                    var relatedSkillItem = onetSkillRepository.Get(d => d.Status == ContentLifecycleStatus.Master && d.GetValue<string>(nameof(OnetSkill.Title)) == socSkillMatrix.Skill);
                    if (relatedSkillItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(temp, relatedSkillItem, RelatedSkillField);
                    }
                }

                if (!string.IsNullOrWhiteSpace(socSkillMatrix.SocCode))
                {
                    dynamicContentExtensions.DeleteRelatedFieldValues(temp, RelatedSocField);
                    var relatedSocItem = socCodeRepository.Get(d => d.Status == ContentLifecycleStatus.Master && d.GetValue<string>(nameof(SocCode.SOCCode)) == socSkillMatrix.SocCode);
                    if (relatedSocItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(temp, relatedSocItem, RelatedSocField);
                    }
                }

                var updatedMaster = socMatrixRepository.CheckinTemp(temp);

                socMatrixRepository.Update(updatedMaster);
                socMatrixRepository.Commit();
            }
            else
            {
                var newSocMatrix = socMatrixRepository.Create();
                newSocMatrix.UrlName = socSkillMatrix.SfUrlName;
                dynamicContentExtensions.SetFieldValue(newSocMatrix, nameof(SocSkillMatrix.Title), socSkillMatrix.Title);
                dynamicContentExtensions.SetFieldValue(newSocMatrix, nameof(SocSkillMatrix.Contextualised), socSkillMatrix.Contextualised);
                dynamicContentExtensions.SetFieldValue(newSocMatrix, nameof(SocSkillMatrix.ONetAttributeType), socSkillMatrix.ONetAttributeType);
                dynamicContentExtensions.SetFieldValue(newSocMatrix, nameof(SocSkillMatrix.ONetRank), socSkillMatrix.ONetRank);
                dynamicContentExtensions.SetFieldValue(newSocMatrix, nameof(SocSkillMatrix.Rank), socSkillMatrix.Rank);

                if (!string.IsNullOrWhiteSpace(socSkillMatrix.Skill))
                {
                    var relatedSkillItem = onetSkillRepository.Get(d => d.Status == ContentLifecycleStatus.Master && d.GetValue<string>(nameof(OnetSkill.Title)) == socSkillMatrix.Skill);
                    if (relatedSkillItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(newSocMatrix, relatedSkillItem, RelatedSkillField);
                    }
                }

                if (!string.IsNullOrWhiteSpace(socSkillMatrix.SocCode))
                {
                    var relatedSocItem = socCodeRepository.Get(d => d.Status == ContentLifecycleStatus.Master && d.GetValue<string>(nameof(SocCode.SOCCode)) == socSkillMatrix.SocCode);
                    if (relatedSocItem != null)
                    {
                        dynamicContentExtensions.SetRelatedFieldValue(newSocMatrix, relatedSocItem, RelatedSocField);
                    }
                }

                socMatrixRepository.Add(newSocMatrix);
                socMatrixRepository.Commit();
            }

            return new RepoActionResult
            {
                Success = true
            };
        }

        public IEnumerable<SocSkillMatrix> GetSocSkillMatricesBySocCode(string socCode)
        {
            throw new NotImplementedException();
        }
    }
}
