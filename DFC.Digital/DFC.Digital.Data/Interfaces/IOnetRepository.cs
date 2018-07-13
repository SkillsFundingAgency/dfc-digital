using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IOnetRepository
    {
        RepoActionResult UpsertOnetSkill(OnetSkill onetSkill);

        RepoActionResult UpsertSocSkillMatrix(SocSkillMatrix socSkillMatrix);

        IEnumerable<SocSkillMatrix> GetSocSkillMatricesBySocCode(string socCode);
    }
}
