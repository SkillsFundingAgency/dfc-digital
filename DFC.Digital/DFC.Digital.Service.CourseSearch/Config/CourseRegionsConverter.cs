using AutoMapper;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using FAC = DFC.FindACourseClient;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal class CourseRegionsConverter : IValueConverter<List<FAC.SubRegion>, IList<CourseRegion>>
    {
        public IList<CourseRegion> Convert(List<FAC.SubRegion> subRegions, ResolutionContext context)
        {
            List<CourseRegion> courseRegions = new List<CourseRegion>();
            var groupedRegions = subRegions.GroupBy(g => g.ParentRegion.Name);

            foreach (var region in groupedRegions)
            {
                CourseRegion courseRegion = new CourseRegion() { Region = region.Key };
                var areas = region.Select(r => r.Name);
                courseRegion.Area = string.Join(", ", areas);
                courseRegions.Add(courseRegion);
            }

            return courseRegions.OrderBy(r => r.Region).ToList();
        }
    }
}
