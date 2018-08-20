using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET
{
    public class SocMappingRepository : ISocMappingRepository
    {
        private readonly OnetSkillsFramework onetDbContext;

        public SocMappingRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public void SetUpdateStatusForSocs(IEnumerable<SocCode> socCodes, UpdateStatus updateStatus)
        {
            var socArray = socCodes.Select(s => s.SOCCode).ToArray();
            var socMappings = onetDbContext.DFC_SocMappings.Where(s => socArray.Contains(s.SocCode)).ToList();
            socMappings.ForEach(m => m.UpdateStatus = updateStatus.ToString());
            onetDbContext.SaveChanges();
        }
    }
}
