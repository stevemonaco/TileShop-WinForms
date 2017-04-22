using System;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop
{
    public class ImageProperty
    {
        public int ColorDepth;
        public bool RowInterlace;
        public int[] RowPixelOrder;

        public ImageProperty()
        {

        }

        public ImageProperty(int colorDepth, bool rowInterlace, int[] rowPixelOrder)
        {
            ColorDepth = colorDepth;
            RowInterlace = rowInterlace;
            RowPixelOrder = rowPixelOrder;
        }
    }

    public class GraphicsFormat
    {
        public string Name;
        public bool FixedSize;
        public int Width;
        public int Height;
        public string ImageType; // "tiled" or "linear"
        public int ColorDepth;
        public string ColorType; // "indexed" or "direct"
        public int Stride;

        /// <summary>
        /// Size of an element in bits
        /// </summary>
        /// <returns></returns>
        public int Size() { return (Width + Stride) * Height * ColorDepth; }

        public List<ImageProperty> ImagePropertyList = new List<ImageProperty>();

        // Processing Operations
        public bool HFlip;
        public bool VFlip;
        public bool Remap;

        // Pixel remap operations

        // Load a codec via XML format
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
                    imagetype = e.Descendants("imagetype").First().Value,
                    height = e.Descendants("height").First().Value,
                    width = e.Descendants("width").First().Value,
                    fixedsize = e.Descendants("fixedsize").First().Value
                });

            ColorType = codecs.First().colortype;
            ColorDepth = int.Parse(codecs.First().colordepth);
            ImageType = codecs.First().imagetype;
            Width = int.Parse(codecs.First().width);
            Height = int.Parse(codecs.First().height);
            FixedSize = bool.Parse(codecs.First().fixedsize);

            var images = xe.Descendants("image")
                         .Select(e => new
                         {
                             colordepth = e.Descendants("colordepth").First().Value,
                             rowinterlace = e.Descendants("rowinterlace").First().Value,
                             rowpixelorder = e.Descendants("rowpixelorder")
                         });

            foreach(var image in images)
            {
                int[] rowPixelOrder = new int[Width];

                if (FixedSize && image.rowpixelorder.Count() > 0) // Parse rowpixelorder
                {
                    string order = image.rowpixelorder.First().Value;
                    order.Replace(" ", "");
                    string[] orderInts = order.Split(',');

                    if (orderInts.Length > Width)
                        throw new Exception("rowpixel order contains more entries than the width of the row");
                    for (int i = 0; i < orderInts.Length; i++)
                        rowPixelOrder[i] = int.Parse(orderInts[i]);
                }
                else
                {
                    for (int i = 0; i < Width; i++)
                        rowPixelOrder[i] = i;
                }

                ImagePropertyList.Add(new ImageProperty(int.Parse(image.colordepth), bool.Parse(image.rowinterlace), rowPixelOrder));
            }

            return true;
        }
    }
}
