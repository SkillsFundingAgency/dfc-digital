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
        IQueryable<OnetAttribute> GetSkillsForONetOccupationCode(string oNetOccupationCode);

        IQueryable<OnetAttribute> GetAbilitiesForONetOccupationCode(string oNetOccupationCode);

        IQueryable<OnetAttribute> GetKowledgeForONetOccupationCode(string oNetOccupationCode);

        IQueryable<OnetAttribute> GetWorkStylesForONetOccupationCode(string oNetOccupationCode);
    }
}
