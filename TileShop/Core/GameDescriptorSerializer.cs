using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using TileShop.ExtensionMethods;

namespace TileShop.Core
{
    /// <summary>
    /// Provides XML support for reading/writing Game Descriptor Files and loading the content into the ProjectExplorerControl
    /// </summary>
    public class GameDescriptorSerializer
    {
        /// <summary>
        /// Base directory for all file locations on disk
        /// </summary>
        private string BaseDirectory = null;

        #region XML Deserialization methods        
        /// <summary>
        /// Deserializes a Stream of XML data
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="baseDirectory">The full path of the directory containing the stream.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">LoadProject was called with a null stream</exception>
        /// <exception cref="ArgumentException">LoadProject was called with a null or empty baseDirectory</exception>
        public Dictionary<string, ProjectResourceBase> DeserializeProject(Stream stream, string baseDirectory)
        {
            if (stream == null)
                throw new ArgumentNullException("LoadProject was called with a null stream");
            if (String.IsNullOrEmpty(baseDirectory))
                throw new ArgumentException("LoadProject was called with a null or empty baseDirectory");

            XElement doc = XElement.Load(stream);
            XElement projectNode = doc.Element("project");

            Directory.SetCurrentDirectory(baseDirectory);
            BaseDirectory = baseDirectory;

            /*var settings = xe.Descendants("settings")
                .Select(e => new
                {
                    numberformat = e.Descendants("filelocationnumberformat").First().Value
                });*/

            var resourceTree = new Dictionary<string, ProjectResourceBase>();

            foreach (XElement node in projectNode.Elements())
            {
                if (node.Name == "folder")
                {
                    var folder = new ResourceFolder();
                    folder.Deserialize(node);
                    resourceTree.Add(node.Attribute("name").Value, folder);
                }
                else if (node.Name == "datafile")
                {
                    var df = new DataFile(node.Attribute("name").Value);
                    df.Deserialize(node);
                    resourceTree.Add(df.Name, df);
                }
                else if (node.Name == "palette")
                {
                    var pal = new Palette(node.Attribute("name").Value);
                    pal.Deserialize(node);
                    resourceTree.Add(pal.Name, pal);
                }
                else if (node.Name == "arranger")
                {
                    var arr = new ScatteredArranger();
                    arr.Rename(node.Attribute("name").Value);
                    arr.Deserialize(node);
                    resourceTree.Add(arr.Name, arr);
                }
            }

            return resourceTree;
        }
        #endregion

        #region XML Serialization methods
        /// <summary>
        /// Iterates over tree nodes and saves project settings to XML
        /// </summary>
        /// <param name="XmlFileName"></param>
        /// <returns></returns>
        public bool SerializeProject(string XmlFileName)
        {
            if (String.IsNullOrEmpty(XmlFileName))
                throw new ArgumentException();

            var xmlRoot = new XElement("gdf");
            var projectRoot = new XElement("project");
            var settingsRoot = new XElement("settings");

            xmlRoot.Add(settingsRoot);

            // Sort so that we create 
            /*var sortedKeys = from string res in ResourceManager.Instance.ResourceKeys
                             orderby res.Count(x => x == Path.DirectorySeparatorChar)
                             orderby res
                             select res;*/

            // Create a map of referenceable XElement folder nodes

            var FolderMap = new Dictionary<string, XElement>();
            foreach (string key in ResourceManager.Instance.ResourceKeys)
            {
                if (key.Contains(".SequentialArranger") || key.Contains("Default"))
                    continue;

                int index = key.LastIndexOf(Path.DirectorySeparatorChar);
                string folderName;

                while(index != -1) // Has parent folders
                {
                    folderName = key.Substring(0, index);

                    if (FolderMap.ContainsKey(folderName))
                        break;

                    XElement xFolder = new XElement("folder");
                    xFolder.SetAttributeValue("name", folderName);
                    FolderMap.Add(folderName, xFolder);

                    index = folderName.LastIndexOf(Path.DirectorySeparatorChar);
                }
            }

            // Now add resource nodes to folder nodes
            foreach(string key in ResourceManager.Instance.ResourceKeys)
            {
                ProjectResourceBase res = ResourceManager.Instance.GetResource(key);
                XElement xe;

                if (key.Contains(".SequentialArranger") || key.Contains("Default"))
                    continue;

                if (res is DataFile df)
                    xe = SaveDataFile(df);
                else if (res is Arranger arr)
                    xe = SaveArranger(arr);
                else if (res is Palette pal)
                    xe = SavePalette(pal);
                else
                    throw new NotImplementedException($"Resource serialization not supported for {res.Name}");

                int index = key.LastIndexOf(Path.DirectorySeparatorChar);

                if (index == -1) // File in root directory
                    FolderMap.Add(key, xe);
                else
                {
                    string folderName = key.Substring(0, index);
                    XElement xFolder = FolderMap[folderName];
                    xFolder.Add(xe);
                }
            }

            var sortedKeys = from key in FolderMap.Keys
                             orderby key.Count(x => x == Path.DirectorySeparatorChar)
                             orderby key
                             select key;

            foreach(string key in sortedKeys)
            {
                int index = key.LastIndexOf(Path.DirectorySeparatorChar);

                if (index == -1) // Folder in root directory
                    projectRoot.Add(FolderMap[key]);
                else
                {
                    string parentFolderName = key.Substring(0, index);
                    XElement xFolder = FolderMap[parentFolderName];
                    xFolder.Add(FolderMap[key]);
                }
            }

            /*foreach (TreeNode tn in tnc)
            {
                if (tn is FolderNode folderNode)
                    projectRoot.Add(SaveFolderNode(folderNode));
                else if (tn is DataFileNode fileNode)
                    projectRoot.Add(SaveDataFileNode(fileNode));
                else if (tn is PaletteNode paletteNode)
                    projectRoot.Add(SavePaletteNode(paletteNode));
                else if (tn is ArrangerNode arrangerNode)
                    projectRoot.Add(SaveArrangerNode(arrangerNode));
            }*/

            xmlRoot.Add(projectRoot);

            xmlRoot.Save(XmlFileName);

            return true;
        }

        public XElement SaveFolderNode(FolderNode fn)
        {
            XElement xe = new XElement("folder");
            xe.SetAttributeValue("name", fn.Name);

            /*foreach (TreeNode tn in fn.Nodes)
            {
                if (tn is FolderNode folderNode)
                    xe.Add(SaveFolderNode(folderNode));
                else if (tn is DataFileNode fileNode)
                    SaveDataFileNode(fileNode);
                else if (tn is PaletteNode paletteNode)
                    SavePaletteNode(paletteNode);
                else if (tn is ArrangerNode arrangerNode)
                    SaveArrangerNode(arrangerNode);
            }*/

            return xe;
        }

        public XElement SaveDataFile(DataFile df)
        {
            XElement xe = new XElement("datafile");
            xe.SetAttributeValue("name", df.Name);
            xe.SetAttributeValue("location", df.Location);

            return xe;
        }

        public XElement SavePalette(Palette pal)
        {
            XElement xe = new XElement("palette");

            xe.SetAttributeValue("name", pal.Name);
            xe.SetAttributeValue("fileoffset", String.Format("{0:X}", pal.FileAddress.FileOffset));
            xe.SetAttributeValue("bitoffset", String.Format("{0:X}", pal.FileAddress.BitOffset));
            xe.SetAttributeValue("datafile", pal.DataFileKey);
            xe.SetAttributeValue("format", Palette.ColorModelToString(pal.ColorModel));
            xe.SetAttributeValue("entries", pal.Entries);
            xe.SetAttributeValue("zeroindextransparent", pal.ZeroIndexTransparent);

            return xe;
        }

        public XElement SaveArranger(Arranger arr)
        {
            XElement xe = new XElement("arranger");

            xe.SetAttributeValue("name", arr.Name);
            xe.SetAttributeValue("elementsx", arr.ArrangerElementSize.Width);
            xe.SetAttributeValue("elementsy", arr.ArrangerElementSize.Height);
            xe.SetAttributeValue("width", arr.ElementPixelSize.Width);
            xe.SetAttributeValue("height", arr.ElementPixelSize.Height);

            if (arr.Layout == ArrangerLayout.TiledArranger)
                xe.SetAttributeValue("layout", "tiled");
            else if (arr.Layout == ArrangerLayout.LinearArranger)
                xe.SetAttributeValue("layout", "linear");

            string DefaultPalette = FindMostFrequentElementValue(arr, "PaletteKey");
            string DefaultFile = FindMostFrequentElementValue(arr, "DataFileKey");
            string DefaultFormat = FindMostFrequentElementValue(arr, "FormatName");

            xe.SetAttributeValue("defaultformat", DefaultFormat);
            xe.SetAttributeValue("defaultdatafile", DefaultFile);
            xe.SetAttributeValue("defaultpalette", DefaultPalette);

            for (int y = 0; y < arr.ArrangerElementSize.Height; y++)
            {
                for (int x = 0; x < arr.ArrangerElementSize.Width; x++)
                {
                    var graphic = new XElement("element");
                    ArrangerElement el = arr.GetElement(x, y);

                    graphic.SetAttributeValue("fileoffset", String.Format("{0:X}", el.FileAddress.FileOffset));
                    if(el.FileAddress.BitOffset != 0)
                        graphic.SetAttributeValue("bitoffset", String.Format("{0:X}", el.FileAddress.BitOffset));
                    graphic.SetAttributeValue("posx", x);
                    graphic.SetAttributeValue("posy", y);
                    if (el.FormatName != DefaultFormat)
                        graphic.SetAttributeValue("format", el.FormatName);
                    if (el.DataFileKey != DefaultFile)
                        graphic.SetAttributeValue("file", el.DataFileKey);
                    if (el.PaletteKey != DefaultPalette)
                        graphic.SetAttributeValue("palette", el.PaletteKey);

                    xe.Add(graphic);
                }
            }

            return xe;
        }
        #endregion

        /// <summary>
        /// Find most frequent of an attribute within an arranger's elements
        /// </summary>
        /// <param name="arr">Arranger to search</param>
        /// <param name="attributeName">Name of the attribute to find most frequent value of</param>
        /// <returns></returns>
        private string FindMostFrequentElementValue(Arranger arr, string attributeName)
        {
            Type T = typeof(ArrangerElement);
            PropertyInfo P = T.GetProperty(attributeName);

            var query = from ArrangerElement el in arr.ElementGrid
                        group el by P.GetValue(el) into grp
                        select new { key = grp.Key, count = grp.Count() };

            return query.MaxBy(x => x.count).key as string;

            /*Dictionary<string, int> freq = new Dictionary<string, int>();
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

            var max = freq.FirstOrDefault(x => x.Value == freq.Values.Max()).Key;*/

            //return max;
        }
    }
}
