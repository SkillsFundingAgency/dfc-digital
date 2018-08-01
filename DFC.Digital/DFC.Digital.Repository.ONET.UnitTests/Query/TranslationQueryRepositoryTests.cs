using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using DFC.Digital.Repository.ONET.Mapper;
using DFC.Digital.Repository.ONET.UnitTests.Model;
using DFC.Digital.Repository.ONET.Query;
namespace DFC.Digital.Repository.ONET.UnitTests.Query
{


    public class TranslationQueryRepositoryTests : HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(OnetFrameworkSkillTranslationData))]
        public void GetByIdTest(List<DFC_GDSTranlations> setupDbSetData,List<content_model_reference> contentModelSetupData,List<DFC_GDSCombinations> combinationSetupData, FrameworkSkill mappedWhatitTakesData, string onetElementId)
        {

            //Arrange
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeTranslationDbSet= A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>)))
                    .SetupData(setupDbSetData);
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                    .SetupData(contentModelSetupData);
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                    .SetupData(combinationSetupData);
            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTranslationDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.GetById(onetElementId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mappedWhatitTakesData);
            result.ONetElementId.Should().Be(onetElementId);

        }

        [Theory]
        [MemberData(nameof(OnetFrameworkSkillTranslationData))]
        public void GetTest(List<DFC_GDSTranlations> setupDbSetData, List<content_model_reference> contentModelSetupData,List<DFC_GDSCombinations> combinationSetupData, FrameworkSkill mappedWhatitTakesData, string onetElementId)
        {
            //InProgress as have to yield single object against collection
            //Arrange
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeTranslationDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData);
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                .SetupData(contentModelSetupData);
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                .SetupData(combinationSetupData);
            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTranslationDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.Get(x => x.ONetElementId == onetElementId);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(mappedWhatitTakesData);
            result.ONetElementId.Should().Be(onetElementId);

        }

        [Theory]
        [MemberData(nameof(OnetGetAllTranslationsData))]
        public void GetAllTest(List<DFC_GDSTranlations> setupDbSetData, List<content_model_reference> contentModelSetupData, List<DFC_GDSCombinations> combinationSetupData, List<FrameworkSkill> mappedReturnDbSetData)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeTransDataDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData);
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                .SetupData(contentModelSetupData);
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                .SetupData(combinationSetupData);

            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTransDataDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.GetAll();
            result.Should().BeEquivalentTo(mappedReturnDbSetData);

        }

        [Theory]
        [MemberData(nameof(OnetWhatitTakesManyData))]
        public void GetManyTest(ICollection<DFC_GDSTranlations> setupDbSetData, ICollection<content_model_reference> contentModelSetupData, ICollection<DFC_GDSCombinations> combinationSetupData, ICollection<FrameworkSkill> mappedWhatitTakesData, string onetElementId1, string onetElementId2)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            IMapper actualMapper = new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new SkillsFrameworkMapper())));
            var fakeTranslationDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData);
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                .SetupData(contentModelSetupData);
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                .SetupData(combinationSetupData);
            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTranslationDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);

            var repo = new TranslationQueryRepository(fakeDbContext, actualMapper);

            //Assert
            var result = repo.GetMany(x => x.ONetElementId == onetElementId1 || x.ONetElementId==onetElementId2);
            result.Should().BeEquivalentTo(mappedWhatitTakesData);

        }
    }
}