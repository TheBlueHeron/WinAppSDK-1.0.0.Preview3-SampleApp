using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GoogleMapper.Contracts.Services;
using GoogleMapper.Helpers;
using Microsoft.UI.Xaml;
//using Microsoft.Services.Store.Engagement;
using System.Windows.Input;
using Windows.ApplicationModel;

namespace GoogleMapper.ViewModels
{
    /// <summary>
    /// An <see cref="ObservableRecipient"/> that serves as the ViewModel for the <see cref="Views.SettingsPage"/>.
    /// </summary>
    public class SettingsViewModel : ObservableRecipient
    {
        #region Objects and variables

        private ElementTheme _elementTheme;
        private ICommand _launchFeedbackHubCommand;
        private ICommand _switchThemeCommand;
        private readonly IThemeSelectorService _themeSelectorService;
        private string _versionDescription;

        #endregion

        #region Properties

        /// <summary>
        /// The selected <see cref="Microsoft.UI.Xaml.ElementTheme"/>.
        /// </summary>
        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { SetProperty(ref _elementTheme, value); }
        }

        /// <summary>
        /// Feedback visible only when it is supported on this device.
        /// </summary>
        public Visibility FeedbackLinkVisibility => Visibility.Collapsed; // If(Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported(), Visibility.Visible, Visibility.Collapsed)

        /// <summary>
        /// Returns the <see cref="ICommand"/> that asynchronously launches the feedback hub.
        /// <seealso>Store Services SDK https://docs.microsoft.com/windows/uwp/monetize/microsoft-store-services-sdk </seealso>
        /// </summary>
        public ICommand LaunchFeedbackHubCommand
        {
            get
            {
                if (_launchFeedbackHubCommand == null)
                {
                    _launchFeedbackHubCommand = new RelayCommand(() =>
                   {
                       Windows.Foundation.IAsyncOperation<bool> launch = Windows.System.Launcher.LaunchUriAsync(new System.Uri("mailto:helpdesk@draaijerpartners.nl", System.UriKind.Absolute));
                       //var launcher = StoreServicesFeedbackLauncher.GetDefault();
                       //await launcher.LaunchAsync();
                   });
                }
                return _launchFeedbackHubCommand;
            }
        }

        /// <summary>
        /// Returns the <see cref="ICommand"/> that asynchronously switches the current <see cref="Microsoft.UI.Xaml.ElementTheme"/> to the selected <see cref="ElementTheme"/>.
        /// </summary>
        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            if (ElementTheme != param)
                            {
                                ElementTheme = param;
                                await _themeSelectorService.SetThemeAsync(param);
                            }
                        });
                }

                return _switchThemeCommand;
            }
        }

        /// <summary>
        /// Returns the name and version of this application.
        /// </summary>
        public string VersionDescription
        {
            get { return _versionDescription; }

            set { SetProperty(ref _versionDescription, value); }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes this ViewModel.
        /// </summary>
        /// <param name="themeSelectorService"></param>
        public SettingsViewModel(IThemeSelectorService themeSelectorService)
        {
            _themeSelectorService = themeSelectorService;
            _elementTheme = _themeSelectorService.Theme;
            VersionDescription = GetVersionDescription();
        }

        #endregion

        #region Private methods and functions

        /// <summary>
        /// Returns the name and version of this application.
        /// </summary>
        private static string GetVersionDescription()
        {
            var appName = ResourceExtensions.AppTitleKey.GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        #endregion

    }
}