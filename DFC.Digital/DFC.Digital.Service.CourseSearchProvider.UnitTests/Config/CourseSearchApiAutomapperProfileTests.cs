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
        [Fact]
        public void AutoMapperProfileCourseRegionsConverter()
        {
            Mapper.Initialize(m => m.AddProfile<CourseSearchApiAutomapperProfile>());
            var clientCourseDetails = new FAC.CourseDetails();
            clientCourseDetails.Title = nameof(FAC.CourseDetails.Title);
            clientCourseDetails.SubRegions = new List<FAC.SubRegion>();

            var expectedVenuesOutput = new List<CourseRegion>();

            //Build the input and expected output
            for (int regions = 0; regions < 2; regions++)
            {
                var parentRegion = new FAC.ParentRegion() { Name = $"Region-{regions}" };
                var expectedCourseRegion = new CourseRegion() { Region = parentRegion.Name };
                for (int subRegions = 0; subRegions < 2; subRegions++)
                {
                    var subRegion = new FAC.SubRegion() { Name = $"SubRegion-{regions}-{subRegions}", ParentRegion = parentRegion };
                    clientCourseDetails.SubRegions.Add(subRegion);
                    expectedCourseRegion.Area += $", {subRegion.Name}";
                }

                expectedCourseRegion.Area = expectedCourseRegion.Area.Substring(2);
                expectedVenuesOutput.Add(expectedCourseRegion);
            }

            var mapped = Mapper.Map<CourseDetails>(clientCourseDetails);

            mapped.Title.Should().Be(clientCourseDetails.Title);
            mapped.CourseRegions.Should().BeEquivalentTo(expectedVenuesOutput);
        }
    }
}
