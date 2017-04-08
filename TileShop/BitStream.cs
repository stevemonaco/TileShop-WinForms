using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileShop
{
    class BitStream
    {
        private byte workingbyte; // Current byte being read
        private int bitnumber; // Current bit to read
        private int idx; // Index into data array, next byte to read
        private byte[] data;
        private int bitsremaining;

        public BitStream(byte[] Data, int DataBits)
        {
            data = Data;
            bitsremaining = DataBits;
            bitnumber = 8;
            workingbyte = data[0];
            idx = 1;
        }

        public BitStream(BinaryReader br, int DataBits, int FirstByteBits)
        {
            int ReadLength = (int)Math.Ceiling((DataBits + (8 - FirstByteBits)) / 8.0);
            data = br.ReadBytes(ReadLength);
            byte mask = (byte)((1 << FirstByteBits) - 1);
            data[0] = (byte)(data[0] & mask);

            bitnumber = FirstByteBits;
            bitsremaining = DataBits;
            workingbyte = data[0];
            idx = 1;
        }

        public int ReadBit()
        {
            if (bitsremaining == 0)
                throw new EndOfStreamException();

            if (bitnumber == 0)
            {
                if (idx == data.Length)
                    throw new EndOfStreamException();

                workingbyte = data[idx];
                idx++;
                bitnumber = 8;
            }

            int bit = (workingbyte >> (bitnumber - 1)) & 1;
            bitsremaining--;
            bitnumber--;

            return bit;
        }
    }
}
