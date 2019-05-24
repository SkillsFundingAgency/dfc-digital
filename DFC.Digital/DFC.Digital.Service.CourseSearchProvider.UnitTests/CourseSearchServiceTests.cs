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

namespace DFC.Digital.Service.CourseSearchProvider.UnitTests
{
    public class CourseSearchServiceTests
    {
        [Theory]
        [InlineData("keywords", true)]
        [InlineData("keywords", false)]
        public async Task CourseSearchServiceTestAsync(string keywords, bool coursesAvailable)
        {
            //Arrange
            var serviceHelperFake = A.Fake<IServiceHelper>();
            var manageCoursesFake = A.Fake<ICourseOpportunityBuilder>(ops => ops.Strict());
            var courseSearchAuditRepository = A.Fake<IAuditRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var fakePolicy = A.Fake<ITolerancePolicy>();
            var fakeMessageBuilder = A.Fake<IBuildTribalMessage>();

            //Setup Calls and Dummies
            A.CallTo(() => serviceHelperFake.UseAsync(A<Func<ServiceInterface, Task<CourseListOutput>>>._, Constants.CourseSearchEndpointConfigName)).Returns(coursesAvailable ? GetDummyCourseOutput() : new CourseListOutput());
            A.CallTo(() => manageCoursesFake.SelectCoursesForJobProfile(A<IEnumerable<Course>>._))
                .Returns(GenerateDummyCourses());
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).DoesNothing();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListOutput>._)).DoesNothing();

            var courseSearchService = new CourseSearchService(manageCoursesFake, serviceHelperFake, courseSearchAuditRepository, loggerFake, fakePolicy, fakeMessageBuilder);

            //Act
            await courseSearchService.GetCoursesAsync(keywords);

            //Assert
            A.CallTo(() => serviceHelperFake.UseAsync(A<Func<ServiceInterface, Task<CourseListOutput>>>._, Constants.CourseSearchEndpointConfigName)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListOutput>._)).MustHaveHappened();
            if (coursesAvailable)
            {
                A.CallTo(() => manageCoursesFake.SelectCoursesForJobProfile(A<IEnumerable<Course>>.That.Matches(m => m.Any()))).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => manageCoursesFake.SelectCoursesForJobProfile(A<IEnumerable<Course>>.That.IsEmpty())).MustHaveHappened();
            }
        }

        [Fact]
        public async Task CourseSearchServiceFailureTestAsync()
        {
            //Arrange
            var serviceHelperFake = A.Fake<IServiceHelper>();
            var manageCoursesFake = A.Fake<ICourseOpportunityBuilder>(ops => ops.Strict());
            var courseSearchAuditRepository = A.Fake<IAuditRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var fakePolicy = A.Fake<ITolerancePolicy>();
            var fakeMessageBuilder = A.Fake<IBuildTribalMessage>();

            //Setup Calls ANY exception will do as we are catching all of them for this call
            A.CallTo(() => serviceHelperFake.UseAsync(A<Func<ServiceInterface, Task<CourseListOutput>>>._, Constants.CourseSearchEndpointConfigName)).Throws(new HttpException("failed cause I want to"));

            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).DoesNothing();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<Exception>._)).DoesNothing();
            A.CallTo(() => loggerFake.ErrorJustLogIt(A<string>._, A<Exception>._)).DoesNothing();

            var courseSearchService = new CourseSearchService(manageCoursesFake, serviceHelperFake, courseSearchAuditRepository, loggerFake, fakePolicy, fakeMessageBuilder);

            //Act
            var results = await courseSearchService.GetCoursesAsync("CourseKeyWords");

            //Assert
            results.Should().BeEmpty();
            A.CallTo(() => serviceHelperFake.UseAsync(A<Func<ServiceInterface, Task<CourseListOutput>>>._, Constants.CourseSearchEndpointConfigName)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<CourseListInput>._)).MustHaveHappened();
            A.CallTo(() => courseSearchAuditRepository.CreateAudit(A<Exception>._)).MustHaveHappened();
            A.CallTo(() => loggerFake.ErrorJustLogIt(A<string>._, A<Exception>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData(true, ServiceState.Green)]
        [InlineData(false, ServiceState.Amber)]
        public async Task GetServiceStatusAsyn(bool coursesAvailable, ServiceState expectedServiceStatus)
        {
            //Arrange
            var serviceHelperFake = A.Fake<IServiceHelper>();
            var courseSearchAuditRepository = A.Fake<IAuditRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var manageCoursesFake = A.Fake<ICourseOpportunityBuilder>(ops => ops.Strict());
            var fakePolicy = A.Fake<ITolerancePolicy>();
            var fakeMessageBuilder = A.Fake<IBuildTribalMessage>();

            //Setup Calls and Dummies
            A.CallTo(() => serviceHelperFake.UseAsync(A<Func<ServiceInterface, Task<CourseListOutput>>>._, Constants.CourseSearchEndpointConfigName)).Returns(coursesAvailable ? GetDummyCourseOutput() : new CourseListOutput());
            A.CallTo(() => loggerFake.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception acctivity id");

            var courseSearchService = new CourseSearchService(manageCoursesFake, serviceHelperFake, courseSearchAuditRepository, loggerFake, fakePolicy, fakeMessageBuilder);

            //Act
            var serviceStatus = await courseSearchService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().Be(expectedServiceStatus);
        }

        [Fact]
        public async Task GetServiceStatusExceptionAsync()
        {
            //Arrange
            var serviceHelperFake = A.Fake<IServiceHelper>();
            var courseSearchAuditRepository = A.Fake<IAuditRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var manageCoursesFake = A.Fake<ICourseOpportunityBuilder>(ops => ops.Strict());
            var fakePolicy = A.Fake<ITolerancePolicy>();
            var fakeMessageBuilder = A.Fake<IBuildTribalMessage>();

            //Setup Calls and Dummies
            A.CallTo(() => serviceHelperFake.Use(A<Func<ServiceInterface, CourseListOutput>>._, "Bad EndPoint")).Returns(GetDummyCourseOutput());
            A.CallTo(() => loggerFake.LogExceptionWithActivityId(A<string>._, A<Exception>._)).Returns("Exception logged");

            var courseSearchService = new CourseSearchService(manageCoursesFake, serviceHelperFake, courseSearchAuditRepository, loggerFake, fakePolicy, fakeMessageBuilder);

            //Act
            var serviceStatus = await courseSearchService.GetCurrentStatusAsync();

            //Asserts
            serviceStatus.Status.Should().NotBe(ServiceState.Green);
            serviceStatus.Notes.Should().Contain("Exception");
            A.CallTo(() => loggerFake.LogExceptionWithActivityId(A<string>._, A<Exception>._)).MustHaveHappened();
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