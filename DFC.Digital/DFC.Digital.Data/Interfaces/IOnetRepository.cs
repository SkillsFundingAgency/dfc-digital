using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Data.Interfaces
{
    public interface IOnetRepository
    {
        RepoActionResult UpsertOnetSkill(OnetSkill onetSkill);

        RepoActionResult UpsertSocSkillMatrix(SocSkillMatrix socSkillMatrix);

        IEnumerable<SocSkillMatrix> GetSocSkillMatrices();

        IQueryable<OnetSkill> GetOnetSkills();
    }
}
