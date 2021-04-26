using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Tes.Repository;

namespace TesApi.Web
{
    /// <summary>
    /// Implements caching and retries for <see cref="IRepository{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CachingWithRetriesRepository<T> : IRepository<T> where T : RepositoryItem<T>
    {
        private readonly IRepository<T> repository;
        private readonly object cacheLock = new object();
        private readonly IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        private readonly IList<object> itemsPredicateCachedKeys = new List<object>();
        private readonly ILogger logger;
        private readonly LoggingExecutor logingExecutor;

        private readonly AsyncRetryPolicy retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

        /// <summary>
        /// Constructor to create a cache and retry wrapper for <see cref="IRepository{T}"/>
        /// </summary>
        /// <param name="repository"><see cref="IRepository{T}"/> to wrap with caching and retries</param>
        /// <param name="logger">The logger</param>
        public CachingWithRetriesRepository(IRepository<T> repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
            this.logingExecutor = new LoggingExecutor(logger);
        }

        /// <inheritdoc/>
        public async Task<T> CreateItemAsync(T item)
        {
            var repositoryItem = await retryPolicy.ExecuteAsync(() => this.logingExecutor.Execute("CreateItemAsync", item.GetId(), () => repository.CreateItemAsync(item)));
            ClearAllItemsPredicateCachedKeys();
            return repositoryItem;
        }

        /// <inheritdoc/>
        public async Task DeleteItemAsync(string id)
        {
            if (cache.TryGetValue(id, out var cachedRepositoryItem))
            {
                cache.Remove(id);
            }

            await retryPolicy.ExecuteAsync(() => this.logingExecutor.Execute("DeleteItemAsync", id, () => repository.DeleteItemAsync(id)));
            ClearAllItemsPredicateCachedKeys();
        }

        /// <inheritdoc/>
        public async Task<bool> TryGetItemAsync(string id, Action<T> onSuccess)
        {
            T repositoryItem = null;

            if (cache.TryGetValue(id, out repositoryItem))
            {
                onSuccess(repositoryItem);
                return true;
            }

            var repositoryItemFound = await retryPolicy.ExecuteAsync(() => this.logingExecutor.Execute("GetItemsAsyncWithPagination", id, () => repository.TryGetItemAsync(id, item => repositoryItem = item)));

            if (repositoryItemFound)
            {
                cache.Set(id, repositoryItem, TimeSpan.FromMinutes(5));
                onSuccess(repositoryItem);
            }

            return repositoryItemFound;
        }

        /// <inheritdoc/>
        public Task<(string, IEnumerable<T>)> GetItemsAsync(Expression<Func<T, bool>> predicate, int pageSize, string continuationToken)
            => retryPolicy.ExecuteAsync(() => this.logingExecutor.Execute("GetItemsAsyncWithPagination", null, () => repository.GetItemsAsync(predicate, pageSize, continuationToken)));

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            var key = predicate.ToString().GetHashCode();
            IEnumerable<T> repositoryItems = new List<T>();

            if (cache.TryGetValue(key, out repositoryItems))
            {
                return repositoryItems;
            }

            repositoryItems = await retryPolicy.ExecuteAsync(() => this.logingExecutor.Execute("GetItemsAsyncNoPagination", null, () => repository.GetItemsAsync(predicate)));

            lock (cacheLock)
            {
                cache.Set(key, repositoryItems, DateTimeOffset.MaxValue);
                itemsPredicateCachedKeys.Add(key);
            }

            return repositoryItems;
        }

        /// <inheritdoc/>
        public async Task<T> UpdateItemAsync(T item)
        {
            var id = item.GetId();

            if (cache.TryGetValue(id, out var cachedRepositoryItem))
            {
                cache.Remove(id);
            }

            var repositoryItem = await retryPolicy.ExecuteAsync(() => this.logingExecutor.Execute("UpdateItemAsync", id, () => repository.UpdateItemAsync(item)));
            ClearAllItemsPredicateCachedKeys();
            return repositoryItem;
        }

        private void ClearAllItemsPredicateCachedKeys()
        {
            lock (cacheLock)
            {
                foreach (var key in itemsPredicateCachedKeys)
                {
                    cache.Remove(key);
                }

                itemsPredicateCachedKeys.Clear();
            }
        }
    }
}
