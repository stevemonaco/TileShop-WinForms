using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TileShop
{
    public partial class ProjectExplorerControl : DockContent
    {
        TileShopForm tsf = null;
        ExplorerNode filesNode = new ExplorerNode();
        ExplorerNode palettesNode = new ExplorerNode();
        ExplorerNode arrangersNode = new ExplorerNode();

        public ProjectExplorerControl(TileShopForm tileShopForm)
        {
            tsf = tileShopForm;
            InitializeComponent();

            filesNode.Text = "Files";
            palettesNode.Text = "Palettes";
            arrangersNode.Text = "Arrangers";

            projectTreeView.Nodes.Add(filesNode);
            projectTreeView.Nodes.Add(palettesNode);
            projectTreeView.Nodes.Add(arrangersNode);
        }

        // Full filename with path
        public bool AddFile(string Filename, bool Show)
        {
            FileNode fn = new FileNode();
            fn.Text = Filename;
            fn.Tag = Filename;

            if (!FileManager.Instance.LoadFile(Filename))
                return false;

            filesNode.Nodes.Add(fn);

            if (Show)
            {
                projectTreeView.SelectedNode = fn;
                return ShowSequentialArranger(Filename);
            }

            return true;
        }

        // Full filename with path
        public bool RemoveFile(string Filename)
        {
            foreach(FileNode fn in filesNode.Nodes)
            {
                if((string)fn.Tag == Filename)
                {
                    fn.Remove();
                    return true;
                }
            }

            return false;
        }

        public bool AddArranger(Arranger arr, bool Show)
        {
            ArrangerNode an = new ArrangerNode();
            an.Text = arr.Name;
            an.Tag = arr.Name;

            arrangersNode.Nodes.Add(an);

            FileManager.Instance.AddArranger(arr);

            if(Show)
            {
                if (arr.Mode == ArrangerMode.SequentialArranger)
                    throw new InvalidOperationException("AddArranger is not meant for SequentialArrangers");
                else if (arr.Mode == ArrangerMode.ScatteredArranger)
                    return ShowScatteredArranger(arr.Name);
                else
                    throw new NotSupportedException("AddArranger does not support arranger types other than ScatteredArranger");
            }

            return true;
        }

        public bool RemoveArranger(string ArrangerName)
        {
            foreach (ArrangerNode an in arrangersNode.Nodes)
            {
                if ((string)an.Tag == ArrangerName)
                {
                    an.Remove();
                    return true;
                }
            }

            return false;
        }

        public bool AddPalette(Palette pal)
        {
            PaletteNode pn = new PaletteNode();
            pn.Text = pal.Name;
            pn.Tag = pal.Name;

            palettesNode.Nodes.Add(pn);
            FileManager.Instance.AddPalette(pal.Name, pal);

            return true;
        }

        public bool RemovePalette(string PaletteName)
        {
            foreach (PaletteNode tn in palettesNode.Nodes)
            {
                if ((string)tn.Tag == PaletteName)
                {
                    tn.Remove();
                    return true;
                }
            }

            return false;
        }

        public bool ShowSequentialArranger(string Filename)
        {
            if (FileManager.Instance.LoadSequentialArrangerFromFilename(Filename))
            {
                GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(tsf, Filename);
                gv.WindowState = FormWindowState.Maximized;
                gv.SetZoom(6);
                gv.Show(tsf.dockPanel, DockState.Document);
                return true;
            }
            else
                return false;
        }

        public bool ShowScatteredArranger(string ArrangerName)
        {
            // Consider doing on-demand loading of scattered arrangers from XML in the future

            GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(tsf, ArrangerName);
            gv.WindowState = FormWindowState.Maximized;
            gv.SetZoom(6);
            gv.Show(tsf.dockPanel, DockState.Document);
            return true;
        }

        public bool ClearAll()
        {
            projectTreeView.Nodes.Clear();

            return true;
        }

        // Loads data files, palettes, and arrangers from XML
        public bool LoadFromXml(string XmlFileName)
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
                AddFile(datafile.location, false);

            var palettes = xe.Descendants("palette")
                .Select(e => new
                {
                    name = e.Attribute("name").Value,
                    fileoffset = long.Parse(e.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber),
                    datafile = e.Attribute("datafile").Value,
                    entries = int.Parse(e.Attribute("entries").Value),
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

                pal.LoadPalette(palette.datafile, palette.fileoffset, format, palette.entries);
                AddPalette(pal);
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
                    fileoffset = long.Parse(e.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber),
                    posx = int.Parse(e.Attribute("posx").Value),
                    posy = int.Parse(e.Attribute("posy").Value),
                    format = e.Attribute("format"),
                    palette = e.Attribute("palette"),
                    file = e.Attribute("file")
                });

                foreach (var graphic in graphics)
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

                AddArranger(arr, false);
            }

            return true;
        }

        public bool SaveToXml(string XmlFileName)
        {
            XElement root = new XElement("gdf");

            // Save settings
            XElement settings = new XElement("settings");
            XElement numberformat = new XElement("filelocationnumberformat");
            numberformat.SetValue("hexadecimal");
            settings.Add(numberformat);

            root.Add(settings);

            // Save each data file
            XElement datafiles = new XElement("datafiles");

            foreach(FileNode fn in filesNode.Nodes)
            {
                XElement el = new XElement("file");
                el.SetAttributeValue("location", fn.Text);
                datafiles.Add(el);
            }

            root.Add(datafiles);

            // Save Palettes
            XElement palettes = new XElement("palettes");

            foreach(PaletteNode pn in palettesNode.Nodes)
            {
                Palette pal = FileManager.Instance.GetPalette(pn.Text);
                XElement el = new XElement("palette");
                el.SetAttributeValue("name", pal.Name);
                el.SetAttributeValue("fileoffset", String.Format("{0:X}", pal.FileOffset));
                el.SetAttributeValue("datafile", pal.FileName);
                el.SetAttributeValue("format", pal.ColorFormat.ToString());
                el.SetAttributeValue("entries", pal.Entries);
                palettes.Add(el);
            }

            root.Add(palettes);

            // Save each arranger
            XElement arrangers = new XElement("arrangers");
            
            foreach(ArrangerNode an in arrangersNode.Nodes)
            {
                Arranger arr = FileManager.Instance.GetArranger(an.Text);
                XElement el = new XElement("arranger");
                el.SetAttributeValue("name", arr.Name);
                el.SetAttributeValue("elementsx", arr.ArrangerElementSize.Width);
                el.SetAttributeValue("elementsy", arr.ArrangerElementSize.Height);
                el.SetAttributeValue("width", arr.ElementPixelSize.Width);
                el.SetAttributeValue("height", arr.ElementPixelSize.Height);

                string DefaultPalette = FindMostFrequentValue(arr, "Palette");
                string DefaultFile = FindMostFrequentValue(arr, "FileName");
                string DefaultFormat = FindMostFrequentValue(arr, "Format");

                el.SetAttributeValue("defaultformat", DefaultFormat);
                el.SetAttributeValue("defaultfile", DefaultFile);
                el.SetAttributeValue("defaultpalette", DefaultPalette);

                for(int y = 0; y < arr.ArrangerElementSize.Height; y++)
                {
                    for(int x = 0; x < arr.ArrangerElementSize.Width; x++)
                    {
                        XElement graphic = new XElement("graphic");
                        ArrangerElement arrel = arr.GetElement(x, y);

                        graphic.SetAttributeValue("fileoffset", String.Format("{0:X}", arrel.FileOffset));
                        graphic.SetAttributeValue("posx", x);
                        graphic.SetAttributeValue("posy", y);
                        if (arrel.Format != DefaultFormat)
                            graphic.SetAttributeValue("format", arrel.Format);
                        if (arrel.FileName != DefaultFile)
                            graphic.SetAttributeValue("file", arrel.FileName);
                        if (arrel.Palette != DefaultPalette)
                            graphic.SetAttributeValue("palette", arrel.Palette);

                        el.Add(graphic);
                    }
                }

                arrangers.Add(el);
            }

            root.Add(arrangers);

            root.Save(XmlFileName);

            return true;
        }

        // Used to determine property defaults for XML
        private string FindMostFrequentValue(Arranger arr, string attributeName)
        {
            Dictionary<string, int> freq = new Dictionary<string, int>();
            Type T = typeof(ArrangerElement);
            PropertyInfo P  = T.GetProperty(attributeName);

            foreach (ArrangerElement el in arr.ElementList)
            {
                string s = (string) P.GetValue(el);

                if (s == "")
                    continue;

                if (freq.ContainsKey(s))
                    freq[s]++;
                else
                    freq.Add(s, 1);
            }

            var max = freq.FirstOrDefault(x => x.Value == freq.Values.Max()).Key;

            return max;
        }

        private void projectTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //tsf.openExistingArranger(e.Node.Text);
        }
    }

    public abstract class BaseNode : TreeNode
    {
        public abstract void BuildContextMenu(ContextMenu Menu);

    }

    public class ExplorerNode : BaseNode
    {
        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();
        }
    }

    public class FileNode : ExplorerNode
    {
        public override void BuildContextMenu(ContextMenu Menu)
        {
            // Open File Viewer

            // Remove File from Project
        }
    }

    public class PaletteNode : ExplorerNode
    {
        public override void BuildContextMenu(ContextMenu Menu)
        {
            // Open Palette Editor

            // Remove Palette from Project
        }
    }

    public class ArrangerNode : ExplorerNode
    {
        public override void BuildContextMenu(ContextMenu Menu)
        {
            // Open Arranger

            // Remove Arranger from Project

            // Export Arranger from Image
        }
    }

    public class GameDescriptorSettings
    {
        public enum FileLocationNumberFormat { Decimal = 0, Hexadecimal = 1 }

        public FileLocationNumberFormat NumberFormat { set; get; }
    }
}
