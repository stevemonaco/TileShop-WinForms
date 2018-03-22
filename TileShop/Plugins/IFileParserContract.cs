using System.Collections.Generic;
using TileShop.Core;

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

        /// <summary>
        /// The host requests filenames for the files necessary for the previously generated palettes and arrangers;
        /// </summary>
        /// <returns></returns>
        List<DataFile> RetrieveDataFiles();
    }
}
