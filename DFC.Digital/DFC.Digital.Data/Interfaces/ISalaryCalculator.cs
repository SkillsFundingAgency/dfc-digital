using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISalaryCalculator
    {
        decimal? GetStarterSalary(JobProfileSalary jobProfileSalary);

        decimal? GetExperiencedSalary(JobProfileSalary jobProfileSalary);
    }
}