using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace TileShop
{
    public class GraphicsCodec
    {
        public static void Decode(Bitmap bmp, GraphicsFormat fmt, int OffsetX, int OffsetY, BinaryReader br, Palette pal)
        {
            if (fmt.ColorType == "indexed")
                IndexedDecode(bmp, fmt, OffsetX, OffsetY, br, pal);
        }

        // Draws a blank background
        public static void DecodeBlank(Bitmap bmp, ArrangerElement el)
        {
            Color c = Color.FromArgb(el.Palette[0]);
            Brush b = new SolidBrush(c);
            Rectangle r = new Rectangle(el.X1, el.Y1, (el.X2 - el.X1) + 1, (el.Y2 - el.Y1) + 1);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(b, r);
        }

        public unsafe static void IndexedDecode(Bitmap bmp, GraphicsFormat fmt, int OffsetX, int OffsetY, BinaryReader br, Palette pal)
        {
            byte[] data = br.ReadBytes(fmt.Size());
            BitStream bs = new BitStream(data, data.Length * 8); // TODO: Add Constructor for BinaryReader, length (optimize copy)

            int plane = 0;
            int pos = 0;

            foreach (ImageProperty tp in fmt.ImagePropertyList)
            {
                pos = 0;
                if (tp.RowInterlace)
                {
                    for (int y = 0; y < fmt.Height; y++)
                    {
                        for (int curPlane = plane; curPlane < plane + tp.ColorDepth; curPlane++)
                        {
                            pos = y * fmt.Height;
                            for (int x = 0; x < fmt.Width; x++, pos++)
                                fmt.TileData[curPlane][pos] = (byte)bs.ReadBit();
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < fmt.Height; y++)
                        for (int x = 0; x < fmt.Width; x++, pos++)
                            for (int curPlane = plane; curPlane < plane + tp.ColorDepth; curPlane++)
                                fmt.TileData[curPlane][pos] = (byte)bs.ReadBit();
                }

                plane += tp.ColorDepth;
            }

            // Merge into composite tile
            byte idx = 0;

            for(pos = 0; pos < fmt.MergedData.Length; pos++)
            {
                idx = 0;
                for (int i = 0; i < fmt.ColorDepth; i++)
                    idx |= (byte)(fmt.TileData[i][pos] << i);
                fmt.MergedData[pos] = idx;
            }

            DrawBitmap(bmp, fmt, OffsetX, OffsetY, pal);
        }

        private static unsafe void DrawBitmap(Bitmap bmp, GraphicsFormat fmt, int OffsetX, int OffsetY, Palette pal)
        {
            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

            // Draw bitmap
            uint* dest = (uint*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            dest += (bd.Stride / 4) * OffsetY;

            fixed(byte* fixedData = fmt.MergedData) // Fix fmt.MergedData in memory so pointers can be used in unsafe code without copying via marshal
            {
                byte* src = fixedData;
                for (int y = 0; y < fmt.Height; y++)
                {
                    dest += OffsetX;
                    for (int x = 0; x < fmt.Width; x++)
                    {
                        *dest = pal[*src];
                        dest++;
                        src++;
                    }
                    dest += (bmp.Width - OffsetX - fmt.Width);
                    dest += StrideWidth;
                }
            }

            bmp.UnlockBits(bd);
        }
    }
}
