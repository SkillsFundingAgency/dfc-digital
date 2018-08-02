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
            // CodeReview: TK: Please rmeove unused fields
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

        // CodeReview: TK: Magic strings could used as inline data to test different conditions
        [Theory]
        [InlineData("2215A", "29-1021.00")]
        public void GetAllSocMappings(string socCode,string onetSocCode)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new SocMappingsQueryRepository(dbcontext, mapper);
                var all = repository.GetAll();
                all.Should().NotBeNull();
                var count = all.Count();
                count.Should().Be(780);

                var single = repository.GetById(socCode);
                single.Should().NotBeNull();
                single.ONetOccupationalCode.Should().Be(onetSocCode);

                var singleByExpression = repository.Get(s => s.SOCCode == socCode);
                singleByExpression.Should().NotBeNull();
                singleByExpression.ONetOccupationalCode.Should().Be(onetSocCode);

                var manyByExpression = repository.GetMany(s => s.SOCCode.StartsWith(socCode.Substring(0, 2), StringComparison.CurrentCulture));
                manyByExpression.Should().NotBeNull();

            }
        }

        // CodeReview: TK: Magic strings could used as inline data to test different conditions
        // Remove unused variables liek fakeskillsRepository, resultList
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
                var fakeFrameworkSkill = A.Fake<IQueryRepository<FrameworkSkill>>();

                var combinationRepository = new CombinationsQueryRepository(dbcontext);
                var suppressionRepository = new SuppressionsQueryRepository(dbcontext);
                var contentReferenceRepository = new ContentReferenceQueryRepository(dbcontext);


                ISkillFrameworkBusinessRuleEngine skillFrameworkBusinessRuleEngine = new SkillFrameworkBusinessRuleEngine(
                    autoMapper, skillsRepository, suppressionRepository, 
                    combinationRepository, contentReferenceRepository);

                var skillsFrameworkService = new SkillsFrameworkService(fakeLogger, fakeSocRepository, fakeDigitalSkillRepository, fakeDigitalTranslationRepository, skillFrameworkBusinessRuleEngine);
                var result = skillsFrameworkService.GetRelatedSkillMapping(testOnetCode);

                result.Should().NotBeNull();

            }

        }

        // CodeReview: TK: Magic strings could used as inline data to test different conditions
        [Fact]
        public void GetDigitalSkillsRank()
        {
            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new DigitalSkillsQueryRepository(dbcontext);

                var single = repository.GetById("11-1011.00");
                single.Should().NotBeNull();
                single.ApplicationCount.Should().BeGreaterThan(0);
            }
        }

        // CodeReview: TK: Magic strings could used as inline data to test different conditions
        [Fact]
        public void SkillsFrameworkServiceGetDigitalSkills()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();

            IQueryRepository<SocCode> socCodeRepository=new SocMappingsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<DigitalSkill> digitalSkillsRepository=new DigitalSkillsQueryRepository(new OnetSkillsFramework());
            IQueryRepository<FrameworkSkill> frameWorkRepository=new TranslationQueryRepository(new OnetSkillsFramework(), mapper);
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill,fakeContentReference);

            ISkillsFrameworkService skillService =new SkillsFrameworkService(fakeLogger,socCodeRepository,digitalSkillsRepository,frameWorkRepository,ruleEngine);

            var level= (int)skillService.GetDigitalSkillLevel("11-1011.00");
            level.Should().BeGreaterThan(0);

        }

        // CodeReview: TK: removed unused variables
        [Fact]
        public void GetAllSocMapping()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();

            IQueryRepository<SocCode> socCodeRepository = new SocMappingsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<DigitalSkill> digitalSkillsRepository = new DigitalSkillsQueryRepository(new OnetSkillsFramework());
            IQueryRepository<FrameworkSkill> frameWorkRepository = new TranslationQueryRepository(new OnetSkillsFramework(), mapper);
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            ISkillsFrameworkService skillService = new SkillsFrameworkService(fakeLogger, socCodeRepository, digitalSkillsRepository, frameWorkRepository, ruleEngine);

            var level = skillService.GetAllSocMappings().ToList();
            level.Should().NotBeNull();

        }

    }
}
