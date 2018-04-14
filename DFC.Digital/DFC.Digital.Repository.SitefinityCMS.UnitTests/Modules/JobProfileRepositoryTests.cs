using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
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
            var urlName = "testURLName";
            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);
            jobProfileRepository.GetByUrlName(urlName);

            //A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => ExpressionEqualityComparer.Instance.Equals()))).MustHaveHappened();
            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>.That.Matches(m => IsExpressionEqual(m, item => item.UrlName == urlName && item.Status == ContentLifecycleStatus.Live && item.Visible == true)))).MustHaveHappened();
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

            if (x.NodeType != y.NodeType)
            {
                return false;
            }

            switch (x.NodeType)
            {
                case ExpressionType.Equal:
                case ExpressionType.AndAlso:
                    var hasMatched = ExpressionComparer(((BinaryExpression)x).Left, ((BinaryExpression)y).Left)
                        && ExpressionComparer(((BinaryExpression)x).Right, ((BinaryExpression)y).Right);
                    return hasMatched;

                case ExpressionType.Lambda:
                    var hasMatched2 = ExpressionComparer(((LambdaExpression)x).Body, ((LambdaExpression)y).Body);
                    return hasMatched2;

                case ExpressionType.Convert:
                    var hasMatched3 = ExpressionComparer(((UnaryExpression)x).Operand, ((UnaryExpression)y).Operand);
                    return hasMatched3;

                case ExpressionType.MemberAccess:
                    MemberExpression mex = (MemberExpression)x;
                    MemberExpression mey = (MemberExpression)y;
                    var hasMatched4 = mex.Member == mey.Member;
                    if (!hasMatched4)
                    {
                        var left = Expression.Lambda<Func<string>>(x).Compile().Invoke();
                        var right = Expression.Lambda<Func<string>>(y).Compile().Invoke();
                        hasMatched4 = left == right;
                    }

                    return hasMatched4;

                case ExpressionType.Constant:
                    var hasMatched5 = ((ConstantExpression)x).Value.ToString() == ((ConstantExpression)y).Value.ToString();
                    return hasMatched5;

                default:
                    throw new NotImplementedException($"{x.NodeType}-{x.GetType().Name}-{x.Type.Name}-{x}");
            }
        }
    }
}