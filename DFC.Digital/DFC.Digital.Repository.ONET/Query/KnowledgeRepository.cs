using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET.Query
{

    public class KnowledgeRepository : IQueryRepository<OnetAttribute>
    {
        public OnetAttribute Get(Expression<Func<OnetAttribute, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OnetAttribute> GetAll()
        {
            throw new NotImplementedException();
        }

        public OnetAttribute GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<OnetAttribute> GetMany(Expression<Func<OnetAttribute, bool>> where)
        {
            //Get by occupation code just the knowledge items not titles

          /*    var selectedKnowledge = from knwldg in onetDbContext.knowledges
                                                               
                                    where knwldg.recommend_suppress != "Y"
                                        && knwldg.not_relevant != "Y"
                                        && knwldg.onetsoc_code == onetOccupationalCode
                                   
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = knwldgGroup.Key.onetsoc_code,
                                        Id = knwldgGroup.Key.element_id,
                                        Description = knwldgGroup.Key.description,
                                        Name = knwldgGroup.Key.element_name,
                                        Type = AttributeType.Knowledge,
                                        Score = knwldgGroup.Sum(v => v.data_value) / 2
                                    };
                           */

            throw new NotImplementedException();
        }

       
    }
}
