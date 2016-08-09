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
    // RenderManager
    // Class that 

    public class RenderManager
    {
        public Bitmap Image { get; set; }
        private Graphics g = null;
        private bool NeedsRedraw = true;

        public bool Render(ArrangerList list)
        {
            if (list == null)
                throw new NullReferenceException();
            if (list.ArrangerPixelSize.Width == 0 || list.ArrangerPixelSize.Height == 0)
                return false;

            if (Image == null || list.ArrangerPixelSize.Width != Image.Width || list.ArrangerPixelSize.Height != Image.Height)
            {
                Image = new Bitmap(list.ArrangerPixelSize.Width, list.ArrangerPixelSize.Height, PixelFormat.Format32bppRgb);
                g = Graphics.FromImage(Image);
            }

            if(g == null || Image == null)
                throw new NullReferenceException();

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

            for(int i = 0; i < list.ArrangerElementSize.Height; i++)
            {
                for(int j = 0; j < list.ArrangerElementSize.Width; j++)
                {
                    ArrangerElement el = list.ElementList[j, i];
                    if (!list.IsSequential) // Reader update required
                    {
                        br = new BinaryReader(FileManager.Instance.GetFileStream(el.FileName));
                        br.BaseStream.Seek(el.FileOffset, SeekOrigin.Begin);
                    }

                    GraphicsCodec.Decode(Image, FileManager.Instance.GetFormat(el.Format), el.X1, el.Y1, br, FileManager.Instance.GetPalette(el.Palette));
                }
            }

            NeedsRedraw = false;

            return true;
        }

        // Forces a redraw for next Render call
        public void Invalidate()
        {
            NeedsRedraw = true;
        }

        bool SetPixel(int x, int y, Color color)
        {
            return false;
        }

    }
}
