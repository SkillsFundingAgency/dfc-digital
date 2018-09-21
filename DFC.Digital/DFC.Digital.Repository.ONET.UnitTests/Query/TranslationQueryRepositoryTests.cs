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
using DFC.Digital.Repository.ONET;

namespace DFC.Digital.Repository.ONET.UnitTests
{
    public class TranslationQueryRepositoryTests : HelperOnetDatas
    {
        [Theory]
        [MemberData(nameof(OnetFrameworkSkillTranslationData))]
        public void GetByIdTest(IReadOnlyCollection<DFC_GDSTranlations> setupDbSetData, IReadOnlyCollection<content_model_reference> contentModelSetupData, IReadOnlyCollection<DFC_GDSCombinations> combinationSetupData, FrameworkSkill mappedWhatitTakesData, string onetElementId)
        {

            //Arrange
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeTranslationDbSet= A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>)))
                    .SetupData(setupDbSetData.ToList());
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                    .SetupData(contentModelSetupData.ToList());
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                    .SetupData(combinationSetupData.ToList());
            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTranslationDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext);

            //Assert
            var result = repo.GetById(onetElementId);

            result.Should().NotBeNull();
            if (mappedWhatitTakesData == null || result == null)
            {
                Xunit.Assert.True(false,"Mapped data should not be null ");
            }
            else
            {
                result.Should().BeEquivalentTo(mappedWhatitTakesData);
                result.ONetElementId.Should().Be(onetElementId);
            }

        }

        [Theory]
        [MemberData(nameof(OnetFrameworkSkillTranslationData))]
        public void GetTest(IReadOnlyCollection<DFC_GDSTranlations> setupDbSetData, IReadOnlyCollection<content_model_reference> contentModelSetupData, IReadOnlyCollection<DFC_GDSCombinations> combinationSetupData, FrameworkSkill mappedWhatitTakesData, string onetElementId)
        {
            //InProgress as have to yield single object against collection
            //Arrange
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeTranslationDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData.ToList());
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                .SetupData(contentModelSetupData.ToList());
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                .SetupData(combinationSetupData.ToList());
            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTranslationDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext);

            //Assert
            var result = repo.Get(x => x.ONetElementId == onetElementId);

            result.Should().NotBeNull();
            if (mappedWhatitTakesData == null || result == null)
            {
                Xunit.Assert.True(false, "The mapped data should not be null ");
            }
            else
            {
                result.Should().BeEquivalentTo(mappedWhatitTakesData);
                result.ONetElementId.Should().Be(onetElementId);
            }

        }

        [Theory]
        [MemberData(nameof(OnetGetAllTranslationsData))]
        public void GetAllTest(IReadOnlyCollection<DFC_GDSTranlations> setupDbSetData, IReadOnlyCollection<content_model_reference> contentModelSetupData, IReadOnlyCollection<DFC_GDSCombinations> combinationSetupData, IReadOnlyCollection<FrameworkSkill> mappedReturnDbSetData)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeTransDataDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData.ToList());
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                .SetupData(contentModelSetupData.ToList());
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                .SetupData(combinationSetupData.ToList());

            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTransDataDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);
            var repo = new TranslationQueryRepository(fakeDbContext);

            //Assert
            var result = repo.GetAll();
            if (mappedReturnDbSetData == null || result == null)
            {
                Xunit.Assert.True(false, "The mapped data should not be null ");
            }
            else
            {
                result.Should().BeEquivalentTo(mappedReturnDbSetData);
            }

        }

        [Theory]
        [MemberData(nameof(OnetWhatitTakesManyData))]
        public void GetManyTest(IReadOnlyCollection<DFC_GDSTranlations> setupDbSetData, IReadOnlyCollection<content_model_reference> contentModelSetupData, IReadOnlyCollection<DFC_GDSCombinations> combinationSetupData, IReadOnlyCollection<FrameworkSkill> mappedWhatitTakesData, string onetElementId1, string onetElementId2)
        {
            var fakeDbContext = A.Fake<OnetSkillsFramework>();
            var fakeTranslationDbSet = A.Fake<DbSet<DFC_GDSTranlations>>(c => c
                .Implements(typeof(IQueryable<DFC_GDSTranlations>))
                .Implements(typeof(IDbAsyncEnumerable<DFC_GDSTranlations>))).SetupData(setupDbSetData.ToList());
            var fakeContentDbSet = A.Fake<DbSet<content_model_reference>>(c => c
                    .Implements(typeof(IQueryable<content_model_reference>))
                    .Implements(typeof(IDbAsyncEnumerable<content_model_reference>)))
                .SetupData(contentModelSetupData.ToList());
            var fakeCombinationDbSet = A.Fake<DbSet<DFC_GDSCombinations>>(c => c
                    .Implements(typeof(IQueryable<DFC_GDSCombinations>))
                    .Implements(typeof(IDbAsyncEnumerable<DFC_GDSCombinations>)))
                .SetupData(combinationSetupData.ToList());
            //Act
            A.CallTo(() => fakeDbContext.DFC_GDSTranlations).Returns(fakeTranslationDbSet);
            A.CallTo(() => fakeDbContext.content_model_reference).Returns(fakeContentDbSet);
            A.CallTo(() => fakeDbContext.DFC_GDSCombinations).Returns(fakeCombinationDbSet);

            var repo = new TranslationQueryRepository(fakeDbContext);

            //Assert
            var result = repo.GetMany(x => x.ONetElementId == onetElementId1 || x.ONetElementId==onetElementId2);
            if (mappedWhatitTakesData == null || result == null)
            {
                Xunit.Assert.True(false, "The mapped data should not be null ");
            }
            else
            {
                result.Should().BeEquivalentTo(mappedWhatitTakesData);
            }


        }
    }
}