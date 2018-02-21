using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xunit;

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class CourseSearchServiceTests
    {
        [Theory]
        [InlineData("keywords", true)]
        [InlineData("keywords", false)]
        public void CourseSearchServiceTest(string keywords, bool coursesAvailable)
        {
            //Arrange
            var serviceHelperFake = A.Fake<IServiceHelper>();
            var manageCoursesFake = A.Fake<ICourseOpportunityBuilder>(ops => ops.Strict());
            var courseSearchAuditRepository = A.Fake<IAuditRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());

            //Setup Calls and Dummies
            A.CallTo(() => serviceHelperFake.Use(A<Func<ServiceInterface, CourseListOutput>>._, Constants.CourseSerachEndpointConfigName)).Returns(coursesAvailable ? GetDummyCourseOutput() : new CourseListOutput());
            A.CallTo(() => manageCoursesFake.SelectCoursesForJobProfile(A<IEnumerable<Course>>._))
                .Returns(GenerateDummyCourses());
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).DoesNothing();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListOutput>._)).DoesNothing();

            var courseSearchService = new CourseSearchService(manageCoursesFake, serviceHelperFake, courseSearchAuditRepository, loggerFake);

            //Act
            courseSearchService.GetCourses(keywords);

            //Assert
            A.CallTo(() => serviceHelperFake.Use(A<Func<ServiceInterface, CourseListOutput>>._, Constants.CourseSerachEndpointConfigName)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListOutput>._)).MustHaveHappened();
            if (coursesAvailable)
            {
                A.CallTo(() => manageCoursesFake.SelectCoursesForJobProfile(A<IEnumerable<Course>>.That.Matches(m => m.Count() > 0))).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => manageCoursesFake.SelectCoursesForJobProfile(A<IEnumerable<Course>>.That.IsEmpty())).MustHaveHappened();
            }
        }

        [Fact]
        public void CourseSearchServiceFailureTest()
        {
            //Arrange
            var serviceHelperFake = A.Fake<IServiceHelper>();
            var manageCoursesFake = A.Fake<ICourseOpportunityBuilder>(ops => ops.Strict());
            var courseSearchAuditRepository = A.Fake<IAuditRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());

            //Setup Calls ANY exception will do as we are catching all of them for this call
            A.CallTo(() => serviceHelperFake.Use(A<Func<ServiceInterface, CourseListOutput>>._, Constants.CourseSerachEndpointConfigName)).Throws(new HttpException("failed cause I want to"));

            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).DoesNothing();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<Exception>._)).DoesNothing();
            A.CallTo(() => loggerFake.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();

            var courseSearchService = new CourseSearchService(manageCoursesFake, serviceHelperFake, courseSearchAuditRepository, loggerFake);

            //Act
            var results = courseSearchService.GetCourses("CourseKeyWords");

            //Assert
            results.Should().BeEmpty();
            A.CallTo(() => serviceHelperFake.Use(A<Func<ServiceInterface, CourseListOutput>>._, Constants.CourseSerachEndpointConfigName)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<Exception>._)).MustHaveHappened();
            A.CallTo(() => loggerFake.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
        }

        private IEnumerable<Course> GenerateDummyCourses()
        {
            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)}",
                Location = $"dummy {nameof(Course.Location)}",
                CourseId = $"dummy {nameof(Course.CourseId)}",
                StartDate = DateTime.Now,
                ProviderName = "Provider 1",
            };

            yield return new Course
            {
                Title = $"dummy {nameof(Course.Title)}",
                Location = $"dummy {nameof(Course.Location)}",
                CourseId = $"dummy {nameof(Course.CourseId)}",
                StartDate = DateTime.Now.AddDays(20),
                ProviderName = "Provider 2",
            };
        }

        private CourseListOutput GetDummyCourseOutput()
        {
            return new CourseListOutput
            {
                CourseListResponse = new CourseListResponseStructure
                {
                    CourseDetails = new[]
                    {
                        new CourseStructure
                        {
                            Course = new CourseInfo
                            {
                                CourseTitle = nameof(CourseInfo.CourseTitle),
                                CourseID = nameof(CourseInfo.CourseID)
                            },
                            Provider = new ProviderInfo
                            {
                                ProviderName = "Provider 1"
                            },
                            Opportunity = new OpportunityInfo
                            {
                                StartDate = new StartDateType
                                {
                                    Item = DateTime.Now.ToString("dd MMMM yyyy")
                                },
                                Item = new VenueInfo
                                {
                                    VenueAddress = new AddressType
                                    {
                                        Town = nameof(AddressType.Town)
                                    }
                                }
                            }
                        },  new CourseStructure
                        {
                            Course = new CourseInfo
                            {
                                CourseTitle = nameof(CourseInfo.CourseTitle),
                                CourseID = nameof(CourseInfo.CourseID)
                            },
                            Provider = new ProviderInfo
                            {
                                ProviderName = "Provider 1"
                            },
                            Opportunity = new OpportunityInfo
                            {
                                StartDate = new StartDateType
                                {
                                    Item = DateTime.Now.ToString("dd MMMM yyyy")
                                },
                                Item = new VenueInfo
                                {
                                    VenueAddress = new AddressType
                                    {
                                        Town = nameof(AddressType.Town)
                                    }
                                }
                            }
                        }, new CourseStructure
                        {
                            Course = new CourseInfo
                            {
                                CourseTitle = nameof(CourseInfo.CourseTitle),
                                CourseID = nameof(CourseInfo.CourseID)
                            },
                            Provider = new ProviderInfo
                            {
                                ProviderName = "Provider 2"
                            },
                            Opportunity = new OpportunityInfo
                            {
                                StartDate = new StartDateType
                                {
                                    Item = DateTime.Now.AddDays(5).ToString("dd MMMM yyyy")
                                },
                                Item = new VenueInfo
                                {
                                    VenueAddress = new AddressType
                                    {
                                        Town = nameof(AddressType.Town)
                                    }
                                }
                            }
                        }, new CourseStructure
                        {
                            Course = new CourseInfo
                            {
                                CourseTitle = nameof(CourseInfo.CourseTitle),
                                CourseID = nameof(CourseInfo.CourseID)
                            },
                            Provider = new ProviderInfo
                            {
                                ProviderName = "Provider 2"
                            },
                            Opportunity = new OpportunityInfo
                            {
                                StartDate = new StartDateType
                                {
                                    Item = DateTime.Now.AddDays(20).ToString("dd MMMM yyyy")
                                },
                                Item = new VenueInfo
                                {
                                    VenueAddress = new AddressType
                                    {
                                        Town = nameof(AddressType.Town)
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}