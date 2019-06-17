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
            var apiCourseDetail = apiResult.CourseDetails?.SingleOrDefault(co => co.Course.CourseID == courseId);

            if (apiCourseDetail == null)
            {
                return null;
            }

            OpportunityDetail activeOppurtunity = (!string.IsNullOrEmpty(oppurtunityId)) ?
                         apiCourseDetail.Opportunity.SingleOrDefault(op => op.OpportunityId == oppurtunityId)
                         : activeOppurtunity = apiCourseDetail.Opportunity.FirstOrDefault();

            var venue = apiCourseDetail.Venue.Where(v => v.VenueID.ToString() == activeOppurtunity.Items[0])?.FirstOrDefault();

            return new CourseDetails
            {
                Title = apiCourseDetail.Course.CourseTitle,
                Description = apiCourseDetail.Course.CourseSummary,
                EntryRequirements = apiCourseDetail.Course.EntryRequirements,
                AssessmentMethod = apiCourseDetail.Course.AssessmentMethod,
                EquipmentRequired = apiCourseDetail.Course.EquipmentRequired,
                QualificationName = apiCourseDetail.Course.QualificationTitle,
                QualificationLevel = apiCourseDetail.Course.QualificationLevel,
                VenueDetails =
                new Venue
                {
                    EmailAddress = venue?.Email,
                    Location = new Address
                    {
                        AddressLine1 = venue?.VenueAddress.Address_line_1,
                        AddressLine2 = venue?.VenueAddress.Address_line_2,
                        County = venue?.VenueAddress.County,
                        Town = venue?.VenueAddress.Town,
                        Postcode = venue?.VenueAddress.PostCode,
                        Longitude = venue?.VenueAddress.Longitude,
                        Latitude = venue?.VenueAddress.Latitude,
                    },
                    PhoneNumber = venue?.Phone,
                    Website = venue?.Website,
                    VenueName = venue?.VenueName,
                    Fax = venue?.Fax,
                },
                ProviderDetails = new ProviderDetails
                {
                    EmailAddress = apiCourseDetail.Provider.Email,
                    AddressLine = apiCourseDetail.Provider.ProviderAddress.Address_line_1,
                    AddressLine2 = apiCourseDetail.Provider.ProviderAddress.Address_line_2,
                    Town = apiCourseDetail.Provider.ProviderAddress.Town,
                    County = apiCourseDetail.Provider.ProviderAddress.County,
                    PostCode = apiCourseDetail.Provider.ProviderAddress.PostCode,
                    Longitude = apiCourseDetail.Provider.ProviderAddress.Longitude,
                    Latitude = apiCourseDetail.Provider.ProviderAddress.Latitude,
                    PhoneNumber = apiCourseDetail.Provider.Phone,
                    Name = apiCourseDetail.Provider.ProviderName,
                    LearnerSatisfactionSpecified = apiCourseDetail.Provider.FEChoices_LearnerSatisfactionSpecified,
                    EmployerSatisfactionSpecified = apiCourseDetail.Provider.FEChoices_EmployerSatisfactionSpecified,
                    LearnerSatisfaction = apiCourseDetail.Provider.FEChoices_LearnerSatisfaction,
                    EmployerSatisfaction = apiCourseDetail.Provider.FEChoices_EmployerSatisfaction
                },
                Oppurtunities = GetOppurtunities(apiCourseDetail, activeOppurtunity?.OpportunityId),
                CourseLink = apiCourseDetail.Course.URL,
                CourseId = apiCourseDetail.Course.CourseID,
                Cost = activeOppurtunity?.Price,
                StartDateLabel = activeOppurtunity?.StartDate.Item,
                AttendanceMode = activeOppurtunity?.AttendanceMode,
                AttendancePattern = activeOppurtunity?.AttendancePattern,
                StudyMode = activeOppurtunity?.StudyMode,
                Duration = $"{activeOppurtunity?.Duration?.DurationValue} {activeOppurtunity?.Duration?.DurationUnit}"
            };
        }

        private static IList<Oppurtunity> GetOppurtunities(CourseDetailStructure apiCourseDetail, string oppurtunityId)
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