using CommunityToolkit.Mvvm.ComponentModel;
using WinAppSDKApp.Contracts.ViewModels;

namespace WinAppSDKApp.ViewModels
{
    public class MainViewModel : ObservableRecipient, INavigationAware
    {
        public MainViewModel()
        {
        }

        public void OnNavigatedFrom()
        {
            // Run code when the app navigates away from this page
        }

        public void OnNavigatedTo(object parameter)
        {
            // Run code when the app navigates to this page
        }
    }
}
