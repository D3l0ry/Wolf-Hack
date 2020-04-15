using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Wolf_Hack.SDK.WinAPI
{
    [SuppressUnmanagedCodeSecurity]
    internal class NativeMethods
    {
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(KeysCode vKey);
    }
}