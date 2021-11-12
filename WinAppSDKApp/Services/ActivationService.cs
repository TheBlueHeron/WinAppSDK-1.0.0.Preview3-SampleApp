using CommunityToolkit.Mvvm.DependencyInjection;
using GoogleMapper.Activation;
using GoogleMapper.Contracts.Services;
using GoogleMapper.Helpers;
using GoogleMapper.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using System;

namespace GoogleMapper.Services
{
    /// <summary>
    /// The <see cref="IActivationService"/> implementation of this app.
    /// </summary>
    public class ActivationService : IActivationService
    {
        #region Objects and variables

        private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
        private readonly IEnumerable<IActivationHandler> _activationHandlers;
        private readonly INavigationService _navigationService;
        private readonly IThemeSelectorService _themeSelectorService;
        private readonly FirstRunDisplayService _firstRunDisplayService;
        private ShellPage _shell;
        private bool _xamlLoaded;

        #endregion

        #region Construction

        /// <summary>
        /// Creates a new ActivationService, using the given <see cref="IActivationHandler"/>s, <see cref="INavigationService"/> and <see cref="IThemeSelectorService"/>.
        /// </summary>
        /// <param name="defaultHandler">The default handler</param>
        /// <param name="activationHandlers">The list of handlers</param>
        /// <param name="navigationService">The <see cref="INavigationService"/> to use</param>
        /// <param name="themeSelectorService">The <see cref="IThemeSelectorService"/> to use</param>
        public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService, IThemeSelectorService themeSelectorService, FirstRunDisplayService firstRunDisplayService)
        {
            _defaultHandler = defaultHandler;
            _activationHandlers = activationHandlers;
            _navigationService = navigationService;
            _themeSelectorService = themeSelectorService;
            _firstRunDisplayService = firstRunDisplayService;
        }

        #endregion

        #region Public methods and functions

        /// <summary>
        /// Initializes needed services, sets the <see cref="ShellPage"/> as content for the main application window and sets the configured theme.
        /// </summary>
        /// <param name="activationArgs">Activation arguments</param>
        /// <returns>A <see cref="Task"/></returns>
        public async Task ActivateAsync(object activationArgs)
        {
            await InitializeAsync();

            if (App.MainWindow.Content == null)
            {
                _shell = Ioc.Default.GetService<ShellPage>();
                _shell.Loaded += OnXamlLoaded; // dialogs need an XamlRoot, which is present after activation AND loading of the Visual tree
                
                App.MainWindow.Content = _shell;
                App.MainWindow.ExtendsContentIntoTitleBar = true;
                App.MainWindow.SetTitleBar(_shell.TitleBar);
                App.MainWindow.SetWindowSize(800, 600);
                //App.MainWindow.SetIcon(@"Assets\map.ico");
            }

            await HandleActivationAsync(activationArgs);

            App.MainWindow.Activate(); // Ensure the current window is active
        }

        #endregion

        #region Private methods and functions

        /// <summary>
        /// Lets the first available and capable handler handle the given arguments.
        /// </summary>
        /// <param name="activationArgs">The arguments to handle</param>
        /// <returns>A <see cref="Task"/></returns>
        private async Task HandleActivationAsync(object activationArgs)
        {
            IActivationHandler activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (_defaultHandler.CanHandle(activationArgs))
            {
                await _defaultHandler.HandleAsync(activationArgs);
            }
        }

        /// <summary>
        /// Initializes services that are needed before app activation. The splash screen is shown while this code runs.
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        private async Task InitializeAsync()
        {
            await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Handles the Loaded event of the <see cref="ShellPage"/>, meaning there is access to an <see cref="XamlRoot"/> that can be used for dialogs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void OnXamlLoaded(object sender, RoutedEventArgs args)
        {
            if (!_xamlLoaded)
            {
                await StartupAsync();
            }
        }

        /// <summary>
        /// Performs tasks when the Visual tree is accessible:
        /// Initializes dialogs and sets the configured <see cref="ElementTheme"/> asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        private async Task StartupAsync()
        {
#if DEBUG

            ApplicationData.Current.LocalSettings.Values.Clear();

#endif
            _xamlLoaded = true;
            await _themeSelectorService.SetRequestedThemeAsync();
            _firstRunDisplayService.ShowIfAppropriateAsync();
            await Task.CompletedTask;
        }

        #endregion
    }
}