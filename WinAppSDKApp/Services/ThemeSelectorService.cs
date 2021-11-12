using GoogleMapper.Contracts.Services;
using GoogleMapper.Helpers;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace GoogleMapper.Services
{
    public class ThemeSelectorService : IThemeSelectorService
    {
        private const string SettingsKey = "AppBackgroundRequestedTheme";

        public ElementTheme Theme { get; set; } = ElementTheme.Default;

        public async Task InitializeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
            await Task.CompletedTask;
        }

        public async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync();
            await SaveThemeInSettingsAsync(Theme);
        }

        public async Task SetRequestedThemeAsync()
        {
            if (App.MainWindow.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = Theme;
            }

            await Task.CompletedTask;
        }

        private static async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            string themeName = await ApplicationData.Current.LocalSettings.ReadAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                if (Enum.TryParse(themeName, out ElementTheme cacheTheme))
                {
                    return cacheTheme;
                }
            }

            return ElementTheme.Default;
        }

        private static async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString());
        }
    }
}
