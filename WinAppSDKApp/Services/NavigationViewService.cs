using GoogleMapper.Contracts.Services;
using GoogleMapper.Helpers;
using GoogleMapper.ViewModels;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleMapper.Services
{
    /// <summary>
    /// Service that handles a <see cref="NavigationView"/>, using a <see cref="NavigationService"/> and a <see cref="PageService"/>.
    /// </summary>
    public class NavigationViewService : INavigationViewService
    {
        #region Objects and variables

        private readonly INavigationService _navigationService;
        private NavigationView _navigationView;
        private readonly IPageService _pageService;

        #endregion

        #region Properties

        /// <summary>
        /// The list of menu items.
        /// </summary>
        public IList<object> MenuItems
            => _navigationView.MenuItems;

        /// <summary>
        /// The menu item for navigation to the <see cref="GoogleMapper.Views.SettingsPage"/>.
        /// </summary>
        public object SettingsItem
            => _navigationView.SettingsItem;

        #endregion

        #region Construction

        /// <summary>
        /// Creates a new NavigationViewService, using the given <see cref="GoogleMapper.Contracts.Services.INavigationService"/> and <see cref="GoogleMapper.Contracts.Services.IPageService"/>.
        /// </summary>
        /// <param name="navigationService">The <see cref="GoogleMapper.Contracts.Services.INavigationService"/> to use</param>
        /// <param name="pageService">The <see cref="GoogleMapper.Contracts.Services.IPageService"/> to use</param>
        public NavigationViewService(INavigationService navigationService, IPageService pageService)
        {
            _navigationService = navigationService;
            _pageService = pageService;
        }

        #endregion

        #region Public methods and functions

        /// <summary>
        /// Initializes this NavigationViewService, using the given <see cref="NavigationView"/>.
        /// </summary>
        /// <param name="navigationView">The <see cref="NavigationView"/> to use</param>
        public void Initialize(NavigationView navigationView)
        {
            _navigationView = navigationView;
            _navigationView.BackRequested += OnBackRequested;
            _navigationView.ItemInvoked += OnItemInvoked;
        }

        /// <summary>
        /// Returns the selected menu item for the given page type.
        /// </summary>
        /// <param name="pageType">The page type</param>
        /// <returns>A <see cref="NavigationViewItem"/></returns>
        public NavigationViewItem GetSelectedItem(Type pageType)
            => GetSelectedItem(_navigationView.MenuItems, pageType);

        /// <summary>
        /// Detaches all events.
        /// </summary>
        public void UnregisterEvents()
        {
            _navigationView.BackRequested -= OnBackRequested;
            _navigationView.ItemInvoked -= OnItemInvoked;
        }

        #endregion

        #region Private methods and functions

        /// <summary>
        /// Determines whether the given <see cref="NavigationViewItem"/> is the correct menu item for the given page type.
        /// </summary>
        /// <param name="menuItem">The <see cref="NavigationViewItem"/></param>
        /// <param name="sourcePageType">The page type</param>
        /// <returns>Boolean, true if the given <see cref="NavigationViewItem"/> is the correct menu item for the given page type</returns>
        private bool IsMenuItemForPageType(NavigationViewItem menuItem, Type sourcePageType)
        {
            return menuItem.GetValue(NavHelper.NavigateToProperty) is string pageKey && _pageService.GetPageType(pageKey) == sourcePageType;
        }

        /// <summary>
        /// Causes the <see cref="NavigationService"/> to navigate to attempt to go to the previous page.
        /// </summary>
        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
            => _navigationService.GoBack();

        /// <summary>
        /// Handles the <see cref="NavigationView.ItemInvoked"/> event.
        /// </summary>
        /// <param name="sender">The <see cref="NavigationView"/> that raised the event</param>
        /// <param name="args">The <see cref="NavigationViewItemInvokedEventArgs"/></param>
        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                _navigationService.NavigateTo(typeof(SettingsViewModel).FullName);
            }
            else
            {
                NavigationViewItem selectedItem = args.InvokedItemContainer as NavigationViewItem;

                if (selectedItem.GetValue(NavHelper.NavigateToProperty) is string pageKey)
                {
                    _ = _navigationService.NavigateTo(pageKey);
                }
            }
        }

        /// <summary>
        /// Returns the correct <see cref="NavigationViewItem"/> for the given page type.
        /// </summary>
        /// <param name="menuItems">The list of menu items <inheritdocwhich to search/></param>
        /// <param name="pageType">The page type</param>
        /// <returns>A <see cref="NavigationViewItem"/></returns>
        private NavigationViewItem GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (NavigationViewItem item in menuItems.OfType<NavigationViewItem>())
            {
                if (IsMenuItemForPageType(item, pageType))
                {
                    return item;
                }
                NavigationViewItem selectedChild = GetSelectedItem(item.MenuItems, pageType);
                if (selectedChild != null)
                {
                    return selectedChild;
                }
            }
            return null;
        }

        #endregion
    }
}