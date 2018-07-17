using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Linq;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
   public class FrameworkSkillRepository : IFrameworkSkillRepository
    {
        private readonly IDynamicModuleRepository<FrameworkSkill> onetSkillRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        private readonly IDynamicModuleConverter<FrameworkSkill> onetskillConverter;

        public FrameworkSkillRepository(IDynamicModuleRepository<FrameworkSkill> onetSkillRepository, IDynamicContentExtensions dynamicContentExtensions, IDynamicModuleConverter<FrameworkSkill> onetskillConverter)
        {
            this.onetSkillRepository = onetSkillRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
            this.onetskillConverter = onetskillConverter;
        }

        public RepoActionResult UpsertOnetSkill(FrameworkSkill onetSkill)
        {
            //CodeReview: Worth abstrating this out to a get by url
            var repoSkill = onetSkillRepository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == onetSkill.SfUrlName);

            if (repoSkill != null)
            {
                var master = onetSkillRepository.GetMaster(repoSkill);
                var temp = onetSkillRepository.GetTemp(master);

                dynamicContentExtensions.SetFieldValue(temp, nameof(FrameworkSkill.Description), onetSkill.Description);
                dynamicContentExtensions.SetFieldValue(temp, nameof(FrameworkSkill.OnetElementId), onetSkill.OnetElementId);

                var updatedMaster = onetSkillRepository.CheckinTemp(temp);

                //CodeReview: Change it to publish
                onetSkillRepository.Update(updatedMaster);
                onetSkillRepository.Commit();
            }
            else
            {
                var newRepoSkill = onetSkillRepository.Create();
                newRepoSkill.UrlName = onetSkill.SfUrlName;

                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(FrameworkSkill.Title), onetSkill.Title);
                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(FrameworkSkill.Description), onetSkill.Description);
                dynamicContentExtensions.SetFieldValue(newRepoSkill, nameof(FrameworkSkill.OnetElementId), onetSkill.OnetElementId);

                onetSkillRepository.Add(newRepoSkill);
                onetSkillRepository.Commit();
            }

            return new RepoActionResult
            {
                Success = true
            };
        }

        public IQueryable<FrameworkSkill> GetOnetSkills()
        {
            var socSkillMatrices = onetSkillRepository.GetMany(item => item.Visible && item.Status == ContentLifecycleStatus.Live);

            if (socSkillMatrices.Any())
            {
                return socSkillMatrices.Select(item => onetskillConverter.ConvertFrom(item));
            }

            return Enumerable.Empty<FrameworkSkill>().AsQueryable();
        }
    }
}
