using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace TileShop
{
    public enum OldGraphicsGridType { LinearGrid = 0, TiledGrid };
    public enum OldGraphicsEncodingType { NES2bpp = 1, SNES2bpp = 2 };

    public struct OldGraphicsFormat
    {
        public uint Width, Height;
        public uint ColorDepth;
        public string ImageType;
        public OldGraphicsGridType GridType;
        public OldGraphicsEncodingType GraphicsType;
        public int[] TiledInterlace;

        public uint Size()
        {
            return Width * Height * ColorDepth / 8;
        }
    }

    public class OldGraphicsCodec
    {
        public static void Decode(Bitmap bmp, OldGraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            switch(fmt.GraphicsType)
            {
                case OldGraphicsEncodingType.NES2bpp:
                    TiledDecodeNESFast(bmp, fmt, Offset, br, pal);
                    break;
                case OldGraphicsEncodingType.SNES2bpp:
                    TiledDecodeSNESFast(bmp, fmt, Offset, br, pal);
                    break;
            }
        }

        private static void TiledDecodeNES(Bitmap bmp, OldGraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            // NES
            int idx = 0;

            // Setup BitStreams
            if (Offset != -1) // -1 For no seek
                br.BaseStream.Seek(Offset, SeekOrigin.Begin);
            byte[] data = br.ReadBytes((int)fmt.Size());
            byte[] data1 = new byte[data.Length / 2];
            byte[] data2 = new byte[data.Length / 2];

            for (int i = 0, j = data.Length / 2; i < data.Length / 2; i++, j++)
            {
                data1[i] = data[i];
                data2[i] = data[j];
            }

            BitStream bs1 = new BitStream(data1, data1.Length * 8);
            BitStream bs2 = new BitStream(data2, data2.Length * 8);

            for (int y = 0; y < fmt.Height; y++)
            {
                for (int x = 0; x < fmt.Width; x++)
                {
                    idx = bs2.ReadBit() << 1;
                    idx |= bs1.ReadBit();

                    bmp.SetPixel(x, y, pal[idx]);
                }
            }

        }

        private static void TiledDecodeSNES(Bitmap bmp, OldGraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            int idx = 0;

            // Setup BitStreams
            if (Offset != -1) // -1 For no seek
                br.BaseStream.Seek(Offset, SeekOrigin.Begin);
            byte[] data = br.ReadBytes((int)fmt.Size());
            byte[] data1 = new byte[data.Length / 2];
            byte[] data2 = new byte[data.Length / 2];

            for (int i = 0; i < data.Length / 2; i++)
            {
                data1[i] = data[i * 2];
                data2[i] = data[i * 2 + 1];
            }

            BitStream bs1 = new BitStream(data1, data1.Length * 8);
            BitStream bs2 = new BitStream(data2, data2.Length * 8);

            for (int y = 0; y < fmt.Height; y++)
            {
                for (int x = 0; x < fmt.Width; x++)
                {
                    idx = bs2.ReadBit() << 1;
                    idx |= bs1.ReadBit();

                    bmp.SetPixel(x, y, pal[idx]);
                }
            }

        }

        private static unsafe void TiledDecodeNESFast(Bitmap bmp, OldGraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            // Setup BitStreams
            if (Offset != -1) // -1 For no seek
                br.BaseStream.Seek(Offset, SeekOrigin.Begin);

            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

            byte[] data = br.ReadBytes((int)fmt.Size());
            byte[] data1 = new byte[data.Length / 2];
            byte[] data2 = new byte[data.Length / 2];

            for (int i = 0, j = data.Length / 2; i < data.Length / 2; i++, j++)
            {
                data1[i] = data[i];
                data2[i] = data[j];
            }

            BitStream bs1 = new BitStream(data1, data1.Length * 8);
            BitStream bs2 = new BitStream(data2, data2.Length * 8);

            int idx = 0;
            int* dest = (int*)(void*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            for (int y = 0; y < fmt.Height; y++)
            {
                for (int x = 0; x < fmt.Width; x++)
                {
                    idx = bs2.ReadBit() << 1;
                    idx |= bs1.ReadBit();

                    *dest = pal[idx].ToArgb();
                    dest++;
                }
                dest += StrideWidth;
            }

            bmp.UnlockBits(bd);

        }

        // Unsafe code with LockBits - removes SetPixel, still uses BitStream
        private static unsafe void TiledDecodeSNESFast(Bitmap bmp, OldGraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            // Setup BitStreams
            if (Offset != -1) // -1 For no seek
                br.BaseStream.Seek(Offset, SeekOrigin.Begin);

            Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

            byte[] data = br.ReadBytes((int)fmt.Size());
            byte[] data1 = new byte[data.Length / 2];
            byte[] data2 = new byte[data.Length / 2];

            for (int i = 0; i < data.Length / 2; i++)
            {
                data1[i] = data[i * 2];
                data2[i] = data[i * 2 + 1];
            }

            BitStream bs1 = new BitStream(data1, data1.Length * 8);
            BitStream bs2 = new BitStream(data2, data2.Length * 8);

            int idx = 0;
            int* dest = (int*)(void*)bd.Scan0;
            int StrideWidth = bd.Stride - (bmp.Width * 4);

            for (int y = 0; y < fmt.Height; y++)
            {
                for (int x = 0; x < fmt.Width; x++)
                {
                    idx = bs2.ReadBit() << 1;
                    idx |= bs1.ReadBit();

                    *dest = pal[idx].ToArgb();
                    dest++;
                }
                dest += StrideWidth;
            }

            bmp.UnlockBits(bd);
        }

        // Unsafe code and restricted to 8x8 tiles
        private static void TiledDecodeSNESFast2(Bitmap bmp, OldGraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {
            // Setup BitStreams
            if (Offset != -1) // -1 For no seek
                br.BaseStream.Seek(Offset, SeekOrigin.Begin);

            unsafe
            {
                Rectangle lockRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                BitmapData bd = bmp.LockBits(lockRect, ImageLockMode.WriteOnly, bmp.PixelFormat);

                byte[] data = br.ReadBytes((int)fmt.Size());

                int idx = 0;
                int* dest = (int*)(void*)bd.Scan0;
                int StrideWidth = bd.Stride - (bmp.Width * 4);

                int pos = 0;

                for (int y = 0; y < fmt.Height; y++)
                {
                    for (int x = 0; x < fmt.Width; x++)
                    {
                        idx = (data[pos] & (1 << (7 - x))) >> (7 - x);
                        idx |= (data[pos + 1] & (1 << (7 - x))) >> (6 - x);

                        *dest = pal[idx].ToArgb();
                        dest++;
                    }
                    dest += StrideWidth;
                    pos += 2;
                }

                bmp.UnlockBits(bd);
            }

        }

        private static void LinearDecode(Bitmap bmp, OldGraphicsFormat fmt, int Offset, BinaryReader br, Color[] pal)
        {

        }

        public static void Encode(Bitmap bmp, OldGraphicsFormat format)
        {
        }
    }
}
