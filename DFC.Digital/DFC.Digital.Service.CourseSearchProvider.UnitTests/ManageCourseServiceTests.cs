using DFC.Digital.Data.Model;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class ManageCourseServiceTests
    {
        /// <summary>
        /// Selects the job profile courses maximum returned test.
        /// </summary>
        /// <param name="courseCount">The course count.</param>
        [Theory]
        [InlineData(2)]
        [InlineData(20)]
        public void SelectJobProfileCoursesMaxReturnedTest(int courseCount)
        {
            //Arrange
            var coursesInput = GeneratedNewCourses(courseCount);
            var manageCoursesFake = new CourseOpportunityBuilder();

            //Act
            var results = manageCoursesFake.SelectCoursesForJobProfile(coursesInput);

            //Assert
            results.Count().Should().BeLessOrEqualTo(2);
        }

        /// <summary>
        /// Selects the job profile courses different provider test.
        /// Scenario: [DFC-1508 - A2] Return courses that have the earliest start date with different providers
        /// </summary>
        [Fact]
        public void SelectJobProfileCoursesDifferentProviderTest()
        {
            //Arrange
            var coursesInput = GenerateDummyCourses();
            var manageCoursesFake = new CourseOpportunityBuilder();

            //Act
            var results = manageCoursesFake.SelectCoursesForJobProfile(coursesInput);

            //Assert
            results.Select(course => course.ProviderName).Distinct().Count().Should().BeGreaterThan(1);
            results.First().Should().BeEquivalentTo(coursesInput.First());
            results.Last().Should().BeEquivalentTo(coursesInput.First(i => i.ProviderName.Equals("Provider 2")));
        }

        /// <summary>
        /// Selects the job profile courses same provider test.
        ///  [DFC-1508 - A1] Return training courses by same provider if they're the only 2 available
        /// </summary>
        /// <param name="courseCount">The course count.</param>
        [Theory]
        [InlineData(2)]
        [InlineData(20)]
        public void SelectJobProfileCoursesSameProviderTest(int courseCount)
        {
            //Arrange
            var coursesInput = GeneratedNewCourses(courseCount);
            var manageCoursesFake = new CourseOpportunityBuilder();

            //Act
            var results = manageCoursesFake.SelectCoursesForJobProfile(coursesInput);

            //Assert
            results.Count().Should().BeLessOrEqualTo(2);
            results.Select(course => course.ProviderName).Distinct().Count().Should().Be(1);
            results.First().Should().BeEquivalentTo(coursesInput.First());
            results.Last().Should().BeEquivalentTo(coursesInput.Skip(1).First());
        }

        private IEnumerable<Course> GeneratedNewCourses(int jobCount)
        {
            var courses = new List<Course>();
            for (var i = 0; i < jobCount; i++)
            {
                courses.Add(new Course
                {
                    Title = $"dummy {nameof(Course.Title)}",
                    Location = $"dummy {nameof(Course.Location)}",
                    CourseId = $"dummy {nameof(Course.CourseId)}",
                    StartDate = DateTime.Today.AddDays(-i),
                    ProviderName = $"dummy {nameof(Course.ProviderName)}",
                });
            }

            return courses;
        }

        private IEnumerable<Course> GenerateDummyCourses()
        {
            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)}",
                Location = $"dummy {nameof(Course.Location)}",
                CourseId = $"dummy {nameof(Course.CourseId)}",
                StartDate = DateTime.Today,
                ProviderName = "Provider 1",
            };

            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)}",
                Location = $"dummy {nameof(Course.Location)}",
                CourseId = $"dummy {nameof(Course.CourseId)}",
                StartDate = DateTime.Today.AddDays(20),
                ProviderName = "Provider 2",
            };

            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)} 1",
                Location = $"dummy {nameof(Course.Location)} 1",
                CourseId = $"dummy {nameof(Course.CourseId)} 1",
                StartDate = DateTime.Today.AddDays(1),
                ProviderName = "Provider 1",
            };

            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)} 2",
                Location = $"dummy {nameof(Course.Location)} 2",
                CourseId = $"dummy {nameof(Course.CourseId)} 2",
                StartDate = DateTime.Today.AddDays(19),
                ProviderName = "Provider 2",
            };

            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)} 2",
                Location = $"dummy {nameof(Course.Location)} 2",
                CourseId = $"dummy {nameof(Course.CourseId)} 2",
                StartDate = DateTime.Today.AddDays(0.5),
                ProviderName = "Provider 1",
            };

            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)} 1",
                Location = $"dummy {nameof(Course.Location)} 1",
                CourseId = $"dummy {nameof(Course.CourseId)} 1",
                StartDate = DateTime.Today.AddDays(21),
                ProviderName = "Provider 2",
            };
        }
    }
}