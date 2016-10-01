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
    // Class that is responsible for rendering an Arranger into a bitmap

    public class RenderManager
    {
        public Bitmap Image { get; set; }
        bool NeedsRedraw = true;

        public bool Render(Arranger arranger)
        {
            if (arranger == null)
                throw new ArgumentNullException();
            if (arranger.ArrangerPixelSize.Width <= 0 || arranger.ArrangerPixelSize.Height <= 0)
                return false;

            if (Image == null || arranger.ArrangerPixelSize.Width != Image.Width || arranger.ArrangerPixelSize.Height != Image.Height)
                Image = new Bitmap(arranger.ArrangerPixelSize.Width, arranger.ArrangerPixelSize.Height, PixelFormat.Format32bppArgb);

            if(Image == null)
                throw new NullReferenceException();

            if (!NeedsRedraw)
                return true;

            BinaryReader br = null;

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
                        if (el.Format == "") // Empty format means a blank tile
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

        public Color GetPixel(int x, int y)
        {
            if (Image == null)
                throw new NullReferenceException();

            return Image.GetPixel(x, y);
        }

        public bool SetPixel(int x, int y, Color color)
        {
            Image.SetPixel(x, y, color);
            return true;
        }

    }
}
