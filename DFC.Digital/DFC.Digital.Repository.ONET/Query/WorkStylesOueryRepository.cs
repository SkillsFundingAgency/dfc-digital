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

namespace DFC.Digital.Repository.ONET.Query
{

    public class WorkStylesOueryRepository : ISkillsRepository
    {
        private readonly OnetSkillsFramework onetDbContext;

        public WorkStylesOueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public IQueryable<OnetAttribute> GetSkillsForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from workStyle in onetDbContext.work_styles
                                    where workStyle.recommend_suppress != "Y"
                                        && workStyle.onetsoc_code == oNetOccupationCode
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = workStyle.onetsoc_code,
                                        Id = workStyle.element_id,
                                        Type = AttributeType.WorkStyle,
                                        Score = workStyle.data_value
                                    };
            return attributes;
        }
    }
}
