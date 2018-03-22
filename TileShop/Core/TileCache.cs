using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TileShop.Core
{
    class TileCache
    {
        private Dictionary<UInt32, Bitmap> TileLookup = new Dictionary<uint, Bitmap>();

        public void Clear()
        {
            TileLookup.Clear();
        }

        public Bitmap GetTile(UInt32 FileOffset)
        {
            if (TileLookup.ContainsKey(FileOffset))
                return TileLookup[FileOffset];
            else
                return null;
        }

        public bool AddTile(Bitmap bmp, UInt32 FileOffset)
        {
            TileLookup.Add(FileOffset, bmp);
            return true;
        }

        public bool RemoveTile(UInt32 FileOffset)
        {
            if (TileLookup.ContainsKey(FileOffset))
            {
                TileLookup.Remove(FileOffset);
                //Bitmap bmp = TileLookup[FileOffset];
                //bmp.Dispose();
            }

            return true;
        }
    }
}
