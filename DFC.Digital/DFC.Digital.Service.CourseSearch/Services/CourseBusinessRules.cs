using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseBusinessRules : ICourseBusinessRules
    {
        public DateTime GetEarliestStartDate(DateTime inputDate)
        {
            var earliestDate = DateTime.Now.AddYears(-1);
            var latestDate = DateTime.Now.AddMonths(6);
            return inputDate >= earliestDate && inputDate < latestDate ? inputDate :
                inputDate < earliestDate ? earliestDate :
                latestDate;
        }
    }
}