using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISalaryService
    {
        Task<JobProfileSalary> GetSalaryBySocAsync(string socCode);
    }
}