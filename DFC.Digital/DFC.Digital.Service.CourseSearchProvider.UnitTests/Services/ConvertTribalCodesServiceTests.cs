using DFC.Digital.Data.Model;
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
            var convertTribalCodesService = new ConvertTribalCodes();

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
            var convertTribalCodesService = new ConvertTribalCodes();

            //Act
            var result = convertTribalCodesService.GetTribalStudyModes(studyMode);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(GetTribalQualificationLevelsTestsInput))]
        public void GetTribalQualificationLevelsTest(StartDate startDate, string startDateFrom, string expectedResult)
        {
            // Assign
            var convertTribalCodesService = new ConvertTribalCodes();

            //Act
            var result = convertTribalCodesService.GetEarliestStartDate(startDate, startDateFrom);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}