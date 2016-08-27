using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TileShop
{
    public enum PaletteColorFormat { RGB24 = 0, ARGB32, BGR15, ABGR16, NES }

    public class Palette
    {
        public string Name { get; private set; }
        public PaletteColorFormat ColorFormat { get; private set; }
        public long FileOffset { get; private set; }
        public string FileName { get; private set; }
        public int Entries { get; private set; }

        UInt32[] palette;

        public Palette(string PaletteName)
        {
            Name = PaletteName;
        }

        // Load Palette from a new file, only in ARGB32 format
        public bool LoadPalette(string filename)
        {
            palette = new UInt32[256];

            BinaryReader br = new BinaryReader(File.OpenRead(filename));

            int numColors = (int)br.BaseStream.Length / 4;

            if (numColors != 256)
                return false;

            for (int idx = 0; idx < numColors; idx++)
            {
                palette[idx] = br.ReadUInt32();
                //palette[idx] = ((palette[idx] & 0xFF) << 24) | ((palette[idx] & 0xFF00) << 8) | ((palette[idx] & 0xFF0000) >> 8) | ((palette[idx] & 0xFF0000) >> 24);
            }

            ColorFormat = PaletteColorFormat.ARGB32;
            FileOffset = 0;
            FileName = filename;
            Entries = 256;

            return true;
        }

        // Load Palette from an already loaded file
        public bool LoadPalette(string FileId, long FileOffset, PaletteColorFormat Format, int NumEntries)
        {
            if (NumEntries > 256)
                throw new ArgumentException("Maximum palette indices must be 256 or less");

            palette = new UInt32[256];

            FileStream fs = FileManager.Instance.GetFileStream(FileId);
            BinaryReader br = new BinaryReader(fs);

            fs.Seek(FileOffset, SeekOrigin.Begin);

            int ReadSize = 4;

            if (Format == PaletteColorFormat.BGR15 || Format == PaletteColorFormat.ABGR16)
                ReadSize = 2;

            if (Format == PaletteColorFormat.RGB24)
                ReadSize = 3;

            for(int i = 0; i < NumEntries; i++)
            {
                UInt32 ColorIn;

                if (ReadSize == 1)
                    ColorIn = br.ReadByte();
                else if (ReadSize == 2)
                    ColorIn = br.ReadUInt16();
                else if (ReadSize == 3)
                {
                    ColorIn = ((UInt32)br.ReadByte());
                    ColorIn = ((UInt32)br.ReadByte()) << 8;
                    ColorIn = ((UInt32)br.ReadByte()) << 16;
                }
                else // 4 bytes
                    ColorIn = br.ReadUInt32();

                UInt32 ColorOut = ToArgb32(ColorIn, Format);

                palette[i] = ColorOut;
            }

            ColorFormat = Format;
            FileOffset = 0;
            FileName = FileId;
            Entries = NumEntries;

            return true;
        }

        public UInt32 ToArgb32(UInt32 ColorIn, PaletteColorFormat ColorFormat)
        {
            UInt32 ColorOut;

            if (ColorFormat == PaletteColorFormat.BGR15)
            {
                ColorOut = (ColorIn & 0x1F) << 19; // Red
                ColorOut |= (ColorIn & 0x3E0) << 6; // Blue
                ColorOut |= (ColorIn & 0x7C00) >> 7; // Green
            }
            else if (ColorFormat == PaletteColorFormat.ABGR16)
            {
                ColorOut = (ColorIn & 0x1F) << 19; // Red
                ColorOut |= (ColorIn & 0x3E0) << 6; // Blue
                ColorOut |= (ColorIn & 0x7C00) >> 7; // Green
                ColorOut |= ((ColorIn & 0x8000) * 255) << 24;
            }
            else
                throw new ArgumentException("Unsupported PaletteColorFormat");

            return ColorOut;
        }

        public UInt32 this[int i]
        {
            get
            {
                if (palette == null)
                    throw new ArgumentNullException();

                return palette[i];
            }
        }
    }
}
