using Microsoft.UI.Xaml;
using PInvoke;
using System;
using WinRT.Interop;

namespace WinAppSDKApp.Helpers
{
    /// <summary>
    /// Helper functions for <see cref="Window"/> objects.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Returns a <see cref="IntPtr"/> handle to this <see cref="Window"/>.
        /// </summary>
        /// <param name="wnd">The <see cref="Window"/></param>
        /// <returns>An <see cref="IntPtr"/></returns>
        public static IntPtr GetWindowHandle(this Window wnd)
        {
            return WindowNative.GetWindowHandle(wnd);
        }

        /// <summary>
        /// Resizes this <see cref="Window" />.
        /// </summary>
        /// <param name="wnd">The <see cref="Window"/> to resize</param>
        /// <param name="width">The desired width</param>
        /// <param name="height">The desired height</param>
        public static void SetWindowSize(this Window wnd, int width, int height)
        {
            IntPtr hwnd = wnd.GetWindowHandle();
            int dpi = User32.GetDpiForWindow(hwnd); // Win32 uses pixels and WinUI 3 uses effective pixels -> apply the DPI scale factor
            float scalingFactor = (float)dpi / 96;

            width = (int)(width * scalingFactor);
            height = (int)(height * scalingFactor);

            _ = User32.SetWindowPos(hwnd, User32.SpecialWindowHandles.HWND_TOP, 0, 0, width, height, User32.SetWindowPosFlags.SWP_NOMOVE);
        }

        /// <summary>
        /// Sets an icon resource for this <see cref="Window"/>.
        /// </summary>
        /// <param name="wnd">The <see cref="Window"/> for which to set the icon</param>
        /// <param name="iconName">The name of the icon resource</param>
        public static void SetIcon(this Window wnd, string iconName)
        {
            IntPtr hwnd = wnd.GetWindowHandle();
            IntPtr hIcon = User32.LoadImage(IntPtr.Zero, iconName, User32.ImageType.IMAGE_ICON, 16, 16, User32.LoadImageFlags.LR_LOADFROMFILE);

            _ = User32.SendMessage(hwnd, User32.WindowMessage.WM_SETICON, (IntPtr)0, hIcon);
        }
    }
}