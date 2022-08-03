using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GameTranslator
{
    static class WinAPIWrapper
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr windowPtr, IntPtr newParentWindow);

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        // Может использоваться не просто для изменения позиции окна на экране, но также изменения Z order позиции, позволяя сделать окно TopMost
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int width, int height, uint uFlags);

        // Позволяет менять значение прозрачности (альфы) всего окна полностью (фактически работает как Opacity свойство), а также цвет который будет отвечать за альфу
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint color, byte alpha, uint dwFlags);

        public const int GWL_EXSTYLE = -20;
        public const uint WS_EX_LAYERED = 0x80000;
        public const uint WS_EX_TRANSPARENT = 0x20;
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern uint SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
