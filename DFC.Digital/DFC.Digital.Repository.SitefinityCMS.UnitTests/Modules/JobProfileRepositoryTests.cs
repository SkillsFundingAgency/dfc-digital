using DFC.Digital.Data.Model;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileRepositoryTests
    {
        private IDynamicModuleConverter<JobProfile> fakeJobProfileConverter;
        private IDynamicModuleRepository<JobProfile> fakeRepository;

        public JobProfileRepositoryTests()
        {
            fakeJobProfileConverter = A.Fake<IDynamicModuleConverter<JobProfile>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();
        }

        [Fact]
        public void GetByUrlNameForPreviewTest()
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);
            jobProfileRepository.GetByUrlNameForPreview("testURLName");

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).MustHaveHappened();
        }

        [Fact]
        public void GetByUrlNameForSearchIndexTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void GetContentTypeTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void GetProviderNameTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact]
        public void GetByUrlNameTest()
        {
            var dummyJobProfile = A.Dummy<JobProfile>();
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);
            jobProfileRepository.GetByUrlName("testURLName");

            //A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => ExpressionEqualityComparer.Instance.Equals()))).MustHaveHappened();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => IsExpressionEqual(m, item => item.UrlName == "testURLName" && item.Status == ContentLifecycleStatus.Live && item.Visible == true)))).MustHaveHappened();
        }

        private static bool IsExpressionEqual(Expression<Func<DynamicContent, bool>> x, Expression<Func<DynamicContent, bool>> y)
        {
            return ExpressionComparer(x, y);
        }

        private static bool ExpressionComparer(Expression x, Expression y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.NodeType != y.NodeType || x.Type != y.Type)
            {
                return false;
            }

            switch (x.NodeType)
            {
                case ExpressionType.AndAlso:
                case ExpressionType.Lambda:
                    return ExpressionComparer(((LambdaExpression)x).Body, ((LambdaExpression)y).Body);
                case ExpressionType.MemberAccess:
                    MemberExpression mex = (MemberExpression)x, mey = (MemberExpression)y;
                    return mex.Member == mey.Member; // should really test down-stream expression
                default:
                    throw new NotImplementedException(x.NodeType.ToString());
            }
        }
    }
}