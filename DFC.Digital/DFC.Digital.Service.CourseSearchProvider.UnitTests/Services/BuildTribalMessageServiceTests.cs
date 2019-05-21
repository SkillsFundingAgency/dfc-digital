using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class BuildTribalMessageServiceTests : MemberDataHelper
    {
        private readonly IConvertTribalCodes fakeConvertTribalCodesService;
        private readonly IConfigurationProvider fakeConfiguration;

        public BuildTribalMessageServiceTests()
        {
            fakeConfiguration = A.Fake<IConfigurationProvider>(ops => ops.Strict());
            fakeConvertTribalCodesService = A.Fake<IConvertTribalCodes>(ops => ops.Strict());
            SetupCalls();
        }

        [Theory]
        [MemberData(nameof(GetCourseSearchInputTestsInput))]
        public void GetCourseSearchInputTest(string courseName, CourseSearchProperties courseSearchProperties, CourseSearchFilters courseSearchFilters, CourseListInput expectedCourseListInput)
        {
            // Assign
            var buildTribalMessageService = new BuildTribalMessageService(fakeConvertTribalCodesService, fakeConfiguration);

            //Act
            var result = buildTribalMessageService.GetCourseSearchInput(courseName, courseSearchProperties, courseSearchFilters);

            //Assert
            result.Should().BeEquivalentTo(expectedCourseListInput);
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalAttendanceModes(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalAttendancePatterns(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalQualificationLevels(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalStudyModes(A<string>._)).MustHaveHappened();

            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).MustHaveHappened(1, Times.Exactly);
        }

        [Theory]
        [MemberData(nameof(GetCourseDetailInputTestsInput))]
        public void GetCourseDetailInputTest(string courseId, CourseDetailInput expectedCourseDetailInput)
        {
            // Assign
            var buildTribalMessageService = new BuildTribalMessageService(fakeConvertTribalCodesService, fakeConfiguration);

            //Act
            var result = buildTribalMessageService.GetCourseDetailInput(courseId);

            //Assert
            result.Should().BeEquivalentTo(expectedCourseDetailInput);
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).MustHaveHappened(1, Times.Exactly);
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeConfiguration.GetConfig<string>(A<string>._)).Returns("apiKey");
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalAttendanceModes(A<string>._)).Returns(null);
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalAttendancePatterns(A<string>._)).Returns(null);
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalQualificationLevels(A<string>._)).Returns(null);
            A.CallTo(() => fakeConvertTribalCodesService.GetTribalStudyModes(A<string>._)).Returns(null);
        }
    }
}