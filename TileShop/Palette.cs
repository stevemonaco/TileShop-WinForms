using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace TileShop
{
    public class Palette
    {
        private UInt32[] palette = new UInt32[256];
        
        public bool LoadPalette(string filename)
        {
            BinaryReader br = new BinaryReader(File.OpenRead(filename));

            int numColors = (int)br.BaseStream.Length / 4;

            if (numColors != 256)
                return false;

            for (int idx = 0; idx < numColors; idx++)
            {
                palette[idx] = br.ReadUInt32();
                //palette[idx] = ((palette[idx] & 0xFF) << 24) | ((palette[idx] & 0xFF00) << 8) | ((palette[idx] & 0xFF0000) >> 8) | ((palette[idx] & 0xFF0000) >> 24);
            }

            return true;
        }

        public UInt32 this[int i]
        {
            get { return palette[i]; }
        }
    }
}
