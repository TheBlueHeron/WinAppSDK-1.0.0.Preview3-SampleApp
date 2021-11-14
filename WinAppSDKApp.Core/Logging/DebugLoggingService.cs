using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WinAppSDKApp.Core.Logging
{
    /// <summary>
    /// Logs messages to the default debug output window.
    /// </summary>
    public class DebugLoggingService : ILoggingService
    {
        /// <summary>
        /// Logs messages to the default debug output window.
        /// </summary>
        /// <param name="logLevel">The <see cref="LogLevel"/> of the message</param>
        /// <param name="message">The message to log</param>
        /// <param name="ex">Any <see cref="Exception"/> that occurred</param>
        /// <returns>A <see cref="Task"/></returns>
        public Task Log(LogLevel logLevel, string message, Exception ex = null)
        {
            Debug.WriteLine($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} {logLevel} {message}");
            if (ex != null)
            {
                Debug.IndentLevel += 1;
                Debug.WriteLine(ex.Output());
                Debug.IndentLevel -= 1;
            }
            return Task.FromResult(0);
        }
    }
}