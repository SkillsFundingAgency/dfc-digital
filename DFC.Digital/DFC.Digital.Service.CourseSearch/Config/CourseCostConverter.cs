using AutoMapper;
using System.Globalization;

namespace DFC.Digital.Service.CourseSearchProvider
{
        public class CourseCostConverter : IValueConverter<string, string>
        {
            public string Convert(string sourceCost, ResolutionContext context)
            {
                if (decimal.TryParse(sourceCost, out decimal outValue))
                {
                    return outValue.ToString("C", new CultureInfo("en-GB"));
                }

                return sourceCost;
            }
        }
}
