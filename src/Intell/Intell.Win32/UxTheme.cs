using System;
using System.Runtime.InteropServices;

namespace Intell.Win32 {
    public static class UxTheme {
        [DllImport("uxtheme", CharSet = CharSet.Unicode)] public static extern int SetWindowTheme(int hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("uxtheme", CharSet = CharSet.Unicode)] public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        [DllImport("uxtheme")] public static extern int GetWindowTheme(IntPtr hWnd);
    }
}
