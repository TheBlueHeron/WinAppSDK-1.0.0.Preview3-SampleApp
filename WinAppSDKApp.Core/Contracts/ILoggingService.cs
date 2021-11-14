using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WinAppSDKApp.Core.Logging
{
    /// <summary>
    /// Interface defintion for logging services.
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Logs the given message.
        /// </summary>
        /// <param name="logLevel">The <see cref="LogLevel"/> of the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="ex">Any <see cref="Exception"/> that occurred</param>
        /// <returns>A <see cref="Task"/></returns>
        Task Log(LogLevel logLevel, string message, Exception ex = null);
    }
}