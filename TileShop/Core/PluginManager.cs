using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;

namespace TileShop.Plugins
{
    /// <summary>
    /// Class to manage the loading and operation of TileShop plugins
    /// </summary>
    class PluginManager
    {
        CompositionContainer container;

        [ImportMany]
        public IEnumerable<Lazy<IFileParserContract, IFileParserData>> ParserPlugins { get; private set; }

        /// <summary>
        /// Loads a directory of plugins
        /// </summary>
        /// <param name="path">Path to plugin directory</param>
        /// <returns></returns>
        public bool LoadPlugins(string path)
        {
            AggregateCatalog catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(path));
            container = new CompositionContainer(catalog);

            try
            {
                container.ComposeParts(this);
            }
            catch(CompositionException ex)
            {
                MessageBox.Show(ex.Message); // TODO: Change error handling
            }
            catch(ReflectionTypeLoadException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return true;
        }
    }
}
