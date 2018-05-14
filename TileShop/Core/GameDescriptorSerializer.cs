using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using MoreLinq;

namespace TileShop.Core
{
    /// <summary>
    /// Provides XML support for reading/writing Game Descriptor Files and loading the content into the ProjectExplorerControl
    /// </summary>
    static class GameDescriptorSerializer
    {
        #region XML Deserialization methods        
        /// <summary>
        /// Deserializes a Stream of XML data
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="baseDirectory">The full path of the directory containing the stream.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">LoadProject was called with a null stream</exception>
        /// <exception cref="ArgumentException">LoadProject was called with a null or empty baseDirectory</exception>
        public static Dictionary<string, ProjectResourceBase> DeserializeProject(Stream stream, string baseDirectory)
        {
            if (stream == null)
                throw new ArgumentNullException("LoadProject was called with a null stream");
            if (String.IsNullOrEmpty(baseDirectory))
                throw new ArgumentException("LoadProject was called with a null or empty baseDirectory");

            XElement doc = XElement.Load(stream);
            XElement projectNode = doc.Element("project");

            Directory.SetCurrentDirectory(baseDirectory);

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
        public static void SerializeProject(Dictionary<string, ProjectResourceBase> projectTree, Stream stream)
        {
            if (projectTree == null)
                throw new ArgumentNullException($"SerializeProject was called with a null {nameof(projectTree)}");
            if (stream == null)
                throw new ArgumentNullException("SerializeProject was called with a null stream");
            if (!stream.CanWrite)
                throw new ArgumentException("SerializeProject was called with a stream without write access");

            var xmlRoot = new XElement("gdf");
            var projectRoot = new XElement("project");
            var settingsRoot = new XElement("settings");

            xmlRoot.Add(settingsRoot);
            xmlRoot.Add(projectRoot);

            var orderedNodes = projectTree.Values.OrderBy(x => x, new ProjectResourceBaseComparer()).Where(x => x.ShouldBeSerialized);
            orderedNodes.ForEach(x => projectRoot.Add(x.Serialize()));

            xmlRoot.Save(stream);
        }
        #endregion
    }
}
