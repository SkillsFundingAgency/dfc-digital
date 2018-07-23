using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using FakeItEasy;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using DFC.Digital.Repository.ONET.Mapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Model;
//using DFC.Digital.Repository.ONET.Impl;
//using DFC.Digital.Repository.ONET.Helper;
using DFC.Digital.Repository.ONET;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Query;

namespace DFC.Digital.Service.SkillsFramework.Integration.Tests
{

    [TestClass]
    public class SkillsFrameworkIntegrationTest
    {
        //[Fact]
        //public void GetAllTransalations()
        //{

        //    IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
        //    var appLogger = A.Fake<IApplicationLogger>();
        //    IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();
        //    using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
        //    {
        //        var all = repository.GetAllTranslationsAsync<DfcOnetTranslation>().Result;
        //        all.Should().NotBeNull();
        //    }
        //}

        [Fact]
        public void GetAllSocMappings()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var appLogger = A.Fake<IApplicationLogger>();
            //IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new SocMappingsQueryRepository(dbcontext, mapper);
                var all = repository.GetAll();
                //var dfcGdsSocMappingses = all as IList<SocCode> ?? all.ToList();
                all.Should().NotBeNull();
                var count = all.Count();
                count.Should().Be(153);

                var single = repository.GetById("1150");
                single.Should().NotBeNull();
                single.ONetOccupationalCode.Should().Be("11-3031.02");

                var singleByExpression = repository.Get(s => s.SOCCode == "2123");
                singleByExpression.Should().NotBeNull();
                singleByExpression.ONetOccupationalCode.Should().Be("17-2071.00");

                var manyByExpression = repository.GetMany(s => s.SOCCode.StartsWith("212"));
                manyByExpression.Should().NotBeNull();
                manyByExpression.Count().Should().Be();

            }
        }

        //[Fact]
        //public void GetDigitalSkillRanks()
        //{
        //    IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
        //    var GetRank = 0;
        //    const string onetCode = "11-1011.00";
        //    var appLogger = A.Fake<IApplicationLogger>();
        //    IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();

        //    using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
        //    {
        //        var rankResult = repository.GetDigitalSkillsRankAsync<int>(onetCode).Result;
        //        rankResult.Should().BeGreaterThan(0);
        //        if(rankResult > Convert.ToInt32(RangeChecker.FirstRange))
        //            GetRank = 1;
        //        if((rankResult > Convert.ToInt32(RangeChecker.SecondRange)) && (rankResult < Convert.ToInt32(RangeChecker.FirstRange)))
        //            GetRank = 2;
        //        if((rankResult > Convert.ToInt32(RangeChecker.ThirdRange)) && (rankResult < Convert.ToInt32(RangeChecker.SecondRange)))
        //            GetRank = 3;
        //        if((rankResult > Convert.ToInt32(RangeChecker.FourthRange)) && (rankResult < Convert.ToInt32(RangeChecker.ThirdRange)))
        //            GetRank = 4;
        //    }
        //}

        //[Fact]
        //public void GetDigitalSkills()
        //{
        //    IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
        //    const string onetCode = "11-1011.00";
        //    var appLogger = A.Fake<IApplicationLogger>();
        //    IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();
        //    using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
        //    {
        //        var digitalSkillsData = repository.GetDigitalSkillsAsync<DfcOnetDigitalSkills>(onetCode).Result;
        //        digitalSkillsData.DigitalSkillsCollection.Should().NotBeNull();
        //        digitalSkillsData.DigitalSkillsCount.Should().NotBe(0);
        //    }
        //}

        //[Fact]
        //public void GetAttributes()
        //{
        //    IMapper iMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
        //    const string onetCode = "11-1011.00";
        //    var appLogger = A.Fake<IApplicationLogger>();
        //    IObjectContextFactory<OnetRepositoryDbContext> contextFactory = new ObjectContextFactory<OnetRepositoryDbContext>();

        //    using(IOnetRepository repository = new OnetRepository(contextFactory, iMapper, appLogger))
        //    {
        //        var digitalSkillsData = repository.GetAttributesValuesAsync<DfcOnetAttributesData>(onetCode).Result;
        //        var dfcGdsAttributesDatas = digitalSkillsData as IList<DfcOnetAttributesData> ?? digitalSkillsData.ToList();
        //        dfcGdsAttributesDatas.Should().HaveCount(20);
        //        dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.Knowledge);
        //        dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.Skills);
        //        dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.Abilities);
        //        dfcGdsAttributesDatas.Should().Contain(x => x.Attribute == Attributes.WorkStyles);

        //    }
        //}
    }
}
