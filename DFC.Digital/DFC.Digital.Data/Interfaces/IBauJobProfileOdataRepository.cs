using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IBauJobProfileOdataRepository
    {
        Task<IEnumerable<BauJobProfile>> GetAllJobProfilesBySourcePropertiesAsync(IEnumerable<string> sourceProperties);
    }
}