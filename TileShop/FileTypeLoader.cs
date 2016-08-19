using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TileShop
{
    class FileTypeLoader
    {
        Dictionary<string, string> LookupDefaultFormat = new Dictionary<string, string>();

        public FileTypeLoader()
        {
            LookupDefaultFormat.Add(".nes", "NES 1bpp");
            LookupDefaultFormat.Add(".sfc", "SNES 2bpp");
            LookupDefaultFormat.Add(".smc", "SNES 2bpp");
            LookupDefaultFormat.Add("", "");
        }

        // Retrieves default graphics format name from a filename
        public string GetDefaultFormatName(string Filename)
        {
            return LookupDefaultFormat[Path.GetExtension(Filename)];
        }
    }
}
