using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface ISessionStorage<T>
    {
        T Get();

        void Save(T data);

        void Remove();
    }
}
