using CommunityToolkit.Mvvm.DependencyInjection;
using WinAppSDKApp.Activation;
using WinAppSDKApp.Contracts.Services;
using WinAppSDKApp.Core.Services;
using WinAppSDKApp.Helpers;
using WinAppSDKApp.Services;
using WinAppSDKApp.ViewModels;
using WinAppSDKApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using Windows.ApplicationModel.Activation;
using WinAppSDKApp.Core.Logging;

namespace WinAppSDKApp
{
    /// <summary>
    /// The <see cref="Application"/> instance.
    /// </summary>
    public partial class App : Application
    {
        #region Objects and variables



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the main <see cref="Window"/> of this application.
        /// </summary>
        public static Window MainWindow { get; set; } = new Window() { Title = ResourceExtensions.AppTitleKey.GetLocalized() };

        #endregion

        #region Construction and destruction

        /// <summary>
        /// Initializes this component, hooks up the <see cref="UnhandledExceptionEventHandler"/> and calls <see cref="ConfigureServices()"/>.
        /// </summary>
        public App()
        {
            InitializeComponent();
            UnhandledException += App_UnhandledException;
            Ioc.Default.ConfigureServices(ConfigureServices());
        }

        #endregion

        #region Private methods and functions

        /// <summary>
        /// Registers services, viewmodels, pages and activation handlers.
        /// </summary>
        /// <returns>An <see cref="IServiceProvider"/></returns>
        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddTransient<ActivationHandler<Microsoft.UI.Xaml.LaunchActivatedEventArgs>, DefaultActivationHandler>() // Default Activation Handler

                .AddTransient<IActivationHandler, CommandLineActivationHandler>() // Other Activation Handlers
                .AddTransient<IActivationHandler, BackgroundTaskActivationHandler>()
                .AddTransient<IActivationHandler, ToastNotificationsService>()

                .AddSingleton<ILoggingService, DebugLoggingService>() // Services
                .AddSingleton<IThemeSelectorService, ThemeSelectorService>()
                .AddSingleton<INavigationService, NavigationService>()
                .AddTransient<INavigationViewService, NavigationViewService>()
                .AddSingleton<IPageService, PageService>()
                .AddSingleton<FirstRunDisplayService, FirstRunDisplayService>()
                .AddSingleton<SqlServerDataService, SqlServerDataService>()
                .AddSingleton<ToastNotificationsService, ToastNotificationsService>()
                .AddSingleton<IActivationService, ActivationService>()

                .AddTransient<ShellViewModel>() // ViewModels
                .AddTransient<MainViewModel>()
                .AddTransient<SettingsViewModel>()
                .AddTransient<DataViewModel>()

                .AddTransient<ShellPage>() // Views
                .AddTransient<MainPage>()
                .AddTransient<SettingsPage>()
                .AddTransient<DataGridPage>()

                .BuildServiceProvider(); // generate IServiceprovider
        }

        /// <summary>
        /// Eventhandler for binding failures.
        /// </summary>
        private void OnBindingFailed(object sender, BindingFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Event is fired after launching of the app.
        /// Handles app activation through the <see cref="ActivationService"/> and applies main window dimensions.
        /// </summary>
        /// <param name="args">The <see cref="LaunchActivatedEventArgs" /></param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
#if DEBUG
            DebugSettings.BindingFailed += OnBindingFailed;
#endif
            IActivationService activationService = Ioc.Default.GetService<IActivationService>();

            string[] cmdArgs = Environment.GetCommandLineArgs(); // known issue: https://github.com/microsoft/microsoft-ui-xaml/issues/3368
            if (cmdArgs.Length > 1)
            {
                if (cmdArgs[1].StartsWith("Toast", StringComparison.InvariantCulture))
                {
                    await activationService.ActivateAsync(new ToastNotificationActivatedEventArgsMock(cmdArgs[1], null, args.UWPLaunchActivatedEventArgs.Kind, args.UWPLaunchActivatedEventArgs.PreviousExecutionState, args.UWPLaunchActivatedEventArgs.SplashScreen));
                }
                else
                {
                    await activationService.ActivateAsync(new CommandLineActivatedEventArgsMock(cmdArgs, args.UWPLaunchActivatedEventArgs.Kind, args.UWPLaunchActivatedEventArgs.PreviousExecutionState, args.UWPLaunchActivatedEventArgs.SplashScreen));
                }
            } else
            {
                await activationService.ActivateAsync(args);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event handler for otherwise unhandled exceptions.
        /// </summary>
        /// <param name="e">The <see cref="Microsoft.UI.Xaml.UnhandledExceptionEventArgs"/> with error details</param>
        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.unhandledexceptioneventargs
        }

        #endregion
    }
}
