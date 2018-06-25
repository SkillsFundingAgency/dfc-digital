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


    public class OnetRepository : IRepository<SkillsFramework>, IDisposable
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



        #endregion

        #region Implementation of IDisposable

        public void Dispose ( )
        {
            _db.Dispose ( );
            GC.SuppressFinalize ( this );
        }

        public SkillsFramework Get ( Expression<Func<SkillsFramework , bool>> where )
        {
            throw new NotImplementedException ( );
        }

        public IQueryable<SkillsFramework> GetAll ( )
        {
            throw new NotImplementedException ( );
        }

        public SkillsFramework GetById ( string id )
        {
            throw new NotImplementedException ( );
        }
        #endregion

        #region SkillsFramework Repository Implemetation 
        public IQueryable<SkillsFramework> GetMany ( Expression<Func<SkillsFramework , bool>> where )
        {
            var result = _db.Set<SkillsFramework> ( ).AsQueryable ( ).ProjectTo<SkillsFramework> ( where );
            return _mapper.Map<SkillsFramework> ( result ) as IQueryable<SkillsFramework>;
        }

        public void Update ( SkillsFramework entity )
        {
            throw new NotImplementedException ( );
        }

        #endregion
    }
}
