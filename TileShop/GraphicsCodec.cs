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
    /// <summary>
    /// GraphicsCodec provides a set of codecs for generalized bitmap formats
    /// The process goes through several stages of transformations
    /// For indexed bitmaps:
    /// 1. Deinterlace a bitmap's pixels into separate bitplanes
    /// 2. Merge bitplanes into indexed foreign colors
    /// 3. Create bitmap by translating foreign colors into local colors using a palette
    /// 4. Apply pixel remapping operations
    /// 
    /// For direct bitmaps:
    /// In development
    /// </summary>
    public class GraphicsCodec
    {
        #region Graphics Decoding Functions

        /*
        public static void DecodeElement(Bitmap bmp, ArrangerElement el)
        {
            GraphicsFormat fmt = FileManager.Instance.GetGraphicsFormat(el.Format);
            BinaryReader br = new BinaryReader(FileManager.Instance.GetFileStream(el.FileName));
            br.BaseStream.Seek(el.FileOffset, SeekOrigin.Begin);

            if (fmt.ColorType == "indexed")
                IndexedDecode(bmp, el.X1, el.Y1, fmt, br, FileManager.Instance.GetPalette(el.Palette));
            else if (fmt.ColorType == "direct")
                DirectDecode(bmp, el.X1, el.Y1, fmt, br);
            else
                throw new NotSupportedException("GraphicsFormat ColorType " + fmt.ColorType + " is not supported");
        }
        */

        /// <summary>
        /// General-purpose routine to decode a single graphical element
        /// </summary>
        /// <param name="bmp">Bitmap to draw onto</param>
        /// <param name="PixelX">Upper left x-coordinate to begin drawing</param>
        /// <param name="PixelY">Upper left y-coordinate to begin drawing</param>
        /// <param name="format">Graphics format to decode</param>
        /// <param name="br">Binary reader seeked to the source of the graphic offset</param>
        /// <param name="pal">Palette to use for indexed color decodes</param>
        public static void Decode(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat format, BinaryReader br, Palette pal)
        {
            if (format.ColorType == "indexed")
                IndexedDecode(bmp, PixelX, PixelY, format, br, pal);
            else if (format.ColorType == "direct")
                DirectDecode(bmp, PixelX, PixelY, format, br);
        }

        /// <summary>
        /// Draws a blank element using the 0-index color from the default palette
        /// Used for when an arranger does not have a graphic assigned to every element
        /// </summary>
        /// <param name="bmp">Bitmap to draw onto</param>
        /// <param name="el">Element with specified coordinates</param>
        public static void DecodeBlank(Bitmap bmp, ArrangerElement el)
        {
            Color c = Color.FromArgb(el.PaletteName[0]);
            Brush b = new SolidBrush(c);
            Rectangle r = new Rectangle(el.X1, el.Y1, (el.X2 - el.X1) + 1, (el.Y2 - el.Y1) + 1);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(b, r);
        }

        unsafe static void IndexedDecode(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, BinaryReader br, Palette pal)
        {
            BitStream bs = BitStream.OpenRead(br, fmt.Size() * 8, 8);

            int plane = 0;
            int pos = 0;

            // Deinterlace into separate bitplanes
            foreach (ImageProperty ip in fmt.ImagePropertyList)
            {
                pos = 0;
                if (ip.RowInterlace)
                {
                    for (int y = 0; y < fmt.Height; y++)
                    {
                        for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
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
                            for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
                                fmt.TileData[curPlane][pos] = (byte)bs.ReadBit();
                }

                plane += ip.ColorDepth;
            }

            // Merge into foreign colors
            byte foreignColor = 0;

            for(pos = 0; pos < fmt.MergedData.Length; pos++)
            {
                foreignColor = 0;
                for (int i = 0; i < fmt.ColorDepth; i++)
                    foreignColor |= (byte)(fmt.TileData[i][pos] << i);
                fmt.MergedData[pos] = foreignColor;
            }

            // Translate foreign colors to local colors and draw to bitmap
            DrawBitmap(bmp, PixelX, PixelY, fmt, pal);
        }



        unsafe static void DirectDecode(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, BinaryReader br)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Graphics Encoding Functions
        public unsafe static void EncodeElement(Bitmap bmp, ArrangerElement el)
        {

        }

        public unsafe static void Encode(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, BinaryWriter bw, Palette pal)
        {
            if (fmt.ColorType == "indexed")
                IndexedEncode(bmp, PixelX, PixelY, fmt, bw, pal);
            else if (fmt.ColorType == "direct")
                DirectEncode(bmp, PixelX, PixelY, fmt, bw);
        }

        unsafe static void IndexedEncode(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, BinaryWriter bw, Palette pal)
        {
            // ReadBitmap for local->foreign color conversion into fmt.MergedData
            ReadBitmap(bmp, PixelX, PixelY, fmt, pal);

            // Loop over MergedData to split foreign colors into bit planes in fmt.TileData
            for (int pos = 0; pos < fmt.MergedData.Length; pos++)
            {
                for (int i = 0; i < fmt.ColorDepth; i++)
                    fmt.TileData[i][pos] = (byte)((fmt.MergedData[pos] >> i) & 0x1);
            }

            // Loop over planes and putbit to data buffer with proper interlacing
            BitStream bs = BitStream.OpenWrite(fmt.Size() * 8, 8);
            int plane = 0;


            foreach (ImageProperty ip in fmt.ImagePropertyList)
            {
                int pos = 0;

                if (ip.RowInterlace)
                {
                    for (int y = 0; y < fmt.Height; y++)
                    {
                        for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
                        {
                            pos = y * fmt.Height;
                            for (int x = 0; x < fmt.Width; x++, pos++)
                                bs.WriteBit(fmt.TileData[curPlane][pos]);
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < fmt.Height; y++)
                    {
                        for (int x = 0; x < fmt.Width; x++, pos++)
                            for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
                                bs.WriteBit(fmt.TileData[curPlane][pos]);
                    }
                }

                plane += ip.ColorDepth;
            }

            bw.Write(bs.Data, 0, fmt.Size());
        }

        unsafe static void DirectEncode(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// Draws an element onto an ARGB32 Bitmap at the specified location
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="PixelX"></param>
        /// <param name="PixelY"></param>
        /// <param name="fmt"></param>
        /// <param name="pal"></param>
        unsafe static void DrawBitmap(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, Palette pal)
        {
            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            // Draw bitmap
            uint* dest = (uint*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            dest += (bd.Stride / 4) * PixelY; // Seek to scanline PixelY in the bitmap

            fixed(byte* fixedData = fmt.MergedData) // Fix fmt.MergedData in memory so unsafe pointers can be used
            {
                byte* src = fixedData;
                for (int y = 0; y < fmt.Height; y++)
                {
                    dest += PixelX; // Seek to PixelX in the scanline
                    for (int x = 0; x < fmt.Width; x++)
                    {
                        *dest = pal[*src];
                        dest++;
                        src++;
                    }
                    dest += (bmp.Width - PixelX - fmt.Width);
                    dest += StrideWidth;
                }
            }

            bmp.UnlockBits(bd);
        }

        /// <summary>
        /// Reads an element at a specified location on a ARGB32 Bitmap
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="PixelX"></param>
        /// <param name="PixelY"></param>
        /// <param name="fmt"></param>
        /// <param name="pal"></param>
        unsafe static void ReadBitmap(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, Palette pal)
        {
            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

            // Read from bitmap
            uint* src = (uint*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            src += (bd.Stride / 4) * PixelY; // Seek to scanline PixelY in the bitmap

            fixed(byte* fixedData = fmt.MergedData)  // Fix fmt.MergedData in memory so unsafe pointers can be used
            {
                byte* dest = fixedData;

                for (int y = 0; y < fmt.Height; y++)
                {
                    src += PixelX; // Seek to PixelX in the scanline
                    for (int x = 0; x < fmt.Width; x++)
                    {
                        *dest = pal.GetIndexByColor(*src, true);
                        dest++;
                        src++;
                    }
                    src += (bmp.Width - PixelX - fmt.Width);
                    src += StrideWidth;
                }
            }

            bmp.UnlockBits(bd);
        }
    }
}
