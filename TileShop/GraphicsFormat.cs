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

        public ImageProperty()
        {

        }

        public ImageProperty(int colorDepth, bool rowInterlace)
        {
            ColorDepth = colorDepth;
            RowInterlace = rowInterlace;
        }
    }

    public class GraphicsFormat
    {
        public string Name;
        public bool FixedSize;
        public int Width;
        public int Height;
        public string ImageType; // "Tiled" or "Linear"
        public int ColorDepth;
        public string ColorType; // "Indexed" or "Direct"
        public int Stride;

        public int Size() { return (Width + Stride) * Height * ColorDepth / 8; }

        public List<ImageProperty> ImagePropertyList = new List<ImageProperty>();

        // Processing Operations
        public bool HFlip;
        public bool VFlip;
        public bool Remap;

        // Pixel remap operations

        // Preallocated TileData buffers
        public List<byte[]> TileData = new List<byte[]>();
        public byte[] MergedData;

        public void AllocateBuffers()
        {
            TileData.Clear();
            for (int i = 0; i < ColorDepth; i++)
            {
                byte[] data = new byte[Width * Height];
                TileData.Add(data);
            }

            MergedData = new byte[Width * Height];
        }

        public bool Load(string Filename)
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
                         .Select(e => new { colordepth = e.Descendants("colordepth").First().Value, rowinterlace = e.Descendants("rowinterlace").First().Value });

            foreach(var image in images)
            {
                ImagePropertyList.Add(new ImageProperty(int.Parse(image.colordepth), bool.Parse(image.rowinterlace)));
            }

            AllocateBuffers();

            return true;
        }
    }
}
