using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop.Core
{
    public class ResourceEventArgs : EventArgs
    {
        public string ResourceName { get; private set; }

        public ResourceEventArgs(string name)
        {
            ResourceName = name;
        }
    }
}
