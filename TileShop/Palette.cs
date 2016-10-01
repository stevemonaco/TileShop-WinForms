using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using ColorMine.ColorSpaces.Comparisons;
using ColorMine.ColorSpaces.Conversions;
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
        public bool HasAlpha { get; private set; }
        public bool ZeroIndexTransparent { get; private set; }

        uint[] palette; // Colors stored in ARGB order

        public Palette(string PaletteName)
        {
            Name = PaletteName;

            HasAlpha = false;
            Entries = 0;
            FileName = null;
            FileOffset = 0;
            ZeroIndexTransparent = true;
        }

        // Load Palette from a new file, only in ARGB32 format
        public bool LoadPalette(string filename)
        {
            palette = new uint[256];

            BinaryReader br = new BinaryReader(File.OpenRead(filename));

            int numColors = (int)br.BaseStream.Length / 4;

            if (numColors != 256)
                return false;

            for (int idx = 0; idx < numColors; idx++)
            {
                palette[idx] = br.ReadUInt32();
                palette[idx] |= 0xFF000000; // Disable transparency
            }

            ColorFormat = PaletteColorFormat.ARGB32;
            FileOffset = 0;
            FileName = filename;
            Entries = 256;

            return true;
        }

        // Load Palette from an already loaded file
        public bool LoadPalette(string FileId, long offset, PaletteColorFormat Format, int NumEntries)
        {
            if (NumEntries > 256)
                throw new ArgumentException("Maximum palette indices must be 256 or less");

            palette = new UInt32[256];

            FileStream fs = FileManager.Instance.GetFileStream(FileId);
            BinaryReader br = new BinaryReader(fs);

            fs.Seek(offset, SeekOrigin.Begin);

            int ReadSize;

            switch(Format)
            {
                case PaletteColorFormat.BGR15:
                    ReadSize = 2;
                    HasAlpha = false;
                    break;
                case PaletteColorFormat.ABGR16:
                    ReadSize = 2;
                    HasAlpha = true;
                    break;
                case PaletteColorFormat.RGB24:
                    ReadSize = 3;
                    HasAlpha = false;
                    break;
                case PaletteColorFormat.ARGB32:
                    ReadSize = 4;
                    HasAlpha = true;
                    break;
                default:
                    throw new NotSupportedException();
            }

            for(int i = 0; i < NumEntries; i++)
            {
                uint ColorIn;

                if (ReadSize == 1)
                    ColorIn = br.ReadByte();
                else if (ReadSize == 2)
                    ColorIn = br.ReadUInt16();
                else if (ReadSize == 3)
                {
                    ColorIn = ((uint)br.ReadByte());
                    ColorIn |= ((uint)br.ReadByte()) << 8;
                    ColorIn |= ((uint)br.ReadByte()) << 16;
                }
                else // 4 bytes
                    ColorIn = br.ReadUInt32();

                uint ColorOut = ToArgb32(ColorIn, Format);

                if (i == 0)
                    palette[i] = ColorOut & 0x00FFFFFF;
                else
                    palette[i] = ColorOut;
            }

            ColorFormat = Format;
            FileOffset = offset;
            FileName = FileId;
            Entries = NumEntries;

            return true;
        }

        public static uint ToArgb32(uint ColorIn, PaletteColorFormat Format)
        {
            uint ColorOut;

            if (Format == PaletteColorFormat.BGR15)
            {
                ColorOut = (ColorIn & 0x1F) << 19; // Red
                ColorOut |= (ColorIn & 0x3E0) << 6; // Blue
                ColorOut |= (ColorIn & 0x7C00) >> 7; // Green
                ColorOut |= 0xFF000000; // Alpha
            }
            else if (Format == PaletteColorFormat.ABGR16)
            {
                ColorOut = (ColorIn & 0x1F) << 19; // Red
                ColorOut |= (ColorIn & 0x3E0) << 6; // Blue
                ColorOut |= (ColorIn & 0x7C00) >> 7; // Green
                ColorOut |= ((ColorIn & 0x8000) * 255) << 24; // Alpha
            }
            else
                throw new ArgumentException("Unsupported PaletteColorFormat");

            return ColorOut;
        }

        public static uint GetNativeInArgb32(uint Argb32In, PaletteColorFormat ColorFormat)
        {
            uint R, G, B, A;
            uint ColorOut;

            if(ColorFormat == PaletteColorFormat.BGR15)
            {
                R = (Argb32In & 0xFF0000) >> 19;
                G = (Argb32In & 0xFF00) >> 11;
                B = (Argb32In & 0xFF) >> 3;
                ColorOut = (R << 16) | (G << 8) | B;
            }
            else if(ColorFormat == PaletteColorFormat.ABGR16)
            {
                R = (Argb32In & 0xFF0000) >> 19;
                G = (Argb32In & 0xFF00) >> 11;
                B = (Argb32In & 0xFF) >> 3;
                if ((Argb32In & 0xFF000000) == 0xFF000000)
                    A = 0x80;
                else
                    A = 0;

                ColorOut = (A << 24) | (R << 16) | (G << 8) | B;
            }
            else
                throw new ArgumentException("Unsupported PaletteColorFormat");

            return ColorOut;
        }

        public uint this[int i]
        {
            get
            {
                if (palette == null)
                    throw new ArgumentNullException();

                return palette[i];
            }
        }

        public Color GetColor(int idx)
        {
            if (palette == null)
                throw new ArgumentNullException();

            return Color.FromArgb((int)palette[idx]);
        }

        public int GetIndexByColor(Color col, bool ExactColorOnly)
        {
            uint c = ColorToUint(col);

            return GetIndexByColor(c, ExactColorOnly);
        }

        public int GetIndexByColor(uint col, bool ExactColorOnly)
        {
            for (int i = 0; i < Entries; i++)
            {
                if (palette[i] == col)
                    return i;
            }

            if (ExactColorOnly) // Do not pick closest color in palette
                return -1;

            // Color matching involves converting colors to hue-saturation-luminance and comparing
            var c1 = new ColorMine.ColorSpaces.Rgb { R = GetR(col), G = GetG(col), B = GetB(col) };
            var h1 = c1.To<ColorMine.ColorSpaces.Hsl>();

            double MinDistance = double.MaxValue;
            int MinIndex = -1;
            Cie94Comparison comparator = new Cie94Comparison(Cie94Comparison.Application.GraphicArts);

            for(int i = 0; i < Entries; i++)
            {
                var c2 = new ColorMine.ColorSpaces.Rgb { R = GetR(palette[i]), G = GetG(palette[i]), B = GetB(palette[i]) };
                var h2 = c2.To<ColorMine.ColorSpaces.Hsl>();

                double Distance = c1.Compare(c2, comparator);

                if(Distance < MinDistance)
                {
                    MinDistance = Distance;
                    MinIndex = i;
                }
            }

            return MinIndex;
        }

        private uint ColorToUint(Color col)
        {
            uint c = ((uint)col.A) << 24 | ((uint)col.R) << 16 | ((uint)col.G) << 8 | ((uint)col.B);
            return c;
        }

        private uint GetA(uint col)
        {
            return (col & 0xFF000000) >> 24;
        }

        private uint GetR(uint col)
        {
            return (col & 0xFF0000) >> 16;
        }

        private uint GetG(uint col)
        {
            return (col & 0xFF00) >> 8;
        }

        private uint GetB(uint col)
        {
            return col & 0xFF;
        }

        /*public uint GetNativeColor(int idx)
        {

        }

        public int GetNativeRed(int idx)
        {

        }

        public int GetNativeGreen(int idx)
        {

        }

        public int GetNativeBlue(int idx)
        {

        }

        public int GetNativeAlpha(int idx)
        {

        }*/

        public static PaletteColorFormat StringToFormat(string PaletteFormat)
        {
            switch(PaletteFormat)
            {
                case "RGB24":
                    return PaletteColorFormat.RGB24;
                case "ARGB32":
                    return PaletteColorFormat.ARGB32;
                case "BGR15":
                    return PaletteColorFormat.BGR15;
                case "ABGR16":
                    return PaletteColorFormat.ABGR16;
                case "NES":
                    return PaletteColorFormat.NES;
                default:
                    throw new ArgumentException("PaletteColorFormat " + PaletteFormat + " is not supported");
            }
        }

        public static List<string> GetPaletteColorFormatsNameList()
        {
            return Enum.GetNames(typeof(PaletteColorFormat)).Cast<string>().ToList<string>();
        }
    }
}
