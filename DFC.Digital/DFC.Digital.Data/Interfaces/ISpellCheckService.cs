using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISpellCheckService
    {
        Task<SpellCheckResult> CheckSpellingAsync(string term);
    }
}