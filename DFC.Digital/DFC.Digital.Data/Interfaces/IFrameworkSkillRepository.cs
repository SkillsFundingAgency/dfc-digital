using DFC.Digital.Data.Model;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface IFrameworkSkillRepository
    {
        RepoActionResult UpsertFrameworkSkill(FrameworkSkill onetSkill);

        IQueryable<FrameworkSkill> GetFrameworkSkills();
    }
}
