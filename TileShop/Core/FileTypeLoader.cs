using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TileShop.Core
{
    class FileTypeLoader
    {
        Dictionary<string, string> LookupDefaultFormat = new Dictionary<string, string>();

        public FileTypeLoader()
        {
            LookupDefaultFormat.Add(".nes", "NES 1bpp");
            LookupDefaultFormat.Add(".sfc", "SNES 2bpp");
            LookupDefaultFormat.Add(".smc", "SNES 2bpp");
            //LookupDefaultFormat.Add("", "");
        }

        /// <summary>
        /// Retrieves the default graphics format name for a given filename
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        public string GetDefaultFormatName(string Filename)
        {
            if (LookupDefaultFormat.ContainsKey(Path.GetExtension(Filename)))
                return LookupDefaultFormat[Path.GetExtension(Filename)];
            else
                return "NES 1bpp";
        }

        public GraphicsFormat GetDefaultFormat(string Filename)
        {
            string name;
            if (LookupDefaultFormat.ContainsKey(Path.GetExtension(Filename)))
                name = LookupDefaultFormat[Path.GetExtension(Filename)];
            else
                name = "NES 1bpp";

            return ResourceManager.Instance.GetGraphicsFormat(name);
        }
    }
}
