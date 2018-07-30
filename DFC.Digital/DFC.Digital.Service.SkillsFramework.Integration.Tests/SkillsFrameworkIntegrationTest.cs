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
using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Service.SkillsFramework.Integration.Tests
{

    [TestClass]
    public class SkillsFrameworkIntegrationTest
    {
        [Fact]
        public void GetAllTransalations()
        {

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var appLogger = A.Fake<IApplicationLogger>();

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext, mapper);
                var all = repository.GetAll();
                all.Should().NotBeNull();
                var count = all.Count();
                count.Should().Be(153);

                var single = repository.GetById("1.A.1.a");
                single.Should().NotBeNull();
                single.Title.Should().Be("1.A.1.a");

                var singleByExpression = repository.Get(s => s.Title == "1.A.1.a");
                singleByExpression.Should().NotBeNull();
                singleByExpression.Title.Should().Be("1.A.1.a");

                var manyByExpression = repository.GetMany(s => s.Title.StartsWith("1.A.", StringComparison.Ordinal));
                manyByExpression.Should().NotBeNull();

            }
        }

        [Theory]
        [InlineData("1.A.1.a")]
        public void GetById(string onetElementId)
        {
            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext, autoMapper);


                var single = repository.GetById(onetElementId);
                single.Should().NotBeNull();
                single.Title.Should().Be(onetElementId);


            }
        }

        [Theory]
        [InlineData("1.A.1.a")]
        public void Get(string onetElementId)
        {
            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext, autoMapper);


                var single = repository.Get(x=>x.Title==onetElementId);
                single.Should().NotBeNull();
                single.Title.Should().Be(onetElementId);


            }

        }

        [Theory]
        [InlineData("1.A.1.a", "excellent verbal communication skills")]
        [InlineData("1.A.1.b", "thinking and reasoning skills")]
        [InlineData("1.A.1.c", "maths skills")]
        public void GetMany(string field1, string field2)
        {
            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext, autoMapper);


                var single = repository.GetMany(x => x.Title == field1 && x.Description==field2 );
                single.Should().NotBeNull();
                single.Should().Contain(x => x.Title == field1);

            }
        }
        [Fact]
        public void GetAllSocMappings()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
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

            }
        }

        [Fact]
        public void GetSkillsForOnetCodeTest()
        {
            string testOnetCode = "53-2011.00";
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeSocRepository = A.Fake<IRepository<SocCode>>();
            var fakeDigitalSkillRepository = A.Fake<IRepository<DigitalSkill>>();
            var fakeDigitalTranslationRepository = A.Fake<IRepository<FrameworkSkill>>();

            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));



            using (OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var knowledgeRepository = new KnowledgeOueryRepository(dbcontext);

                var fakeSkillsRepository = A.Fake<ISkillsRepository>();


                ISkillFrameworkBusinessRuleEngine skillFrameworkBusinessRuleEngine = new SkillFrameworkBusinessRuleEngine(autoMapper, knowledgeRepository, fakeSkillsRepository, fakeSkillsRepository, fakeSkillsRepository);
                var skillsFrameworkService = new SkillsFrameworkService(fakeLogger, fakeSocRepository, fakeDigitalSkillRepository, fakeDigitalTranslationRepository, skillFrameworkBusinessRuleEngine);
                var result = skillsFrameworkService.GetRelatedSkillMapping(testOnetCode);
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
