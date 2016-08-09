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
using WeifenLuo.WinFormsUI.Docking;

namespace TileShop
{
    public partial class GraphicsViewerMdiChild : DockContent
    {
        TileShopForm parentInstance = null;
        ArrangerMode arrangerMode;
        int prevFormatIndex = -1;
        string filename;

        public int zoom = 1;
        bool showGridlines = true;
        long FileOffset = 0;

        Size DisplayElements = new Size(8, 16);
        RenderManager rm = new RenderManager();
        ArrangerList list = null;

        // Selection
        bool inSelection = false;
        bool hasSelection = false;
        Rectangle ViewSelectionRect = new Rectangle(0, 0, 0, 0);
        Point beginSelectionPoint = Point.Empty;
        Point endSelectionPoint = Point.Empty;

        public GraphicsFormat graphicsFormat = null;
        Pen p = new Pen(Brushes.Magenta);
        Brush b = new SolidBrush(Color.FromArgb(200, 255, 0, 255));

        //Color[] pal = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Green };
        TileCache cache = new TileCache();

        public GraphicsViewerMdiChild(TileShopForm parent, ArrangerMode v)
        {
            parentInstance = parent;
            arrangerMode = v;

            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
            //CanvasPanel, new object[] { true });

            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            p.Width = (float)zoom;

            this.GotFocus += GraphicsMdiChild_GotFocus;
        }

        public void LoadTileArranger(GraphicsFormat format, Size TileArrangerSize, string XmlFilename)
        {
            if(XmlFilename == null) // Create blank arranger
            {
                list = new ArrangerList(TileArrangerSize.Width, TileArrangerSize.Height, null, format, arrangerMode);

                
                //rm.NewTileArranger(TileArrangerSize.Width, TileArrangerSize.Height, null, format);
            }
            else // Load arrangement via XML
            {
                //rm.NewArranger(TileArrangerSize.Width, TileArrangerSize.Height, null, );
            }
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

            int formatIdx = 0;

            switch(Path.GetExtension(InputFilename))
            {
                case ".sfc": case ".smc":
                    arrangerMode = ArrangerMode.SequentialFile;
                    formatIdx = 1;
                    formatSelectBox.Visible = true;
                    break;
                case ".nes":
                    arrangerMode = ArrangerMode.SequentialFile;
                    formatIdx = 0;
                    formatSelectBox.Visible = true;
                    break;
                case ".xml":
                    arrangerMode = ArrangerMode.ScatteredArranger;
                    formatSelectBox.Visible = false;
                    break;
                default:
                    arrangerMode = ArrangerMode.SequentialFile;
                    formatIdx = 0;
                    formatSelectBox.Visible = true;
                    break;
            }

            graphicsFormat = GetGraphicsFormatFromIndex(formatIdx);

            list = new ArrangerList(DisplayElements.Width, DisplayElements.Height, Path.GetFileNameWithoutExtension(InputFilename), graphicsFormat, arrangerMode);

            zoomSelectBox.SelectedIndex = 0;
            formatSelectBox.SelectedIndex = formatIdx;

            Invalidate();
            return true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Add || keyData == Keys.Oemplus && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.ByteDown);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            if (keyData == Keys.Subtract || keyData == Keys.OemMinus && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.ByteUp);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            if (keyData == Keys.Down && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.RowDown);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Up && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.RowUp);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Right && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.ColRight);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Left && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.ColLeft);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageDown && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.PageDown);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageUp && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.PageUp);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Home && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.Home);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.End && arrangerMode == ArrangerMode.SequentialFile)
            {
                FileOffset = list.Move(ArrangerMoveType.End);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Escape) // Cancel selection
            {
                CancelSelection();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.OemPeriod && arrangerMode == ArrangerMode.SequentialFile) // Make arranger one element wider
            {
                DisplayElements.Width++;
                FileOffset = list.ResizeArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }
            else if(keyData == Keys.Oemcomma && arrangerMode == ArrangerMode.SequentialFile) // Make arranger one element thinner
            {
                DisplayElements.Width--;
                if (DisplayElements.Width < 1)
                    DisplayElements.Width = 1;

                FileOffset = list.ResizeArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }
            else if(keyData == Keys.L && arrangerMode == ArrangerMode.SequentialFile) // Make arranger one element shorter
            {
                DisplayElements.Height--;
                if (DisplayElements.Height < 1)
                    DisplayElements.Height = 1;

                FileOffset = list.ResizeArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }
            else if(keyData == Keys.OemSemicolon && arrangerMode == ArrangerMode.SequentialFile) // Make arranger one element taller
            {
                DisplayElements.Height++;
                FileOffset = list.ResizeArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void formatSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = formatSelectBox.SelectedIndex;

            if (index == prevFormatIndex) // No need to change formats and clear cache
                return;

            graphicsFormat = GetGraphicsFormatFromIndex(index);
            list.SetGraphicsFormat(graphicsFormat.Name);

            FileOffset = list.GetInitialSequentialFileOffset();
            parentInstance.updateOffsetLabel(FileOffset);
            prevFormatIndex = index;
            cache.Clear();
            rm.Invalidate();
            Invalidate();
            return;
        }

        private GraphicsFormat GetGraphicsFormatFromIndex(int index)
        {
            if(index == 0) // NES 1bpp
                return FileManager.Instance.GetFormat("NES 2bpp");
            else if (index == 1) // SNES 2bpp
                return FileManager.Instance.GetFormat("SNES/GB 2bpp");
            else if (index == 2) // NES 1bpp
                return FileManager.Instance.GetFormat("NES 1bpp");
            else if (index == 3) // SNES 4bpp
                return FileManager.Instance.GetFormat("SNES 4bpp");
            else if (index == 4) // SNES 3bpp
                return FileManager.Instance.GetFormat("SNES 3bpp");

            return FileManager.Instance.GetFormat("Transparent");
        }

        private void GraphicsMdiChild_Paint(object sender, PaintEventArgs e)
        {
            if (list == null)
                return;

            rm.Render(list);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            Rectangle src = new Rectangle(0, 0, list.ArrangerPixelSize.Width, list.ArrangerPixelSize.Height);
            Rectangle dest = new Rectangle(0, toolStrip1.Height, list.ArrangerPixelSize.Width * zoom, list.ArrangerPixelSize.Height * zoom);

            e.Graphics.DrawImage(rm.Image, dest, src, GraphicsUnit.Pixel);

            // Paint overlays
            DrawGridlines(e.Graphics);
            DrawSelection(e.Graphics);
        }

        private void DrawGridlines(Graphics g)
        {
            if (showGridlines)
            {
                for (int y = 0; y < DisplayElements.Height; y++) // Draw horizontal lines
                    g.DrawLine(Pens.White, 0, y * graphicsFormat.Height * zoom + toolStrip1.Height, DisplayElements.Width * graphicsFormat.Width * zoom, y * graphicsFormat.Height * zoom + toolStrip1.Height);

                for (int x = 0; x < DisplayElements.Width; x++) // Draw vertical lines
                    g.DrawLine(Pens.White, x * graphicsFormat.Width * zoom, 0, x * graphicsFormat.Width * zoom, DisplayElements.Height * graphicsFormat.Height * zoom + toolStrip1.Height);
            }
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

        private void CancelSelection()
        {
            beginSelectionPoint = Point.Empty;
            endSelectionPoint = Point.Empty;
            ViewSelectionRect = new Rectangle(0, 0, 0, 0);
            hasSelection = false;
            inSelection = false;
        }

        private void zoomSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            zoom = zoomSelectBox.SelectedIndex + 1;
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

            Rectangle ArrRect = list.GetSelectionRect(UnzoomedRect);

            Rectangle ResizedRect = new Rectangle(new Point(ArrRect.Left * zoom, (ArrRect.Top * zoom) + toolStrip1.Height),
                new Size(ArrRect.Width * zoom, ArrRect.Height * zoom));

            return ResizedRect;
        }
    }
}
