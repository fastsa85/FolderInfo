using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Drawing;

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


        public void SetIconPosition(string iconName)
        {
            var iconIndex = _currentIconsOrder.IndexOf(iconName);
            Windows.SendMessage(_desktopHandle, Windows.LVM_SETITEMPOSITION, iconIndex, Windows.MakeLParamFor_LVM_SETITEMPOSITION(960, 540));
            
        }

        public int GetWidth()
        {
            //Rect rect = new Rect();
            //Windows.GetWindowRect(_desktopHandle, out rect);
            //return (int)rect.Width;

            var rect = Screen.PrimaryScreen.Bounds;
            return rect.Width;
        }

        public int GetIconSize(string iconName)
        {
            var iconIndex = _currentIconsOrder.IndexOf(iconName);
            Rect rect = new Rect();

            IntPtr pRct = Marshal.AllocHGlobal(Marshal.SizeOf(rect));
            Marshal.StructureToPtr(rect, pRct, false);
            
            var r = Windows.SendMessage(_desktopHandle, Windows.LVM_GETITEMRECT, iconIndex, pRct);

            Rect result = (Rect)Marshal.PtrToStructure(pRct, typeof(Rect));
            Marshal.FreeHGlobal(pRct);


            return (int)result.Width;
            

        }
    }
}
