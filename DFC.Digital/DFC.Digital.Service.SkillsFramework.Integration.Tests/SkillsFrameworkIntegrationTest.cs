using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using FakeItEasy;
using Xunit;
using System;
using System.Linq;
using FluentAssertions;
using DFC.Digital.Repository.ONET.Mapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Query;

namespace DFC.Digital.Service.SkillsFramework.Integration.Tests
{

    using Data.Interfaces;

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
                var all = repository.GetAll().ToList();
                all.Should().NotBeNull();
                var count = all.Count();

                var single = repository.GetById("1.A.1.a");
                single.Should().NotBeNull();
                single.ONetElementId.Should().Be("1.A.1.a");

                var singleByExpression = repository.Get(s => s.ONetElementId == "1.A.1.a");
                singleByExpression.Should().NotBeNull();
                singleByExpression.ONetElementId.Should().Be("1.A.1.a");

                var manyByExpression = repository.GetMany(s => s.ONetElementId.StartsWith("1.A.", StringComparison.Ordinal));
                manyByExpression.Should().NotBeNull();

            }
        }

        [Theory]
        [InlineData("1.A.1.a")]
        public void GetTranslationById(string onetElementId)
        {
            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext, autoMapper);


                var single = repository.GetById(onetElementId);
                single.Should().NotBeNull();
                single.ONetElementId.Should().Be(onetElementId);


            }
        }

        [Theory]
        [InlineData("1.A.1.a")]
        public void GetTranslation(string onetElementId)
        {
            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext, autoMapper);


                var single = repository.Get(x=>x.ONetElementId==onetElementId);
                single.Should().NotBeNull();
                single.ONetElementId.Should().Be(onetElementId);


            }

        }

        [Theory]
        [InlineData("1.A.1.a", "excellent verbal communication skills")]
        [InlineData("1.A.1.b", "thinking and reasoning skills")]
        [InlineData("1.A.1.c", "maths skills")]
        public void GetTranslationsMany(string field1, string field2)
        {
            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext, autoMapper);


                var single = repository.GetMany(x => x.ONetElementId == field1 && x.Description==field2 );
                single.Should().NotBeNull();
                single.Should().Contain(x => x.ONetElementId == field1);

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
            string testOnetCode = "17-1011.00";
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeSocRepository = A.Fake<IRepository<SocCode>>();
            var fakeDigitalSkillRepository = A.Fake<IRepository<DigitalSkill>>();
            var fakeDigitalTranslationRepository = A.Fake<IRepository<FrameworkSkill>>();

            IMapper autoMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));



            using (OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var skillsRepository = new SkillsOueryRepository(dbcontext);
        

                var fakeSkillsRepository = A.Fake<ISkillsRepository>();
                var fakeframeworkSkill = A.Fake<IQueryRepository<FrameworkSkill>>();

                var combinationRepository = new CombinationsQueryRepository(dbcontext);
                var suppressionRepository = new SuppressionsQueryRepository(dbcontext);
                var contentReferenceRepository = new ContentReferenceQueryRepository(dbcontext);


                ISkillFrameworkBusinessRuleEngine skillFrameworkBusinessRuleEngine = new SkillFrameworkBusinessRuleEngine(
                    autoMapper, skillsRepository, suppressionRepository, 
                    combinationRepository, contentReferenceRepository);

                var skillsFrameworkService = new SkillsFrameworkService(fakeLogger, fakeSocRepository, fakeDigitalSkillRepository, fakeDigitalTranslationRepository, skillFrameworkBusinessRuleEngine);
                var result = skillsFrameworkService.GetRelatedSkillMapping(testOnetCode);
            }
        }

        [Fact]
        public void GetDigitalSkillsRank()
        {

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new DigitalSkillsQueryRepository(dbcontext, mapper);

                var single = repository.GetById("11-1011.00");
                single.Should().NotBeNull();
                single.ApplicationCount.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public void SkillsFrameworkServiceGetDigitalSkills()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeframeworkSkill = A.Fake<IQueryRepository<FrameworkSkill>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();

            IQueryRepository<SocCode> socCodeRepository=new SocMappingsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<DigitalSkill> digitalSkillsRepository=new DigitalSkillsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<FrameworkSkill> frameWorkRepository=new TranslationQueryRepository(new OnetSkillsFramework(), mapper);
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository,fakeframeworkSkill,fakeCombinationSkill,fakeContentReference);

            ISkillsFrameworkService skillService =new SkillsFrameworkService(fakeLogger,socCodeRepository,digitalSkillsRepository,frameWorkRepository,ruleEngine);

            var level= (int)skillService.GetDigitalSkillLevel("11-1011.00");
            level.Should().BeGreaterThan(0);

        }
        [Fact]
        public void GetAllSocMapping()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeframeworkSkill = A.Fake<IQueryRepository<FrameworkSkill>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();

            IQueryRepository<SocCode> socCodeRepository = new SocMappingsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<DigitalSkill> digitalSkillsRepository = new DigitalSkillsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<FrameworkSkill> frameWorkRepository = new TranslationQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<FrameworkSkill> contentRepository = new TranslationQueryRepository(new OnetSkillsFramework(), mapper);
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, fakeframeworkSkill, fakeCombinationSkill, fakeContentReference);

            ISkillsFrameworkService skillService = new SkillsFrameworkService(fakeLogger, socCodeRepository, digitalSkillsRepository, frameWorkRepository, ruleEngine);

            var level = skillService.GetAllSocMappings().ToList();
            level.Should().NotBeNull();

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
