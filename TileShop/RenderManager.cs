using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace TileShop
{
    public class RenderManager
    {
        public Bitmap bmp { get; set; }
        public ArrangerList list { get; set; }

        private Graphics g = null;
        private bool NeedsRedraw = true;

        public bool Render()
        {
            if (list == null)
                return false;
            if (list.ListX == 0 || list.ListY == 0)
                return false;

            if (!NeedsRedraw)
                return true;

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            BinaryReader br = null;

            // TODO: Refactor into separate render loops for sequential and arranged versions
            // Remember TileCache

            if(list.IsSequential)
            {
                br = new BinaryReader(FileManager.Instance.GetFileStream(list.ElementList[0, 0].FileName));
                br.BaseStream.Seek(list.ElementList[0, 0].FileOffset, SeekOrigin.Begin);
            }

            for(int i = 0; i < list.ListY; i++)
            {
                for(int j = 0; j < list.ListX; j++)
                {
                    ArrangerElement el = list.ElementList[j, i];
                    if (!list.IsSequential) // Reader update required
                    {
                        br = new BinaryReader(FileManager.Instance.GetFileStream(el.FileName));
                        br.BaseStream.Seek(el.FileOffset, SeekOrigin.Begin);
                    }

                    GraphicsCodec.Decode(bmp, FileManager.Instance.GetFormat(el.Format), el.X1, el.Y1, br, FileManager.Instance.GetPalette(el.Palette));
                }
            }

            NeedsRedraw = false;

            return true;
        }

        public bool NewArranger(int ElementsX, int ElementsY, string Filename, GraphicsFormat format)
        {
            list = new ArrangerList(ElementsX, ElementsY, Filename, format);
            bmp = new Bitmap(ElementsX * format.Width, ElementsY * format.Height, PixelFormat.Format32bppRgb);
            g = Graphics.FromImage(bmp);

            NeedsRedraw = true;

            return true;
        }

        public long ResizeArranger(int ElementsX, int ElementsY)
        {
            long offset = GetInitialSequentialFileOffset();
            GraphicsFormat fmt = FileManager.Instance.GetFormat(list.ElementList[0, 0].Format);
            list = new ArrangerList(ElementsX, ElementsY, list.ElementList[0, 0].FileName, fmt);
            offset = list.Move(offset);

            bmp = new Bitmap(ElementsX * fmt.Width, ElementsY * fmt.Height, PixelFormat.Format32bppRgb);
            g = Graphics.FromImage(bmp);

            NeedsRedraw = true;

            return offset;
        }

        public long MoveOffset(ArrangerMoveType MoveType)
        {
            if (list == null)
                return -1;

            long offset = list.ElementList[0, 0].FileOffset;
            long newoffset = list.Move(MoveType);
            if (newoffset != offset) // Same offset, no move
                NeedsRedraw = true;

            return newoffset;
        }

        public bool SetGraphicsFormat(string Format)
        {
            if (list == null)
                return false;

            long offset = list.ElementList[0, 0].FileOffset;
            GraphicsFormat fmt = FileManager.Instance.GetFormat(Format);
            int elemsize = fmt.Width * fmt.Height * fmt.ColorDepth / 8;
            list.ArrangerByteSize = list.ListX * list.ListY * elemsize;

            if (list.FileSize < offset + list.ArrangerByteSize)
                offset = list.FileSize - list.ArrangerByteSize;

            foreach (ArrangerElement el in list.ElementList)
            {
                el.FileOffset = offset;
                offset += elemsize;
                el.Format = Format;
            }

            NeedsRedraw = true;

            return true;
        }

        public long GetInitialSequentialFileOffset()
        {
            if (list != null)
                return list.ElementList[0, 0].FileOffset;
            else
                return 0;
        }

        bool SetPixel(int x, int y, Color color)
        {
            return false;
        }

    }
}
