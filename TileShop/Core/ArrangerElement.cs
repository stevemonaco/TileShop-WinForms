using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop
{
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

            GraphicsFormat format = ResourceManager.Instance.GetGraphicsFormat(FormatName);
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
            GraphicsFormat format = ResourceManager.Instance.GetGraphicsFormat(FormatName);
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
