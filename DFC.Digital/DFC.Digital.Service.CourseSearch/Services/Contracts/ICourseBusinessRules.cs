using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CourseSearchProvider
{
    public interface ICourseBusinessRules
    {
        bool IsEarliestStartDateValid(DateTime earliestStartDate);
    }
}
