//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Globalization;
//using System.Reflection;
//using System.Web.Mvc;
//using Sfa.Careers.Common.Extensions;


//namespace DFC.Digital.Web.Core
//{
//    public class DateRangeAttribute : ValidationAttribute, IClientValidatable
//    {
//        public int StartDateDay { get; set; }
//        public int EndDateDay { get; set; }
//        public string DateRangeErrorMessage { get; set; }
//        public string FormattedDateRangeErrorMessage { get; set; }
//        public string InvalidErrorMessage { get; set; }

//        public DateRangeAttribute(int startDateDay, int endDateDay)
//        {
//            StartDateDay = startDateDay;
//            EndDateDay = endDateDay;
//        }

//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            PropertyInfo propertyDay = validationContext.ObjectType.GetProperty("Day");
//            PropertyInfo propertyMonth = validationContext.ObjectType.GetProperty("Month");
//            PropertyInfo propertyYear = validationContext.ObjectType.GetProperty("Year");

//            if (propertyDay == null)
//            {
//                return new ValidationResult("Day Property is not set");
//            }
//            else if (propertyMonth == null)
//            {
//                return new ValidationResult("Month Property is not set");
//            }
//            else if (propertyYear == null)
//            {
//                return new ValidationResult("Year Property is not set");
//            }
//            else
//            {
//                string selectedDateDay = propertyDay.GetValue(validationContext.ObjectInstance, null) != null ? propertyDay.GetValue(validationContext.ObjectInstance, null).ToString() : string.Empty;
//                string selectedDateMonth = propertyMonth.GetValue(validationContext.ObjectInstance, null) != null ? propertyMonth.GetValue(validationContext.ObjectInstance, null).ToString() : string.Empty;
//                string selectedDateYear = propertyYear.GetValue(validationContext.ObjectInstance, null) != null ? propertyYear.GetValue(validationContext.ObjectInstance, null).ToString() : string.Empty;

//                FormattedDateRangeErrorMessage = FormatDateRangeErrorMessage();

//                // The selectedDate is concatinated value of three fields for Day, Month and Year, parsed as a Date
//                DateTime selectedDate = default(DateTime);
//                CultureInfo enGb = new CultureInfo("en-GB");
//                string sdf = string.Empty;
//                if (!string.IsNullOrEmpty(selectedDateDay) && !string.IsNullOrEmpty(selectedDateMonth) && !string.IsNullOrEmpty(selectedDateYear))
//                {
//                    sdf = string.Format("{0}-{1}-{2}", selectedDateYear.PadLeft(4, '0') , selectedDateMonth.PadLeft(2, '0'), selectedDateDay.PadLeft(2, '0'));
//                }

//                // DateRangeAttribute is not Required property and having no value is allowed
//                // If we want to make it required, we can just add RequiredAttribute to the same property
//                if (string.IsNullOrEmpty(sdf) && string.IsNullOrEmpty(selectedDateDay) && string.IsNullOrEmpty(selectedDateMonth) && string.IsNullOrEmpty(selectedDateYear))
//                {
//                    // DateRange is NOT required
//                    return null;
//                }
//                else
//                {
//                    // If we have a value for selectedDate we are assesing, whether value is valid and if it is within valid Start and End range
//                    if (DateTime.TryParseExact(sdf, "yyyy-MM-dd", enGb, DateTimeStyles.AdjustToUniversal, out selectedDate))
//                    {
//                        var validStartDate = DateTime.Now.AddDays(-StartDateDay);
//                        var validEndDate = DateTime.Now.AddDays(EndDateDay);

//                        if (selectedDate >= validStartDate.Date && selectedDate <= validEndDate.Date)
//                        {
//                            // correct date within the range
//                            return null;
//                        }
//                        else
//                        {
//                            // out of the given date range
//                            return new ValidationResult(FormattedDateRangeErrorMessage);
//                        }
//                    }
//                    else
//                    {
//                        // NOT valid date
//                        return new ValidationResult(InvalidErrorMessage);
//                    }
//                }
//            }
//        }

//        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
//        {
//            FormattedDateRangeErrorMessage = FormatDateRangeErrorMessage();

//            var rule = new ModelClientValidationRule()
//            {
//                ErrorMessage = String.IsNullOrEmpty(ErrorMessage) ? FormatErrorMessage(metadata.DisplayName) : ErrorMessage,
//                ValidationType = "daterange",
//            };
//            var dates = new List<string>
//                               {
//                                   StartDateDay.ToString(),
//                                   EndDateDay.ToString()
//                               };
//            var errorMessages = new List<string>
//                                    {
//                                        FormattedDateRangeErrorMessage,
//                                        InvalidErrorMessage
//                                    };

//            rule.ValidationParameters.Add("dates", dates.ToConcatenatedString(" "));
//            rule.ValidationParameters.Add("errormessages", errorMessages.ToConcatenatedString());
//            yield return rule;
//        }

//        public string FormatDateRangeErrorMessage()
//        {
//            var validStartDate = DateTime.Now.AddDays(-StartDateDay);
//            var validEndDate = DateTime.Now.AddDays(EndDateDay);

//            string invalidDateError = string.Empty;
//            if (string.IsNullOrEmpty(DateRangeErrorMessage))
//            {
//                FormattedDateRangeErrorMessage = $"Enter a date between {validStartDate.ToString("dd-MM-yyyy")} and {validEndDate.ToString("dd-MM-yyyy")}.​";
//            }
//            else
//            {
//                FormattedDateRangeErrorMessage = string.Format(DateRangeErrorMessage, validStartDate.ToString("dd-MM-yyyy"), validEndDate.ToString("dd-MM-yyyy"));
//            }

//            return FormattedDateRangeErrorMessage;
//        }
//    }
//}
