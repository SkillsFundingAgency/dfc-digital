using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISalaryCalculator
    {
        decimal? GetStarterSalary(JobProfileSalary jobProfileSalary);

        decimal? GetExperiencedSalary(JobProfileSalary jobProfileSalary);
    }
}
