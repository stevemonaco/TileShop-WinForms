using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TileShop.Core
{
    public class ResourceFolder: ProjectResource
    {
        public ResourceFolder()
        {
            CanContainChildResources = true;
            ChildResources = new Dictionary<string, ProjectResource>();
        }

        public override void Rename(string name)
        {
            throw new NotImplementedException();
        }

        public override ProjectResource Clone()
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
                    ChildResources.Add(new KeyValuePair<string, ProjectResource>(node.Attribute("name").Value, folder));
                }
                else if (node.Name == "datafile")
                {
                    var df = new DataFile(node.Attribute("name").Value);
                    df.Deserialize(node);
                    ChildResources.Add(new KeyValuePair<string, ProjectResource>(df.Name, df));
                }
                else if (node.Name == "palette")
                {
                    var pal = new Palette(node.Attribute("name").Value);
                    pal.Deserialize(node);
                    ChildResources.Add(new KeyValuePair<string, ProjectResource>(pal.Name, pal));
                }
                else if (node.Name == "arranger")
                {
                    //Arranger arr = Arranger.NewScatteredArranger()
                    //var arr = new DataFile(node.Attribute("name").Value);
                    //df.Deserialize(node);
                    //ChildResources.Add(new KeyValuePair<string, ProjectResource>(node.Attribute("name").Value, df));
                }
            }

            return true;
        }
    }
}
