using System;
using System.Runtime.InteropServices;

namespace Intell.Win32;

public static class Gdi32 {
    [DllImport("gdi32")] public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);
    [DllImport("gdi32")] public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);
    [DllImport("gdi32")] public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    [DllImport("gdi32")] public static extern bool DeleteDC([In] IntPtr hdc);
    [DllImport("gdi32")] public static extern bool DeleteObject([In] IntPtr hObject);
}
