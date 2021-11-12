using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace GoogleMapper.Contracts.Services
{
    /// <summary>
    /// Interface definition for the <see cref="GoogleMapper.Services.ThemeSelectorService"/>.
    /// </summary>
    public interface IThemeSelectorService
    {
        /// <summary>
        /// The current <see cref="ElementTheme"/>.
        /// </summary>
        ElementTheme Theme { get; }

        /// <summary>
        /// Asynchronously initializes the service.
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        Task InitializeAsync();

        /// <summary>
        /// Asynchronously sets the given <see cref="ElementTheme"/>.
        /// </summary>
        /// <param name="theme">The <see cref="ElementTheme"/> to activate</param>
        /// <returns>A <see cref="Task"/></returns>
        Task SetThemeAsync(ElementTheme theme);

        /// <summary>
        /// Asynchronously sets the configured <see cref="ElementTheme"/>.
        /// </summary>
        /// <returns>A <see cref="Task"/></returns>
        Task SetRequestedThemeAsync();
    }
}