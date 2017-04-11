using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileShop
{
    public enum BitStreamAccess { Read, Write, ReadWrite };

    /// <summary>
    /// Struct used to store a file address that does not start on a byte-aligned address
    /// </summary>
    struct FileBitAddress
    {
        public FileBitAddress(long fileOffset, int bitOffset)
        {
            FileOffset = fileOffset;
            BitOffset = bitOffset;
        }

        /// <summary>
        /// File offset in bytes
        /// </summary>
        long FileOffset;

        /// <summary>
        /// Number of bits to skip after FileOffset
        /// Valid range is 0-7 inclusive
        /// </summary>
        int BitOffset;
    }

    class BitStream
    {
        private int bitnumber; // Current bit to read
        private int idx; // Index into data array, next byte to read
        private int bitsremaining;
        private BitStreamAccess Access;

        public byte[] Data
        {
            get { return data; }
            private set { data = value; }
        }
        private byte[] data;

        private BitStream() { }

        /// <summary>
        /// Opens an array for bit reading
        /// </summary>
        /// <param name="ReadData">Data to be read</param>
        /// <param name="DataBits">Number of valid bits to read in the array</param>
        /// <returns></returns>
        public static BitStream OpenRead(byte[] ReadData, int DataBits)
        {
            BitStream bs = new BitStream();

            bs.Data = ReadData;
            bs.bitsremaining = DataBits;
            bs.bitnumber = 8;
            bs.idx = 0;
            bs.Access = BitStreamAccess.Read;

            return bs;
        }

        public static BitStream OpenRead(BinaryReader br, int DataBits, int FirstByteBits)
        {
            BitStream bs = new BitStream();

            int ReadLength = (int)Math.Ceiling((DataBits + (8 - FirstByteBits)) / 8.0);
            bs.Data = br.ReadBytes(ReadLength);
            byte mask = (byte)((1 << FirstByteBits) - 1);
            bs.Data[0] = (byte)(bs.Data[0] & mask);

            bs.bitnumber = FirstByteBits;
            bs.bitsremaining = DataBits;
            bs.idx = 0;
            bs.Access = BitStreamAccess.Read;

            return bs;
        }

        public static BitStream OpenWrite(int DataBits, int FirstByteBits)
        {
            BitStream bs = new BitStream();

            int BufferLength = (int)Math.Ceiling((DataBits + (8 - FirstByteBits)) / 8.0);
            bs.Data = new byte[BufferLength];

            bs.bitnumber = FirstByteBits;
            bs.bitsremaining = DataBits;
            bs.idx = 0;
            bs.Access = BitStreamAccess.Write;

            return bs;
        }

        public int ReadBit()
        {
            if (Access != BitStreamAccess.Read && Access != BitStreamAccess.ReadWrite)
                throw new InvalidOperationException();
            if (bitsremaining == 0)
                throw new EndOfStreamException();

            if (bitnumber == 0)
            {
                idx++;
                if (idx == Data.Length)
                    throw new EndOfStreamException();

                bitnumber = 8;
            }

            int bit = (Data[idx] >> (bitnumber - 1)) & 1;
            bitsremaining--;
            bitnumber--;

            return bit;
        }

        public void WriteBit(byte bit)
        {
            if ((bit & 0xFE) > 1)
                throw new ArgumentOutOfRangeException();
            if (Access != BitStreamAccess.Write && Access != BitStreamAccess.ReadWrite)
                throw new InvalidOperationException();
            if (bitsremaining == 0)
                throw new EndOfStreamException();

            if(bitnumber == 0)
            {
                if (idx == Data.Length)
                    throw new EndOfStreamException();

                idx++;
                bitnumber = 8;
            }

            Data[idx] |= (byte)(bit << (bitnumber - 1));
            bitsremaining--;
            bitnumber--;
        }

        public void FlushWrites()
        {
            if(bitnumber != 8) // Some work has been done
            {

            }
        }
    }
}
