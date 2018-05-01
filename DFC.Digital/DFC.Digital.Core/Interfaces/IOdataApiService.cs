using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public interface IOdataApiService<T>
        where T : class, new()
    {
        Task<PagedOdataResult<T>> GetResultAsync(Uri requestUri);

        Task<string> PostAsync(Uri requestUri, T data);

        Task<string> PutAsync(Uri requestUri, string relatedEntityLink);

        Task DeleteAsync(string requestUri);

        Task<IList<T>> GetAllAsync(Uri uri);
    }
}