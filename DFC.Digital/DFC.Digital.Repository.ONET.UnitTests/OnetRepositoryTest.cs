using DFC.Digital.Repository.ONET.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.Mapper;
using FakeItEasy;
using DFC.Digital.Repository.ONET.Impl;
using Xunit;

namespace DFC.Digital.Repository.ONET.UnitTests
{
    using System.Linq;

    [TestClass]
    public class OnetRepositoryTest
    {
        [Fact]
        public void GetAllTransalations ( )
        {

            IMapper _mapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            IDbContext context = new MetaSchemaContext();
            var mapper=A.Fake<IMapper>();
            using(ISkillsFrameworkRepository repository = new OnetRepository(context, _mapper))
            {
              var all=  repository.GetAllTranslationsAsync<DfcGdsTranslation>().Result;

            }
        }

        [Fact]
        public void GetAllSocMappings()
        {
            IMapper _mapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            IDbContext context = new MetaSchemaContext();
            var mapper = A.Fake<IMapper>();
            using(ISkillsFrameworkRepository repository = new OnetRepository(context, _mapper))
            {
                var all = repository.GetAllSocMappingsAsync<DfcGdsSocMappings>().Result;

            }
        }

        [Fact]
        public void GetDigitalSkillRanks()
        {
            IMapper _mapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            IDbContext context = new MetaSchemaContext();
            var mapper = A.Fake<IMapper>();
            string onetCode = "11-1011.00";
            using(ISkillsFrameworkRepository repository = new OnetRepository(context, _mapper))
            {
                var all = repository.GetDigitalSkilRank<DfcGdsDigitalSkills>(onetCode).Result;

            }
        }
    }
}
