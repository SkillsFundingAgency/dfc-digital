using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISocMappingRepository : IQueryRepository<SocCode>
    {
        void SetUpdateStatusForSocs(IEnumerable<SocCode> socCodes, SkillsFrameworkUpdateStatus updateStatus);

        IQueryable<SocCode> GetSocsAwaitingUpdate();

        SocMappingStatus GetSocMappingStatus();

        IQueryable<SocCode> GetSocsInStartedState();

        void AddNewSOCMappings(IEnumerable<SocCode> newSocCodes);
    }
}
