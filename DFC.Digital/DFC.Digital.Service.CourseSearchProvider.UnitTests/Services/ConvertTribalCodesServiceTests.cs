using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using Xunit;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class ConvertTribalCodesServiceTests : MemberDataHelper
    {
        private readonly ICourseBusinessRules fakeBusinessRules;

        public ConvertTribalCodesServiceTests()
        {
            fakeBusinessRules = A.Fake<ICourseBusinessRules>(ops => ops.Strict());
        }

        [Theory]
        [MemberData(nameof(GetTribalAttendanceModesTestInput))]
        public void GetTribalAttendanceModesTest(CourseType attendanceMode, string[] expectedResult)
        {
            // Assign
            var convertTribalCodesService = new ConvertTribalCodes(fakeBusinessRules);

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
            var convertTribalCodesService = new ConvertTribalCodes(fakeBusinessRules);

            //Act
            var result = convertTribalCodesService.GetTribalStudyModes(studyMode);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(GetTribalQualificationLevelsTestsInput))]
        public void GetTribalQualificationLevelsTest(StartDate startDate, DateTime startDateFrom, string expectedResult)
        {
            // Assign
            var convertTribalCodesService = new ConvertTribalCodes(fakeBusinessRules);

            //Act
            var result = convertTribalCodesService.GetEarliestStartDate(startDate, startDateFrom);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}