using System;

namespace WinAppSDKApp.Core
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/>s.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns <see cref="Exception"/> details as a single string.
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/></param>
        /// <returns>A string</returns>
        public static string Output(this Exception ex, bool includeInnerException = true, bool includeStackTrace = true)
        {
            string details;
            string stackTrace = includeStackTrace ? $"\r\n{ex.StackTrace}" : "";

            details = $"{ex.Message}{stackTrace}";
            if (includeInnerException && ex.InnerException != null)
            {
                details += "\r\n" + ex.InnerException.Output();
            }

            return details;
        }
    }
}
