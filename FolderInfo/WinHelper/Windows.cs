using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FolderInfo.WinHelper
{
    internal class Windows
    {
        /// <summary>
        /// Windows system provided message.
        /// Moves an item to a specified position in a list-view control
        /// see: https://docs.microsoft.com/en-us/windows/desktop/Controls/lvm-setitemposition
        /// </summary>
        public const uint LVM_SETITEMPOSITION = 0x1000 + 15;
        public const uint LVM_GETITEMRECT = 0x1000 + 14;

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rect lpRect);

        [DllImport("user32.dll")]
        public static extern void ListView_GetItemRect(IntPtr hwnd, int i, out Rect prc, int code);

        public static IntPtr GetDesktopWindow()
        {
            var shellWindow = GetShellWindow();
            var _SHELLDLL_DefView = FindWindowEx(shellWindow, IntPtr.Zero, "SHELLDLL_DefView", null);
            return FindWindowEx(_SHELLDLL_DefView, IntPtr.Zero, "SysListView32", "FolderView");
        }

        /// <summary>
        /// Cretaes LParam for message LVM_SETITEMPOSITION
        /// The LOWORD specifies the new x-position of the item's upper-left corner, in view coordinates.
        /// The HIWORD specifies the new y-position of the item's upper-left corner, in view coordinates.
        /// see: https://docs.microsoft.com/en-us/windows/desktop/Controls/lvm-setitemposition
        /// </summary>
        /// <param name="wLow">New x-position of the item's upper-left corner, in view coordinates.</param>
        /// <param name="wHigh">New y-position of the item's upper-left corner, in view coordinates.</param>
        /// <returns></returns>
        public static IntPtr MakeLParamFor_LVM_SETITEMPOSITION(int wLow, int wHigh)
        {
            return (IntPtr)(((short)wHigh << 16) | (wLow & 0xffff));
        }
        
    }
}
