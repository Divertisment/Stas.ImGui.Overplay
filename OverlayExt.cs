
using Stas.ImGui.Overplay.Win32;
using System.Runtime.InteropServices;
using System;

namespace Stas.ImGui.Overplay {
    public abstract partial class Overlay  {
        public nint OverlayPtr => this.window == null ? default : this.window.Handle;
        const int GWL_EXSTYLE = -20;
        const int GWL_STYLE = -16;
        const byte WS_EX_TOPMOST = 0x8;
        const short SWP_NOMOVE = 0X2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 0X4;
        const int SWP_SHOWWINDOW = 0x0040;
        const int SWP_FRAMECHANGED = 0x0020;
        const int HWND_TOPMOST = -1;
        const int HWND_NOTOPMOST = -2;
        //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowlongptra
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        public void SetOverlayClickable(bool clickable, bool must_be_top = true) {
            Utils.SetOverlayClickable(this.window.Handle, clickable);
            SetOverlayTopMost(must_be_top);
        }
        //https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
        public void SetOverlayTopMost(bool b_top_most = true) {
            var curr_exstyle = GetWindowLongPtr(this.window.Handle, GWL_EXSTYLE).ToInt64();
            //var curr_exstyle_hex = curr_exstyle.ToString("X");

            var have = (curr_exstyle & WS_EX_TOPMOST) == WS_EX_TOPMOST;
            if (have && !b_top_most) {
                curr_exstyle &= ~WS_EX_TOPMOST;
                //var new_wiout8 = curr_exstyle.ToString("X");
                SetWindowLongPtr(this.window.Handle, GWL_EXSTYLE, new IntPtr(curr_exstyle));
                SetWindowPos(this.window.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
                //var nces = GetWindowLongPtr(this.window.Handle, GWL_EXSTYLE).ToString("X"); //new curr extended style
            }
            if (!have && b_top_most) {
                curr_exstyle |= WS_EX_TOPMOST;
                var cs_hex = curr_exstyle.ToString("X");
                SetWindowLongPtr(this.window.Handle, GWL_EXSTYLE, new IntPtr(curr_exstyle));
                SetWindowPos(this.window.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
                // var nces = GetWindowLongPtr(this.window.Handle, GWL_EXSTYLE).ToString("X"); //new curr extended style
            }
        }
    }
}
