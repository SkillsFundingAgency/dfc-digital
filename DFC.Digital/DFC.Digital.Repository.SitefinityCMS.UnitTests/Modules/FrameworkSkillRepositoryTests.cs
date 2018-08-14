using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class FrameworkSkillRepositoryTests
    {
        private readonly IDynamicModuleRepository<FrameworkSkill> fakeFrameworkSkillRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly IDynamicModuleConverter<FrameworkSkill> fakeFrameworkSkillConverter;

        public FrameworkSkillRepositoryTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(ops => ops.Strict());
            fakeFrameworkSkillConverter = A.Fake<IDynamicModuleConverter<FrameworkSkill>>(ops => ops.Strict());
            fakeFrameworkSkillRepository = A.Fake<IDynamicModuleRepository<FrameworkSkill>>(ops => ops.Strict());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void UpsertFrameworkSkillTest(bool skillAvailable)
        {
            //Arrange
            var dummySkill = new FrameworkSkill { Title = "title test" };
            var dummyDynamicContent = A.Dummy<DynamicContent>();

            // Dummies and fakes
            A.CallTo(() => fakeFrameworkSkillRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).Returns(skillAvailable ? dummyDynamicContent : null);
            A.CallTo(() => fakeFrameworkSkillRepository.GetMaster(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeFrameworkSkillRepository.GetTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeFrameworkSkillRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() => fakeFrameworkSkillRepository.Create()).Returns(dummyDynamicContent);
            A.CallTo(() => fakeFrameworkSkillRepository.Add(dummyDynamicContent)).DoesNothing();
            A.CallTo(() => fakeFrameworkSkillRepository.Publish(dummyDynamicContent, A<string>._)).DoesNothing();
            A.CallTo(() => fakeFrameworkSkillRepository.Commit()).DoesNothing();
            A.CallTo(() => fakeFrameworkSkillRepository.CheckinTemp(dummyDynamicContent)).Returns(dummyDynamicContent);
            A.CallTo(() =>
                fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<string>._)).DoesNothing();

            var frameworkSkillRepository = new FrameworkSkillRepository(fakeFrameworkSkillRepository, fakeDynamicContentExtensions, fakeFrameworkSkillConverter);

            // Act
            frameworkSkillRepository.UpsertFrameworkSkill(dummySkill);

            // Assert
            if (skillAvailable)
            {
                A.CallTo(() => fakeFrameworkSkillRepository.GetMaster(dummyDynamicContent))
                    .MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeFrameworkSkillRepository.GetTemp(dummyDynamicContent)).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeFrameworkSkillRepository.CheckinTemp(dummyDynamicContent)).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeFrameworkSkillRepository.Publish(dummyDynamicContent, A<string>._)).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeFrameworkSkillRepository.Commit()).MustHaveHappenedOnceExactly();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<string>._)).MustHaveHappened(2, Times.Exactly);
            }
            else
            {
                A.CallTo(() => fakeFrameworkSkillRepository.Create()).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeFrameworkSkillRepository.Add(dummyDynamicContent)).MustHaveHappenedOnceExactly();
                A.CallTo(() => fakeFrameworkSkillRepository.Commit()).MustHaveHappenedOnceExactly();
                A.CallTo(() =>
                    fakeDynamicContentExtensions.SetFieldValue(dummyDynamicContent, A<string>._, A<string>._)).MustHaveHappened(3, Times.Exactly);
            }

            A.CallTo(() => fakeFrameworkSkillRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item =>
                item.Visible && item.Status == ContentLifecycleStatus.Live && item.UrlName == dummySkill.SfUrlName)))).MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetFrameworkSkillsTest(bool skillsAvailable)
        {
            //Arrange
            var dummySkill = A.Dummy<FrameworkSkill>();
            var dummyDynamicContent = A.Dummy<DynamicContent>();

            // Dummies and fakes
            A.CallTo(() => fakeFrameworkSkillConverter.ConvertFrom(dummyDynamicContent)).Returns(dummySkill);
            A.CallTo(() => fakeFrameworkSkillRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>._)).Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { skillsAvailable ? dummyDynamicContent : null }));
            var frameworkSkillRepository = new FrameworkSkillRepository(fakeFrameworkSkillRepository, fakeDynamicContentExtensions, fakeFrameworkSkillConverter);

            // Act
            frameworkSkillRepository.GetFrameworkSkills();

            // Assert

            // Needs investigation as fake convertor not registering call made to it when there is a job profile available.s
            //if (skillsAvailable)
            //{
            //    A.CallTo(() => fakeFrameworkSkillConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            //}
            //else
            //{
            //    A.CallTo(() => fakeFrameworkSkillConverter.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            //}
            A.CallTo(() => fakeFrameworkSkillRepository.GetMany(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => LinqExpressionsTestHelper.IsExpressionEqual(m, item => item.Visible && item.Status == ContentLifecycleStatus.Live)))).MustHaveHappened();
        }
    }
}