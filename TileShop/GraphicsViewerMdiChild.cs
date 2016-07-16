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
    public partial class GraphicsViewerMdiChild : Form
    {
        TileShopForm parentInstance = null;
        int prevFormatIndex = -1;
        string filename;

        public int zoom = 1;
        bool showGridlines = true;
        long FileOffset = 0;

        int TilesX = 16;
        int TilesY = 16;
        RenderManager rm = new RenderManager();

        // Selection
        bool inSelection = false;
        bool hasSelection = false;
        Rectangle ViewSelectionRect = new Rectangle(0, 0, 0, 0);
        Point beginSelectionPoint = Point.Empty;
        Point endSelectionPoint = Point.Empty;

        GraphicsFormat fmt = new GraphicsFormat();
        Pen p = new Pen(Brushes.Magenta);
        Brush b = new SolidBrush(Color.FromArgb(200, 255, 0, 255));

        //Color[] pal = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Green };
        TileCache cache = new TileCache();

        public GraphicsViewerMdiChild(TileShopForm parent)
        {
            parentInstance = parent;
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
            //CanvasPanel, new object[] { true });

            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            p.Width = (float)zoom;

            this.GotFocus += GraphicsMdiChild_GotFocus;
        }

        private void GraphicsMdiChild_GotFocus(object sender, EventArgs e)
        {
            parentInstance.updateOffsetLabel(FileOffset);
        }

        public bool OpenFile(string InputFilename)
        {
            if(!FileManager.Instance.LoadFile(InputFilename))
            {
                MessageBox.Show("Failed to open " + InputFilename, "File Error", MessageBoxButtons.OK);
                this.Close();
                return false;
            }

            this.Text = InputFilename;
            filename = Path.GetFileNameWithoutExtension(InputFilename);
            FileOffset = 0;
            parentInstance.updateOffsetLabel(FileOffset);

            vertScroll.Maximum = (int) FileManager.Instance.GetFileStream(filename).Length;
            vertScroll.Minimum = 0;

            switch(Path.GetExtension(InputFilename))
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

            rm.NewArranger(TilesX, TilesY, Path.GetFileNameWithoutExtension(InputFilename), fmt);

            Invalidate();
            return true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Add || keyData == Keys.Oemplus)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.ByteDown);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            if (keyData == Keys.Subtract || keyData == Keys.OemMinus)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.ByteUp);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            if (keyData == Keys.Down)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.RowDown);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Up)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.RowUp);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Right)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.ColRight);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Left)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.ColLeft);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageDown)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.PageDown);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageUp)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.PageUp);

                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Home)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.Home);
                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.End)
            {
                FileOffset = rm.MoveOffset(ArrangerMoveType.End);
                parentInstance.updateOffsetLabel(FileOffset);
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Escape) // Cancel selection
            {
                beginSelectionPoint = Point.Empty;
                endSelectionPoint = Point.Empty;
                ViewSelectionRect = new Rectangle(0, 0, 0, 0);
                hasSelection = false;
                inSelection = false;
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
                    fmt = FileManager.Instance.GetFormat("NES 2bpp");
                    rm.SetGraphicsFormat("NES 2bpp");
                    break;
                case 1: // SNES 2bpp
                    fmt = FileManager.Instance.GetFormat("SNES/GB 2bpp");
                    rm.SetGraphicsFormat("SNES/GB 2bpp");
                    break;
                case 2: // NES 1bpp
                    fmt = FileManager.Instance.GetFormat("NES 1bpp");
                    rm.SetGraphicsFormat("NES 1bpp");
                    break;
                case 3: // SNES 4bpp
                    fmt = FileManager.Instance.GetFormat("SNES 4bpp");
                    rm.SetGraphicsFormat("SNES 4bpp");
                    break;
            }

            prevFormatIndex = index;
            cache.Clear();
            Invalidate();
            return;
        }

        private void GraphicsMdiChild_Paint(object sender, PaintEventArgs e)
        {
            rm.Render();

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            Rectangle src = new Rectangle(0, 0, rm.list.ArrangerSize.Width, rm.list.ArrangerSize.Height);
            Rectangle dest = new Rectangle(0, toolStrip1.Height, rm.list.ArrangerSize.Width * zoom, rm.list.ArrangerSize.Height * zoom);

            e.Graphics.DrawImage(rm.bmp, dest, src, GraphicsUnit.Pixel);

            // Paint overlays
            DrawGridlines(e.Graphics);
            DrawSelection(e.Graphics);
        }

        private void DrawGridlines(Graphics g)
        {
            for (int y = 0; y < TilesY; y++) // Draw horizontal lines
                g.DrawLine(Pens.White, 0, y * fmt.Height * zoom + toolStrip1.Height, TilesX * fmt.Width * zoom, y * fmt.Height * zoom + toolStrip1.Height);

            for (int x = 0; x < TilesX; x++) // Draw vertical lines
                g.DrawLine(Pens.White, x * fmt.Width * zoom, 0, x * fmt.Width * zoom, TilesY * fmt.Height * zoom + toolStrip1.Height);
        }

        private void DrawSelection(Graphics g)
        {
            // Paint selection
            if (hasSelection)
            {
                g.DrawRectangle(p, ViewSelectionRect);
                g.FillRectangle(b, ViewSelectionRect);
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
                case 4:
                    zoom = 5;
                    break;
                case 5:
                    zoom = 6;
                    break;
                case 6:
                    zoom = 7;
                    break;
                case 7:
                    zoom = 8;
                    break;
            }

            Invalidate();
        }

        private void GraphicsMdiChild_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (beginSelectionPoint != Point.Empty)
                {
                    inSelection = true;
                    hasSelection = true;
                    endSelectionPoint = e.Location;
                    Rectangle NewViewSelectionRect = ResizeSelectionRect(PointsToRectangle(beginSelectionPoint, endSelectionPoint));

                    // Re-render only if the ViewSelectionRect has changed
                    if (ViewSelectionRect.Location != NewViewSelectionRect.Location || ViewSelectionRect.Size != NewViewSelectionRect.Size)
                    {
                        ViewSelectionRect = NewViewSelectionRect;
                        Invalidate();
                    }
                }
            }
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

        private void GraphicsViewerMdiChild_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (hasSelection && ViewSelectionRect.Contains(e.Location)) // Drop and drag
                {

                }
                else // New selection
                {
                    inSelection = true;
                    hasSelection = true;
                    beginSelectionPoint = e.Location;
                    endSelectionPoint = e.Location;

                    Rectangle NewViewSelectionRect = ResizeSelectionRect(PointsToRectangle(beginSelectionPoint, endSelectionPoint));
                    // Re-render only if the ViewSelectionRect has changed
                    if (ViewSelectionRect.Location != NewViewSelectionRect.Location || ViewSelectionRect.Size != NewViewSelectionRect.Size)
                    {
                        ViewSelectionRect = NewViewSelectionRect;
                        Invalidate();
                    }
                }
            }
        }

        private void GraphicsViewerMdiChild_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (beginSelectionPoint != Point.Empty && endSelectionPoint != Point.Empty)
                {
                    Rectangle NewViewSelectionRect = ResizeSelectionRect(PointsToRectangle(beginSelectionPoint, endSelectionPoint));
                    beginSelectionPoint = Point.Empty;
                    endSelectionPoint = Point.Empty;
                    inSelection = false;

                    // Re-render only if the ViewSelectionRect has changed
                    if (ViewSelectionRect.Location != NewViewSelectionRect.Location || ViewSelectionRect.Size != NewViewSelectionRect.Size)
                    {
                        ViewSelectionRect = NewViewSelectionRect;
                        Invalidate();
                    }
                }
            }
        }

        private Rectangle PointsToRectangle(Point beginPoint, Point endPoint)
        {
            int top = beginPoint.Y < endPoint.Y ? beginPoint.Y : endPoint.Y;
            int bottom = beginPoint.Y > endPoint.Y ? beginPoint.Y : endPoint.Y;
            int left = beginPoint.X < endPoint.X ? beginPoint.X : endPoint.X;
            int right = beginPoint.X > endPoint.X ? beginPoint.X : endPoint.X;

            Rectangle rect = new Rectangle(left, top, (right - left), (bottom - top));
            return rect;
        }

        // Resizes the client selection rect so that it correctly highlights all selected tiles (or pixels for linear)
        private Rectangle ResizeSelectionRect(Rectangle ClientRect)
        {
            // Creating UnzoomedRect with some floating point avoids one-off errors due to integer truncations
            int pleft = ClientRect.Left / zoom;
            int ptop = (ClientRect.Top - toolStrip1.Height) / zoom;
            int pright = (int)(ClientRect.Left / (float)zoom + (ClientRect.Right - ClientRect.Left) / (float)zoom);
            int pbottom = (int)((ClientRect.Top - toolStrip1.Height) / (float)zoom + (ClientRect.Bottom - ClientRect.Top) / (float)zoom);

            Rectangle UnzoomedRect = new Rectangle(pleft, ptop, pright - pleft, pbottom - ptop);

            Rectangle ArrRect = rm.list.GetSelectionRect(UnzoomedRect);

            Rectangle ResizedRect = new Rectangle(new Point(ArrRect.Left * zoom, (ArrRect.Top * zoom) + toolStrip1.Height),
                new Size(ArrRect.Width * zoom, ArrRect.Height * zoom));

            return ResizedRect;
        }
    }
}
