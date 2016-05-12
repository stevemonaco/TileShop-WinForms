using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace TileShop
{
    public partial class GraphicsMdiChild : Form
    {
        TileShopForm parentInstance = null;
        int prevFormatIndex = -1;
        string filename;

        int zoom = 2;
        long FileOffset = 0;
        BinaryReader br = null;
        byte[] FileData = null;
        BinaryReader mr = null;
        TileCache cache = new TileCache();
        Color[] pal = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Green };

        uint TilesX = 32;
        uint TilesY = 32;

        GraphicsFormat fmt = new GraphicsFormat();
        OldGraphicsCodec codec = new OldGraphicsCodec();

        public GraphicsMdiChild(TileShopForm parent)
        {
            parentInstance = parent;
            InitializeComponent();

            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
            //CanvasPanel, new object[] { true });

            this.GotFocus += GraphicsMdiChild_GotFocus;
        }

        private void GraphicsMdiChild_GotFocus(object sender, EventArgs e)
        {
            parentInstance.updateOffsetLabel(FileOffset);
        }

        public bool OpenFile(string InputFilename)
        {
            filename = InputFilename;
            br = new BinaryReader(File.OpenRead(filename));

            if (br == null)
                return false;

            this.Text = filename;
            FileData = br.ReadBytes((int)br.BaseStream.Length);
            mr = new BinaryReader(new MemoryStream(FileData));

            vertScroll.Maximum = (int)br.BaseStream.Length;
            vertScroll.Minimum = 0;

            switch(Path.GetExtension(filename))
            {
                case ".sfc": case ".smc":
                    formatSelectBox.SelectedIndex = 1;
                    break;
                case ".nes":
                    formatSelectBox.SelectedIndex = 0;
                    break;
                default:
                    formatSelectBox.SelectedIndex = 0;
                    break;
            }

            Invalidate();
            return true;
        }

        /*private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            if (br == null)
                return;

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

            mr.BaseStream.Seek(FileOffset, SeekOrigin.Begin);
            uint TileOffset = (uint)FileOffset;

            Rectangle src = new Rectangle(0, 0, (int)fmt.PixelsX, (int)fmt.PixelsY);
            Rectangle dest = new Rectangle(0, 0, 0, 0);

            int xzoomsize = (int)fmt.PixelsX * zoom;
            int yzoomsize = (int)fmt.PixelsY * zoom;

            // Paint data
            for (int y = 0; y < TilesY; y++)
            {
                for (int x = 0; x < TilesX; x++)
                {
                    Bitmap bmp = cache.GetTile(TileOffset);
                    if (bmp == null) // Decode tile
                    {
                        bmp = new Bitmap((int)fmt.PixelsX, (int)fmt.PixelsY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        mr.BaseStream.Seek(TileOffset, SeekOrigin.Begin);
                        codec.Decode(bmp, fmt, -1, mr, pal);
                        cache.AddTile(bmp, TileOffset);
                    }

                    int posx = x * (int)fmt.PixelsX * zoom;
                    int posy = y * (int)fmt.PixelsY * zoom;
                    dest = new Rectangle(posx, posy, xzoomsize, yzoomsize);

                    //e.Graphics.DrawImage(bmp, x * PixelsX, y * PixelsY);
                    e.Graphics.DrawImage(bmp, dest, src, GraphicsUnit.Pixel);

                    TileOffset = TileOffset + (uint)fmt.Bytes();
                }
            }
        }*/

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Down)
            {
                long delta = TilesX * fmt.Size();
                FileOffset += delta;
                long MaxOffset = br.BaseStream.Length - (TilesX * TilesY * fmt.Size());
                FileOffset = Math.Min(FileOffset, MaxOffset);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Up)
            {
                long delta = TilesX * fmt.Size();
                FileOffset -= delta;
                FileOffset = Math.Max(FileOffset, 0);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Right)
            {
                long delta = fmt.Size();
                FileOffset += delta;
                long MaxOffset = br.BaseStream.Length - (TilesX * TilesY * fmt.Size());
                FileOffset = Math.Min(FileOffset, MaxOffset);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Left)
            {
                long delta = fmt.Size();
                FileOffset -= delta;
                FileOffset = Math.Max(FileOffset, 0);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageDown)
            {
                long delta = TilesX * (TilesY / 2) * fmt.Size();
                FileOffset += delta;
                long MaxOffset = br.BaseStream.Length - (TilesX * TilesY * fmt.Size());
                FileOffset = Math.Min(FileOffset, MaxOffset);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageUp)
            {
                long delta = TilesX * (TilesY / 2) * fmt.Size();
                FileOffset -= delta;
                FileOffset = Math.Max(FileOffset, 0);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if(keyData == Keys.Home)
            {
                FileOffset = 0;
                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
            }
            else if(keyData == Keys.End)
            {
                FileOffset = br.BaseStream.Length - (TilesX * TilesY * fmt.Size());
                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void formatSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = formatSelectBox.SelectedIndex;

            if (index == prevFormatIndex) // No need to change formats and clear cache
                return;

            switch(index)
            {
                case 0: // 2bpp NES
                    fmt.Load("C:\\Projects\\TileShop\\codecs\\NES2bpp.xml");
                    break;
                case 1: // SNES
                    fmt.Load("C:\\Projects\\TileShop\\codecs\\SNES2bpp.xml");
                    break;
                case 2:
                    fmt.Load("C:\\Projects\\TileShop\\codecs\\NES1bpp.xml");
                    break;
            }

            prevFormatIndex = index;
            cache.Clear();
            Invalidate();
            return;
        }

        private void GraphicsMdiChild_Paint(object sender, PaintEventArgs e)
        {
            if (br == null)
                return;

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

            mr.BaseStream.Seek(FileOffset, SeekOrigin.Begin);
            uint TileOffset = (uint)FileOffset;

            Rectangle src = new Rectangle(0, 0, (int)fmt.Width, (int)fmt.Height);
            Rectangle dest = new Rectangle(0, 0, 0, 0);

            int xzoomsize = (int)fmt.Width * zoom;
            int yzoomsize = (int)fmt.Height * zoom;

            // Paint data
            for (int y = 0; y < TilesY; y++)
            {
                for (int x = 0; x < TilesX; x++)
                {
                    //Bitmap bmp = cache.GetTile(TileOffset);
                    Bitmap bmp = null;
                    if (bmp == null) // Decode tile
                    {
                        bmp = new Bitmap((int)fmt.Width, (int)fmt.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        mr.BaseStream.Seek(TileOffset, SeekOrigin.Begin);
                        GraphicsCodec.Decode(bmp, fmt, -1, mr, pal);
                        //cache.AddTile(bmp, TileOffset);
                    }

                    int posx = x * (int)fmt.Width * zoom;
                    int posy = y * (int)fmt.Height * zoom + toolStrip1.Height;
                    dest = new Rectangle(posx, posy, xzoomsize, yzoomsize);

                    //e.Graphics.DrawImage(bmp, x * PixelsX, y * PixelsY);
                    e.Graphics.DrawImage(bmp, dest, src, GraphicsUnit.Pixel);

                    TileOffset = TileOffset + (uint)fmt.Size();
                }
            }
        }
    }
}
