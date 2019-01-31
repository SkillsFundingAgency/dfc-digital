using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET
{
    public class SocMappingRepository : ISocMappingRepository 
    {
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;

        public SocMappingRepository(OnetSkillsFramework onetDbContext, IMapper autoMapper)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
        }

        public void SetUpdateStatusForSocs(IEnumerable<SocCode> socCodes, SkillsFrameworkUpdateStatus updateStatus)
        {
            var socArray = socCodes.Select(s => s.SOCCode).ToArray();
            var socMappings = onetDbContext.DFC_SocMappings.Where(s => socArray.Contains(s.SocCode)).ToList();
            socMappings.ForEach(m => m.UpdateStatus = updateStatus.ToString());

            foreach (var m in socMappings)
            {
                onetDbContext.Entry(m).State = System.Data.Entity.EntityState.Modified;
            }
            onetDbContext.SaveChanges();
        }


        public IQueryable<SocCode> GetSocsAwaitingUpdate()
        {
            return GetSocsQuery(s => s.UpdateStatus == SkillsFrameworkUpdateStatus.AwaitingUpdate.ToString() || s.UpdateStatus == null);
        }

        public IQueryable<SocCode> GetSocsInStartedState()
        {
            return GetSocsQuery(s => s.UpdateStatus == SkillsFrameworkUpdateStatus.SelectedForUpdate.ToString());
        }
                
        public SocMappingStatus GetSocMappingStatus()
        {
            var socMappingStatus = new SocMappingStatus
            {
                AwaitingUpdate = onetDbContext.DFC_SocMappings.Count(s => s.UpdateStatus == SkillsFrameworkUpdateStatus.AwaitingUpdate.ToString() || s.UpdateStatus == null),
                SelectedForUpdate = onetDbContext.DFC_SocMappings.Count(s => s.UpdateStatus == SkillsFrameworkUpdateStatus.SelectedForUpdate.ToString()),
                UpdateCompleted = onetDbContext.DFC_SocMappings.Count(s => s.UpdateStatus == SkillsFrameworkUpdateStatus.UpdateCompleted.ToString())
            };
            return socMappingStatus;
        }

        public SocCode GetById(string id)
        {
            return onetDbContext.DFC_SocMappings.ProjectToSingle<DFC_SocMappings, SocCode>(m => m.SocCode == id, autoMapper.ConfigurationProvider);
        }

        public SocCode Get(Expression<Func<SocCode, bool>> where)
        {
            return GetAll().Single(where);
        }

        public IQueryable<SocCode> GetAll()
        {
                var result = (from soc in onetDbContext.DFC_SocMappings
                              orderby soc.SocCode
                              select new SocCode()
                              {
                                  Id = Guid.Empty,
                                  ONetOccupationalCode = soc.ONetCode,
                                  SOCCode = soc.SocCode,
                                  Description = null
                              });
                return result;
        }

        public IQueryable<SocCode> GetMany(Expression<Func<SocCode, bool>> where)
        {
             return GetAll().Where(where);
        }

        public void AddNewSOCMappings(IEnumerable<SocCode> NewSocCodes)
        {
            foreach (var socCode in NewSocCodes)
            {
                onetDbContext.DFC_SocMappings.Add(new DFC_SocMappings { SocCode = socCode.SOCCode, ONetCode = socCode.ONetOccupationalCode, JobProfile = socCode.Description });
            }
            onetDbContext.SaveChanges();
        }

        private IQueryable<SocCode> GetSocsQuery(Expression<Func<DFC_SocMappings, bool>> where)
        {
            var mapping = onetDbContext.DFC_SocMappings.Where(where);
            var result = (from soc in mapping
                          orderby soc.SocCode
                          select new SocCode()
                          {
                              Id = Guid.Empty,
                              ONetOccupationalCode = soc.ONetCode,
                              SOCCode = soc.SocCode,
                              Description = null
                          });
            return result;
        }
    }
}
