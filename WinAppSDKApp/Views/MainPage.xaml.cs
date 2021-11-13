using CommunityToolkit.Mvvm.DependencyInjection;

using WinAppSDKApp.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace WinAppSDKApp.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; }

        public MainPage()
        {
            ViewModel = Ioc.Default.GetService<MainViewModel>();
            InitializeComponent();
        }
    }
}
