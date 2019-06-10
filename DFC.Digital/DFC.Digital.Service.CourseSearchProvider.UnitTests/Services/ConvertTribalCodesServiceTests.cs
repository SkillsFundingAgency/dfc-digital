using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using Xunit;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class ConvertTribalCodesServiceTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(GetTribalAttendanceModesTestInput))]
        public void GetTribalAttendanceModesTest(CourseType attendanceMode, string[] expectedResult)
        {
            // Assign
            var convertTribalCodesService = new TribalCodesConverter();

            //Act
            var result = convertTribalCodesService.GetTribalAttendanceModes(attendanceMode);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(GetTribalStudyModesTestsInput))]
        public void GetTribalStudyModesTest(CourseHours studyMode, string[] expectedResult)
        {
            // Assign
            var convertTribalCodesService = new TribalCodesConverter();

            //Act
            var result = convertTribalCodesService.GetTribalStudyModes(studyMode);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}