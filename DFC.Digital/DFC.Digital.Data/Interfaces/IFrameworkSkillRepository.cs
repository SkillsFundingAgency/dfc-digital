using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface IFrameworkSkillRepository
    {
        RepoActionResult UpsertOnetSkill(FrameworkSkill onetSkill);

        IQueryable<FrameworkSkill> GetOnetSkills();
    }
}
