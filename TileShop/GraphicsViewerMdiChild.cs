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
        int prevFormatIndex = -1;

        public int Zoom { get; private set; }
        bool showGridlines = true;
        long FileOffset = 0;

        Size DisplayElements;
        Size ElementSize;
        RenderManager rm = new RenderManager();
        public Arranger arranger { get; private set; }

        // Selection
        bool inSelection = false;
        bool hasSelection = false;
        Rectangle ViewSelectionRect = new Rectangle(0, 0, 0, 0);
        Point beginSelectionPoint = Point.Empty;
        Point endSelectionPoint = Point.Empty;

        //public GraphicsFormat graphicsFormat = null; // Sequential format only
        Pen p = new Pen(Brushes.Magenta);
        Brush b = new SolidBrush(Color.FromArgb(200, 255, 0, 255));

        //Color[] pal = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Green };
        TileCache cache = new TileCache();

        public GraphicsViewerMdiChild(TileShopForm parent, string ArrangerName)
        {
            parentInstance = parent;

            InitializeComponent();

            // Setup arranger variables
            arranger = FileManager.Instance.GetArranger(ArrangerName);
            DisplayElements = arranger.ArrangerElementSize;
            ElementSize = arranger.ElementPixelSize;
            this.Text = arranger.Name;

            if (arranger.Mode == ArrangerMode.SequentialArranger)
            {
                GraphicsFormat graphicsFormat = FileManager.Instance.GetGraphicsFormat(arranger.GetSequentialGraphicsFormat());

                // Initialize the codec select box
                List<string> formatList = FileManager.Instance.GetGraphicsFormatsNameList();
                foreach (string s in formatList)
                {
                    formatSelectBox.Items.Add(s);
                }

                formatSelectBox.SelectedIndex = formatSelectBox.Items.IndexOf(graphicsFormat.Name);
            }
            else
                formatSelectBox.Enabled = false;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            p.Width = (float)Zoom;
            zoomSelectBox.SelectedIndex = 0;

            this.GotFocus += GraphicsMdiChild_GotFocus;
        }

        private void GraphicsMdiChild_GotFocus(object sender, EventArgs e)
        {
            parentInstance.updateOffsetLabel(FileOffset);
        }

        /*public bool OpenFile(string InputFilename)
        {
            if (InputFilename == null)
                throw new ArgumentNullException();

            if (!FileManager.Instance.LoadFile(InputFilename))
            {
                MessageBox.Show("Failed to open " + InputFilename, "File Error", MessageBoxButtons.OK);
                this.Close();
                return false;
            }

            //if (Path.GetExtension(InputFilename) == ".xml")
            //    return LoadScatteredArrangerFile(InputFilename);

            this.Text = InputFilename;
            filename = Path.GetFileNameWithoutExtension(InputFilename);
            FileOffset = 0;
            parentInstance.updateOffsetLabel(FileOffset);

            vertScroll.Maximum = (int)FileManager.Instance.GetFileStream(filename).Length;
            vertScroll.Minimum = 0;

            int formatIdx = 0;

            return true;
        }*/

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Add || keyData == Keys.Oemplus && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.ByteDown);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            if (keyData == Keys.Subtract || keyData == Keys.OemMinus && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.ByteUp);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            if (keyData == Keys.Down && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.RowDown);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Up && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.RowUp);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Right && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.ColRight);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Left && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.ColLeft);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageDown && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.PageDown);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.PageUp && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.PageUp);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.Home && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.Home);
                parentInstance.updateOffsetLabel(FileOffset);
                CancelSelection();
                rm.Invalidate();
                Invalidate();
                return true;
            }
            else if (keyData == Keys.End && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                FileOffset = arranger.Move(ArrangerMoveType.End);
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
            else if (keyData == Keys.OemPeriod && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element wider
            {
                DisplayElements.Width++;
                FileOffset = arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }
            else if (keyData == Keys.Oemcomma && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element thinner
            {
                DisplayElements.Width--;
                if (DisplayElements.Width < 1)
                    DisplayElements.Width = 1;

                FileOffset = arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }
            else if (keyData == Keys.L && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element shorter
            {
                DisplayElements.Height--;
                if (DisplayElements.Height < 1)
                    DisplayElements.Height = 1;

                FileOffset = arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }
            else if (keyData == Keys.OemSemicolon && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element taller
            {
                DisplayElements.Height++;
                FileOffset = arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                parentInstance.updateOffsetLabel(FileOffset);
                rm.Invalidate();
                Invalidate();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void formatSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (arranger.Mode != ArrangerMode.SequentialArranger)
                return;

            int index = formatSelectBox.SelectedIndex;

            if (index == prevFormatIndex) // No need to change formats and clear cache
                return;

            arranger.SetGraphicsFormat((string)formatSelectBox.SelectedItem);

            FileOffset = arranger.GetInitialSequentialFileOffset();
            parentInstance.updateOffsetLabel(FileOffset);
            prevFormatIndex = index;
            cache.Clear();
            CancelSelection();

            rm.Invalidate();
            Invalidate();
            return;
        }

        private void GraphicsMdiChild_Paint(object sender, PaintEventArgs e)
        {
            if (arranger == null)
                return;

            rm.Render(arranger);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            Rectangle src = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width, arranger.ArrangerPixelSize.Height);
            Rectangle dest = new Rectangle(0, toolStrip1.Height, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);

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
                    g.DrawLine(Pens.White, 0, y * ElementSize.Height * Zoom + toolStrip1.Height, DisplayElements.Width * ElementSize.Width * Zoom, y * ElementSize.Height * Zoom + toolStrip1.Height);

                for (int x = 0; x < DisplayElements.Width; x++) // Draw vertical lines
                    g.DrawLine(Pens.White, x * ElementSize.Width * Zoom, 0, x * ElementSize.Width * Zoom, DisplayElements.Height * ElementSize.Height * Zoom + toolStrip1.Height);
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

        public void SetZoom(int ZoomLevel)
        {
            //Zoom = ZoomLevel;
            zoomSelectBox.SelectedIndex = ZoomLevel - 1;
        }

        private void zoomSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Zoom = zoomSelectBox.SelectedIndex + 1;
            CancelSelection();
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
            int pleft = ClientRect.Left / Zoom;
            int ptop = (ClientRect.Top - toolStrip1.Height) / Zoom;
            int pright = (int)(ClientRect.Left / (float)Zoom + (ClientRect.Right - ClientRect.Left) / (float)Zoom);
            int pbottom = (int)((ClientRect.Top - toolStrip1.Height) / (float)Zoom + (ClientRect.Bottom - ClientRect.Top) / (float)Zoom);

            Rectangle UnzoomedRect = new Rectangle(pleft, ptop, pright - pleft, pbottom - ptop);

            Rectangle ArrRect = arranger.GetSelectionRect(UnzoomedRect);

            Rectangle ResizedRect = new Rectangle(new Point(ArrRect.Left * Zoom, (ArrRect.Top * Zoom) + toolStrip1.Height),
                new Size(ArrRect.Width * Zoom, ArrRect.Height * Zoom));

            return ResizedRect;
        }

        private void GraphicsViewerMdiChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FileManager.Instance.
        }
    }
}
