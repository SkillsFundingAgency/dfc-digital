using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISkillsRepository
    {
        IQueryable<OnetSkill> GetSkillsForONetOccupationCode(string oNetOccupationCode);

        IQueryable<OnetSkill> GetAbilitiesForONetOccupationCode(string oNetOccupationCode);

        IQueryable<OnetSkill> GetKowledgeForONetOccupationCode(string oNetOccupationCode);

        IQueryable<OnetSkill> GetWorkStylesForONetOccupationCode(string oNetOccupationCode);
    }
}
