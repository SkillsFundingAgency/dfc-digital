using Microsoft.Extensions.Caching.Memory;
using Polly;
using Polly.Caching;
using Polly.Caching.Memory;
using Polly.Registry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public class CachePolicy : ICachingPolicy
    {
        private readonly IApplicationLogger logger;
        private IMemoryCache memoryCache;
        private MemoryCacheProvider memoryCacheProvider;
        private PolicyRegistry policyRegistry;

        public CachePolicy(IApplicationLogger logger)
        {
            this.logger = logger;

            memoryCache = new MemoryCache(new MemoryCacheOptions());
            memoryCacheProvider = new MemoryCacheProvider(memoryCache);
            policyRegistry = new PolicyRegistry();
        }

        public T Execute<T>(Func<T> action, CachePolicyType cachePolicyType, string policyName, string cacheContext)
        {
            switch (cachePolicyType)
            {
                case CachePolicyType.AbsoluteContext:
                case CachePolicyType.SlidingContext:
                    if (!policyRegistry.ContainsKey(policyName))
                    {
                        policyRegistry.Add(policyName, GetContextualPolicy());
                    }

                    var cachePolicy = policyRegistry.Get<ISyncPolicy>(policyName);
                    var contextData = new Dictionary<string, object>
                    {
                        [ContextualTtl.TimeSpanKey] = TimeSpan.FromMinutes(15),
                        [ContextualTtl.SlidingExpirationKey] = cachePolicyType == CachePolicyType.SlidingContext
                    };

                    return cachePolicy.Execute(pollyContext => action(), new Context(cacheContext, contextData));

                default:
                    return Policy.NoOp<T>().Execute(action);
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, CachePolicyType cachePolicyType, string policyName, string cacheContext)
        {
            switch (cachePolicyType)
            {
                case CachePolicyType.AbsoluteContext:
                case CachePolicyType.SlidingContext:
                    if (!policyRegistry.ContainsKey(policyName))
                    {
                        policyRegistry.Add(policyName, GetContextualAsyncPolicy());
                    }

                    var cachePolicy = policyRegistry.Get<IAsyncPolicy>(policyName);
                    var contextData = new Dictionary<string, object>
                    {
                        { ContextualTtl.TimeSpanKey, new Ttl(TimeSpan.FromMinutes(15)) },
                        { ContextualTtl.SlidingExpirationKey, cachePolicyType == CachePolicyType.SlidingContext }
                    };

                    return await cachePolicy.ExecuteAsync(pollyContext => action(), new Context(cacheContext, contextData));

                default:
                    return await Policy.NoOpAsync<T>().ExecuteAsync(action);
            }
        }

        private IAsyncPolicy GetContextualAsyncPolicy()
        {
            return Policy.CacheAsync(
                memoryCacheProvider,
                new ContextualTtl(),
                new DefaultCacheKeyStrategy(),
                onCachePut: (context, key) => logger.Trace($"Caching '{key}'."),
                onCacheGet: (context, key) => logger.Trace($"Retrieving '{key}' from the cache."),
                onCachePutError: (context, key, exception) => logger.ErrorJustLogIt($"Cannot add '{key}' to the cache.", exception),
                onCacheGetError: (context, key, exception) => logger.ErrorJustLogIt($"Cannot retrieve '{key}' from the cache.", exception),
                onCacheMiss: (context, key) => logger.Trace($"Cache miss for '{key}'."));
        }

        private ISyncPolicy GetContextualPolicy()
        {
            return Policy.Cache(
                memoryCacheProvider,
                new ContextualTtl(),
                new DefaultCacheKeyStrategy(),
                onCachePut: (context, key) => logger.Trace($"Caching '{key}'."),
                onCacheGet: (context, key) => logger.Trace($"Retrieving '{key}' from the cache."),
                onCachePutError: (context, key, exception) => logger.ErrorJustLogIt($"Cannot add '{key}' to the cache.", exception),
                onCacheGetError: (context, key, exception) => logger.ErrorJustLogIt($"Cannot retrieve '{key}' from the cache.", exception),
                onCacheMiss: (context, key) => logger.Trace($"Cache miss for '{key}'."));
        }
    }
}