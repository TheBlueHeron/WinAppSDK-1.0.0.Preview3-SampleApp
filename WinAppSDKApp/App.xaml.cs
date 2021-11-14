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
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using WinAppSDKApp.Core.Logging;
using Microsoft.Extensions.Logging;
using UnhandledExceptionEventArgs = Microsoft.UI.Xaml.UnhandledExceptionEventArgs;
using Windows.Storage;

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
            CoreApplication.Suspending += OnSuspending;
            CoreApplication.Resuming += OnResuming;
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

                //ConfigureLogging(LogMode.Debug) // Services
                .ConfigureLogging(LogMode.File, ResourceExtensions.AppTitleKey.GetLocalized(), ApplicationData.Current.LocalFolder.Path)
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
            _ = Ioc.Default.GetService<ILoggingService>().Log(LogLevel.Error, e.Message);
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
        /// Invoked when application execution is being resumed.
        /// </summary>
        /// <param name="sender">The source of the resume request</param>
        /// <param name="e">???</param>
        private async void OnResuming(object sender, object e)
        {
            await SuspensionManager.RestoreAsync();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.
        /// Application state is saved without knowing whether the application will be terminated or resumed with the contents of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request</param>
        /// <param name="e">Details about the suspend request</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        /// <summary>
        /// Event handler for otherwise unhandled exceptions.
        /// </summary>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> with error details</param>
        /// <seealso>https://docs.microsoft.com/windows/winui/api/microsoft.ui.xaml.unhandledexceptioneventargs</seealso>
        private void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try  // be safe and
            {
                _ = Ioc.Default.GetService<ILoggingService>().Log(LogLevel.Critical, e.Message, e.Exception);
            }
            finally
            {
                // die gracefully
            }
        }

        #endregion
    }
}
