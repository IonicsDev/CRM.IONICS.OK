using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace CRM.Website.Models
{
    public static class EnumerableAsync
    {
        public static Task ForEachAsync<TSource, TResult>(
                this IEnumerable<TSource> source,
                Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor)
        {
            var oneAtATime = new SemaphoreSlim(initialCount: 1, maxCount: 1);

            return Task.WhenAll(
                       from item in source
                       select ProcessAsync(item, taskSelector, resultProcessor, oneAtATime));
        }

        private static async Task ProcessAsync<TSource, TResult>(
            TSource item,
            Func<TSource, Task<TResult>> taskSelector, Action<TSource, TResult> resultProcessor,
            SemaphoreSlim oneAtATime)
        {
            TResult result = await taskSelector(item);
            await oneAtATime.WaitAsync();

            try
            {
                resultProcessor(item, result);
            }
            finally
            {
                oneAtATime.Release();
            }
        }
    }
}