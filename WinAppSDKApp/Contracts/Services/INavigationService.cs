using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinAppSDKApp.Contracts.Services
{
    /// <summary>
    /// Interface definition for the <see cref="WinAppSDKApp.Services.NavigationService"/>.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// The Navigated event.
        /// </summary>
        event NavigatedEventHandler Navigated;

        /// <summary>
        /// Returns a boolean, signifying whether navigating to the previous page is possible.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Gets or sets the navigation <see cref="Frame"/> to use.
        /// </summary>
        Frame Frame { get; set; }

        /// <summary>
        /// Navigates to the page with the given key.
        /// </summary>
        /// <param name="pageKey">The key with which the page was registered.</param>
        /// <param name="parameter">Any parameter to pass to the page.</param>
        /// <param name="clearNavigation">If true, navigation history is cleared</param>
        /// <returns>A boolean, true if the operation was successful</returns>
        bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false);

        /// <summary>
        /// Navigates to the previous page.
        /// </summary>
        /// <returns>Boolean, true if the operation was successful</returns>
        bool GoBack();
    }
}
