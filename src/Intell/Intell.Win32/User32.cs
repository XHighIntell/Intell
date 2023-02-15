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

        [DllImport("user32")] public static extern int PeekMessage(out MSG message, IntPtr window, int filterMin, int filterMax, int remove);
        [DllImport("user32")] public static extern int GetMessageA(out MSG message, IntPtr window, int filterMin, int filterMax);
        [DllImport("user32")] public static extern int GetMessageW(out MSG message, IntPtr window, int filterMin, int filterMax);
        [DllImport("user32")] public static extern int TranslateMessage(ref MSG message);
        [DllImport("user32")] public static extern int DispatchMessageA(ref MSG message);
        [DllImport("user32")] public static extern int DispatchMessageW(ref MSG message);

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
        [DllImport("user32")] public static extern int RegisterHotKey(IntPtr hWnd, int id, int Modifiers, int vkey);
        [DllImport("user32")] public static extern int UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32")] public static extern int GetWindowLongA(IntPtr hWnd, int nIndex);
        [DllImport("user32")] public static extern int GetWindowLongW(IntPtr hWnd, int nIndex);
        [DllImport("user32")] public static extern int SetWindowLongA(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32")] public static extern int SetWindowLongPtrA(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32")] public static extern int SetWindowLongPtrW(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32")] public static extern int SetLayeredWindowAttributes(IntPtr hwnd, int crKey, byte bAlpha, int dwFlags);
        [DllImport("user32")] public static extern int DefWindowProc(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32")] public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32")] public static extern int TrackPopupMenu(IntPtr hMenu, uint uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);

        //#if WINDOWS || NET46
        //        ///<summary>Get message, translate and dispatch incoming messages if any exist.</summary>
        //        public static void DoEvents() {
        //            MSG nativeMessage;
        //
        //            while (User32.PeekMessage(out nativeMessage, IntPtr.Zero, 0, 0, 0) != 0) {
        //                User32.GetMessageW(out nativeMessage, IntPtr.Zero, 0, 0);
        //
        //                System.Windows.Forms.Message message = new System.Windows.Forms.Message() { HWnd = nativeMessage.Handle, Msg = nativeMessage.Message, WParam = nativeMessage.wParam, LParam = nativeMessage.lParam };
        //                if (System.Windows.Forms.Application.FilterMessage(ref message) == false) {
        //                    User32.TranslateMessage(ref nativeMessage);
        //                    User32.DispatchMessageW(ref nativeMessage);
        //                }
        //            }
        //        }
        //#endif

        ///<summary>https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-msg</summary>
        public struct MSG {
            public IntPtr Handle;
            public int Message;
            public IntPtr wParam;
            public IntPtr lParam;
            public int Time;
            public Point Location;
        }
        public struct POINT { public int x; public int y; }
        public struct RECT { public int left; public int top; public int right; public int bottom; }
        public struct WINDOWPOS {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public uint flags;
        }
        public struct MINMAXINFO {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
        public struct NCCALCSIZE_PARAMS {
            public RECT rgrc0;
            public RECT rgrc1;
            public RECT rgrc2;
            public WINDOWPOS lppos;
        }

        public static class NCHitTest {
            public const int HTERROR = -1;
            public const int HTTRANSPARENT = -1;
            public const int HTNOWHERE = 0;
            public const int HTCLIENT = 1;
            public const int HTCAPTION = 2;
            public const int HTSYSMENU = 3;
            public const int HTGROWBOX = 4;
            public const int HTMENU = 5;
            public const int HTHSCROLL = 6;
            public const int HTVSCROLL = 7;
            public const int HTMINBUTTON = 8;
            public const int HTMAXBUTTON = 9;
            public const int HTLEFT = 10;
            public const int HTRIGHT = 11;
            public const int HTTOP = 12;
            public const int HTTOPLEFT = 13;
            public const int HTTOPRIGHT = 14;
            public const int HTBOTTOM = 15;
            public const int HTBOTTOMLEFT = 16;
            public const int HTBOTTOMRIGHT = 17;
            public const int HTBORDER = 18;
        }
        public static class WindowStyles {
            public const int WS_MAXIMIZEBOX = 0x00010000;
            public const int WS_MINIMIZEBOX = 0x00020000;
            public const int WS_THICKFRAME = 0x00040000;
            public const int WS_SYSMENU = 0x00080000;
            public const int WS_HSCROLL = 0x00100000;
            public const int WS_VSCROLL = 0x00200000;
            public const int WS_CLIPCHILDREN = 0x02000000;
            public const int WS_CLIPSIBLINGS = 0x04000000;
        }
        public static class WindowStylesExtended {
            public const int WS_EX_TOPMOST = 0x00000008;
            public const int WS_EX_TRANSPARENT = 0x00000020;
            public const int WS_EX_LAYERED = 0x00080000;
        }
    }

    
}
#pragma warning restore CA1401