using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public class CourseBusinessRules : ICourseBusinessRules
    {
        public bool IsEarliestStartDateValid(DateTime earliestStartDate)
        {
            return earliestStartDate > DateTime.Now.AddYears(-1) && earliestStartDate < DateTime.Now.AddYears(1);
        }
    }
}
