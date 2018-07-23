using System.Collections.Generic;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IRelatedSkillsMappingRepository
    {
        //Get
        IEnumerable<RelatedSkillMapping> GetByONetOccupationalCode(string onetOccupationalCode);
    }
}