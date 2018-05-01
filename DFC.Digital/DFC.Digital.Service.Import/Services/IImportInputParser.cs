using System.Collections.Generic;

namespace DFC.Digital.Service.Import
{
    public interface IImportInputParser<T>
    {
        IEnumerable<T> Parse(IEnumerable<string> content);
    }
}