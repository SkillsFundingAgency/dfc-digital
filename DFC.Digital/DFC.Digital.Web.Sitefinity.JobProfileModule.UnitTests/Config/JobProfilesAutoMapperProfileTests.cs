using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Config
{
    public class JobProfilesAutoMapperProfileTests
    {
        private IMapper mapper;

        public JobProfilesAutoMapperProfileTests()
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<JobProfilesAutoMapperProfile>());
            mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void MapSearchResultItem_JobProfileIndex_ToJobProfileSearchResultItemViewModel_ShouldDisplayCaveatAutoMapperTest()
        {
            var caveatValue = "no";
            var source = new SearchResultItem<JobProfileIndex>
            {
                Rank = 1,
                ResultItem = A.Dummy<JobProfileIndex>(),
                Score = 2,
            };

            source.ResultItem.TrainingRoutes = new List<string> { caveatValue };

            var dest = mapper.Map<JobProfileSearchResultItemViewModel>(source, opts =>
            {
                opts.Items.Add(nameof(PsfSearchController.CaveatFinderIndexFieldName), nameof(JobProfileIndex.TrainingRoutes));
                opts.Items.Add(nameof(PsfSearchController.CaveatFinderIndexValue), caveatValue);
            });

            dest.ShouldDisplayCaveat.Should().BeTrue();
        }
    }
}
