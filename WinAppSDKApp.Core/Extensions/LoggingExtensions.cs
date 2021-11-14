using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WinAppSDKApp.Core.Logging
{
    /// <summary>
    /// Extension method to configure logging.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Configures the <see cref="ILoggingService"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> in which to register</param>
        /// <param name="logMode">The <see cref="LogMode"/> to use</param>
        /// <param name="appName"><see cref="LogMode.File"/> only: The application name</param>
        /// <param name="appDataPath"><see cref="LogMode.File"/> only: The local AppData folder for this application</param>
        /// <returns>The configured <see cref="IServiceCollection"/></returns>
        public static IServiceCollection ConfigureLogging(this IServiceCollection services, LogMode logMode, string appName = null, string appDataPath = null)
        {
            switch (logMode)
            {
                case LogMode.File:
                    return services
                        .AddLogging()
                        .AddSingleton<ILoggingService, FileLoggingService>((s) =>
                        {
                            return new FileLoggingService(Ioc.Default.GetService<ILoggerFactory>(), appName, appDataPath);
                        });
                case LogMode.Debug:
                default:
                    return services.AddSingleton<ILoggingService, DebugLoggingService>();
            }
        }
    }
}