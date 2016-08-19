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
        Graphics g = null;
        bool NeedsRedraw = true;

        public bool Render(Arranger arranger)
        {
            if (arranger == null)
                throw new ArgumentNullException();
            if (arranger.ArrangerPixelSize.Width == 0 || arranger.ArrangerPixelSize.Height == 0)
                return false;

            if (Image == null || arranger.ArrangerPixelSize.Width != Image.Width || arranger.ArrangerPixelSize.Height != Image.Height)
            {
                Image = new Bitmap(arranger.ArrangerPixelSize.Width, arranger.ArrangerPixelSize.Height, PixelFormat.Format32bppRgb);
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

            if(arranger.IsSequential)
            {
                br = new BinaryReader(FileManager.Instance.GetFileStream(arranger.ElementList[0, 0].FileName));
                br.BaseStream.Seek(arranger.ElementList[0, 0].FileOffset, SeekOrigin.Begin);
            }

            for(int i = 0; i < arranger.ArrangerElementSize.Height; i++)
            {
                for(int j = 0; j < arranger.ArrangerElementSize.Width; j++)
                {
                    ArrangerElement el = arranger.ElementList[j, i];
                    if (!arranger.IsSequential) // Reader update required
                    {
                        if (el.Format == null) // Null format means a blank tile
                        {
                            GraphicsCodec.DecodeBlank(Image, el);
                            continue;
                        }
                        br = new BinaryReader(FileManager.Instance.GetFileStream(el.FileName));
                        br.BaseStream.Seek(el.FileOffset, SeekOrigin.Begin);
                    }

                    GraphicsCodec.Decode(Image, FileManager.Instance.GetGraphicsFormat(el.Format), el.X1, el.Y1, br, FileManager.Instance.GetPalette(el.Palette));
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
