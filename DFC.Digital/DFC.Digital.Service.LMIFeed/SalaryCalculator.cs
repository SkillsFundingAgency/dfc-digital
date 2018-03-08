using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Linq;

namespace DFC.Digital.Service.LMIFeed
{
    public class SalaryCalculator : ISalaryCalculator
    {
        #region Implementation of ISalaryCalculator

        public decimal? GetStarterSalary(JobProfileSalary jobProfileSalary)
        {
            return jobProfileSalary?.Deciles.Min(s => s.Value) * Constants.Multiplier;
        }

        public decimal? GetExperiencedSalary(JobProfileSalary jobProfileSalary)
        {
            return jobProfileSalary?.Deciles.Max(s => s.Value) * Constants.Multiplier;
        }

        #endregion Implementation of ISalaryCalculator
    }
}