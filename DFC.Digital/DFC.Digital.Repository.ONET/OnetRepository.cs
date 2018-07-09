using DFC.Digital.Data.Interfaces;
using System;
using System.Linq;
using DFC.Digital.Data.Model;
using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DFC.Digital.Repository.ONET.Interface;
using System.Diagnostics.Contracts;

namespace DFC.Digital.Repository.ONET
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataModel;


    public class OnetRepository : ISkillsFrameworkRepository
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public OnetRepository ( IDbContext db , IMapper mapper )
        {
            _db = db;
            _mapper = mapper;
        }

        #region NotImplemented
        public void Add ( SkillsFramework entity )
        {
            throw new NotImplementedException ( );
        }

        public void Delete ( SkillsFramework entity )
        {
            throw new NotImplementedException ( );
        }

        public void Delete ( Expression<Func<SkillsFramework , bool>> where )
        {
            throw new NotImplementedException ( );
        }
        #endregion

        #region Implementation of IDisposable
        public void Dispose ( )
        {
            _db.Dispose ( );

            GC.SuppressFinalize ( this );
        }
        #endregion


        public IQueryable<T> GetAll<T> ( ) where T : class
        {
            var ret=  _db.Set<T>().ToList().AsQueryable();//.ProjectTo<DFC_GDSTranlations>();
            return (System.Linq.IQueryable<T>)ret;
        }

        #region SkillsFramework Repository Implemetation
        public IQueryable<DFC_GDSTranlations> GetMany ( Expression<Func<DFC_GDSTranlations, bool>> where )
        {

            var result = _db.Set<SkillsFramework> ( ).AsQueryable ( ).ProjectTo<DFC_GDSTranlations> ( where );
            return (IQueryable<DFC_GDSTranlations>)_mapper.Map<DFC_GDSTranlations>(result);
        }

      #endregion

        #region Implementation of IOnetRespository

        public async  Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : DfcGdsOnetEntity
        {
            var ret = await _db.Set<DFC_GDSTranlations>()
                .AsQueryable()
                .ProjectToListAsync<T>(_mapper.ConfigurationProvider)
                .ConfigureAwait(false);
            return _mapper.Map<IEnumerable<T>>(ret);
        }

        public async Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : DfcGdsOnetEntity
        {
            var ret = await _db.Set<DFC_SocMappings>()
                .AsQueryable()
                .ProjectToListAsync<T>(_mapper.ConfigurationProvider)
                .ConfigureAwait(false);
            return _mapper.Map<IEnumerable<T>>(ret);
        }

        public  async Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode)
        {
            IOrderedQueryable<DfcGdsAttributesData> attributes=null;
            await Task.Factory.StartNew(() =>
            {
                attributes = (from knwldg in _db.Set<knowledge>()
                        join cmr in _db.Set<content_model_reference>() on knwldg.element_id.Substring(1, 7) equals cmr
                            .element_id
                        join gas in _db.Set<DFC_GlobalAttributeSuppression>() on cmr.element_id equals gas
                            .onet_element_id
                        where knwldg.recommend_suppress == "N" && knwldg.not_relevant == "N"
                              && knwldg.onetsoc_code == socCode && gas.onet_element_id == null
                        group knwldg by new
                        {
                            knwldg.onetsoc_code,
                            cmr.element_name,
                            cmr.description,
                            knwldg.element_id,

                        }
                        into knwldgGroup
                        select new DfcGdsAttributesData()
                        {
                            ElementDescription = knwldgGroup.Key.description,
                            ElementName = knwldgGroup.Key.element_name,
                            Attribute = @"Knowledge",
                            Value = knwldgGroup.Sum(x => x.data_value / 2)

                        })
                    .Union(from skl in _db.Set<skill>()
                        join cmr in _db.Set<content_model_reference>() on skl.element_id.Substring(1, 7) equals cmr
                            .element_id
                        join gas in _db.Set<DFC_GlobalAttributeSuppression>() on skl.element_id equals gas
                            .onet_element_id
                        where skl.recommend_suppress == "N" && skl.not_relevant == "N"
                              && skl.onetsoc_code == socCode && gas.onet_element_id == null
                        group skl by new
                        {
                            skl.onetsoc_code,
                            cmr.element_name,
                            cmr.description,
                            skl.element_id,

                        }
                        into knwldgGroup
                        select new DfcGdsAttributesData()
                        {
                            ElementDescription = knwldgGroup.Key.description,
                            ElementName = knwldgGroup.Key.element_name,
                            Attribute = @"Skill",
                            Value = knwldgGroup.Sum(x => x.data_value / 2)
                        })
                    .Union(from ablty in _db.Set<ability>()
                        join cmr in _db.Set<content_model_reference>() on ablty.element_id.Substring(1, 7) equals cmr
                            .element_id
                        join gas in _db.Set<DFC_GlobalAttributeSuppression>() on ablty.element_id equals gas
                            .onet_element_id
                        where ablty.recommend_suppress == "N" && ablty.not_relevant == "N"
                              && ablty.onetsoc_code == socCode && gas.onet_element_id == null
                        group ablty by new
                        {
                            ablty.onetsoc_code,
                            cmr.element_name,
                            cmr.description,
                            ablty.element_id,

                        }
                        into knwldgGroup
                        select new DfcGdsAttributesData()
                        {
                            ElementDescription = knwldgGroup.Key.description,
                            ElementName = knwldgGroup.Key.element_name,
                            Attribute = @"Ability",
                            Value = knwldgGroup.Sum(x => x.data_value / 2)
                        })
                    .Union(from wkstyl in _db.Set<work_styles>()
                        join cmr in _db.Set<content_model_reference>() on wkstyl.element_id.Substring(1, 7) equals cmr
                            .element_id
                        join gas in _db.Set<DFC_GlobalAttributeSuppression>() on wkstyl.element_id equals gas
                            .onet_element_id
                        where wkstyl.recommend_suppress == "N"
                              && wkstyl.onetsoc_code == socCode && gas.onet_element_id == null
                        group wkstyl by new
                        {
                            wkstyl.onetsoc_code,
                            cmr.element_name,
                            cmr.description,
                            wkstyl.element_id,

                        }
                        into knwldgGroup
                        select new DfcGdsAttributesData()
                        {
                            ElementDescription = knwldgGroup.Key.description,
                            ElementName = knwldgGroup.Key.element_name,
                            Attribute = @"WorkStyle",
                            Value = knwldgGroup.Sum(x => x.data_value / 2)
                        })
                    .OrderBy(x => x.Value);
            }).ConfigureAwait(false);

            foreach (var a in attributes.ToList())
            {
                var x = a.ElementName;
                var y = a.ElementDescription;
                var z = a.Value;
                var z1 = a.Attribute;
            }

            return (IEnumerable<T>)Convert.ChangeType(attributes, typeof(IEnumerable<T>));
        }

        public async Task<T> GetDigitalSkillsRankAsync<T>(string socCode) where T : struct
        {
            var count=0;
            await Task.Factory.StartNew(() =>
            {
                count = (from o in _db.Set<tools_and_technology>()
                    join od in _db.Set<unspsc_reference>() on o.commodity_code equals od.commodity_code
                    where o.onetsoc_code == socCode
                    orderby o.t2_type, od.class_title
                    select o).Count();
            }).ConfigureAwait(false);

            return (T)(object)count;
        }

        public async Task<T> GetDigitalSkillsAsync<T>(string socCode) where T : DfcGdsOnetEntity
        {
            IQueryable<DfcGdsToolsAndTechnology> dfcToolsandTech = null;
            await Task.Factory.StartNew(() =>
            {
               dfcToolsandTech = from o in _db.Set<tools_and_technology>()
                    join od in _db.Set<unspsc_reference>() on o.commodity_code equals od.commodity_code
                    where o.onetsoc_code == socCode
                    orderby o.t2_type, od.class_title
                    select new DfcGdsToolsAndTechnology()
                    {

                        ClassTitle = od.class_title,
                        T2Example = o.t2_example,
                        T2Type = o.t2_type,
                        SocCode = socCode,
                        OnetSocCode = o.onetsoc_code

                    };
            }).ConfigureAwait(false);
            var digitalSkills = new DfcGdsDigitalSkills
            {
                DigitalSkillsCollection = dfcToolsandTech,
                DigitalSkillsRank = dfcToolsandTech.Count()
            };
            return (T)Convert.ChangeType(digitalSkills,typeof(T));
        }

        public Task<IEnumerable<T>> GetAttributesValuesAsync<T>(Expression<Func<T, bool>> predicate) where T : DfcGdsOnetEntity
        {

         
            return null;
        }

        #endregion
    }
}
