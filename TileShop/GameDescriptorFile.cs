using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace TileShop
{
    public class GameDescriptorFile
    {
        //GameDescriptorSettings settings;
        private GameDescriptorFile()
        {
        }

        // Loads data files, palettes, and arrangers from XML
        public static bool LoadFromXml(string XmlFileName)
        {
            XElement xe = XElement.Load(XmlFileName);

            string path = Path.GetDirectoryName(XmlFileName);

            Directory.SetCurrentDirectory(path);

            var settings = xe.Descendants("settings")
                .Select(e => new
                {
                    numberformat = e.Descendants("filelocationnumberformat").First().Value
                });

            var datafiles = xe.Descendants("file")
                .Select(e => new
                {
                    location = e.Attribute("location").Value,
                });

            foreach (var datafile in datafiles)
                FileManager.Instance.LoadFile(datafile.location);

            var palettes = xe.Descendants("palette")
                .Select(e => new
                {
                    name = e.Attribute("name").Value,
                    fileoffset = e.Attribute("fileoffset").Value,
                    datafile = e.Attribute("datafile").Value,
                    entries = e.Attribute("entries").Value,
                    format = e.Attribute("format").Value
                });

            foreach (var palette in palettes)
            {
                Palette pal = new Palette(palette.name);
                PaletteColorFormat format;

                switch (palette.format)
                {
                    case "RGB24":
                        format = PaletteColorFormat.RGB24;
                        break;
                    case "ARGB32":
                        format = PaletteColorFormat.ARGB32;
                        break;
                    case "BGR15":
                        format = PaletteColorFormat.BGR15;
                        break;
                    case "ABGR15":
                        format = PaletteColorFormat.ABGR16;
                        break;
                    case "NES":
                        format = PaletteColorFormat.NES;
                        break;
                    default:
                        throw new NotImplementedException(palette.format + " is not supported");                   
                }

                pal.LoadPalette(palette.datafile, long.Parse(palette.fileoffset), format, int.Parse(palette.entries));
                FileManager.Instance.AddPalette(pal.Name, pal);
            }

            var arrangers = xe.Descendants("arranger")
                .Select(e => new
                {
                    name = e.Attribute("name").Value,
                    elementsx = int.Parse(e.Attribute("elementsx").Value),
                    elementsy = int.Parse(e.Attribute("elementsy").Value),
                    height = int.Parse(e.Attribute("height").Value),
                    width = int.Parse(e.Attribute("width").Value),
                    defaultformat = e.Attribute("defaultformat").Value,
                    defaultfile = e.Attribute("defaultfile").Value,
                    defaultpalette = e.Attribute("defaultpalette").Value,
                    graphiclist = e.Descendants("graphic")
                });

            foreach (var arranger in arrangers)
            {
                Arranger arr = Arranger.NewScatteredArranger(arranger.elementsx, arranger.elementsy, arranger.width, arranger.height);
                arr.Name = arranger.name;

                var graphics = arranger.graphiclist.Select(e => new
                {
                    fileoffset = int.Parse(e.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber),
                    posx = int.Parse(e.Attribute("posx").Value),
                    posy = int.Parse(e.Attribute("posy").Value),
                    format = e.Attribute("format"),
                    palette = e.Attribute("palette"),
                    file = e.Attribute("file")
                });

                foreach(var graphic in graphics)
                {
                    ArrangerElement el = arr.GetElement(graphic.posx, graphic.posy);
                    el.FileName = arranger.defaultfile;
                    el.Palette = arranger.defaultpalette;
                    el.Format = arranger.defaultformat;

                    if (graphic.file != null)
                        el.FileName = graphic.file.Value;
                    if (graphic.palette != null)
                        el.Palette = graphic.palette.Value;
                    if (graphic.format != null)
                        el.Format = graphic.format.Value;

                    el.FileOffset = graphic.fileoffset;
                    el.Height = arranger.height;
                    el.Width = arranger.width;
                    el.X1 = graphic.posx * el.Width;
                    el.Y1 = graphic.posy * el.Height;
                    el.X2 = el.X1 + el.Width - 1;
                    el.Y2 = el.Y1 + el.Height - 1;

                    arr.SetElement(el, graphic.posx, graphic.posy);
                }

                FileManager.Instance.AddArranger(arr);
            }

            return true;
        }

        private bool WriteToXml(string XmlFileName)
        {
            return true;
        }
    }

    public class GameDescriptorSettings
    {
        public enum FileLocationNumberFormat { Decimal = 0, Hexadecimal = 1 }

        public FileLocationNumberFormat NumberFormat { set; get; }
    }
}
