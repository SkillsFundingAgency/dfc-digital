using System;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IAsyncHelper
    {
        void Synchronise(Func<Task> asyncFunction);

        T Synchronise<T>(Func<Task<T>> asyncFunction);
    }
}