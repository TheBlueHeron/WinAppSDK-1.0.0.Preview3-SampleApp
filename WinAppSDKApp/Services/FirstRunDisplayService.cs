using CommunityToolkit.WinUI.Helpers;
using GoogleMapper.Views;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using System;

namespace GoogleMapper.Services
{
    public sealed class FirstRunDisplayService
    {
        #region Objects and variables

        private bool shown;

        #endregion

        #region Public methods and functions

        public void ShowIfAppropriateAsync()
        {
            async void callBack()
            {
                if (SystemInformation.Instance.IsFirstRun && !shown)
                {
                    shown = true;
                    FirstRunDialog dialog = new();
                    dialog.XamlRoot = App.MainWindow.Content.XamlRoot;
                    ContentDialogResult result = await dialog.ShowAsync();
                }
            }

            _ = App.MainWindow.DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, callBack);
        }

        #endregion
    }
}
