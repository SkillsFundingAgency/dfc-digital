using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileIndexEnhancer
    {
        void Initialise(JobProfileIndex initialiseJobProfileIndex, bool isPublishing);

        void PopulateRelatedFieldsWithUrl();

        void PopulateSalary();
    }
}