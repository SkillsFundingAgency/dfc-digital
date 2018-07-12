using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
   public class OnetRepository : IOnetRepository
    {
        private readonly IDynamicModuleRepository<OnetSkill> skillRepository;

        public OnetRepository(IDynamicModuleRepository<OnetSkill> skillRepository)
        {
            this.skillRepository = skillRepository;
        }

        public RepoActionResult UpsertOnetSkill(OnetSkill onetSkill)
        {
            var repoSkill = skillRepository.Get(item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == onetSkill.SfUrlName);
            if (repoSkill != null)
            {
                var master = skillRepository.GetMaster(repoSkill);

                var temp = skillRepository.GetTemp(master);

                temp.SetValue(nameof(OnetSkill.Description), onetSkill.Description);
                temp.SetValue(nameof(OnetSkill.OnetElementId), onetSkill.OnetElementId);

                var updatedMaster = skillRepository.CheckinTemp(temp);

                skillRepository.Update(updatedMaster);
                skillRepository.Commit();
            }
            else
            {
                var newRepoSkill = skillRepository.Create();
                newRepoSkill.UrlName = onetSkill.SfUrlName;
                newRepoSkill.SetValue(nameof(OnetSkill.Title), onetSkill.Title);
                newRepoSkill.SetValue(nameof(OnetSkill.Description), onetSkill.Description);
                newRepoSkill.SetValue(nameof(OnetSkill.OnetElementId), onetSkill.OnetElementId);

                skillRepository.Add(newRepoSkill);
                skillRepository.Commit();
            }

            return new RepoActionResult
            {
                Success = true
            };
        }
    }
}
