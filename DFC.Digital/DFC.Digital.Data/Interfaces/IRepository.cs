using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IRepository<T> : IQueryRepository<T>, ICommandRepository<T>
        where T : class
    {
    }
}
