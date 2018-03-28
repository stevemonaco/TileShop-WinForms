using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop.Core
{
    public class ResourceEventArgs : EventArgs
    {
        public string ResourceKey { get; private set; }

        public ResourceEventArgs(string key)
        {
            ResourceKey = key;
        }
    }
}
