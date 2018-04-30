using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TileShop.Core
{
    public class ResourceFolder: ProjectResourceBase
    {
        public ResourceFolder()
        {
            CanContainChildResources = true;
            ChildResources = new Dictionary<string, ProjectResourceBase>();
        }

        public override void Rename(string name)
        {
            throw new NotImplementedException();
        }

        public override ProjectResourceBase Clone()
        {
            ResourceFolder rf = new ResourceFolder();
            rf.Name = Name;

            return rf;
        }

        public override XElement Serialize()
        {
            throw new NotImplementedException();
        }

        public override bool Deserialize(XElement element)
        {
            Name = element.Attribute("name").Value;

            foreach (XElement node in element.Elements())
            {
                if (node.Name == "folder")
                {
                    var folder = new ResourceFolder();
                    folder.Deserialize(node);
                    folder.Parent = this;
                    ChildResources.Add(node.Attribute("name").Value, folder);
                }
                else if (node.Name == "datafile")
                {
                    var df = new DataFile(node.Attribute("name").Value);
                    df.Deserialize(node);
                    df.Parent = this;
                    ChildResources.Add(df.Name, df);
                }
                else if (node.Name == "palette")
                {
                    var pal = new Palette(node.Attribute("name").Value);
                    pal.Deserialize(node);
                    pal.Parent = this;
                    ChildResources.Add(pal.Name, pal);
                }
                else if (node.Name == "arranger")
                {
                    var arr = new ScatteredArranger();
                    arr.Rename(node.Attribute("name").Value);
                    arr.Deserialize(node);
                    arr.Parent = this;
                    ChildResources.Add(arr.Name, arr);
                }
            }

            return true;
        }
    }
}
