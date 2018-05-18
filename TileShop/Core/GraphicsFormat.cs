using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop.Core
{
    /// <summary>
    /// Defines image assembly features
    /// </summary>
    public class ImageProperty
    {
        public int ColorDepth { get; set; }
        public bool RowInterlace { get; set; }

        /// <summary>
        /// Original placement pattern of pixels as specified by the codec
        /// </summary>
        public int[] RowPixelPattern { get; private set; }

        /// <summary>
        /// Placement pattern of pixels extended to the width of the element
        /// </summary>
        public int[] RowExtendedPixelPattern { get; set; }

        public ImageProperty()
        {

        }

        public ImageProperty(int colorDepth, bool rowInterlace, int[] rowPixelPattern)
        {
            ColorDepth = colorDepth;
            RowInterlace = rowInterlace;
            RowPixelPattern = rowPixelPattern;
            RowExtendedPixelPattern = RowPixelPattern;
        }

        /// <summary>
        /// Extends the RowPixelPattern to the required image width
        /// </summary>
        /// <param name="width">The width of the ArrangerElement to be decoded</param>
        public void ExtendRowPattern(int width)
        {
            if (RowExtendedPixelPattern.Length >= width) // Previously sized to be sufficiently large
                return;

            RowExtendedPixelPattern = new int[width];

            int patternIndex = 0; // Index into RowPixelPattern
            int extendedIndex = 0;   // Index into RowExtendedPixelPattern

            while(extendedIndex < width)
            {
                RowExtendedPixelPattern[patternIndex] = extendedIndex + RowPixelPattern[patternIndex];
                extendedIndex++;
                patternIndex = (patternIndex + 1) & RowPixelPattern.Length;
            }
        }
    }

    /// <summary>
    /// Specifies how the graphical viewer will treat the graphic
    /// Tiled graphics will render a grid of multiple images
    /// Linear graphics will render a single image
    /// </summary>
    public enum ImageLayout { Tiled = 0, Linear }

    /// <summary>
    /// Specifies how the pixels' colors are determined for the graphic
    /// Indexed graphics have their full color determined by a palette
    /// Direct graphics have their full color determined by the pixel image data alone
    /// </summary>
    public enum PixelColorType { Indexed = 0, Direct }

    /// <summary>
    /// GraphicsFormat describes properties relating to decoding/encoding a general graphics format
    /// </summary>
    public class GraphicsFormat
    {
        /// <summary>
        /// The name of the codec
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Returns true if the codec requires fixed size elements or false if the codec operates on variable size elements
        /// </summary>
        public bool FixedSize { get; private set; }

        /// <summary>
        /// Specifies if the graphic is rendered and manipulated as a tiled grid or not
        /// </summary>
        public ImageLayout Layout { get; private set; }

        /// <summary>
        /// The color depth of the format in bits per pixel
        /// </summary>
        public int ColorDepth { get; set; }

        /// <summary>
        /// ColorType defines how pixel data is translated into color data
        /// </summary>
        public PixelColorType ColorType { get; set; }

        /// <summary>
        /// Specifies how individual bits of each color are merged according to priority
        ///   Ex: [3, 2, 0, 1] implies the first bit read will merge into bit 3,
        ///   second bit read into bit 2, third bit read into bit 0, fourth bit read into bit 1
        /// </summary>
        public int[] MergePriority { get; set; }

        /// <summary>
        /// Current width of the elements to encode/decode
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Current height of elements to encode/decode
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Default width of an element as specified by the XML file
        /// </summary>
        public int DefaultWidth { get; private set; }

        /// <summary>
        /// Default height of an element as specified by the XML file
        /// </summary>
        public int DefaultHeight { get; private set; }

        /// <summary>
        /// Number of bits to skip after each row
        /// </summary>
        public int RowStride { get; private set; }

        /// <summary>
        /// Number of bits to skip after each element
        /// </summary>
        public int ElementStride { get; private set; }

        /// <summary>
        /// Storage size of an element in bits
        /// </summary>
        /// <returns></returns>
        public int StorageSize(int width, int height) { return (width + RowStride) * height * ColorDepth + ElementStride; }

        public List<ImageProperty> ImagePropertyList { get; private set; }

        // Processing Operations
        public bool HFlip { get; set; }
        public bool VFlip { get; set; }
        public bool Remap { get; set; }

        // Pixel remap operations

        public GraphicsFormat()
        {
            ImagePropertyList = new List<ImageProperty>();
        }

        /// <summary>
        /// Loads a codec from an external XML file
        /// </summary>
        /// <param name="Filename">Filename of the codec</param>
        /// <returns>True on success, false on failure</returns>
        public bool LoadFromXml(string Filename)
        {
            ImagePropertyList.Clear();

            XElement xe = XElement.Load(Filename);

            Name = xe.Attribute("name").Value;

            var codecs = xe.Descendants("codec")
                .Select(e => new
                {
                    colortype = e.Descendants("colortype").First().Value,
                    colordepth = e.Descendants("colordepth").First().Value,
                    layout = e.Descendants("layout").First().Value,
                    height = e.Descendants("defaultheight").First().Value,
                    width = e.Descendants("defaultwidth").First().Value,
                    fixedsize = e.Descendants("fixedsize").First().Value,
                    mergepriority = e.Descendants("mergepriority").First().Value
                }).First();

            if (codecs.colortype == "indexed")
                ColorType = PixelColorType.Indexed;
            else if (codecs.colortype == "direct")
                ColorType = PixelColorType.Direct;
            else
                throw new XmlException(String.Format("Unsupported colortype '{0}'", codecs.colortype));

            ColorDepth = int.Parse(codecs.colordepth);

            if (codecs.layout == "tiled")
                Layout = ImageLayout.Tiled;
            else if (codecs.layout == "linear")
                Layout = ImageLayout.Linear;
            else
                throw new XmlException(String.Format("Unsupported layout '{0}'", codecs.layout));

            DefaultWidth = int.Parse(codecs.width);
            DefaultHeight = int.Parse(codecs.height);
            Width = DefaultWidth;
            Height = DefaultHeight;
            FixedSize = bool.Parse(codecs.fixedsize);

            string mergestring = codecs.mergepriority;
            mergestring.Replace(" ", "");
            string[] mergeInts = mergestring.Split(',');

            if (mergeInts.Length != ColorDepth)
                throw new Exception("The number of entries in mergepriority does not match the colordepth");

            MergePriority = new int[ColorDepth];

            for (int i = 0; i < mergeInts.Length; i++)
                MergePriority[i] = int.Parse(mergeInts[i]);

            var images = xe.Descendants("image")
                         .Select(e => new
                         {
                             colordepth = e.Descendants("colordepth").First().Value,
                             rowinterlace = e.Descendants("rowinterlace").First().Value,
                             rowpixelpattern = e.Descendants("rowpixelpattern")
                         });

            foreach(var image in images)
            {
                int[] rowPixelPattern;

                if (image.rowpixelpattern.Count() > 0) // Parse rowpixelpattern
                {
                    string order = image.rowpixelpattern.First().Value;
                    order.Replace(" ", "");
                    string[] orderInts = order.Split(',');

                    rowPixelPattern = new int[orderInts.Length];

                    for (int i = 0; i < orderInts.Length; i++)
                        rowPixelPattern[i] = int.Parse(orderInts[i]);
                }
                else // Create a default rowpixelpattern in numeric order for the entire row
                {
                    rowPixelPattern = new int[Width];

                    for (int i = 0; i < Width; i++)
                        rowPixelPattern[i] = i;
                }

                ImageProperty ip = new ImageProperty(int.Parse(image.colordepth), bool.Parse(image.rowinterlace), rowPixelPattern);
                ip.ExtendRowPattern(Width);
                ImagePropertyList.Add(ip);
            }

            return true;
        }

        public void Resize(int width, int height)
        {
            Height = height;
            Width = width;

            for (int i = 0; i < ImagePropertyList.Count; i++)
                ImagePropertyList[i].ExtendRowPattern(Width);
        }

        public void Rename(string name)
        {
            name = Name;
        }
    }
}
