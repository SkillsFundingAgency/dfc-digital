using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
   public class FrameworkSkillRepository : IFrameworkSkillRepository
    {
        #region Fields
        private const string UpdateComment = "Updated via the SkillsFramework import process";
        private readonly IDynamicModuleRepository<FrameworkSkill> frameworkSkillRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IDynamicModuleConverter<FrameworkSkill> frameworkSkillConverter;
        #endregion Fields

        #region Ctor
        public FrameworkSkillRepository(IDynamicModuleRepository<FrameworkSkill> frameworkSkillRepository, IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleConverter<FrameworkSkill> frameworkSkillConverter)
        {
            this.frameworkSkillRepository = frameworkSkillRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.frameworkSkillConverter = frameworkSkillConverter;
        }

        #endregion Ctor

        #region Interface Implementations

        public RepoActionResult UpsertOnetSkill(FrameworkSkill onetSkill)
        {
            //CodeReview: Worth abstrating this out to a get by url
            var repoSkill = frameworkSkillRepository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == onetSkill.SfUrlName);

            if (repoSkill != null)
            {
                var master = frameworkSkillRepository.GetMaster(repoSkill);
                var temp = frameworkSkillRepository.GetTemp(master);

                dynamicContentExtensions.SetFieldValue(temp, nameof(FrameworkSkill.Description), onetSkill.Description);
                dynamicContentExtensions.SetFieldValue(temp, nameof(FrameworkSkill.ONetElementId), onetSkill.ONetElementId);

                var updatedMaster = frameworkSkillRepository.CheckinTemp(temp);
                frameworkSkillRepository.Publish(updatedMaster, UpdateComment);
                frameworkSkillRepository.Commit();
            }
            else
            {
                var newRepoSkill = frameworkSkillRepository.Create();
                newRepoSkill.UrlName = onetSkill.SfUrlName;

                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(FrameworkSkill.Title), onetSkill.Title);
                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(FrameworkSkill.Description), onetSkill.Description);
                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(FrameworkSkill.ONetElementId), onetSkill.ONetElementId);

                frameworkSkillRepository.Add(newRepoSkill);
                frameworkSkillRepository.Commit();
            }

            return new RepoActionResult
            {
                Success = true
            };
        }

        public IQueryable<FrameworkSkill> GetOnetSkills()
        {
            var socSkillMatrices = frameworkSkillRepository.GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live);

            if (socSkillMatrices.Any())
            {
                return socSkillMatrices.Select(item => frameworkSkillConverter.ConvertFrom(item));
            }

            return Enumerable.Empty<FrameworkSkill>().AsQueryable();
        }

        #endregion Interface Implementations
    }
}
