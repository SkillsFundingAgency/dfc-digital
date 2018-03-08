using System;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface IAsyncHelper
    {
        void Synchronise(Func<Task> asyncFunction);

        T Synchronise<T>(Func<Task<T>> asyncFunction);
    }
}