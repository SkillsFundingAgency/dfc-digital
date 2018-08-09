using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IRelatedSkillsMappingRepository
    {
        //Get
        IEnumerable<RelatedSkillMapping> GetByONetOccupationalCode(string onetOccupationalCode);
    }
}