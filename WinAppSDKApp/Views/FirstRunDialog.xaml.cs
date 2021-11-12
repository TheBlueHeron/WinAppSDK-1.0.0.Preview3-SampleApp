using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace GoogleMapper.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstRunDialog : ContentDialog
    {
        public FirstRunDialog()
        {
            // TODO WTS: Update the contents of this dialog with any important information you want to show when the app is used for the first time.
            RequestedTheme = (App.MainWindow.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
            CloseButtonText = "Ok";
        }

        private void TermsOfUseContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            // Ensure that the check box is unchecked each time the dialog opens.
            ConfirmAgeCheckBox.IsChecked = false;
        }

        private void ConfirmAgeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            IsPrimaryButtonEnabled = true;
        }

        private void ConfirmAgeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            IsPrimaryButtonEnabled = false;
        }
    }
}
