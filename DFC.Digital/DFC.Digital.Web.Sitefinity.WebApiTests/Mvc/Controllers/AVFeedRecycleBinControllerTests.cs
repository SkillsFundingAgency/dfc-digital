using DFC.Digital.Data.Interfaces;
using FakeItEasy;
using FluentAssertions;
using System.Net;
using System.Web.Http.Results;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.WebApi.Mvc.Controllers.Tests
{
    public class AVFeedRecycleBinControllerTests
    {
        [Theory]
        [InlineData(false, HttpStatusCode.PartialContent)]
        [InlineData(true, HttpStatusCode.OK)]
        public void DeleteTest(bool isLastPage, HttpStatusCode expected)
        {
            var fakeRecycleBinRepo = A.Fake<IRecycleBinRepository>();
            var api = new AVFeedRecycleBinController(fakeRecycleBinRepo);
            A.CallTo(() => fakeRecycleBinRepo.DeleteVacanciesPermanently(A<int>._)).Returns(isLastPage);

            var result = api.Delete(5);

            // Assert
            result.Should().BeOfType<StatusCodeResult>();

            ((StatusCodeResult)result).StatusCode.Should().Be(expected);
            A.CallTo(() => fakeRecycleBinRepo.DeleteVacanciesPermanently(A<int>.That.IsEqualTo(5))).MustHaveHappenedOnceExactly();
        }
    }
}