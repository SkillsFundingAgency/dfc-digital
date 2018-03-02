using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface IConfigurationProvider
    {
        T Get<T>(string key);

        T Get<T>(string key, T defaultValue);

        void Add<T>(string key, T value);
    }
}
