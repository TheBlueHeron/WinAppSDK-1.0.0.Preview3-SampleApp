
namespace WinAppSDKApp.Core.Logging
{
    /// <summary>
    /// Enumeration of available <see cref="ILoggingService"/> implementations.
    /// </summary>
    public enum LogMode
    {
        /// <summary>
        /// A <see cref="DebugLoggingService"/> is used.
        /// </summary>
        Debug = 0,
        /// <summary>
        /// A <see cref="FileLoggingService"/> is used.
        /// </summary>
        File = 1
    }
}