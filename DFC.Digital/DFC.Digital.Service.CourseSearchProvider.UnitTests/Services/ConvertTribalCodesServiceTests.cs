using FluentAssertions;
using Xunit;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class ConvertTribalCodesServiceTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(GetTribalAttendanceModesTestInput))]
        public void GetTribalAttendanceModesTest(string attendanceMode, string[] expectedResult)
        {
            // Assign
            var convertTribalCodesService = new ConvertTribalCodesService();

            //Act
            var result = convertTribalCodesService.GetTribalAttendanceModes(attendanceMode);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(GetTribalStudyModesTestsInput))]
        public void GetTribalStudyModesTest(string studyMode, string[] expectedResult)
        {
            // Assign
            var convertTribalCodesService = new ConvertTribalCodesService();

            //Act
            var result = convertTribalCodesService.GetTribalStudyModes(studyMode);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(GetTribalQualificationLevelsTestsInput))]
        public void GetTribalQualificationLevelsTest(string qualificationLevel, string[] expectedResult)
        {
            // Assign
            var convertTribalCodesService = new ConvertTribalCodesService();

            //Act
            var result = convertTribalCodesService.GetTribalQualificationLevels(qualificationLevel);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(GetTribalAttendancePatternsTestsInput))]
        public void GetTribalAttendancePatternsTest(string attendancePattern, string[] expectedResult)
        {
            // Assign
            var convertTribalCodesService = new ConvertTribalCodesService();

            //Act
            var result = convertTribalCodesService.GetTribalAttendancePatterns(attendancePattern);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}