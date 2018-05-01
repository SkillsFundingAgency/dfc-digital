using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IContentImportService<T>
    {
        Task ImportAsync(ImportConfiguration config);
    }
}
