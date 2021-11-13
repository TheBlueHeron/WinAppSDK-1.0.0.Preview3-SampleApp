using System;

namespace WinAppSDKApp.Contracts.Services
{
    /// <summary>
    /// Interface definition for the <see cref="WinAppSDKApp.Services.PageService"/>.
    /// </summary>
    public interface IPageService
    {
        /// <summary>
        /// Returns the type of the page with the given key.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>A <see cref="Type"/></returns>
        Type GetPageType(string key);
    }
}