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
        /// <param name="el">ArrangerElement to decode</param>
        public static void Decode(Bitmap bmp, ArrangerElement el)
        {
            GraphicsFormat format = FileManager.Instance.GetGraphicsFormat(el.FormatName);

            if (format.ColorType == "indexed")
                IndexedDecode(bmp, el);
            else if (format.ColorType == "direct")
                DirectDecode(bmp, el);
        }

        /// <summary>
        /// Decoding routine to decode indexed (palette-based) graphics
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="el"></param>
        unsafe static void IndexedDecode(Bitmap bmp, ArrangerElement el)
        {
            FileStream fs = FileManager.Instance.GetFileStream(el.FileName);
            GraphicsFormat format = FileManager.Instance.GetGraphicsFormat(el.FormatName);

            byte[] Data = fs.ReadUnshifted(el.FileAddress, format.Size(), true);
            BitStream bs = BitStream.OpenRead(Data, format.Size()); // TODO: Change to account for first bit alignment

            int plane = 0;
            int pos = 0;

            // Deinterlace into separate bitplanes
            foreach (ImageProperty ip in format.ImagePropertyList)
            {
                pos = 0;
                if (ip.RowInterlace)
                {
                    for (int y = 0; y < format.Height; y++)
                    {
                        for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
                        {
                            pos = y * format.Height;
                            for (int x = 0; x < format.Width; x++, pos++)
                                el.TileData[curPlane][pos] = (byte)bs.ReadBit();
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < format.Height; y++)
                        for (int x = 0; x < format.Width; x++, pos++)
                            for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
                                el.TileData[curPlane][pos] = (byte)bs.ReadBit();
                }

                plane += ip.ColorDepth;
            }

            // Merge into foreign colors
            byte foreignColor = 0;

            for (pos = 0; pos < el.MergedData.Length; pos++)
            {
                foreignColor = 0;
                for (int i = 0; i < format.ColorDepth; i++)
                    foreignColor |= (byte)(el.TileData[i][pos] << i);
                el.MergedData[pos] = foreignColor;
            }

            // Translate foreign colors to local colors and draw to bitmap
            DrawBitmap(bmp, el);
        }

        public static void DirectDecode(Bitmap bmp, ArrangerElement el)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws an element onto an ARGB32 Bitmap at the specified location
        /// </summary>
        /// <param name="bmp">Destination bitmap</param>
        /// <param name="el">ArrangerElement to render</param>
        unsafe static void DrawBitmap(Bitmap bmp, ArrangerElement el)
        {
            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            // Draw bitmap
            uint* dest = (uint*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            dest += (bd.Stride / 4) * el.Y1; // Seek to the appropriate vertical scanline in the bitmap

            fixed (byte* fixedData = el.MergedData) // Fix fmt.MergedData in memory so unsafe pointers can be used
            {
                byte* src = fixedData;

                int Height = el.Y2 - el.Y1 + 1;
                int Width = el.X2 - el.X1 + 1;
                Palette pal = FileManager.Instance.GetPalette(el.PaletteName);

                for (int y = 0; y < Height; y++)
                {
                    dest += el.X1; // Seek to PixelX in the scanline
                    for (int x = 0; x < Width; x++)
                    {
                        *dest = pal[*src];
                        dest++;
                        src++;
                    }
                    dest += (bmp.Width - el.X1 - Width);
                    dest += StrideWidth;
                }
            }

            bmp.UnlockBits(bd);
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

        #endregion

        #region Graphics Encoding Functions
        public unsafe static void Encode(Bitmap bmp, ArrangerElement el)
        {
            GraphicsFormat format = FileManager.Instance.GetGraphicsFormat(el.FormatName);

            if (format.ColorType == "indexed")
                IndexedEncode(bmp, el);
            else if (format.ColorType == "direct")
                DirectEncode(bmp, el);
        }

        unsafe static void IndexedEncode(Bitmap bmp, ArrangerElement el)
        {
            // ReadBitmap for local->foreign color conversion into fmt.MergedData
            ReadBitmap(bmp, el);

            FileStream fs = FileManager.Instance.GetFileStream(el.FileName);
            GraphicsFormat format = FileManager.Instance.GetGraphicsFormat(el.FormatName);

            // Loop over MergedData to split foreign colors into bit planes in fmt.TileData
            for (int pos = 0; pos < el.MergedData.Length; pos++)
            {
                for (int i = 0; i < format.ColorDepth; i++)
                    el.TileData[i][pos] = (byte)((el.MergedData[pos] >> i) & 0x1);
            }

            // Loop over planes and putbit to data buffer with proper interlacing
            BitStream bs = BitStream.OpenWrite(format.Size(), 8);
            int plane = 0;


            foreach (ImageProperty ip in format.ImagePropertyList)
            {
                int pos = 0;

                if (ip.RowInterlace)
                {
                    for (int y = 0; y < format.Height; y++)
                    {
                        for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
                        {
                            pos = y * format.Height;
                            for (int x = 0; x < format.Width; x++, pos++)
                                bs.WriteBit(el.TileData[curPlane][pos]);
                        }
                    }
                }
                else
                {
                    for (int y = 0; y < format.Height; y++)
                    {
                        for (int x = 0; x < format.Width; x++, pos++)
                            for (int curPlane = plane; curPlane < plane + ip.ColorDepth; curPlane++)
                                bs.WriteBit(el.TileData[curPlane][pos]);
                    }
                }

                plane += ip.ColorDepth;
            }

            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bs.Data, 0, bs.Data.Length); // TODO: Fix with a shifted, merged write
        }

        unsafe static void DirectEncode(Bitmap bmp, ArrangerElement el)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads an element at a specified location on a ARGB32 Bitmap
        /// </summary>
        /// <param name="bmp">Source bitmap</param>
        /// <param name="el">Destination arranger</param>
        unsafe static void ReadBitmap(Bitmap bmp, ArrangerElement el)
        {
            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

            // Read from bitmap
            uint* src = (uint*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            src += (bd.Stride / 4) * el.Y1; // Seek to scanline PixelY in the bitmap

            fixed (byte* fixedData = el.MergedData)  // Fix fmt.MergedData in memory so unsafe pointers can be used
            {
                byte* dest = fixedData;

                int Height = el.Y2 - el.Y1 + 1;
                int Width = el.X2 - el.X1 + 1;
                Palette pal = FileManager.Instance.GetPalette(el.PaletteName);

                for (int y = 0; y < Height; y++)
                {
                    src += el.X1; // Seek to PixelX in the scanline
                    for (int x = 0; x < Width; x++)
                    {
                        *dest = pal.GetIndexByColor(*src, true);
                        dest++;
                        src++;
                    }
                    src += (bmp.Width - el.X1 - Width);
                    src += StrideWidth;
                }
            }

            bmp.UnlockBits(bd);
        }






        /*public unsafe static void Encode(Bitmap bmp, int PixelX, int PixelY, GraphicsFormat fmt, BinaryWriter bw, Palette pal)
        {
            if (fmt.ColorType == "indexed")
                IndexedEncode(bmp, PixelX, PixelY, fmt, bw, pal);
            else if (fmt.ColorType == "direct")
                DirectEncode(bmp, PixelX, PixelY, fmt, bw);
        }*/

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
            BitStream bs = BitStream.OpenWrite(fmt.Size(), 8);
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
