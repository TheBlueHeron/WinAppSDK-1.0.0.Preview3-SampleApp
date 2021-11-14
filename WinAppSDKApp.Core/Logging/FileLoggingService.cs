using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WinAppSDKApp.Core.Logging
{
    /// <summary>
    /// A <see cref="ILoggingService"/> that logs to a file.
    /// </summary>
    public class FileLoggingService : ILoggingService
    {
        #region Objects and variables

        private readonly ILogger _logger;

        #endregion

        #region Construction

        /// <summary>
        /// Creates a new File logging service.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> responsible for creating <see cref="ILogger"/> instances</param>
        /// <param name="appName">The name of the application</param>
        /// <param name="appDataFolder">The full path to the local application data folder</param>
        public FileLoggingService(ILoggerFactory factory, string appName, string appDataFolder)
        {
            _ = factory.AddFile($"{appDataFolder}\\Logs\\{appName}.log");

            _logger = Ioc.Default.GetService<ILogger<FileLoggingService>>();
        }

        #endregion

        #region Public methods and functions

        /// <summary>
        /// Logs the message to the log file.
        /// </summary>
        /// <param name="logLevel">The <see cref="LogLevel"/> of the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="ex">Any <see cref="Exception"/> that occurred</param>
        /// <returns>A <see cref="Task"/></returns>
        public Task Log(LogLevel logLevel, string message, Exception ex = null)
        {
            if (ex == null)
            {
                _logger.Log(logLevel, message);
            }
            else
            {
                _logger.Log(logLevel, ex, message, ex.Output());
            }
            return Task.CompletedTask;
        }

        #endregion
    }
}