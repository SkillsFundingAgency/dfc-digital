using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.UnitTests;
using FluentAssertions;
using System;
using Xunit;

namespace DFC.Digital.Service.CourseSearchProvider.Tests
{
    public class CourseBusinessRulesProcessorTests : MemberDataHelper
    {
        [Theory]
        [MemberData(nameof(GetEarliestStartDateTestsInput))]
        public void GetEarliestStartDateTest(StartDate startDate, DateTime startDateFrom, string expectedResult)
        {
            // Assign
            var courseBusinessRules = new CourseBusinessRulesProcessor();

            //Act
            var result = courseBusinessRules.GetEarliestStartDate(startDate, startDateFrom);

            //Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}