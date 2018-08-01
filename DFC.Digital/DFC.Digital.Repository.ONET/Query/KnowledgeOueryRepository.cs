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

    public class KnowledgeOueryRepository : ISkillsRepository
    {
        private readonly OnetSkillsFramework onetDbContext;

        public KnowledgeOueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public IQueryable<OnetAttribute> GetSkillsForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from knowledge in onetDbContext.knowledges
                                    where knowledge.recommend_suppress != "Y"
                                        && knowledge.not_relevant != "Y"
                                        && knowledge.onetsoc_code == oNetOccupationCode
                                        
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = knowledge.onetsoc_code,
                                        Id = knowledge.element_id,
                                        Type = AttributeType.Knowledge,
                                        Score = knowledge.data_value,
                                    };
            return attributes;
        }

       
    }
}
