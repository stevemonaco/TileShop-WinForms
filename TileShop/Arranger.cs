using System;
using System.Drawing;

namespace TileShop
{
    /// <summary>
    /// Arranger mode for the arranger.
    /// Sequential arrangers are for sequential file access
    /// Scattered arrangers are for accessing many files and file offsets in a single arranger
    /// Memory arrangers are used as a scratchpad
    /// </summary>
    public enum ArrangerMode { SequentialArranger = 0, ScatteredArranger, MemoryArranger };

    /// <summary>
    /// Move operations for sequential arrangers
    /// </summary>
    public enum ArrangerMoveType { ByteDown = 0, ByteUp, RowDown, RowUp, ColRight, ColLeft, PageDown, PageUp, Home, End, Absolute };

    public class Arranger
    {
        // General Arranger variables

        /// <summary>
        /// Gets individual elements from the arranger
        /// </summary>
        public ArrangerElement[,] ElementList { get; private set; }

        /// <summary>
        /// Gets the Size of the entire arranger in elements
        /// </summary>
        public Size ArrangerElementSize { get; private set; }

        /// <summary>
        /// Gets the Size of the entire arranger in unzoomed pixels
        /// </summary>
        public Size ArrangerPixelSize { get; private set; }

        /// <summary>
        /// Gets the Size of an individual element in unzoomed pixels
        /// </summary>
        public Size ElementPixelSize { get; private set; }

        /// <summary>
        /// Gets the Mode of the arranger
        /// </summary>
        public ArrangerMode Mode { get; private set; }

        /// <summary>
        /// Gets or sets the name of the arranger
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets whether the arranger mode is sequential or not
        /// </summary>
        public bool IsSequential { get; private set; }

        /// <summary>
        /// Gets the filesize of the file associated with a sequential arranger
        /// </summary>
        public long FileSize
        {
            get
            {
                if (Mode != ArrangerMode.SequentialArranger)
                    throw new InvalidOperationException("Cannot retrieve the FileSize for an arranger that is not a SequentialArranger");
                else
                    return fileSize;
            }
            private set { fileSize = value; }
        }
        private long fileSize;

        /// <summary>
        /// Number of bytes required to be read from file sequentially
        /// </summary>
        public long ArrangerByteSize
        {
            get
            {
                if (Mode != ArrangerMode.SequentialArranger)
                    throw new InvalidOperationException("Cannot retrieve the ArrangerByteSize for an arranger that is not a SequentialArranger");
                else
                    return arrangerByteSize;
            }
            private set { arrangerByteSize = value; }
        }
        private long arrangerByteSize;

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

        /// <summary>
        /// Creates a new instance of an arranger in sequential reading mode
        /// </summary>
        /// <param name="ElementsX">Number of elements in the horizontal direction</param>
        /// <param name="ElementsY">Number of elements in the vertical direction</param>
        /// <param name="Filename">Filename of a file already loaded into the FileManager</param>
        /// <param name="format">Graphics format of the element</param>
        /// <returns></returns>
        public static Arranger NewSequentialArranger(int ElementsX, int ElementsY, string Filename, GraphicsFormat format)
        {
            Arranger arr = new Arranger()
            {
                Mode = ArrangerMode.SequentialArranger,
                IsSequential = true,
                FileSize = FileManager.Instance.GetFileStream(Filename).Length,
                Name = Filename
            };
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
                    ArrangerElement el = new ArrangerElement()
                    {
                        FileOffset = offset,
                        X1 = x,
                        Y1 = y,
                        X2 = x + format.Width - 1,
                        Y2 = y + format.Height - 1,
                        Width = format.Width,
                        Height = format.Height,
                        FileName = Filename,
                        Format = format.Name,
                        PaletteName = "Default"
                    };
                    ElementList[j, i] = el;

                    if (format.ImageType == "tiled")
                        offset += format.Size();
                    else // Linear
                        offset += (format.Width + format.Stride) * format.ColorDepth / 4;

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
                    ArrangerElement el = new ArrangerElement()
                    {
                        FileName = "",
                        FileOffset = 0,
                        X1 = x,
                        Y1 = y,
                        X2 = x + Width - 1,
                        Y2 = y + Height - 1,
                        Width = Width,
                        Height = Height,
                        Format = "",
                        PaletteName = "Default"
                    };
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

        // Creates a new arranger based on a selection
        public Arranger CreateSubArranger(string ArrangerName, int ArrangerPosX, int ArrangerPosY, int ElementsX, int ElementsY)
        {
            Arranger arr = new Arranger()
            {
                Mode = ArrangerMode.ScatteredArranger, // Default to scattered arranger due to selections not being the full width of the parent arranger
                Name = ArrangerName,
                ElementList = new ArrangerElement[ElementsX, ElementsY],
                ArrangerElementSize = new Size(ElementsX, ElementsY),
                ElementPixelSize = ElementPixelSize,
                ArrangerPixelSize = new Size(ElementPixelSize.Width * ElementsX, ElementPixelSize.Height * ElementsY)
            };

            if (Mode == ArrangerMode.SequentialArranger)
            {
                arr.IsSequential = IsSequential;
                arr.FileSize = FileSize;
            }

            for (int srcy = ArrangerPosY, desty = 0; srcy < ArrangerPosY + ElementsY; srcy++, desty++)
            {
                for (int srcx = ArrangerPosX, destx = 0; srcx < ArrangerPosX + ElementsX; srcx++, destx++)
                {
                    ArrangerElement el = GetElement(srcx, srcy).Clone();
                    el.X1 = destx * arr.ElementPixelSize.Width;
                    el.X2 = el.X1 + arr.ElementPixelSize.Width - 1;
                    el.Y1 = desty * arr.ElementPixelSize.Height;
                    el.Y2 = el.Y1 + arr.ElementPixelSize.Height - 1;
                    arr.SetElement(el, destx, desty);
                }
            }

            return arr;
        }

        /// <summary>
        /// Sets element to a position in the element array using a shallow copy
        /// </summary>
        /// <param name="element"></param>
        /// <param name="posx"></param>
        /// <param name="posy"></param>
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

        /// <summary>
        /// Creates a deep clone of the Arranger
        /// </summary>
        /// <returns></returns>
        public Arranger Clone()
        {
            Arranger arr = new Arranger()
            {
                ElementList = new ArrangerElement[ArrangerElementSize.Width, ArrangerElementSize.Height],
                ArrangerElementSize = ArrangerElementSize,
                ElementPixelSize = ElementPixelSize,
                ArrangerPixelSize = ArrangerPixelSize,
                Mode = Mode,
                Name = Name,
                IsSequential = IsSequential
            };

            for (int y = 0; y < ArrangerElementSize.Height; y++)
                for (int x = 0; x < ArrangerElementSize.Width; x++)
                    arr.SetElement(ElementList[x, y], x, y);

            if (IsSequential)
            {
                arr.FileSize = fileSize;
                arr.ArrangerByteSize = arrangerByteSize;
            }

            return arr;
        }
    }

    [Serializable]
    public class ArrangerElement
    {
        public string FileName { get; set; }
        public long FileOffset { get; set; }
        public string Format { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string PaletteName { get; set; }
        public int X1 { get; set; } // Locations in unzoomed coordinates
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }

        public ArrangerElement()
        {
            FileName = "";
            FileOffset = 0;
            Format = "";
            Width = 0;
            Height = 0;
            PaletteName = "Default";
            X1 = 0;
            X2 = 0;
            Y1 = 0;
            Y2 = 0;
        }

        /// <summary>
        /// Creates a deep clone
        /// </summary>
        /// <returns></returns>
        public ArrangerElement Clone()
        {
            ArrangerElement el = new ArrangerElement()
            {
                FileName = FileName,
                FileOffset = FileOffset,
                Format = Format,
                Width = Width,
                Height = Height,
                PaletteName = PaletteName,
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2
            };
            return el;
        }
    }
}
