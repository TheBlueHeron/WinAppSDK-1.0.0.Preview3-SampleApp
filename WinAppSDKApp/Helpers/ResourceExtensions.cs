using Windows.ApplicationModel.Resources;

namespace GoogleMapper.Helpers
{
    /// <summary>
    /// Helper functions for retrieval of localized resources.
    /// </summary>
    internal static class ResourceExtensions
    {
        #region Objects and variables

        private static readonly ResourceLoader _resLoader = new();

        internal const string AppTitleKey = "AppDisplayName";

        #endregion

        #region Functions

        /// <summary>
        /// Returns the localized string with the given key.
        /// </summary>
        /// <param name="resourceKey">Key of the string resource</param>
        /// <returns>A localized string of text</returns>
        public static string GetLocalized(this string resourceKey)
        {
            return _resLoader.GetString(resourceKey);
        }

        #endregion
    }
}
