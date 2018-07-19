using DFC.Digital.Repository.ONET.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.Mapper;
using FakeItEasy;
using DFC.Digital.Repository.ONET.Impl;
using Xunit;
using DFC.Digital.Core;
using System;
using DFC.Digital.Repository.ONET.Helper;

namespace DFC.Digital.Repository.ONET.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;

    [TestClass]
    public class OnetRepositoryTest
    {
        [Fact]
        public void GetAllTransalations ( )
        {

            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<OnetRepositoryDbContext> contextFactory=new ObjectContextFactory<OnetRepositoryDbContext>();
            using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper,appLogger))
            {
              var all=  repository.GetAllTranslationsAsync<DfcGdsTranslation>().Result;
              all.Should().NotBeNull();
            }
        }

        [Fact]
        public void GetAllSocMappings()
        {
            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();

            using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var all = repository.GetAllSocMappingsAsync<DfcGdsSocMappings>().Result;
                var dfcGdsSocMappingses = all as IList<DfcGdsSocMappings> ?? all.ToList();
                dfcGdsSocMappingses.Should().NotBeNull();
                dfcGdsSocMappingses.Should().HaveCountGreaterThan(0);

            }
        }

        [Fact]
        public void GetDigitalSkillRanks()
        {
            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var GetRank = 0;
            const string onetCode = "11-1011.00";
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();

            using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var rankResult = repository.GetDigitalSkillsRankAsync<int>(onetCode).Result;
                rankResult.Should().BeGreaterThan(0);
                if (rankResult > Convert.ToInt32(RangeChecker.FirstRange))
                    GetRank = 1;
                if((rankResult > Convert.ToInt32(RangeChecker.SecondRange)) && (rankResult < Convert.ToInt32(RangeChecker.FirstRange)))
                    GetRank = 2;
                if((rankResult > Convert.ToInt32(RangeChecker.ThirdRange)) && (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
                    GetRank = 3;
                if((rankResult > Convert.ToInt32(RangeChecker.FourthRange)) && (rankResult < Convert.ToInt32(RangeChecker.ThirdRange)))
                    GetRank = 4;
            }
        }

        [Fact]
        public void GetDigitalSkills()
        {
            IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            const string onetCode = "11-1011.00";
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();
            using (IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var digitalSkillsData = repository.GetDigitalSkillsAsync<DfcGdsDigitalSkills>(onetCode).Result;
                digitalSkillsData.DigitalSkillsCollection.Should().NotBeNull();
                digitalSkillsData.DigitalSkillsCount.Should().NotBe(0);
            }
        }

        [Fact]
        public void GetAttributes()
        {
            IMapper iMapper =new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            const string onetCode = "11-1011.00";
            var appLogger = A.Fake<IApplicationLogger>();
            IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();

            using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
            {
                var digitalSkillsData = repository.GetAttributesValuesAsync<DfcGdsAttributesData>(onetCode).Result;
                var dfcGdsAttributesDatas = digitalSkillsData as IList<DfcGdsAttributesData> ?? digitalSkillsData.ToList();
                dfcGdsAttributesDatas.Should().HaveCount(20);
                dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.Knowledge);
                dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.Skills);
                dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.Abilities);
                dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.WorkStyles);

            }
        }
    }
}
