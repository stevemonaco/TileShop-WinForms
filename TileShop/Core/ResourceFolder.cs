using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop.Core
{
    public class ResourceFolder: IProjectResource
    {
        /// <summary>
        /// Gets the name of the ResourceFolder
        /// </summary>
        public string Name { get; private set; }

        public void Rename(string name)
        {
            throw new NotImplementedException();
        }

        public IProjectResource Clone()
        {
            ResourceFolder rf = new ResourceFolder();
            rf.Name = Name;

            return rf;
        }
    }
}
