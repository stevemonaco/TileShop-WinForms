using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace TileShop
{
    /// <summary>
    /// Provides XML support for reading/writing Game Descriptor Files and loading the content into the ProjectExplorerControl
    /// </summary>
    public class GameDescriptor
    {
        private ProjectTreeForm ptf = null;

        /// <summary>
        /// Base directory for all file locations on disk
        /// </summary>
        private string baseDirectory = null;

        /// <summary>
        /// Loads data files, palettes, and arrangers from XML
        /// Sets up the nodes in the project tree
        /// The project should be previously cleared before calling
        /// </summary>
        /// <param name="pec">ProjectExplorerControl from which to load data into</param>
        /// <param name="XmlFileName"></param>
        /// <returns></returns>
        public bool LoadProject(ProjectTreeForm projectTreeForm, string XmlFileName)
        {
            ptf = projectTreeForm;

            if (ptf == null)
                return false;

            XElement doc = XElement.Load(XmlFileName);
            XElement projectNode = doc.Element("project");

            baseDirectory = Path.GetDirectoryName(XmlFileName);

            Directory.SetCurrentDirectory(baseDirectory);

            /*var settings = xe.Descendants("settings")
                .Select(e => new
                {
                    numberformat = e.Descendants("filelocationnumberformat").First().Value
                });*/

            // Iterate over each project child node
            foreach (XElement node in projectNode.Elements())
            {
                if (node.Name == "folder")
                    AddFolderNode(node);
                else if (node.Name == "datafile")
                    AddDataFileNode(node);
                else if (node.Name == "palette")
                    AddPaletteNode(node);
                else if (node.Name == "arranger")
                    AddArrangerNode(node);
            }

            return true;
        }

        /// <summary>
        /// Recursive method that adds a folder node and all child nodes to the ProjectExplorerControl
        /// </summary>
        /// <param name="folderNode"></param>
        /// <returns></returns>
        private bool AddFolderNode(XElement folderNode)
        {
            ptf.AddFolderNode(GetNodeKey(folderNode));

            foreach (XElement node in folderNode.Elements())
            {
                if (node.Name == "folder")
                    AddFolderNode(node);
                else if (node.Name == "datafile")
                    AddDataFileNode(node);
                else if (node.Name == "palette")
                    AddPaletteNode(node);
                else if (node.Name == "arranger")
                    AddArrangerNode(node);
            }

            return true;
        }

        private bool AddDataFileNode(XElement fileNode)
        {
            string name = fileNode.Attribute("name").Value;
            string location = fileNode.Attribute("location").Value;

            DataFile df = new DataFile(name);
            df.Open(location);

            ptf.AddDataFile(df, GetNodePath(fileNode));
            return true;
        }

        private bool AddPaletteNode(XElement paletteNode)
        {
            string name = paletteNode.Attribute("name").Value;
            long fileoffset = long.Parse(paletteNode.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber);
            string datafile = paletteNode.Attribute("datafile").Value;
            int entries = int.Parse(paletteNode.Attribute("entries").Value);
            string formatname = paletteNode.Attribute("format").Value;
            bool zeroindextransparent = bool.Parse(paletteNode.Attribute("zeroindextransparent").Value);

            FileBitAddress address;
            if (paletteNode.Attribute("bitoffset") == null)
                address = new FileBitAddress(fileoffset, 0);
            else
                address = new FileBitAddress(fileoffset, int.Parse(paletteNode.Attribute("bitoffset").Value));

            Palette pal = new Palette(name);
            PaletteColorFormat format = Palette.StringToColorFormat(formatname);

            pal.LoadPalette(datafile, address, format, zeroindextransparent, entries);
            ptf.AddPalette(pal, GetNodePath(paletteNode));

            return true;
        }

        private bool AddArrangerNode(XElement arrangerNode)
        {
            string name = arrangerNode.Attribute("name").Value;
            int elementsx = int.Parse(arrangerNode.Attribute("elementsx").Value); // Width of arranger in elements
            int elementsy = int.Parse(arrangerNode.Attribute("elementsy").Value); // Height of arranger in elements
            int width = int.Parse(arrangerNode.Attribute("width").Value); // Width of element in pixels
            int height = int.Parse(arrangerNode.Attribute("height").Value); // Height of element in pixels
            string defaultformat = arrangerNode.Attribute("defaultformat").Value;
            string defaultdatafile = arrangerNode.Attribute("defaultdatafile").Value;
            string defaultpalette = arrangerNode.Attribute("defaultpalette").Value;
            string layoutName = arrangerNode.Attribute("layout").Value;
            IEnumerable<XElement> elementList = arrangerNode.Descendants("element");

            Arranger arr;
            ArrangerLayout layout;

            if (layoutName == "tiled")
                layout = ArrangerLayout.TiledArranger;
            else if (layoutName == "linear")
                layout = ArrangerLayout.LinearArranger;
            else
                throw new XmlException("Incorrect arranger layout type ('" + layoutName + "') for " + name);

            arr = Arranger.NewScatteredArranger(layout, elementsx, elementsy, width, height);
            arr.Name = name;

            var xmlElements = elementList.Select(e => new
            {
                fileoffset = long.Parse(e.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber),
                bitoffset = e.Attribute("bitoffset"),
                posx = int.Parse(e.Attribute("posx").Value),
                posy = int.Parse(e.Attribute("posy").Value),
                format = e.Attribute("format"),
                palette = e.Attribute("palette"),
                file = e.Attribute("file")
            });

            foreach (var xmlElement in xmlElements)
            {
                ArrangerElement el = arr.GetElement(xmlElement.posx, xmlElement.posy);

                el.DataFileKey = xmlElement.file?.Value ?? defaultdatafile;
                el.PaletteKey = xmlElement.palette?.Value ?? defaultpalette;
                el.FormatName = xmlElement.format?.Value ?? defaultformat;

                if (xmlElement.bitoffset != null)
                    el.FileAddress = new FileBitAddress(xmlElement.fileoffset, int.Parse(xmlElement.bitoffset.Value));
                else
                    el.FileAddress = new FileBitAddress(xmlElement.fileoffset, 0);

                el.Height = height;
                el.Width = width;
                el.X1 = xmlElement.posx * el.Width;
                el.Y1 = xmlElement.posy * el.Height;
                el.X2 = el.X1 + el.Width - 1;
                el.Y2 = el.Y1 + el.Height - 1;

                el.AllocateBuffers();

                arr.SetElement(el, xmlElement.posx, xmlElement.posy);
            }

            ptf.AddArranger(arr, GetNodePath(arrangerNode));
            return true;
        }

        /// <summary>
        /// Gets the project path for the specified node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetNodePath(XElement node)
        {
            string path = "";

            XElement currentNode = node;
            while(currentNode.Parent != null && currentNode.Parent.Name == "folder")
            {
                currentNode = currentNode.Parent;
                path = Path.Combine(currentNode.Attribute("name").Value, path);
            }

            return path;
        }

        /// <summary>
        /// Gets the project path key for the specified node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string GetNodeKey(XElement node)
        {
            return Path.Combine(GetNodePath(node), node.Attribute("name").Value);
        }

        /// <summary>
        /// Iterates over tree nodes and saves project settings to XML
        /// </summary>
        /// <param name="pec">ProjectExplorerControl from which to save the project tree data</param>
        /// <param name="XmlFileName"></param>
        /// <returns></returns>
        //public bool SaveProject(ProjectExplorerControl pec, string XmlFileName)
        //{
        //    var root = new XElement("gdf");

        //    // Save settings
        //    var settings = new XElement("settings");
        //    //XElement numberformat = new XElement("filelocationnumberformat");
        //    //numberformat.SetValue("hexadecimal");
        //    //settings.Add(numberformat);

        //    root.Add(settings);

        //    // Save each data file
        //    var datafiles = new XElement("datafiles");
        //    var palettes = new XElement("palettes");
        //    var arrangers = new XElement("arrangers");

        //    foreach (TreeNode tn in pec.ProjectTreeView.GetAllNodes())
        //    {
        //        if (tn is FileNode fn)
        //        {
        //            var xmlfile = new XElement("file");
        //            xmlfile.SetAttributeValue("location", fn.Text);
        //            xmlfile.SetAttributeValue("folder", fn.GetNodePath());
        //            datafiles.Add(xmlfile);
        //        }
        //        else if (tn is PaletteNode pn)
        //        {
        //            Palette pal = FileManager.Instance.GetPersistentPalette(pn.Text);
        //            var xmlpal = new XElement("palette");
        //            xmlpal.SetAttributeValue("name", pal.Name);
        //            xmlpal.SetAttributeValue("folder", pn.GetNodePath());
        //            xmlpal.SetAttributeValue("fileoffset", String.Format("{0:X}", pal.FileAddress.FileOffset));
        //            xmlpal.SetAttributeValue("bitoffset", String.Format("{0:X}", pal.FileAddress.BitOffset));
        //            xmlpal.SetAttributeValue("datafile", pal.FileName);
        //            xmlpal.SetAttributeValue("format", Palette.ColorFormatToString(pal.ColorFormat));
        //            xmlpal.SetAttributeValue("entries", pal.Entries);
        //            xmlpal.SetAttributeValue("zeroindextransparent", pal.ZeroIndexTransparent);
        //            palettes.Add(xmlpal);
        //        }
        //        else if (tn is ArrangerNode an)
        //        {
        //            Arranger arr = FileManager.Instance.GetPersistentArranger(an.Text);
        //            var xmlarr = new XElement("arranger");
        //            xmlarr.SetAttributeValue("name", arr.Name);
        //            xmlarr.SetAttributeValue("folder", an.GetNodePath());
        //            xmlarr.SetAttributeValue("elementsx", arr.ArrangerElementSize.Width);
        //            xmlarr.SetAttributeValue("elementsy", arr.ArrangerElementSize.Height);
        //            xmlarr.SetAttributeValue("width", arr.ElementPixelSize.Width);
        //            xmlarr.SetAttributeValue("height", arr.ElementPixelSize.Height);

        //            if (arr.Layout == ArrangerLayout.TiledArranger)
        //                xmlarr.SetAttributeValue("layout", "tiled");
        //            else if (arr.Layout == ArrangerLayout.LinearArranger)
        //                xmlarr.SetAttributeValue("layout", "linear");

        //            string DefaultPalette = FindMostFrequentValue(arr, "PaletteName");
        //            string DefaultFile = FindMostFrequentValue(arr, "FileName");
        //            string DefaultFormat = FindMostFrequentValue(arr, "FormatName");

        //            xmlarr.SetAttributeValue("defaultformat", DefaultFormat);
        //            xmlarr.SetAttributeValue("defaultfile", DefaultFile);
        //            xmlarr.SetAttributeValue("defaultpalette", DefaultPalette);

        //            for (int y = 0; y < arr.ArrangerElementSize.Height; y++)
        //            {
        //                for (int x = 0; x < arr.ArrangerElementSize.Width; x++)
        //                {
        //                    var graphic = new XElement("graphic");
        //                    ArrangerElement el = arr.GetElement(x, y);

        //                    graphic.SetAttributeValue("fileoffset", String.Format("{0:X}", el.FileAddress.FileOffset));
        //                    graphic.SetAttributeValue("bitoffset", String.Format("{0:X}", el.FileAddress.BitOffset));
        //                    graphic.SetAttributeValue("posx", x);
        //                    graphic.SetAttributeValue("posy", y);
        //                    if (el.FormatName != DefaultFormat)
        //                        graphic.SetAttributeValue("format", el.FormatName);
        //                    if (el.FileName != DefaultFile)
        //                        graphic.SetAttributeValue("file", el.FileName);
        //                    if (el.PaletteName != DefaultPalette)
        //                        graphic.SetAttributeValue("palette", el.PaletteName);

        //                    xmlarr.Add(graphic);
        //                }
        //            }

        //            arrangers.Add(xmlarr);
        //        }
        //    }

        //    root.Add(datafiles);
        //    root.Add(palettes);
        //    root.Add(arrangers);

        //    root.Save(XmlFileName);

        //    return true;
        //}

        /// <summary>
        /// Find most frequent of an attribute within an arranger's elements
        /// </summary>
        /// <param name="arr">Arranger to search</param>
        /// <param name="attributeName">Name of the attribute to find most frequent value of</param>
        /// <returns></returns>
        private string FindMostFrequentValue(Arranger arr, string attributeName)
        {
            Dictionary<string, int> freq = new Dictionary<string, int>();
            Type T = typeof(ArrangerElement);
            PropertyInfo P = T.GetProperty(attributeName);

            foreach (ArrangerElement el in arr.ElementGrid)
            {
                string s = (string)P.GetValue(el);

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
    }
}
