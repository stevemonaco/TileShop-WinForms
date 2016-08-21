using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Xml.Linq;

namespace TileShop
{
    public enum ArrangerMode { SequentialArranger = 0, ScatteredArranger, MemoryArranger };
    public enum ArrangerMoveType { ByteDown = 0, ByteUp, RowDown, RowUp, ColRight, ColLeft, PageDown, PageUp, Home, End, Absolute };

    public class Arranger
    {
        // General Arranger variables
        public ArrangerElement[,] ElementList { get; private set; }
        public Size ArrangerElementSize { get; private set; } // Size of the entire arranger in elements
        public Size ArrangerPixelSize { get; private set; } // Size of the entire arranger in pixels
        public Size ElementPixelSize { get; private set; } // Size of each individual element in pixels
        public ArrangerMode Mode { get; private set; }
        public string Name { get; set; }

        // Sequential Arranger variables
        public bool IsSequential { get; private set; }
        public long FileSize { get; private set; }
        public long ArrangerByteSize { get; private set; } // Number of bytes required to be read from file sequentially

        // Scattered Arranger variables
        //public string DefaultCodec { get; private set; }
        //public string DefaultFile { get; private set; }
        //public string DefaultPalette { get; private set; }

        private Arranger()
        {
        }

        /*void NewBlankArranger(int ElementsX, int ElementsY, GraphicsFormat format)
        {
            if (format == null)
                throw new ArgumentNullException();

            ElementList = new ArrangerElement[ElementsX, ElementsY];

            int x = 0;
            int y = 0;

            for (int i = 0; i < ElementsY; i++)
            {
                x = 0;
                for (int j = 0; j < ElementsX; j++)
                {
                    ArrangerElement el = new ArrangerElement();
                    // Filename
                    el.FileOffset = 0;
                    el.X1 = x;
                    el.Y1 = y;
                    el.X2 = x + format.Width - 1;
                    el.Y2 = y + format.Height - 1;
                    el.Format = format.Name;
                    el.Palette = "Default";
                    ElementList[j, i] = el;

                    x += format.Width;
                }
                y += format.Height;
            }

            ArrangerElement LastElem = ElementList[ElementsX - 1, ElementsY - 1];
            ArrangerPixelSize = new Size(LastElem.X2, LastElem.Y2);
        }*/

        public static Arranger NewSequentialArranger(int ElementsX, int ElementsY, string Filename, GraphicsFormat format)
        {
            Arranger arr = new Arranger();
            arr.Mode = ArrangerMode.SequentialArranger;
            arr.IsSequential = true;
            arr.FileSize = FileManager.Instance.GetFileStream(Filename).Length;
            arr.Name = Filename;

            arr.ResizeSequentialArranger(ElementsX, ElementsY, Filename, format);

            return arr;
        }

        private long ResizeSequentialArranger(int ElementsX, int ElementsY, string Filename, GraphicsFormat format)
        {
            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            long offset = GetInitialSequentialFileOffset();

            ElementList = new ArrangerElement[ElementsX, ElementsY];
            ArrangerByteSize = ElementsX * ElementsY * format.Size();
            int x = 0;
            int y = 0;

            for (int i = 0; i < ElementsY; i++)
            {
                x = 0;
                for (int j = 0; j < ElementsX; j++)
                {
                    ArrangerElement el = new ArrangerElement();
                    el.FileOffset = offset;

                    if (format.ImageType == "tiled")
                        offset += format.Size();
                    else // Linear
                        offset += (format.Width + format.Stride) * format.ColorDepth / 4;

                    el.X1 = x;
                    el.Y1 = y;
                    el.X2 = x + format.Width - 1;
                    el.Y2 = y + format.Height - 1;
                    el.Width = format.Width;
                    el.Height = format.Height;
                    el.FileName = Filename;
                    el.Format = format.Name;
                    el.Palette = "Default";

                    ElementList[j, i] = el;

                    x += format.Width;
                }
                y += format.Height;
            }

            ArrangerElement LastElem = ElementList[ElementsX - 1, ElementsY - 1];
            ArrangerPixelSize = new Size(LastElem.X2 + 1, LastElem.Y2 + 1);
            ArrangerElementSize = new Size(ElementsX, ElementsY);
            ElementPixelSize = new Size(format.Width, format.Height);

            offset = GetInitialSequentialFileOffset();
            offset = Move(offset);

            return offset;
        }

        public long ResizeSequentialArranger(int ElementsX, int ElementsY)
        {
            if (Mode != ArrangerMode.SequentialArranger)
                throw new ArgumentException();

            return ResizeSequentialArranger(ElementsX, ElementsY, ElementList[0, 0].FileName, FileManager.Instance.GetGraphicsFormat(ElementList[0, 0].Format));
        }

        public static Arranger NewScatteredArranger(int ElementsX, int ElementsY, int Width, int Height)
        {
            Arranger arr = new Arranger();
            arr.Mode = ArrangerMode.ScatteredArranger;
            arr.IsSequential = false;

            arr.ElementList = new ArrangerElement[ElementsX, ElementsY];

            int x = 0;
            int y = 0;

            for (int i = 0; i < ElementsY; i++)
            {
                x = 0;
                for (int j = 0; j < ElementsX; j++)
                {
                    ArrangerElement el = new ArrangerElement();
                    el.FileName = null;
                    el.FileOffset = 0;
                    el.X1 = x;
                    el.Y1 = y;
                    el.X2 = x + Width - 1;
                    el.Y2 = y + Height - 1;
                    el.Width = Width;
                    el.Height = Height;
                    el.Format = null;
                    el.Palette = "Default";
                    arr.ElementList[j, i] = el;

                    x += Width;
                }
                y += Height;
            }

            ArrangerElement LastElem = arr.ElementList[ElementsX - 1, ElementsY - 1];
            arr.ArrangerPixelSize = new Size(LastElem.X2 + 1, LastElem.Y2 + 1);
            arr.ArrangerElementSize = new Size(ElementsX, ElementsY);
            arr.ElementPixelSize = new Size(Width, Height);

            return arr;
        }

        public void SetElement(ArrangerElement element, int posx, int posy)
        {
            if (ElementList == null)
                throw new ArgumentNullException();

            if (posx > ArrangerElementSize.Width || posy > ArrangerElementSize.Height)
                throw new ArgumentOutOfRangeException();

            ElementList[posx, posy] = element;
        }

        public ArrangerElement GetElement(int posx, int posy)
        {
            if (ElementList == null)
                throw new ArgumentNullException();

            return ElementList[posx, posy];
        }

        public Rectangle GetSelectionPixelRect(Rectangle r)
        {
            int x1 = r.Left;
            int x2 = r.Right;
            int y1 = r.Top;
            int y2 = r.Bottom;

            // Extend rectangle to include the entirety of partially selected tiles
            foreach(ArrangerElement el in ElementList)
            {
                if (x1 > el.X1 && x1 <= el.X2)
                    x1 = el.X1;
                if (y1 > el.Y1 && y1 <= el.Y2)
                    y1 = el.Y1;
                if (x2 < el.X2 && x2 >= el.X1)
                    x2 = el.X2;
                if (y2 < el.Y2 && y2 >= el.Y1)
                    y2 = el.Y2;
            }

            x2++; // Fix edges
            y2++;

            // Clamp selection rectangle to max bounds of the arranger
            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
            if (x2 >= ArrangerPixelSize.Width)
                x2 = ArrangerPixelSize.Width;
            if (y2 >= ArrangerPixelSize.Height)
                y2 = ArrangerPixelSize.Height;

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        // Returns new fileoffset
        public long Move(ArrangerMoveType MoveType)
        {
            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            long offset = ElementList[0, 0].FileOffset;
            long delta;

            switch (MoveType)
            {
                case ArrangerMoveType.ByteDown:
                    if (offset + 1 + ArrangerByteSize <= FileSize)
                        offset++;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.ByteUp:
                    if (offset >= 1)
                        offset--;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.RowDown:
                    delta = ArrangerElementSize.Width * FileManager.Instance.GetGraphicsFormat(ElementList[0, 0].Format).Size();
                    if (offset + delta + ArrangerByteSize <= FileSize)
                        offset += delta;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.RowUp:
                    delta = ArrangerElementSize.Width * FileManager.Instance.GetGraphicsFormat(ElementList[0, 0].Format).Size();
                    if (offset - delta >= 0)
                        offset -= delta;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.ColRight:
                    delta = FileManager.Instance.GetGraphicsFormat(ElementList[0, 0].Format).Size();
                    if (offset + delta + ArrangerByteSize <= FileSize)
                        offset += delta;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.ColLeft:
                    delta = FileManager.Instance.GetGraphicsFormat(ElementList[0, 0].Format).Size();
                    if (offset - delta >= 0)
                        offset -= delta;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.PageDown:
                    delta = ArrangerElementSize.Width * FileManager.Instance.GetGraphicsFormat(ElementList[0, 0].Format).Size() * ArrangerElementSize.Height / 2;
                    if (offset + delta + ArrangerByteSize <= FileSize)
                        offset += delta;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.PageUp:
                    delta = ArrangerElementSize.Width * FileManager.Instance.GetGraphicsFormat(ElementList[0, 0].Format).Size() * ArrangerElementSize.Height / 2;
                    if (offset - delta >= 0)
                        offset -= delta;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.Home:
                    offset = 0;
                    break;
                case ArrangerMoveType.End:
                    offset = FileSize - ArrangerByteSize;
                    break;
            }

            SetNewSequentialFileOffset(offset);

            return offset;
        }

        long Move(long AbsoluteOffset)
        {
            long newoffset;
            if (AbsoluteOffset + ArrangerByteSize > FileSize)
                newoffset = FileSize - ArrangerByteSize;
            else
                newoffset = AbsoluteOffset;

            SetNewSequentialFileOffset(newoffset);
            return newoffset;
        }

        void SetNewSequentialFileOffset(long FileOffset)
        {
            if (ElementList == null)
                throw new NullReferenceException();

            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            for (int i = 0; i < ArrangerElementSize.Height; i++)
            {
                for (int j = 0; j < ArrangerElementSize.Width; j++)
                {
                    ElementList[j, i].FileOffset = FileOffset;
                    FileOffset += FileManager.Instance.GetGraphicsFormat(ElementList[j, i].Format).Size();
                }
            }
        }

        public long GetInitialSequentialFileOffset()
        {
            if (ElementList != null)
                return ElementList[0, 0].FileOffset;
            else
                return 0;
        }

        public string GetSequentialGraphicsFormat()
        {
            if (ElementList == null)
                throw new NullReferenceException();

            return ElementList[0, 0].Format;
        }

        public bool SetGraphicsFormat(string Format)
        {
            if (ElementList == null)
                throw new NullReferenceException();

            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            long offset = ElementList[0, 0].FileOffset;
            GraphicsFormat fmt = FileManager.Instance.GetGraphicsFormat(Format);
            int elemsize = fmt.Width * fmt.Height * fmt.ColorDepth / 8;
            ArrangerByteSize = ArrangerElementSize.Width * ArrangerElementSize.Height * elemsize;

            if (FileSize < offset + ArrangerByteSize)
                offset = FileSize - ArrangerByteSize;

            foreach (ArrangerElement el in ElementList)
            {
                el.FileOffset = offset;
                offset += elemsize;
                el.Format = Format;
            }

            return true;
        }
    }

    [Serializable]
    public class ArrangerElement
    {
        public string FileName = "";
        public long FileOffset = 0;
        public string Format = "";
        public int Width = 0;
        public int Height = 0;
        public string Palette = "";
        public int X1 = 0; // Locations are unzoomed
        public int Y1 = 0;
        public int X2 = 0;
        public int Y2 = 0;

        public ArrangerElement Clone()
        {
            ArrangerElement el = new ArrangerElement();

            el.FileName = FileName;
            el.FileOffset = FileOffset;
            el.Format = Format;
            el.Width = Width;
            el.Height = Height;
            el.Palette = Palette;
            el.X1 = X1;
            el.Y1 = Y1;
            el.X2 = X2;
            el.Y2 = Y2;

            return el;
        }
    }
}
