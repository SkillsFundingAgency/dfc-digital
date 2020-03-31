﻿using AutoMapper;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using FAC = DFC.FindACourseClient;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class CourseSearchApiAutomapperProfileTests
    {
        private readonly FAC.CourseDetails clientCourseDetails;
        private readonly IMapper mapper;

        public CourseSearchApiAutomapperProfileTests()
        {
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CourseSearchApiAutomapperProfile>();
            }).CreateMapper();

            clientCourseDetails = new FAC.CourseDetails();
            clientCourseDetails.Title = nameof(FAC.CourseDetails.Title);
            clientCourseDetails.SubRegions = new List<FAC.SubRegion>();
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("AA", "AA")]
        [InlineData("1.23", "£1.23")]
        public void AutoMapperProfileCourseCostsConverter(string inCost, string expectedFormatedCost)
        {
            clientCourseDetails.Cost = inCost;

            var mapped = mapper.Map<CourseDetails>(clientCourseDetails);

            mapped.Cost.Should().BeEquivalentTo(expectedFormatedCost);
        }

        [Fact]
        public void AutoMapperProfileCourseRegionsConverter()
        {
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
                new List<CourseRegion>().Add(expectedCourseRegion);
            }

            var mapped = mapper.Map<CourseDetails>(clientCourseDetails);

            mapped.Title.Should().Be(clientCourseDetails.Title);
            mapped.CourseRegions.Should().BeEquivalentTo(new List<CourseRegion>());
        }
    }
}
