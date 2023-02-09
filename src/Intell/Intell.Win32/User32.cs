using System.Runtime.InteropServices;
using System;

namespace Intell.Win32 {
    public class User32 {
        [DllImport("user32")] public static extern IntPtr GetDC(IntPtr hWnd);
    }
}