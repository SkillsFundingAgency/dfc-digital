using DFC.Digital.Data.Model;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISpellcheckService
    {
        Task<SpellcheckResult> CheckSpellingAsync(string term);
    }
}