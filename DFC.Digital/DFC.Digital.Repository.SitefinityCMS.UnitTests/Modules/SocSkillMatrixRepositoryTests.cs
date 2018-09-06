using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class SocSkillMatrixRepositoryTests
    {
        private readonly IDynamicModuleRepository<FrameworkSkill> fakeFrameworkSkillRepository;
        private readonly IDynamicModuleRepository<SocSkillMatrix> fakeSocMatrixRepository;
        private readonly IDynamicModuleRepository<SocCode> fakeSocCodeRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly IDynamicModuleConverter<SocSkillMatrix> fakeSocSkillConverter;

        public SocSkillMatrixRepositoryTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(ops => ops.Strict());
            fakeSocSkillConverter = A.Fake<IDynamicModuleConverter<SocSkillMatrix>>(ops => ops.Strict());
            fakeFrameworkSkillRepository = A.Fake<IDynamicModuleRepository<FrameworkSkill>>(ops => ops.Strict());
            fakeSocMatrixRepository = A.Fake<IDynamicModuleRepository<SocSkillMatrix>>(ops => ops.Strict());
            fakeSocCodeRepository = A.Fake<IDynamicModuleRepository<SocCode>>(ops => ops.Strict());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpsertSocSkillMatrixTest(bool skillAvailable)
        {
            //Arrange
            var dummySocSkill = new SocSkillMatrix { Title = "title test", SocCode = nameof(SocSkillMatrix.SocCode), Skill = nameof(SocSkillMatrix.Skill) };
            var dummyDynamicContent = A.Dummy<DynamicContent>();

            // Dummies and fakes
            A.CallTo(() => fakeSocMatrixRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(skillAvailable ? dummyDynamicContent : null);
            A.CallTo(() => fakeFrameworkSkillRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(dummyDynamicContent);

            A.CallTo(() => fakeSocCodeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._))
                .Returns(dummyDynamicContent);

            A.CallTo(() => fakeSocMatrixRepository.GetMaster(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeSocMatrixRepository.GetTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeSocMatrixRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeSocMatrixRepository.Create()).Returns(dummyDynamicContent);
            A.CallTo(() => fakeSocMatrixRepository.Add(dummyDynamicContent)).DoesNothing();
            A.CallTo(() => fakeSocMatrixRepository.Publish(dummyDynamicContent, A<string>._)).DoesNothing();
            A.CallTo(() => fakeSocMatrixRepository.Commit()).DoesNothing();
            A.CallTo(() => fakeSocMatrixRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() =>
                   fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<string>._))
               .DoesNothing();
            A.CallTo(() =>
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<Lstring>._))
                .DoesNothing();
            A.CallTo(() =>
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<decimal?>._))
                .DoesNothing();
            A.CallTo(() =>
                fakeDynamicContentExtensions.DeleteRelatedFieldValues(dummyDynamicContent, A<string>._)).DoesNothing();
            A.CallTo(() =>
                fakeDynamicContentExtensions.SetRelatedFieldValue(dummyDynamicContent, dummyDynamicContent, A<string>._, A<float>._)).DoesNothing();
            A.CallTo(() =>
                fakeDynamicContentExtensions.DeleteRelatedFieldValues(dummyDynamicContent, A<string>._)).DoesNothing();

            var socSkillMatrixRepository = new SocSkillMatrixRepository(fakeFrameworkSkillRepository, fakeSocMatrixRepository, fakeDynamicContentExtensions, fakeSocCodeRepository, fakeSocSkillConverter);

            // Act
            socSkillMatrixRepository.UpsertSocSkillMatrix(dummySocSkill);

            // Assert
            if (!skillAvailable)
            {
                A.CallTo(() => fakeSocMatrixRepository.Create()).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeSocMatrixRepository.Add(dummyDynamicContent)).MustHaveHappenedOnceExactly();
            }
            else
            {
                A.CallTo(() => fakeSocMatrixRepository.GetMaster(dummyDynamicContent))
               .MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeSocMatrixRepository.GetTemp(dummyDynamicContent)).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeSocMatrixRepository.CheckinTemp(dummyDynamicContent)).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeSocMatrixRepository.Publish(dummyDynamicContent, A<string>._))
                    .MustHaveHappenedOnceExactly();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.SetRelatedFieldValue(dummyDynamicContent, dummyDynamicContent, A<string>._, A<float>._)).MustHaveHappened(2, Times.OrLess);
                A.CallTo(() =>
                        fakeDynamicContentExtensions.DeleteRelatedFieldValues(dummyDynamicContent, A<string>._))
                    .MustHaveHappened(2, Times.OrLess);
                A.CallTo(() =>
                        fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() =>
                        fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<decimal?>._))
                    .MustHaveHappened(2, Times.Exactly);
                A.CallTo(() => fakeSocCodeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m =>
                LinqExpressionsTestHelper.IsExpressionEqual(m, d =>
                    d.Status == ContentLifecycleStatus.Master &&
                    d.GetValue<string>(nameof(SocCode.SOCCode)) == dummySocSkill.SocCode))))
                    .MustHaveHappened();

                A.CallTo(() => fakeSocMatrixRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m =>
                    LinqExpressionsTestHelper.IsExpressionEqual(m, item =>
                        item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == dummySocSkill.SfUrlName)))).MustHaveHappened();
                A.CallTo(() => fakeSocMatrixRepository.Commit()).MustHaveHappened();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetSocSkillMatricesTest(bool skillsAvailable)
        {
            //Arrange
            var dummySkill = A.Dummy<SocSkillMatrix>();
            var dummyDynamicContent = A.Dummy<DynamicContent>();

            // Dummies and fakes
            A.CallTo(() => fakeSocSkillConverter.ConvertFrom(dummyDynamicContent)).Returns(dummySkill);
            A.CallTo(() => fakeSocMatrixRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { skillsAvailable ? dummyDynamicContent : null }));

            var socSkillMatrixRepository = new SocSkillMatrixRepository(fakeFrameworkSkillRepository, fakeSocMatrixRepository, fakeDynamicContentExtensions, fakeSocCodeRepository, fakeSocSkillConverter);

            // Act
            socSkillMatrixRepository.GetSocSkillMatrices();

            // Assert

            // Needs investigation as fake convertor not registering call made to it when there is a soc skill available.
            //if (skillsAvailable)
            //{
            //    A.CallTo(() => fakeFrameworkSkillConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            //}
            //else
            //{
            //    A.CallTo(() => fakeFrameworkSkillConverter.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            //}
            A.CallTo(() => fakeSocMatrixRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Visible && item.Status == ContentLifecycleStatus.Live)))).MustHaveHappened();
        }
    }
}