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
        /// <param name="message">The message to log</param>
        /// <returns>A <see cref="Task"/></returns>
        public Task Log(string message)
        {
            Debug.WriteLine(message);
            return Task.FromResult(0);
        }
    }
}