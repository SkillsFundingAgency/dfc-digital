using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xunit;
using FAC = DFC.FindACourseClient;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class CourseSearchApiAutomapperProfileTests
    {
        public CourseSearchApiAutomapperProfileTests()
        {
            Mapper.Reset();
            Mapper.Initialize(m => m.AddProfile<CourseSearchApiAutomapperProfile>());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("AA", "AA")]
        [InlineData("1.23", "£1.23")]
        public void AutoMapperProfileCourseCostsConverter(string inCost, string expectedFormatedCost)
        {
            var clientCourseDetails = new FAC.CourseDetails();
            clientCourseDetails.Cost = inCost;

            var mapped = Mapper.Map<CourseDetails>(clientCourseDetails);

            mapped.Cost.Should().BeEquivalentTo(expectedFormatedCost);
        }
    }
}
