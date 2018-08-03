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
using DFC.Digital.Data.Interfaces;
namespace DFC.Digital.Service.SkillsFramework.Integration.Tests
{

    public class SkillsFrameworkIntegrationTest
    {
        [Fact]
        public void GetAllTransalations()
        {
            // CodeReview: TK: Please rmeove unused fields (Done - Dinesh)

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext);
                var all = repository.GetAll().ToList();
                all.Should().NotBeNull();
                all.Count.Should().BeGreaterThan(0);

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
            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext);

                var single = repository.GetById(onetElementId);
                single.Should().NotBeNull();
                single.ONetElementId.Should().Be(onetElementId);

            }
        }

        [Theory]
        [InlineData("1.A.1.a")]
        public void GetTranslation(string onetElementId)
        {

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext);


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

            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new TranslationQueryRepository(dbcontext);


                var single = repository.GetMany(x => x.ONetElementId == field1 && x.Description==field2 );
                single.Should().NotBeNull();
                single.Should().Contain(x => x.ONetElementId == field1);

            }
        }

        // CodeReview: TK: Magic strings could used as inline data to test different conditions (Done - Dinesh)
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
        // Remove unused variables liek fakeskillsRepository, resultList - (Done - Dinesh)
        [Theory]
        [InlineData("17-1011.00")]
        public void GetSkillsForOnetCodeTest(string onetSocCode)
        {
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeSocRepository = A.Fake<IRepository<SocCode>>();
            var fakeDigitalSkillRepository = A.Fake<IRepository<DigitalSkill>>();
            var fakeDigitalTranslationRepository = A.Fake<IRepository<FrameworkSkill>>();

            IMapper autoMapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));


            using (OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var skillsRepository = new SkillsOueryRepository(dbcontext);
                var combinationRepository = new CombinationsQueryRepository(dbcontext);
                var suppressionRepository = new SuppressionsQueryRepository(dbcontext);
                var contentReferenceRepository = new ContentReferenceQueryRepository(dbcontext);


                ISkillFrameworkBusinessRuleEngine skillFrameworkBusinessRuleEngine = new SkillFrameworkBusinessRuleEngine(
                    autoMapper, skillsRepository, suppressionRepository,
                    combinationRepository, contentReferenceRepository);

                var skillsFrameworkService = new SkillsFrameworkService(fakeLogger, fakeSocRepository, fakeDigitalSkillRepository, fakeDigitalTranslationRepository, skillFrameworkBusinessRuleEngine);
                var result = skillsFrameworkService.GetRelatedSkillMapping(onetSocCode);

                result.Should().NotBeNull();

            }

        }

        // CodeReview: TK: Magic strings could used as inline data to test different conditions (Done - Dinesh)
        [Theory]
        [InlineData("11-1011.00")]
        public void GetDigitalSkillsRank(string onetSocCode)
        {
            using(OnetSkillsFramework dbcontext = new OnetSkillsFramework())
            {
                var repository = new DigitalSkillsQueryRepository(dbcontext);

                var single = repository.GetById(onetSocCode);
                single.Should().NotBeNull();
                single.ApplicationCount.Should().BeGreaterThan(0);
            }
        }

        // CodeReview: TK: Magic strings could used as inline data to test different conditions (Done-Dinesh)
        [Theory]
        [InlineData("11-1011.00")]
        public void SkillsFrameworkServiceGetDigitalSkills(string onetSocCode)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper()));
            var mapper = mapperConfig.CreateMapper();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var fakeFrameworkSkillSuppression = A.Fake<IQueryRepository<FrameworkSkillSuppression>>();
            var fakeContentReference = A.Fake<IQueryRepository<FrameWorkContent>>();
            var fakeCombinationSkill = A.Fake<IQueryRepository<FrameWorkSkillCombination>>();

            IQueryRepository<SocCode> socCodeRepository=new SocMappingsQueryRepository(new OnetSkillsFramework(), mapper);
            IQueryRepository<DigitalSkill> digitalSkillsRepository=new DigitalSkillsQueryRepository(new OnetSkillsFramework());
            IQueryRepository<FrameworkSkill> frameWorkRepository=new TranslationQueryRepository(new OnetSkillsFramework());
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill,fakeContentReference);

            ISkillsFrameworkService skillService =new SkillsFrameworkService(fakeLogger,socCodeRepository,digitalSkillsRepository,frameWorkRepository,ruleEngine);

            var level= (int)skillService.GetDigitalSkillLevel(onetSocCode);
            level.Should().BeGreaterThan(0);

        }

        // CodeReview: TK: removed unused variables - (Done - Dinesh)
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
            IQueryRepository<FrameworkSkill> frameWorkRepository = new TranslationQueryRepository(new OnetSkillsFramework());
            ISkillsRepository skillsRepository = new SkillsOueryRepository(new OnetSkillsFramework());

            ISkillFrameworkBusinessRuleEngine ruleEngine = new SkillFrameworkBusinessRuleEngine(mapper, skillsRepository, fakeFrameworkSkillSuppression, fakeCombinationSkill, fakeContentReference);

            ISkillsFrameworkService skillService = new SkillsFrameworkService(fakeLogger, socCodeRepository, digitalSkillsRepository, frameWorkRepository, ruleEngine);

            var level = skillService.GetAllSocMappings().ToList();
            level.Should().NotBeNull();

        }

    }
}
