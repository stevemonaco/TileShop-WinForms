using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop
{
    /// <summary>
    /// Contains data that the host uses to identify the plugin
    /// </summary>
    public interface IFileParserData
    {
        /// <summary>
        /// The name of the plugin. This will be displayed in the GUI menu
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The author of the plugin
        /// </summary>
        string Author { get; }

        /// <summary>
        /// The version of the plugin
        /// </summary>
        string Version { get; }

        /// <summary>
        /// A description of what the plugin does
        /// </summary>
        string Description { get; }
    }
}
