using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TesApi.Web
{
    public class LoggingExecutor
    {
        private readonly ILogger logger;

        public LoggingExecutor(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task Execute(string actionDescription, string referenceId, Func<Task> action)
        {
            var start = DateTime.UtcNow;

            try
            {
                await action();
            }
            catch (Exception ex)
            {
                logger.LogTrace($"TIMING_EXCEPTION,{start},{actionDescription},{referenceId},{DateTime.UtcNow.Subtract(start).TotalSeconds},{ex.Message}");
                throw;
            }

            logger.LogTrace($"TIMING,{start},{actionDescription},{referenceId},{DateTime.UtcNow.Subtract(start).TotalSeconds}");
        }

        public async Task<T> Execute<T>(string actionDescription, string referenceId, Func<Task<T>> action)
        {
            T result;

            var start = DateTime.UtcNow;

            try
            {
                result = await action();
            }
            catch (Exception ex)
            {
                logger.LogTrace($"TIMING_EXCEPTION,{start},{actionDescription},{referenceId},{DateTime.UtcNow.Subtract(start).TotalSeconds},{ex.Message}");
                throw;
            }

            logger.LogTrace($"TIMING,{start},{actionDescription},{referenceId},{DateTime.UtcNow.Subtract(start).TotalSeconds}");
            return result;
        }

        public T Execute<T>(string actionDescription, string referenceId, Func<T> action)
        {
            T result;

            var start = DateTime.UtcNow;

            try
            {
                result = action();
            }
            catch (Exception ex)
            {
                logger.LogTrace($"TIMING_EXCEPTION,{start},{actionDescription},{referenceId},{DateTime.UtcNow.Subtract(start).TotalSeconds},{ex.Message}");
                throw;
            }

            logger.LogTrace($"TIMING,{start},{actionDescription},{referenceId},{DateTime.UtcNow.Subtract(start).TotalSeconds}");
            return result;
        }
    }
}
