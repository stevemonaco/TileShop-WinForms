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

        int zoom = 1;
        bool showGridlines = true;
        long FileOffset = 0;
        BinaryReader br = null;
        byte[] FileData = null;
        BinaryReader mr = null;

        uint TilesX = 32;
        uint TilesY = 16;

        // Selection
        bool inSelection = false;
        bool hasSelection = false;
        Rectangle selectionRect;
        Point initialSelectionPoint;

        GraphicsFormat fmt = new GraphicsFormat();
        //Color[] pal = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Green };
        Palette pal = new Palette();
        TileCache cache = new TileCache();

        public GraphicsMdiChild(TileShopForm parent)
        {
            parentInstance = parent;
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
            //CanvasPanel, new object[] { true });

            pal.LoadPalette("C:\\Projects\\TileShop\\pal\\default.pal");

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

            zoomSelectBox.SelectedIndex = 0;

            Invalidate();
            return true;
        }

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
                return true;
            }
            else if(keyData == Keys.End)
            {
                FileOffset = br.BaseStream.Length - (TilesX * TilesY * fmt.Size());
                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
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
                case 1: // SNES 2bpp
                    fmt.Load("C:\\Projects\\TileShop\\codecs\\SNES2bpp.xml");
                    break;
                case 2: // NES 1bpp
                    fmt.Load("C:\\Projects\\TileShop\\codecs\\NES1bpp.xml");
                    break;
                case 3: // SNES 4bpp
                    fmt.Load("C:\\Projects\\TileShop\\codecs\\SNES4bpp.xml");
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
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            mr.BaseStream.Seek(FileOffset, SeekOrigin.Begin);
            uint TileOffset = (uint)FileOffset;

            int xzoomsize = (int)fmt.Width * zoom;
            int yzoomsize = (int)fmt.Height * zoom;

            Rectangle src = new Rectangle(0, 0, (int)fmt.Width, (int)fmt.Height);
            Rectangle dest = new Rectangle(0, toolStrip1.Height, xzoomsize, yzoomsize);

            // Paint data
            for (int y = 0; y < TilesY; y++)
            {
                dest.X = 0;

                for (int x = 0; x < TilesX; x++)
                {
                    //Bitmap bmp = cache.GetTile(TileOffset);
                    Bitmap bmp = null;
                    if (bmp == null) // Decode tile
                    {
                        bmp = new Bitmap((int)fmt.Width, (int)fmt.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                        mr.BaseStream.Seek(TileOffset, SeekOrigin.Begin);
                        GraphicsCodec.Decode(bmp, fmt, -1, mr, pal);
                        //cache.AddTile(bmp, TileOffset);
                    }

                    e.Graphics.DrawImage(bmp, dest, src, GraphicsUnit.Pixel);
                    //e.Graphics.DrawImage(bmp, dest);

                    TileOffset = TileOffset + (uint)fmt.Size();
                    dest.X += xzoomsize;
                }

                dest.Y += yzoomsize;
            }

            // Paint gridlines
            if(showGridlines)
            {
                for(int y = 0; y < TilesY; y++) // Draw horizontal lines
                    e.Graphics.DrawLine(Pens.White, 0, y * yzoomsize + toolStrip1.Height, TilesX * xzoomsize, y * yzoomsize + toolStrip1.Height);

                for(int x = 0; x < TilesX; x++) // Draw vertical lines
                    e.Graphics.DrawLine(Pens.White, x * xzoomsize, 0, x * xzoomsize, TilesY * yzoomsize + toolStrip1.Height);
            }

            // Paint selection
            if(hasSelection)
            {

            }
        }

        private void zoomSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(zoomSelectBox.SelectedIndex)
            {
                case 0:
                    zoom = 1;
                    break;
                case 1:
                    zoom = 2;
                    break;
                case 2:
                    zoom = 3;
                    break;
                case 3:
                    zoom = 4;
                    break;
            }

            Invalidate();
        }

        private void GraphicsMdiChild_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (e.X < TilesX * fmt.Width * zoom && e.Y < TilesY * fmt.Height * zoom)
                {
                    hasSelection = true;
                    string text = String.Format("Selected Pixel {0} {1} Tile {2} {3}", e.X, e.Y, Math.Floor(e.X / (double)fmt.Width / zoom), Math.Floor((e.Y - toolStrip1.Height) / (double)fmt.Height / zoom));
                    parentInstance.updateSelectionLabel(text);
                }
            }
        }

        private void GraphicsMdiChild_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void showGridlinesButton_Click(object sender, EventArgs e)
        {
            showGridlines ^= true;
            if (showGridlines)
                showGridlinesButton.Checked = true;
            else
                showGridlinesButton.Checked = false;
            Invalidate();
        }
    }
}
