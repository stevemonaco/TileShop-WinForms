using System;
using System.Collections.Generic;
using System.Drawing;

namespace TileShop
{
    /// <summary>
    /// Mode for the arranger.
    /// Sequential arrangers are for sequential file access
    /// Scattered arrangers are for accessing many files and file offsets in a single arranger
    /// Memory arrangers are used as a scratchpad (currently unimplemented)
    /// </summary>
    public enum ArrangerMode { SequentialArranger = 0, ScatteredArranger, MemoryArranger };

    /// <summary>
    /// Layout of graphics for the arranger
    /// Each layout directs the UI to perform differently
    /// TiledArranger will snap selection rectangles to tile boundaries
    /// LinearArranger will snap selection rectangles to pixel boundaries
    /// </summary>
    public enum ArrangerLayout { TiledArranger = 0, LinearArranger };

    /// <summary>
    /// Move operations for sequential arrangers
    /// </summary>
    public enum ArrangerMoveType { ByteDown = 0, ByteUp, RowDown, RowUp, ColRight, ColLeft, PageDown, PageUp, Home, End, Absolute };

    /// <summary>
    /// Arranger for graphical screen elements
    /// </summary>
    public class Arranger
    {
        // General Arranger variables

        /// <summary>
        /// Gets individual Elements that compose the Arranger
        /// </summary>
        public ArrangerElement[,] ElementGrid { get; set; }

        /// <summary>
        /// Gets the size of the entire Arranger in Elements
        /// </summary>
        public Size ArrangerElementSize { get; private set; }

        /// <summary>
        /// Gets the Size of the entire Arranger in unzoomed pixels
        /// </summary>
        public Size ArrangerPixelSize { get; private set; }

        /// <summary>
        /// Gets the size of an individual Element in unzoomed pixels
        /// Only valid for Sequential Arranger
        /// </summary>
        public Size ElementPixelSize { get; private set; }

        /// <summary>
        /// Gets the Mode of the Arranger
        /// </summary>
        public ArrangerMode Mode { get; private set; }

        /// <summary>
        /// Gets the ArrangerLayout of the Arranger
        /// </summary>
        public ArrangerLayout Layout { get; private set; }

        /// <summary>
        /// Gets or sets the name of the Arranger
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets whether the Arranger mode is Sequential or not
        /// </summary>
        public bool IsSequential { get; private set; }

        /// <summary>
        /// Gets the filesize of the file associated with a Sequential Arranger
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
        /// Number of bits required to be read from file sequentially
        /// </summary>
        public long ArrangerBitSize
        {
            get
            {
                if (Mode != ArrangerMode.SequentialArranger)
                    throw new InvalidOperationException("Cannot retrieve the ArrangerBitSize for an arranger that is not a SequentialArranger");
                else
                    return arrangerBitSize;
            }
            private set { arrangerBitSize = value; }
        }
        private long arrangerBitSize;

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
        /// <param name="arrangerWidth">Width of Arranger in Elements</param>
        /// <param name="arrangerHeight">Height of Arranger in Elements</param>
        /// <param name="dataFileKey">DataFile key for FileManager</param>
        /// <param name="format">GraphicsFormat for encoding/decoding Elements</param>
        /// <returns></returns>
        public static Arranger NewSequentialArranger(int arrangerWidth, int arrangerHeight, string dataFileKey, GraphicsFormat format)
        {
            DataFile df = FileManager.Instance.GetDataFile(dataFileKey);

            Arranger arr = new Arranger()
            {
                Mode = ArrangerMode.SequentialArranger,
                IsSequential = true,
                FileSize = df.Stream.Length,
                Name = df.Name
            };
            arr.ResizeSequentialArranger(arrangerWidth, arrangerHeight, dataFileKey, format);

            return arr;
        }

        /// <summary>
        /// Resizes a Sequential Arranger with a new number of elements
        /// </summary>
        /// <param name="arrangerWidth">Width of Arranger in Elements</param>
        /// <param name="arrangerHeight">Height of Arranger in Elements</param>
        /// <param name="dataFileKey">DataFile key in FileManager</param>
        /// <param name="format">GraphicsFormat for encoding/decoding Elements</param>
        /// <returns></returns>
        private FileBitAddress ResizeSequentialArranger(int arrangerWidth, int arrangerHeight, string dataFileKey, GraphicsFormat format)
        {
            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            FileBitAddress address;

            if (ElementGrid == null) // New Arranger being resized
                address = 0;
            else address = GetInitialSequentialFileAddress();

            ElementGrid = new ArrangerElement[arrangerWidth, arrangerHeight];

            int x = 0;
            int y = 0;

            for (int i = 0; i < arrangerHeight; i++)
            {
                x = 0;
                for (int j = 0; j < arrangerWidth; j++)
                {
                    ArrangerElement el = new ArrangerElement()
                    {
                        FileAddress = address,
                        X1 = x,
                        Y1 = y,
                        X2 = x + ElementPixelSize.Width - 1,
                        Y2 = y + ElementPixelSize.Height - 1,
                        Width = ElementPixelSize.Width,
                        Height = ElementPixelSize.Height,
                        DataFileKey = dataFileKey,
                        FormatName = format.Name,
                    };
                    if (el.ElementData.Count == 0 || el.MergedData == null)
                        el.AllocateBuffers();

                    ElementGrid[j, i] = el;

                    if (format.Layout == ImageLayout.Tiled)
                        address += el.StorageSize;
                    else // Linear
                        address += (ElementPixelSize.Width + format.RowStride) * format.ColorDepth / 4; // TODO: Fix sequential arranger offsets to be bit-wise

                    x += ElementPixelSize.Width;
                }
                y += ElementPixelSize.Height;
            }

            ArrangerElement lastElem = ElementGrid[arrangerWidth - 1, arrangerHeight - 1];
            ArrangerPixelSize = new Size(lastElem.X2 + 1, lastElem.Y2 + 1);
            ArrangerElementSize = new Size(arrangerWidth, arrangerHeight);
            ElementPixelSize = new Size(ElementPixelSize.Width, ElementPixelSize.Height);

            ArrangerBitSize = arrangerWidth * arrangerHeight * lastElem.StorageSize;

            address = GetInitialSequentialFileAddress();
            address = Move(address);

            return address;
        }

        /// <summary>
        /// Resizes a Sequential Arranger to the specified number of Elements and repopulates Element data
        /// </summary>
        /// <param name="arrangerWidth">Width of Arranger in Elements</param>
        /// <param name="arrangerHeight">Height of Arranger in Elements</param>
        /// <returns></returns>
        public FileBitAddress ResizeSequentialArranger(int arrangerWidth, int arrangerHeight)
        {
            if (Mode != ArrangerMode.SequentialArranger)
                throw new ArgumentException();

            return ResizeSequentialArranger(arrangerWidth, arrangerHeight, ElementGrid[0, 0].DataFileKey, FileManager.Instance.GetGraphicsFormat(ElementGrid[0, 0].FormatName));
        }

        /// <summary>
        /// Resizes a scattered arranger to the specified number of elements and default initializes any new elements
        /// </summary>
        /// <param name="arrangerWidth">Width of Arranger in Elements</param>
        /// <param name="arrangerHeight">Height of Arranger in Elements</param>
        public void ResizeScatteredArranger(int arrangerWidth, int arrangerHeight)
        {
            if (Mode != ArrangerMode.ScatteredArranger)
                throw new ArgumentException();

            ArrangerElement[,] newList = new ArrangerElement[arrangerWidth, arrangerHeight];

            int xCopy = Math.Min(arrangerWidth, ArrangerElementSize.Width);
            int yCopy = Math.Min(arrangerHeight, ArrangerElementSize.Height);
            int Width = ElementPixelSize.Width;
            int Height = ElementPixelSize.Height;

            for (int y = 0; y < arrangerHeight; y++)
            {
                for (int x = 0; x < arrangerWidth; x++)
                {
                    if ((y < ArrangerElementSize.Height) && (x < ArrangerElementSize.Width)) // Copy from old arranger
                        newList[x, y] = ElementGrid[x, y].Clone();
                    else // Create new blank element
                    {
                        ArrangerElement el = new ArrangerElement()
                        {
                            X1 = x * Width,
                            Y1 = y * Height,
                            X2 = x * Width + Width - 1,
                            Y2 = y * Height + Height - 1,
                            Width = Width,
                            Height = Height,
                        };

                        newList[x, y] = el;
                    }
                }
            }

            ElementGrid = newList;
            ArrangerElementSize = new Size(arrangerWidth, arrangerHeight);
            ArrangerPixelSize = new Size(arrangerWidth * Width, arrangerHeight * Height);
        }

        /// <summary>
        /// Creates a new scattered arranger with default initialized elements
        /// </summary>
        /// <param name="layout">Layout type of the arranger</param>
        /// <param name="arrangerWidth">Width of Arranger in Elements</param>
        /// <param name="arrangerHeight">Height of Arranger in Elements</param>
        /// <param name="elementWidth">Width of each element</param>
        /// <param name="elementHeight">Height of each element</param>
        /// <returns></returns>
        public static Arranger NewScatteredArranger(ArrangerLayout layout, int arrangerWidth, int arrangerHeight, int elementWidth, int elementHeight)
        {
            Arranger arr = new Arranger();
            arr.Mode = ArrangerMode.ScatteredArranger;
            arr.IsSequential = false;
            arr.Layout = layout;

            arr.ElementGrid = new ArrangerElement[arrangerWidth, arrangerHeight];

            int x = 0;
            int y = 0;

            for (int i = 0; i < arrangerHeight; i++)
            {
                x = 0;
                for (int j = 0; j < arrangerWidth; j++)
                {
                    ArrangerElement el = new ArrangerElement()
                    {
                        X1 = x,
                        Y1 = y,
                        X2 = x + elementWidth - 1,
                        Y2 = y + elementHeight - 1,
                        Width = elementWidth,
                        Height = elementHeight,
                    };
                    arr.ElementGrid[j, i] = el;

                    x += elementWidth;
                }
                y += elementHeight;
            }

            ArrangerElement LastElem = arr.ElementGrid[arrangerWidth - 1, arrangerHeight - 1];
            arr.ArrangerPixelSize = new Size(LastElem.X2 + 1, LastElem.Y2 + 1);
            arr.ArrangerElementSize = new Size(arrangerWidth, arrangerHeight);
            arr.ElementPixelSize = new Size(elementWidth, elementHeight);

            return arr;
        }

        /// <summary>
        /// Creates a new Scattered Arranger from an existing Arranger
        /// </summary>
        /// <param name="subArrangerName">Arranger name for the newly created Arranger</param>
        /// <param name="arrangerPosX">0-based top-most Element coordinate of parent Arranger selection to copy</param>
        /// <param name="arrangerPosY">0-based left-most Element coordinate of parent Arranger selection to copy</param>
        /// <param name="copyWidth">Width of selection to copy in Elements</param>
        /// <param name="copyHeight">Height of selection to copy in Elements</param>
        /// <returns></returns>
        public Arranger CreateSubArranger(string subArrangerName, int arrangerPosX, int arrangerPosY, int copyWidth, int copyHeight)
        {
            if ((arrangerPosX < 0) || (arrangerPosX + copyWidth > ArrangerElementSize.Width))
                throw new ArgumentOutOfRangeException();

            if ((arrangerPosY < 0) || (arrangerPosY + copyHeight > ArrangerElementSize.Height))
                throw new ArgumentOutOfRangeException();

            Arranger subArranger = new Arranger()
            {
                Mode = ArrangerMode.ScatteredArranger, // Default to scattered arranger
                IsSequential = false,
                Name = subArrangerName,
                ElementGrid = new ArrangerElement[copyWidth, copyHeight],
                ArrangerElementSize = new Size(copyWidth, copyHeight),
                ElementPixelSize = ElementPixelSize,
                ArrangerPixelSize = new Size(ElementPixelSize.Width * copyWidth, ElementPixelSize.Height * copyHeight)
            };

            for (int srcy = arrangerPosY, desty = 0; srcy < arrangerPosY + copyHeight; srcy++, desty++)
            {
                for (int srcx = arrangerPosX, destx = 0; srcx < arrangerPosX + copyWidth; srcx++, destx++)
                {
                    ArrangerElement el = GetElement(srcx, srcy).Clone();
                    el.X1 = destx * subArranger.ElementPixelSize.Width;
                    el.X2 = el.X1 + subArranger.ElementPixelSize.Width - 1;
                    el.Y1 = desty * subArranger.ElementPixelSize.Height;
                    el.Y2 = el.Y1 + subArranger.ElementPixelSize.Height - 1;
                    subArranger.SetElement(el, destx, desty);
                }
            }

            return subArranger;
        }

        /// <summary>
        /// Sets Element to a position in the Arranger ElementGrid using a shallow copy
        /// </summary>
        /// <param name="element">Element to be placed into the ElementGrid</param>
        /// <param name="arrangerPosX">0-based x-coordinate of Element</param>
        /// <param name="arrangerPosY">0-based y-coordinate of Element</param>
        public void SetElement(ArrangerElement element, int arrangerPosX, int arrangerPosY)
        {
            if (ElementGrid == null)
                throw new ArgumentNullException();

            if (arrangerPosX > ArrangerElementSize.Width || arrangerPosY > ArrangerElementSize.Height)
                throw new ArgumentOutOfRangeException();

            ElementGrid[arrangerPosX, arrangerPosY] = element;
        }

        /// <summary>
        /// Gets an Element from a position in the Arranger ElementGrid
        /// </summary>
        /// <param name="arrangerPosX">0-based x-coordinate of Element</param>
        /// <param name="arrangerPosY">0-based y-coordinate of Element</param>
        /// <returns></returns>
        public ArrangerElement GetElement(int arrangerPosX, int arrangerPosY)
        {
            if (ElementGrid == null)
                throw new ArgumentNullException();

            return ElementGrid[arrangerPosX, arrangerPosY];
        }

        /// <summary>
        /// Expands the given selection rectangle to fully contain all pixels of selected Elements
        /// </summary>
        /// <param name="partialRectangle">Selection rectangle in unzoomed pixels containing partially selected Elements</param>
        /// <returns></returns>
        public Rectangle GetExpandedSelectionPixelRect(Rectangle partialRectangle)
        {
            int x1 = partialRectangle.Left;
            int x2 = partialRectangle.Right;
            int y1 = partialRectangle.Top;
            int y2 = partialRectangle.Bottom;

            // Expands rectangle to include the entirety of partially selected tiles
            foreach(ArrangerElement el in ElementGrid)
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

        /// <summary>
        /// Moves a Sequential Arranger's file position and updates each Element
        /// Will not move outside of the bounds of the underlying file
        /// </summary>
        /// <param name="moveType">Type of move requested</param>
        /// <returns>Updated address of first element</returns>
        public FileBitAddress Move(ArrangerMoveType moveType)
        {
            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            if (ElementGrid == null)
                throw new NullReferenceException();

            FileBitAddress address = ElementGrid[0, 0].FileAddress;
            FileBitAddress delta;

            switch (moveType) // Calculate the new address based on the movement command. Negative and post-EOF addresses are handled after the switch
            {
                case ArrangerMoveType.ByteDown:
                    address += 8;
                    break;
                case ArrangerMoveType.ByteUp:
                    address -= 8;
                    break;
                case ArrangerMoveType.RowDown:
                    delta = ArrangerElementSize.Width * ElementGrid[0, 0].StorageSize;
                    address += delta;
                    break;
                case ArrangerMoveType.RowUp:
                    delta = ArrangerElementSize.Width * ElementGrid[0, 0].StorageSize;
                    address -= delta;
                    break;
                case ArrangerMoveType.ColRight:
                    delta = ElementGrid[0, 0].StorageSize;
                    address += delta;
                    break;
                case ArrangerMoveType.ColLeft:
                    delta = ElementGrid[0, 0].StorageSize;
                    address -= delta;
                    break;
                case ArrangerMoveType.PageDown:
                    delta = ArrangerElementSize.Width * ElementGrid[0, 0].StorageSize * ArrangerElementSize.Height / 2;
                    address += delta;
                    break;
                case ArrangerMoveType.PageUp:
                    delta = ArrangerElementSize.Width * ElementGrid[0, 0].StorageSize * ArrangerElementSize.Height / 2;
                    address -= delta;
                    break;
                case ArrangerMoveType.Home:
                    address = 0;
                    break;
                case ArrangerMoveType.End:
                    address = new FileBitAddress(FileSize * 8 - ArrangerBitSize);
                    break;
            }

            if (address + ArrangerBitSize > FileSize * 8) // Calculated address is past EOF (first)
                address = new FileBitAddress(FileSize * 8 - ArrangerBitSize);

            if (address < 0) // Calculated address is before start of file (second)
                address = 0;

            Move(address);

            return address;
        }

        /// <summary>
        /// Moves the sequential arranger to the specified address
        /// If the arranger will overflow the file, then seek only to the furthest offset
        /// </summary>
        /// <param name="absoluteAddress">Specified address to move the arranger to</param>
        /// <returns></returns>
        public FileBitAddress Move(FileBitAddress absoluteAddress)
        {
            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            if (ElementGrid == null)
                throw new NullReferenceException();

            FileBitAddress address;
            FileBitAddress testaddress = absoluteAddress + ArrangerBitSize; // Tests the bounds of the arranger vs the file size

            if (FileSize * 8 < ArrangerBitSize) // Arranger needs more bits than the entire file
                address = new FileBitAddress(0, 0);
            else if (testaddress.Bits() > FileSize * 8)
                address = new FileBitAddress(FileSize * 8 - ArrangerBitSize);
            else
                address = absoluteAddress;

            int ElementStorageSize = ElementGrid[0, 0].StorageSize;

            for (int i = 0; i < ArrangerElementSize.Height; i++)
            {
                for (int j = 0; j < ArrangerElementSize.Width; j++)
                {
                    ElementGrid[j, i].FileAddress = address;
                    address += ElementStorageSize;
                }
            }

            return ElementGrid[0, 0].FileAddress;
        }

        /// <summary>
        /// Gets the initial file address of a Sequential Arranger
        /// </summary>
        /// <returns></returns>
        public FileBitAddress GetInitialSequentialFileAddress()
        {
            if (ElementGrid == null)
                throw new NullReferenceException();

            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            return ElementGrid[0, 0].FileAddress;
        }

        /// <summary>
        /// Gets the GraphicsFormat name for a Sequential Arranger
        /// </summary>
        /// <returns></returns>
        public string GetSequentialGraphicsFormat()
        {
            if (ElementGrid == null)
                throw new NullReferenceException();

            return ElementGrid[0, 0].FormatName;
        }

        /// <summary>
        /// Sets the GraphicsFormat name and Element size for a Sequential Arranger
        /// </summary>
        /// <param name="Format">Name of the GraphicsFormat</param>
        /// <param name="ElementSize">Size of each Element in pixels</param>
        /// <returns></returns>
        public bool SetGraphicsFormat(string Format, Size ElementSize)
        {
            if (ElementGrid == null)
                throw new NullReferenceException();

            if (Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            FileBitAddress address = ElementGrid[0, 0].FileAddress;
            GraphicsFormat fmt = FileManager.Instance.GetGraphicsFormat(Format);

            ElementPixelSize = ElementSize;

            int elembitsize = fmt.StorageSize(ElementSize.Width, ElementSize.Height);
            ArrangerBitSize = ArrangerElementSize.Width * ArrangerElementSize.Height * elembitsize;

            if (FileSize * 8 < address + ArrangerBitSize)
                address = new FileBitAddress(FileSize * 8 - ArrangerBitSize);

            for (int i = 0; i < ArrangerElementSize.Height; i++)
            {
                for (int j = 0; j < ArrangerElementSize.Width; j++)
                {
                    ElementGrid[j, i].FileAddress = address;
                    ElementGrid[j, i].FormatName = Format;
                    ElementGrid[j, i].Width = ElementPixelSize.Width;
                    ElementGrid[j, i].Height = ElementPixelSize.Height;
                    ElementGrid[j, i].X1 = j * ElementPixelSize.Width;
                    ElementGrid[j, i].X2 = j * ElementPixelSize.Width + (ElementPixelSize.Width - 1);
                    ElementGrid[j, i].Y1 = i * ElementPixelSize.Height;
                    ElementGrid[j, i].Y2 = i * ElementPixelSize.Height + (ElementPixelSize.Height - 1);
                    ElementGrid[j, i].AllocateBuffers();
                    address += elembitsize;
                }
            }

            ArrangerElement LastElem = ElementGrid[ArrangerElementSize.Width - 1, ArrangerElementSize.Height - 1];
            ArrangerPixelSize = new Size(LastElem.X2 + 1, LastElem.Y2 + 1);

            return true;
        }

        /// <summary>
        /// Tests the Arranger Elements to see if any Elements are blank
        /// </summary>
        /// <returns></returns>
        public bool ContainsBlankElements()
        {
            foreach(ArrangerElement el in ElementGrid)
            {
                if (el.IsBlank())
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a deep clone of the Arranger
        /// </summary>
        /// <returns></returns>
        public Arranger Clone()
        {
            Arranger arr = new Arranger()
            {
                ElementGrid = new ArrangerElement[ArrangerElementSize.Width, ArrangerElementSize.Height],
                ArrangerElementSize = ArrangerElementSize,
                ElementPixelSize = ElementPixelSize,
                ArrangerPixelSize = ArrangerPixelSize,
                Mode = Mode,
                Name = Name,
                IsSequential = IsSequential
            };

            for (int y = 0; y < ArrangerElementSize.Height; y++)
                for (int x = 0; x < ArrangerElementSize.Width; x++)
                    arr.SetElement(ElementGrid[x, y], x, y);

            if (IsSequential)
            {
                arr.FileSize = fileSize;
                arr.ArrangerBitSize = arrangerBitSize;
            }

            return arr;
        }
    }

    /// <summary>
    /// Contains all necessary data to encode/decode a single element in the arranger
    /// </summary>
    public class ArrangerElement
    {
        /// <summary>
        /// DataFile key in FileManager
        /// </summary>
        public string DataFileKey { get; set; }

        /// <summary>
        /// FileAddress of Element
        /// </summary>
        public FileBitAddress FileAddress { get; set; }

        /// <summary>
        /// GraphicsFormat name for encoding/decoding Element
        /// </summary>
        public string FormatName { get; set; }

        /// <summary>
        /// Width of Element in pixels
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of Element in pixels
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Palette key in FileManager
        /// </summary>
        public string PaletteKey { get; set; }

        /// <summary>
        /// Left X-coordinate of the Element
        /// </summary>
        public int X1 { get; set; } // Locations in unzoomed coordinates

        /// <summary>
        /// Top Y-coordinate of the Element
        /// </summary>
        public int Y1 { get; set; }

        /// <summary>
        /// Right X-coordinate of the Element, inclusive
        /// </summary>
        public int X2 { get; set; }

        /// <summary>
        /// Bottom Y-coordinate of the Element, inclusive
        /// </summary>
        public int Y2 { get; set; }

        // Preallocated TileData buffers
        /// <summary>
        /// Preallocated buffer that separates and stores pixel color data
        /// </summary>
        public List<byte[]> ElementData { get; private set; }

        /// <summary>
        /// Preallocated buffer that stores merged pixel color data
        /// </summary>
        public byte[] MergedData { get; private set; }

        /// <summary>
        /// Number of bits needed to store the Element's foreign pixel data
        /// </summary>
        public int StorageSize { get; private set; }

        /// <summary>
        /// Used to allocate internal buffers to hold graphical data specific to the Element
        /// </summary>
        public void AllocateBuffers()
        {
            if (IsBlank())
                return;

            GraphicsFormat format = FileManager.Instance.GetGraphicsFormat(FormatName);
            ElementData.Clear();
            for (int i = 0; i < format.ColorDepth; i++)
            {
                byte[] data = new byte[Width * Height];
                ElementData.Add(data);
            }

            MergedData = new byte[Width * Height];
            StorageSize = (Width + format.RowStride) * Height * format.ColorDepth + format.ElementStride;
        }

        public ArrangerElement()
        {
            DataFileKey = "";
            FileAddress = new FileBitAddress(0, 0);
            FormatName = "";
            Width = 0;
            Height = 0;
            PaletteKey = "Default";
            X1 = 0;
            X2 = 0;
            Y1 = 0;
            Y2 = 0;

            ElementData = new List<byte[]>();
        }

        /// <summary>
        /// Creates a deep clone
        /// </summary>
        /// <returns></returns>
        public ArrangerElement Clone()
        {
            ArrangerElement el = new ArrangerElement()
            {
                DataFileKey = DataFileKey,
                FileAddress = FileAddress,
                FormatName = FormatName,
                Width = Width,
                Height = Height,
                PaletteKey = PaletteKey,
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2,
                StorageSize = StorageSize
            };

            if (IsBlank()) // Blank elements have no data buffers to copy
                return el;

            // Copy MergedData
            el.AllocateBuffers();
            for (int i = 0; i < MergedData.Length; i++)
                el.MergedData[i] = MergedData[i];

            // Copy TileData
            GraphicsFormat format = FileManager.Instance.GetGraphicsFormat(FormatName);
            for (int i = 0; i < ElementData.Count; i++)
                for (int j = 0; j < ElementData[i].Length; j++)
                    el.ElementData[i][j] = ElementData[i][j];

            return el;
        }

        /// <summary>
        /// Detects if the ArrangerElement is a blank element
        /// </summary>
        /// <returns>True if blank, false if not</returns>
        public bool IsBlank()
        {
            if (FormatName == "")
                return true;
            else
                return false;
        }
    }
}
