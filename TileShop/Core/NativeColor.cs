using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop.Core
{
    /// <summary>
    /// Manages the storage and conversion of internal 32bit ARGB colors
    /// </summary>
    struct NativeColor
    {
        private const int AlphaShift = 24;
        private const int RedShift = 16;
        private const int GreenShift = 8;
        private const int BlueShift = 0;

        NativeColor(UInt32 color)
        {
            Color = color;
        }

        NativeColor(byte A, byte R, byte G, byte B)
        {
            Color = ((UInt32)A << AlphaShift | ((UInt32)R << RedShift) | ((UInt32)G << GreenShift) | ((UInt32)B << BlueShift));
        }

        /// <summary>
        /// Gets or sets the native 32bit ARGB Color
        /// </summary>
        public UInt32 Color;

        #region Color Channel Helper Functions

        /// <summary>
        /// Gets the native alpha channel intensity
        /// </summary>
        /// <returns></returns>
        public byte A()
        {
            return (byte)((Color >> AlphaShift) & 0xFF);
        }

        /// <summary>
        /// Gets the native red channel intensity
        /// </summary>
        /// <returns></returns>
        public byte R()
        {
            return (byte)((Color >> RedShift) & 0xFF);
        }

        /// <summary>
        /// Gets the native green channel intensity
        /// </summary>
        /// <returns></returns>
        public byte G()
        {
            return (byte)((Color >> GreenShift) & 0xFF);
        }

        /// <summary>
        /// Gets the native blue channel intensity
        /// </summary>
        /// <returns></returns>
        public byte B()
        {
            return (byte)((Color >> BlueShift) & 0xFF);
        }

        public (byte A, byte R, byte G, byte B) Split() => (A(), R(), G(), B());

        #endregion

        public static ForeignColor ToForeignColor(PaletteColorFormat format)
        {
            throw new NotImplementedException();

            ForeignColor fc;

            switch(format)
            {
                case PaletteColorFormat.BGR15:
                    break;
                case PaletteColorFormat.ABGR16:
                    break;
                case PaletteColorFormat.RGB15:
                    break;
                default:
                    throw new ArgumentException("Unsupported PaletteColorFormat " + format.ToString());
            }

            return fc;
        }

        #region Cast operators
        public static implicit operator NativeColor(uint color)
        {
            return new NativeColor(color);
        }

        public static implicit operator UInt32(NativeColor color)
        {
            return color.Color;
        }
        #endregion
    }
}
