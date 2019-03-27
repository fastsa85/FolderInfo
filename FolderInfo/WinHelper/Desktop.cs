using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Automation;
using Microsoft.Win32;

namespace FolderInfo.WinHelper
{
    internal class Desktop
    {
        private readonly IntPtr _desktopHandle;
        private readonly List<string> _currentIconsOrder;

        public Desktop()
        {
            _desktopHandle = Windows.GetDesktopWindow();

            AutomationElement el = AutomationElement.FromHandle(_desktopHandle);

            TreeWalker walker = TreeWalker.ContentViewWalker;
            _currentIconsOrder = new List<string>();
            for (AutomationElement child = walker.GetFirstChild(el);
                child != null;
                child = walker.GetNextSibling(child))
            {
                _currentIconsOrder.Add(child.Current.Name);                
            }
        }

        /// <summary>
        /// Set position of icon specified by icon's name.
        /// Desctop top left  corner as desktop origin 
        /// Icon top left corner as icon origin
        /// </summary>
        /// <param name="iconName">Target icon name</param>
        /// <param name="x">X coordinate (0... desktop width)</param>
        /// <param name="y">Y coordinate (0... desktop height)</param>
        public void SetIconPosition(string iconName, int x, int y)
        {
            var iconIndex = _currentIconsOrder.IndexOf(iconName);
            Windows.SendMessage(_desktopHandle, Windows.LVM_SETITEMPOSITION, iconIndex, Windows.MakeLParamFor_LVM_SETITEMPOSITION(x, y));            
        }

        /// <summary>
        /// Returns width of desktop
        /// </summary>
        /// <returns></returns>
        public int GetWidth()
        {            
            var rect = Screen.PrimaryScreen.Bounds;
            return rect.Width;
        }
        
        /// <summary>
        /// Returns actual size of desktop icons
        /// </summary>
        /// <returns></returns>
        public int GetIconSize()
        {
            object size = RegistryHelper.GetRegistryValue(Registry.CurrentUser, @"Control Panel\Desktop\WindowMetrics","Shell Icon Size", -1);
            return Convert.ToInt32(size);
        }
    }
}
