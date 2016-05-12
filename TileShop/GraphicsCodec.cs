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
        public static void Decode(Bitmap bmp, GraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            if (fmt.ImageType == "tiled")
                TiledDecode(bmp, fmt, Offset, br, pal);
        }

        public unsafe static void TiledDecode(Bitmap bmp, GraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            byte[] data = br.ReadBytes(fmt.Size());
            BitStream bs = new BitStream(data, data.Length * 8);

            int plane = 0;
            foreach (ImageProperty tp in fmt.ImagePropertyList)
            {
                if (tp.RowInterlace)
                {
                    for (int y = 0; y < fmt.Height; y++)
                        for(int curPlane = plane; curPlane < plane + tp.ColorDepth; curPlane++)
                            for (int x = 0; x < fmt.Width; x++)
                                fmt.TileData[curPlane][x + y * fmt.Height] = (byte)bs.ReadBit();
                }
                else
                {
                    for (int y = 0; y < fmt.Height; y++)
                        for (int x = 0; x < fmt.Width; x++)
                            for (int curPlane = plane; curPlane < plane + tp.ColorDepth; curPlane++)
                                fmt.TileData[curPlane][x + y * fmt.Height] = (byte)bs.ReadBit();
                }

                plane += tp.ColorDepth;
            }

            // Merge into composite tile
            byte idx = 0;
            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, System.Drawing.Imaging.ImageLockMode.WriteOnly, bmp.PixelFormat);

            for (int y = 0; y < fmt.Height; y++)
            {
                for (int x = 0; x < fmt.Height; x++)
                {
                    idx = 0;
                    for (int i = 0; i < fmt.ColorDepth; i++)
                        idx |= (byte)(fmt.TileData[i][x + y * fmt.Height] << i);
                    fmt.MergedData[x + y * fmt.Height] = idx;
                }
            }

            // Draw bitmap
            int* dest = (int*)(void*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            for (int y = 0; y < fmt.Height; y++)
            {
                for (int x = 0; x < fmt.Width; x++)
                {
                    *dest = pal[fmt.MergedData[x + y * fmt.Height]].ToArgb();
                    dest++;
                }
                dest += StrideWidth;
            }

            bmp.UnlockBits(bd);
        }
    }
}
