using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileShop;

namespace TileShop.Plugins
{
    /// <summary>
    /// Interface for implementing a plugin that can parse files and generate descriptions
    /// for arrangers and palettes
    /// </summary>
    public interface IFileParserContract
    {
        /// <summary>
        /// The host requests that the plugin displays its interface and generate arrangers and/or palette data
        /// </summary>
        /// <returns>True if there are arrangers or palettes generated for the host to receive</returns>
        bool DisplayPluginInterface();

        /// <summary>
        /// The host requests arrangers that were previously generated
        /// </summary>
        /// <returns></returns>
        List<Arranger> RetrieveArrangers();

        /// <summary>
        /// The host requests palettes that were previously generated
        /// </summary>
        /// <returns></returns>
        List<Palette> RetrievePalettes();
    }

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
