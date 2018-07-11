using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Helper;
using DFC.Digital.Repository.ONET.Impl;
using DFC.Digital.Repository.ONET.Interface;

namespace DFC.Digital.Repository.ONET
{
    public class OnetRepository : IDfcGdsSkillsFramework
    {
        private readonly IDbContext _db;
        private readonly IMapper _mapper;

        public OnetRepository(IDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _db.Dispose();

            GC.SuppressFinalize(this);
        }

        #endregion

        public async Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : DfcGdsOnetEntity
        {
            List<T> ret;
            using (var context = ObjectContextFactory<SkillsFrameworkDbContext>.GetContext())
            {
                ret = await context.DFC_SocMappings
                    .AsQueryable()
                    .ProjectToListAsync<T>(_mapper.ConfigurationProvider)
                    .ConfigureAwait(false);
            }
            return _mapper.Map<IEnumerable<T>>(ret);
        }

        public async Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : DfcGdsOnetEntity
        {
            List<T> ret;
            using (var context = ObjectContextFactory<SkillsFrameworkDbContext>.GetContext())
            {
                ret = await context.DFC_GDSTranlations
                    .AsQueryable()
                    .ProjectToListAsync<T>(_mapper.ConfigurationProvider)
                    .ConfigureAwait(false);
            }
            return _mapper.Map<IEnumerable<T>>(ret);
        }

        public async Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode)
        {
            IQueryable<DfcGdsAttributesData> attributes = null;
            using (var context = ObjectContextFactory<SkillsFrameworkDbContext>.GetContext())
            {
                await Task.Factory.StartNew(() =>
                {
                    attributes = (from knwldg in context.knowledges
                            join cmr in context.content_model_reference on knwldg.element_id.Substring(0, 7) equals
                                cmr.element_id
                            where knwldg.recommend_suppress != "Y" && knwldg.not_relevant != "Y" &&
                                  knwldg.onetsoc_code == socCode
                            group knwldg by new
                            {
                                cmr.element_name,
                                cmr.description,
                                knwldg.element_id,
                                knwldg.onetsoc_code
                            }
                            into knwldgGroup
                            select new DfcGdsAttributesData
                            {
                                OnetSocCode = knwldgGroup.Key.onetsoc_code,
                                ElementId = knwldgGroup.Key.element_id,
                                ElementDescription = knwldgGroup.Key.description,
                                ElementName = knwldgGroup.Key.element_name,
                                Attribute = Attributes.Knowledge.ToString(),
                                Value = knwldgGroup.Sum(v => v.data_value) / 2
                            })
                        .Union(from skl in context.skills
                            join cmr in context.content_model_reference on skl.element_id.Substring(0, 7) equals cmr
                                .element_id
                            where skl.recommend_suppress != "Y" && skl.not_relevant != "Y" &&
                                  skl.onetsoc_code == socCode
                            group skl by new
                            {
                                skl.onetsoc_code,
                                cmr.element_name,
                                cmr.description,
                                skl.element_id
                            }
                            into skillGroup
                            select new DfcGdsAttributesData
                            {
                                OnetSocCode = skillGroup.Key.onetsoc_code,
                                ElementId = skillGroup.Key.element_id,
                                ElementDescription = skillGroup.Key.description,
                                ElementName = skillGroup.Key.element_name,
                                Attribute = Attributes.Skills,
                                Value = skillGroup.Sum(x => x.data_value) / 2
                            })
                        .Union(from ablty in context.abilities
                            join cmr in context.content_model_reference on ablty.element_id.Substring(0, 7) equals
                                cmr.element_id
                            where ablty.recommend_suppress != "Y" && ablty.not_relevant == "Y" &&
                                  ablty.onetsoc_code == socCode
                            group ablty by new
                            {
                                cmr.element_name,
                                cmr.description,
                                ablty.element_id,
                                ablty.onetsoc_code
                            }
                            into abilityGroup
                            select new DfcGdsAttributesData
                            {
                                OnetSocCode = abilityGroup.Key.onetsoc_code,
                                ElementId = abilityGroup.Key.element_id,
                                ElementDescription = abilityGroup.Key.description,
                                ElementName = abilityGroup.Key.element_name,
                                Attribute = @Attributes.Abilities,
                                Value = abilityGroup.Sum(x => x.data_value) / 2
                            })
                        .Union(from wkstyl in context.work_styles
                            join cmr in context.content_model_reference on wkstyl.element_id.Substring(0, 7) equals
                                cmr.element_id
                            where wkstyl.recommend_suppress != "Y" && wkstyl.onetsoc_code == socCode
                            group wkstyl by new
                            {
                                wkstyl.onetsoc_code,
                                cmr.element_name,
                                cmr.description,
                                wkstyl.element_id
                            }
                            into workStyleGroup
                            select new DfcGdsAttributesData
                            {
                                OnetSocCode = workStyleGroup.Key.onetsoc_code,
                                ElementId = workStyleGroup.Key.element_id,
                                ElementDescription = workStyleGroup.Key.description,
                                ElementName = workStyleGroup.Key.element_name,
                                Attribute = Attributes.WorkStyles,
                                Value = workStyleGroup.Sum(x => x.data_value) / 2
                            })
                        .OrderBy(x => x.Value);
                }).ConfigureAwait(false);
                var cnt = attributes.ToList().Count;
                foreach (var skl in attributes)
                {
                    if (skl.Attribute == "Knowledge")
                    {
                        if (skl.ElementName.Contains("Mathematics"))
                        {
                            var found = true;
                        }
                        var kk = skl.Attribute;
                    }
                    if (skl.Attribute == "Skills")
                    {
                        if(skl.ElementName.Contains("Mathematics"))
                        {
                            var found = true;
                        }
                        var xx = skl.Attribute;
                    }
                }
                var re = attributes.ToList().FindAll(x =>
                    (x.Attribute == "Knowledge" || x.Attribute == "Skills") && (x.ElementName.Contains("Mathematics")));
                foreach (var r in re)
                {
                    var e = r.Attribute;
                }
            }


           

            foreach (var a in attributes.ToList())
            {
                var x = a.ElementName;
                var y = a.ElementDescription;
                var z = a.Value;
                var z1 = a.Attribute;
            }

            return (IEnumerable<T>) attributes;
        }

        public Task<IEnumerable<T>> GetAttributesValuesAsync<T>
            (Expression<Func<T, bool>> predicate) where T : DfcGdsOnetEntity
        {
            return null;
        }

        public async Task<T> GetDigitalSkillsAsync<T>(string socCode) where T : DfcGdsOnetEntity
        {
            IQueryable<DfcGdsToolsAndTechnology> dfcToolsandTech = null;
            using (var context = ObjectContextFactory<SkillsFrameworkDbContext>.GetContext())
            {
                await Task.Factory.StartNew(() =>
                {
                    dfcToolsandTech = from o in _db.Set<tools_and_technology>()
                        join od in _db.Set<unspsc_reference>() on o.commodity_code equals od.commodity_code
                        where o.onetsoc_code == socCode
                        orderby o.t2_type, od.class_title
                        select new DfcGdsToolsAndTechnology
                        {
                            ClassTitle = od.class_title,
                            T2Example = o.t2_example,
                            T2Type = o.t2_type,
                            SocCode = socCode,
                            OnetSocCode = o.onetsoc_code
                        };
                }).ConfigureAwait(false);
            }
            var digitalSkills = new DfcGdsDigitalSkills
            {
                DigitalSkillsCollection = dfcToolsandTech,
                DigitalSkillsRank = dfcToolsandTech.Count()
            };
            return (T) Convert.ChangeType(digitalSkills, typeof(T));
        }

        public async Task<T> GetDigitalSkillsRankAsync<T>(string socCode) where T : struct
        {
            var count = 0;
            using (var context = ObjectContextFactory<SkillsFrameworkDbContext>.GetContext())
            {
                await Task.Factory.StartNew(() =>
                {
                    count = (from o in context.tools_and_technology
                        join od in _db.Set<unspsc_reference>() on o.commodity_code equals od.commodity_code
                        where o.onetsoc_code == socCode
                        orderby o.t2_type, od.class_title
                        select o).Count();
                }).ConfigureAwait(false);
            }
            return (T) (object) count;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            var ret = _db.Set<T>().ToList().AsQueryable();
            return ret;
        }

        #region SkillsFramework Repository Implemetation

        public IQueryable<DFC_GDSTranlations> GetMany(Expression<Func<DFC_GDSTranlations, bool>> where)
        {
            var result = _db.Set<SkillsFramework>().AsQueryable().ProjectTo<DFC_GDSTranlations>(where);
            return (IQueryable<DFC_GDSTranlations>) _mapper.Map<DFC_GDSTranlations>(result);
        }

        #endregion
    }
}