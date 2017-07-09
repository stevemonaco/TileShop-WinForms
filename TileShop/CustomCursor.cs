using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TileShop
{
    // Enables the loading of color cursors
    class CustomCursor
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool IsIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr MaskBitmap;
            public IntPtr ColorBitmap;
        };

        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

            [DllImport("user32.dll")]
            public static extern IntPtr CreateIconIndirect([In] ref ICONINFO piconinfo);

            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);

            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateBitmap(int nWidth, int nHeight, uint cPlanes, uint cBitsPerPel, IntPtr lpvBits);
        }

        // Load from a Bitmap with transparency
        public static Cursor LoadCursorFromBitmap(Bitmap bitmap, Point hotSpot)
        {
            ICONINFO iconInfo = new ICONINFO();
            Rectangle rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            try
            {
                if (!NativeMethods.GetIconInfo(bitmap.GetHicon(), out iconInfo))
                    throw new Exception("GetIconInfo() failed.");

                iconInfo.xHotspot = hotSpot.X;
                iconInfo.yHotspot = hotSpot.Y;
                iconInfo.IsIcon = false;

                IntPtr cursorPtr = NativeMethods.CreateIconIndirect(ref iconInfo);
                if (cursorPtr == IntPtr.Zero)
                    throw new Exception("CreateIconIndirect() failed.");

                return (new Cursor(cursorPtr));
            }
            finally
            {
                if (iconInfo.ColorBitmap != IntPtr.Zero)
                    NativeMethods.DeleteObject(iconInfo.ColorBitmap);
                if (iconInfo.MaskBitmap != IntPtr.Zero)
                    NativeMethods.DeleteObject(iconInfo.MaskBitmap);
            }
        }
    }
}
