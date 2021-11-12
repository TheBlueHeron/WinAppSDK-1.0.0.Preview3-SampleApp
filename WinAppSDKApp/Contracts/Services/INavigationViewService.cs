using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace GoogleMapper.Contracts.Services
{
    /// <summary>
    /// Interface definition for the <see cref="GoogleMapper.Services.NavigationViewService"/>.
    /// </summary>
    public interface INavigationViewService
    {
        /// <summary>
        /// The list of menu items.
        /// </summary>
        IList<object> MenuItems { get; }

        /// <summary>
        /// The menu item for navigation to the <see cref="GoogleMapper.Views.SettingsPage"/>.
        /// </summary>
        object SettingsItem { get; }

        /// <summary>
        /// Initializes the service, using the given <see cref="NavigationView"/>.
        /// </summary>
        /// <param name="navigationView">The <see cref="NavigationView"/> to use</param>
        void Initialize(NavigationView navigationView);

        /// <summary>
        /// Detaches all events.
        /// </summary>
        void UnregisterEvents();

        /// <summary>
        /// Returns the selected menu item for the given page type.
        /// </summary>
        /// <param name="pageType">The page type</param>
        /// <returns>A <see cref="NavigationViewItem"/></returns>
        NavigationViewItem GetSelectedItem(Type pageType);
    }
}
