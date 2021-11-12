using CommunityToolkit.Mvvm.DependencyInjection;

using GoogleMapper.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace GoogleMapper.Views
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
