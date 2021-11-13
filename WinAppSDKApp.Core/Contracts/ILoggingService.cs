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
        /// <param name="message">The message to log</param>
        /// <returns>A <see cref="Task"/></returns>
        Task Log(string message);
    }
}
