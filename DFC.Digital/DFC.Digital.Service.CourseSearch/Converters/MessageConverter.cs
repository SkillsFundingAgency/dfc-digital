using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.CourseSearchProvider.CourseSearchServiceApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DFC.Digital.Service.CourseSearchProvider
{
    internal static class MessageConverter
    {
        internal static CourseListInput GetCourseListInput(string request)
        {
            var apiRequest = new CourseListInput
            {
                CourseListRequest = new CourseListRequestStructure
                {
                    CourseSearchCriteria = new SearchCriteriaStructure
                    {
                        APIKey = ConfigurationManager.AppSettings[Constants.CourseSearchApiKey],
                        SubjectKeyword = request,
                        EarliestStartDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        AttendanceModes = Convert.ToString(ConfigurationManager.AppSettings[Constants.CourseSearchAttendanceModes])?.Split(',')
                    },
                    RecordsPerPage = ConfigurationManager.AppSettings[Constants.CourseSearchPageSize],
                    PageNo = "1",
                    SortBy = SortType.A,
                    SortBySpecified = true
                }
            };

            return apiRequest;
        }

        internal static IEnumerable<Course> ConvertToCourse(this CourseListOutput apiResult)
        {
            var result = apiResult?.CourseListResponse?.CourseDetails?.Select(c =>
                new Course
                {
                    Title = c.Course.CourseTitle,
                    Location = (c.Opportunity.Item as VenueInfo)?.VenueAddress.Town,
                    ProviderName = c.Provider.ProviderName,
                    StartDate = Convert.ToDateTime(c.Opportunity.StartDate.Item),
                    CourseId = c.Course.CourseID
                });

            return result ?? Enumerable.Empty<Course>();
        }

        internal static IEnumerable<Course> ConvertToSearchCourse(this CourseListOutput apiResult)
        {
            var result = apiResult?.CourseListResponse?.CourseDetails?.Select(c =>
                new Course
                {
                    Title = c.Course.CourseTitle,
                    LocationDetails = GetVenue(c.Opportunity.Item as VenueInfo),
                    ProviderName = c.Provider.ProviderName,
                    StartDateLabel = c.Opportunity.StartDate.Item,
                    CourseId = c.Course.CourseID,
                    AttendanceMode = c.Opportunity.AttendanceMode,
                    AttendancePattern = c.Opportunity.AttendancePattern,
                    QualificationLevel = c.Course.QualificationLevel,
                    StudyMode = c.Opportunity.StudyMode,
                    AdvancedLearnerLoansOffered = c.Provider.TFPlusLoans
                });

            return result ?? Enumerable.Empty<Course>();
        }

        internal static CourseDetails ConvertToCourseDetails(this CourseDetailOutput apiResult, string oppurtunityId, string courseId)
        {
            var apiCourseDetail = apiResult.CourseDetails?.Single(co => co.Course.CourseID == courseId);
            var activeOpportunity = GetActiveOpportunity(oppurtunityId, apiCourseDetail);
            var courseDetails = GetCourseDetailsData(apiCourseDetail, activeOpportunity);

            return courseDetails;
        }

        private static OpportunityDetail GetActiveOpportunity(string oppurtunityId, CourseDetailStructure apiCourseDetail)
        {
            return (!string.IsNullOrEmpty(oppurtunityId)) ?
                      apiCourseDetail?.Opportunity.SingleOrDefault(op => op.OpportunityId == oppurtunityId)
                      : apiCourseDetail?.Opportunity.FirstOrDefault();
        }

        private static CourseDetails GetCourseDetailsData(CourseDetailStructure apiCourseDetail, OpportunityDetail activeOpportunity)
        {
            return apiCourseDetail?.Course is null ? null : new CourseDetails
            {
                Title = apiCourseDetail.Course.CourseTitle,
                Description = apiCourseDetail.Course.CourseSummary,
                EntryRequirements = apiCourseDetail.Course.EntryRequirements,
                AssessmentMethod = apiCourseDetail.Course.AssessmentMethod,
                EquipmentRequired = apiCourseDetail.Course.EquipmentRequired,
                QualificationName = apiCourseDetail.Course.QualificationTitle,
                QualificationLevel = apiCourseDetail.Course.QualificationLevel,
                VenueDetails = GetVenueData(apiCourseDetail.Venue, activeOpportunity),
                ProviderDetails = GetProviderDetailsData(apiCourseDetail.Provider),
                Oppurtunities = GetOpportunities(apiCourseDetail, activeOpportunity?.OpportunityId),
                CourseLink = apiCourseDetail.Course.URL,
                CourseId = apiCourseDetail.Course.CourseID,
                Cost = activeOpportunity?.Price,
                StartDateLabel = activeOpportunity?.StartDate.Item,
                AttendanceMode = activeOpportunity?.AttendanceMode,
                AttendancePattern = activeOpportunity?.AttendancePattern,
                StudyMode = activeOpportunity?.StudyMode,
                Duration = $"{activeOpportunity?.Duration?.DurationValue} {activeOpportunity?.Duration?.DurationUnit}"
            };
        }

        private static ProviderDetails GetProviderDetailsData(ProviderDetail provider)
        {
            return new ProviderDetails
            {
                EmailAddress = provider.Email,
                Website = provider.Website,
                AddressLine = provider.ProviderAddress.Address_line_1,
                AddressLine2 = provider.ProviderAddress.Address_line_2,
                Town = provider.ProviderAddress.Town,
                County = provider.ProviderAddress.County,
                PostCode = provider.ProviderAddress.PostCode,
                Longitude = provider.ProviderAddress.Longitude,
                Latitude = provider.ProviderAddress.Latitude,
                PhoneNumber = provider.Phone,
                Name = provider.ProviderName,
                LearnerSatisfactionSpecified = provider.FEChoices_LearnerSatisfactionSpecified,
                EmployerSatisfactionSpecified = provider.FEChoices_EmployerSatisfactionSpecified,
                LearnerSatisfaction = provider.FEChoices_LearnerSatisfaction,
                EmployerSatisfaction = provider.FEChoices_EmployerSatisfaction
            };
        }

        private static Venue GetVenueData(VenueDetail[] venue, OpportunityDetail activeOpportunity)
        {
            var venueData = venue?.Where(v => v.VenueID.ToString() == activeOpportunity?.Items[0]).FirstOrDefault();
            return venueData is null
                ? null
                : new Venue
                {
                    Location = new Address
                    {
                        AddressLine1 = venueData.VenueAddress?.Address_line_1,
                        AddressLine2 = venueData.VenueAddress?.Address_line_2,
                        County = venueData.VenueAddress?.County,
                        Town = venueData.VenueAddress?.Town,
                        Postcode = venueData.VenueAddress?.PostCode,
                        Longitude = venueData.VenueAddress?.Longitude,
                        Latitude = venueData.VenueAddress?.Latitude,
                    },
                    EmailAddress = venueData.Email,
                    PhoneNumber = venueData.Phone,
                    Website = venueData.Website,
                    VenueName = venueData.VenueName,
                    Fax = venueData.Fax,
                };
        }

        private static IList<Oppurtunity> GetOpportunities(CourseDetailStructure apiCourseDetail, string oppurtunityId)
        {
            return apiCourseDetail.Opportunity.Where(op => op.OpportunityId != oppurtunityId).Select(opp => new Oppurtunity
            {
                StartDate = opp.StartDate.Item,
                OppurtunityId = opp.OpportunityId,
                VenueName = apiCourseDetail.Venue.Where(venue => venue.VenueID.ToString() == opp.Items[0]).FirstOrDefault().VenueName
            }).ToList();
        }

        private static LocationDetails GetVenue(VenueInfo venueInfo)
        {
            if (venueInfo is null)
            {
                return null;
            }

            var address = new Dictionary<string, string>
            {
                [nameof(VenueInfo.VenueAddress.Address_line_1)] = venueInfo.VenueAddress.Address_line_1,
                [nameof(VenueInfo.VenueAddress.Address_line_2)] = venueInfo.VenueAddress.Address_line_2,
                [nameof(VenueInfo.VenueAddress.Town)] = venueInfo.VenueAddress.Town,
                [nameof(VenueInfo.VenueAddress.PostCode)] = venueInfo.VenueAddress.PostCode
            };

            return new LocationDetails
            {
                Distance = venueInfo.Distance,
                LocationAddress = string.Join(", ", address.Where(x => !string.IsNullOrWhiteSpace(x.Value)).Select(add => add.Value))
            };
        }
    }
}