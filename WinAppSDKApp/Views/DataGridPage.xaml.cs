using CommunityToolkit.Mvvm.DependencyInjection;

using WinAppSDKApp.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinAppSDKApp.Views
{
    public sealed partial class DataGridPage : Page
    {
        public DataViewModel ViewModel { get; }

        public DataGridPage()
        {
            ViewModel = Ioc.Default.GetService<DataViewModel>();
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await ViewModel.LoadProjectsAsync();
        }
    }
}