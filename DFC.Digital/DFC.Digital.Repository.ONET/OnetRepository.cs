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
            return (System.Linq.IQueryable<DFC.Digital.Repository.ONET.DataModel.DFC_GDSTranlations>)_mapper.Map<DFC_GDSTranlations>(result);
        }

      #endregion

        #region Implementation of IOnetRespository

        public async  Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : class, new()
        {
            var ret = await _db.Set<DFC_GDSTranlations>()
                .AsQueryable()
                .ProjectToListAsync<T>(_mapper.ConfigurationProvider)
                .ConfigureAwait(false);
            return _mapper.Map<IEnumerable<T>>(ret);
        }

        public async Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : class, new()
        {
            var ret = await _db.Set<DFC_SocMappings>()
                .AsQueryable()
                .ProjectToListAsync<T>(_mapper.ConfigurationProvider)
                .ConfigureAwait(false);
            return _mapper.Map<IEnumerable<T>>(ret);
        }

        public Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode)
        {
            return null;
        }

        public async Task<T> GetDigitalSkilRank<T>(string socCode) where T : class, new()
        {

                var r = from o in _db.Set<tools_and_technology>()
                join od in _db.Set<unspsc_reference>() on o.commodity_code equals od.commodity_code
                where o.onetsoc_code == socCode
                select new DfcGdsToolsAndTechnology()
                {
                    ClassTitle = od.class_title,
                    T2Type = o.t2_type,
                    T2Example = o.t2_example
                };

            var ret = await _db.Set<tools_and_technology>()
                .AsQueryable()
                .ProjectToListAsync<T>(_mapper.ConfigurationProvider)
                .ConfigureAwait(false);
            var res=_mapper.Map<T>(ret);
            return res;
        }

        public Task<IEnumerable<T>> GetAttributesValuesAsync<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            return null;
        }

        #endregion
    }
}
