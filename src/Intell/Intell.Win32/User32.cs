#pragma warning disable CA1401
using System.Runtime.InteropServices;
using System;
using System.Drawing;

namespace Intell.Win32 {
    public static partial class User32 {
        [DllImport("user32")] public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32")] public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32")] public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32")] public static extern bool ReleaseCapture();
        [DllImport("user32")] public static extern bool SetCapture(IntPtr hwnd);

        [DllImport("user32")] public static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);
        [DllImport("user32")] public static extern IntPtr FindWindowExA(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32", CharSet = CharSet.Unicode)] public static extern IntPtr FindWindowW(string lpClassName, string lpWindowName);
        [DllImport("user32", CharSet = CharSet.Unicode)] public static extern IntPtr FindWindowExW(IntPtr hWndParent, IntPtr hWndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32")] public static extern int PostMessageA(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32")] public static extern int PostMessageW(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32")] public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32")] public static extern int SendMessageA(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32")] public static extern int SendMessageW(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32")] public static extern int SendMessageW(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32")] public static extern int GetCursorPos(ref long xy);
        [DllImport("user32")] public static extern int GetCursorPos(out Point point);
        public static int GetCursorPos(out int x, out int y) {
            long xy = 0;
            var result = GetCursorPos(ref xy);
            x = (int)(xy & 4294967295);
            y = (int)(xy >> 32);
            return result;
        }

        [DllImport("user32")] public static extern int SetCursorPos(int x, int y);


        ///<summary>https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msg</summary>
        public struct MSG {
            public IntPtr Handle;
            public int Message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int Time;
            public Point Location;
        }
    }

}
#pragma warning restore CA1401