using CommunityToolkit.Mvvm.DependencyInjection;
using WinAppSDKApp.Contracts.Services;
using WinAppSDKApp.Services;
using WinAppSDKApp.ViewModels;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;

namespace WinAppSDKApp.Activation
{
    /// <summary>
    /// The default <see cref="IActivationHandler"/>.
    /// </summary>
    public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        #region Objects and variables

        private readonly INavigationService _navigationService;

        #endregion

        #region Construction

        /// <summary>
        /// Creates a new default handler, using the given <see cref="INavigationService"/>.
        /// </summary>
        /// <param name="navigationService">The <see cref="INavigationService"/> to use</param>
        public DefaultActivationHandler(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Navigates to the main page by passing the name of the <see cref="MainViewModel"/>.
        /// </summary>
        /// <param name="args">The <see cref="LaunchActivatedEventArgs"/></param>
        /// <returns>A <see cref="Task"/></returns>
        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            _navigationService.NavigateTo(typeof(MainViewModel).FullName, args.Arguments);
            Ioc.Default.GetService<ToastNotificationsService>().ShowSample();
            await Task.CompletedTask;
        }

        /// <summary>
        /// Returns true if no other handler has handled app activation (i.e. the <see cref="INavigationService.Frame"/> is still null).
        /// </summary>
        /// <param name="args">The <see cref="LaunchActivatedEventArgs"/></param>
        /// <returns>Boolean</returns>
        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            return _navigationService.Frame.Content == null;
        }

        #endregion
    }
}