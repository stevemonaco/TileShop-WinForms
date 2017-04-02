﻿using System;
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
    public partial class GraphicsViewerChild : DockContent
    {
        int prevFormatIndex = -1;

        public int Zoom { get; private set; }
        bool showGridlines = true;

        Size DisplayElements; // The number of elements in the entire display
        Size ElementSize; // The size of each element in unzoomed pixels
        Rectangle DisplayRect; // The zoomed pixel size of the entire display
        RenderManager rm = new RenderManager();
        public Arranger arranger { get; private set; }
        public Arranger EditArranger { get; private set; }
        ArrangerSelectionData selectionData;

        // Selection
        Rectangle ViewSelectionRect = new Rectangle(0, 0, 0, 0);

        //public GraphicsFormat graphicsFormat = null; // Sequential format only
        Pen MoveSelectionPen = new Pen(Brushes.Magenta);
        Brush MoveSelectionBrush = new SolidBrush(Color.FromArgb(200, 255, 0, 255));

        Pen EditSelectionPen = new Pen(Brushes.Green);
        Brush EditSelectionBrush = new SolidBrush(Color.FromArgb(200, 0, 200, 0));

        //Color[] pal = new Color[] { Color.Black, Color.Blue, Color.Red, Color.Green };
        TileCache cache = new TileCache();

        // UI Events
        public event EventHandler<EventArgs> EditArrangerChanged;

        public GraphicsViewerChild(string ArrangerName)
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, 
                null, RenderPanel, new object[] { true }); // Enable double buffering of the RenderPanel

            // Setup arranger variables
            arranger = FileManager.Instance.GetArranger(ArrangerName);
            EditArranger = null;
            DisplayElements = arranger.ArrangerElementSize;
            ElementSize = arranger.ElementPixelSize;
            this.Text = arranger.Name;
            selectionData = new ArrangerSelectionData(arranger.Name);

            if (arranger.Mode == ArrangerMode.SequentialArranger)
            {
                GraphicsFormat graphicsFormat = FileManager.Instance.GetGraphicsFormat(arranger.GetSequentialGraphicsFormat());

                editModeButton.Checked = false; // Do not allow edits directly to a sequential arranger
                editModeButton.Visible = false;

                // Initialize the codec select box
                List<string> formatList = FileManager.Instance.GetGraphicsFormatsNameList();
                foreach (string s in formatList)
                {
                    formatSelectBox.Items.Add(s);
                }

                formatSelectBox.SelectedIndex = formatSelectBox.Items.IndexOf(graphicsFormat.Name);
            }
            else
            {
                toolStripSeparator1.Visible = false;
                formatSelectBox.Visible = false;
                offsetLabel.Visible = false;
            }

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            MoveSelectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            MoveSelectionPen.Width = (float)Zoom;
            zoomSelectBox.SelectedIndex = 0;
            selectionData.Zoom = 1;
            DisplayRect = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);
        }

        /// <summary>
        /// Reloads arranger data from underlying source
        /// </summary>
        public void ReloadArranger()
        {
            // Forces the render manager to do a full redraw
            rm.Invalidate();
            // Redraw the viewer graphics
            RenderPanel.Invalidate();
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
                arranger.Move(ArrangerMoveType.ByteDown);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            if (keyData == Keys.Subtract || keyData == Keys.OemMinus && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.ByteUp);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            if (keyData == Keys.Down && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.RowDown);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Up && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.RowUp);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Right && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.ColRight);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Left && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.ColLeft);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.PageDown && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.PageDown);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.PageUp && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.PageUp);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Home && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.Home);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.End && arranger.Mode == ArrangerMode.SequentialArranger)
            {
                arranger.Move(ArrangerMoveType.End);
                UpdateOffsetLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Escape) // Cancel selection
            {
                CancelSelection();
                selectionData.ClearSelection();
                RenderPanel.Cursor = Cursors.Arrow;
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.OemPeriod && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element wider
            {
                DisplayElements.Width++;
                arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateOffsetLabel();
                DisplayRect = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
            }
            else if (keyData == Keys.Oemcomma && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element thinner
            {
                DisplayElements.Width--;
                if (DisplayElements.Width < 1)
                    DisplayElements.Width = 1;

                arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateOffsetLabel();
                DisplayRect = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
            }
            else if (keyData == Keys.L && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element shorter
            {
                DisplayElements.Height--;
                if (DisplayElements.Height < 1)
                    DisplayElements.Height = 1;

                arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateOffsetLabel();
                DisplayRect = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
            }
            else if (keyData == Keys.OemSemicolon && arranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element taller
            {
                DisplayElements.Height++;
                arranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateOffsetLabel();
                DisplayRect = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
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

            UpdateOffsetLabel();
            prevFormatIndex = index;
            cache.Clear();
            CancelSelection();

            rm.Invalidate();
            RenderPanel.Invalidate();
            return;
        }

        private void UpdateOffsetLabel()
        {
            long offset = arranger.GetInitialSequentialFileOffset();
            string sizestring = arranger.FileSize.ToString("X");

            int maxdigit = sizestring.Length;
            if (maxdigit % 2 == 1)
                maxdigit++; // Pad out a zero

            offsetLabel.Text = String.Format("{0:X" + maxdigit.ToString() + "} / {1:X" + maxdigit.ToString() + "}", offset, arranger.FileSize);
        }

        private void DrawGridlines(Graphics g)
        {
            if (showGridlines)
            {
                for (int y = 0; y < DisplayElements.Height; y++) // Draw horizontal lines
                    g.DrawLine(Pens.White, 0, y * ElementSize.Height * Zoom, DisplayElements.Width * ElementSize.Width * Zoom, y * ElementSize.Height * Zoom);

                for (int x = 0; x < DisplayElements.Width; x++) // Draw vertical lines
                    g.DrawLine(Pens.White, x * ElementSize.Width * Zoom, 0, x * ElementSize.Width * Zoom, DisplayElements.Height * ElementSize.Height * Zoom);
            }
        }

        private void DrawSelection(Graphics g)
        {
            // Paint selection
            if (selectionData.HasSelection)
            {
                if (editModeButton.Checked) // Selection Edit mode
                {
                    g.DrawRectangle(EditSelectionPen, ViewSelectionRect);
                    g.FillRectangle(EditSelectionBrush, ViewSelectionRect);
                }
                else // Selection Movement mode
                {
                    g.DrawRectangle(MoveSelectionPen, ViewSelectionRect);
                    g.FillRectangle(MoveSelectionBrush, ViewSelectionRect);
                }
            }
        }

        private void CancelSelection()
        {
            selectionData.ClearSelection();
            ViewSelectionRect = new Rectangle(0, 0, 0, 0);
        }

        public void SetZoom(int ZoomLevel)
        {
            zoomSelectBox.SelectedIndex = ZoomLevel - 1;
        }

        private void zoomSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Zoom = zoomSelectBox.SelectedIndex + 1;
            selectionData.Zoom = Zoom;
            DisplayRect = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);
            CancelSelection();
            RenderPanel.Invalidate();
        }

        private void showGridlinesButton_Click(object sender, EventArgs e)
        {
            showGridlines ^= true;
            if (showGridlines)
                showGridlinesButton.Checked = true;
            else
                showGridlinesButton.Checked = false;
            RenderPanel.Invalidate();
        }

        private void GraphicsViewerMdiChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void RenderPanel_Paint(object sender, PaintEventArgs e)
        {
            if (arranger == null)
                return;

            rm.Render(arranger);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy; // No transparency
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

            Rectangle src = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width, arranger.ArrangerPixelSize.Height);
            Rectangle dest = new Rectangle(0, 0, arranger.ArrangerPixelSize.Width * Zoom, arranger.ArrangerPixelSize.Height * Zoom);

            e.Graphics.DrawImage(rm.Image, dest, src, GraphicsUnit.Pixel);

            // Paint overlays
            DrawGridlines(e.Graphics);
            DrawSelection(e.Graphics);
        }

        private void RenderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (selectionData.HasSelection && !selectionData.InSelection && ViewSelectionRect.Contains(e.Location)) // Drop and drag for multiple elements
                {
                    selectionData.BeginDragDrop();
                    DoDragDrop(selectionData, DragDropEffects.Copy);
                    return;
                }
                else // New selection or single drop-and-drag
                {
                    selectionData.BeginSelection(e.Location, e.Location);
                    ViewSelectionRect = selectionData.SelectedClientRect;
                    RenderPanel.Invalidate();
                    return;
                }
            }
            DockPanel.Focus();
            RenderPanel.Focus();
            base.OnMouseDown(e);
        }

        private void RenderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if(selectionData.InDragState) // Drag+Drop
                {
                    //RenderPanel.Cursor = Cursors.Arrow;
                    selectionData.EndDragDrop();
                    //Drag
                }
                else if (selectionData.InSelection)
                {
                    selectionData.EndSelection();
                    if (editModeButton.Checked) // Edit mode deselects on MouseUp and pushes a subarranger to the pixel editor
                    {
                        selectionData.EndSelection();
                        if (selectionData.HasSelection)
                        {
                            EditArranger = arranger.CreateSubArranger("PixelEditArranger", selectionData.SelectedElements.X, selectionData.SelectedElements.Y,
                                selectionData.SelectedElements.Width, selectionData.SelectedElements.Height);
                            CancelSelection();
                            RenderPanel.Invalidate();

                            EditArrangerChanged?.Invoke(this, null);
                        }
                    }
                    else // Selection mode leaves the selection visible to be moved around between arrangers
                    {
                        ViewSelectionRect = selectionData.SelectedClientRect;
                        RenderPanel.Invalidate();
                    }
                }
            }
        }

        private void RenderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                /*if (selectionData.InDragState)
                {
                    if (DisplayRect.Contains(e.Location) && RenderPanel.Cursor != DragDropCursor)
                        RenderPanel.Cursor = DragDropCursor;
                    else if (!DisplayRect.Contains(e.Location) && RenderPanel.Cursor != CancelDragCursor)
                        RenderPanel.Cursor = CancelDragCursor;
                }
                else*/
                if (selectionData.InSelection)
                {
                    selectionData.UpdateSelection(e.Location);
                    ViewSelectionRect = selectionData.SelectedClientRect;
                    RenderPanel.Invalidate();
                }
            }
        }

        private void RenderPanel_DragDrop(object sender, DragEventArgs e)
        {
            Point LocalLocation = RenderPanel.PointToClient(new Point(e.X, e.Y));
            if (!e.Data.GetDataPresent(typeof(ArrangerSelectionData)))
                return;

            ArrangerSelectionData sel = (ArrangerSelectionData)e.Data.GetData(typeof(ArrangerSelectionData));
            sel.EndDragDrop();
            sel.PopulateData();

            if(arranger.Mode == ArrangerMode.SequentialArranger)
            {
                // Deep copy data into arranger from sel
            }
            else if(arranger.Mode == ArrangerMode.ScatteredArranger)
            {
                // Copy element data only into arranger from sel
                Point ElementLocation = selectionData.PointToElementLocation(LocalLocation);

                for (int ysrc = 0, ydest = ElementLocation.Y; ysrc < sel.SelectionSize.Height; ysrc++, ydest++)
                {
                    for (int xsrc = 0, xdest = ElementLocation.X; xsrc < sel.SelectionSize.Width; xsrc++, xdest++)
                    {
                        ArrangerElement elsrc = sel.GetElement(xsrc, ysrc);
                        ArrangerElement eldest = arranger.GetElement(xdest, ydest);
                        ArrangerElement elnew = elsrc.Clone();
                        elnew.X1 = eldest.X1;
                        elnew.X2 = eldest.X2;
                        elnew.Y1 = eldest.Y1;
                        elnew.Y2 = eldest.Y2;
                        arranger.SetElement(elnew, xdest, ydest);
                    }
                }
            }

            rm.Invalidate();
            RenderPanel.Invalidate();
        }

        private void RenderPanel_DragEnter(object sender, DragEventArgs e)
        {
            Point LocalLocation = PointToClient(new Point(e.X, e.Y));

            if (e.Data.GetDataPresent(typeof(ArrangerSelectionData)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void RenderPanel_DragLeave(object sender, EventArgs e)
        {

        }

        private void RenderPanel_DragOver(object sender, DragEventArgs e)
        {
            Point LocalLocation = PointToClient(new Point(e.X, e.Y));
            if (e.Data.GetDataPresent(typeof(ArrangerSelectionData)) && DisplayRect.Contains(LocalLocation))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void RenderPanel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            e.Action = DragAction.Continue;
        }

        private void editModeButton_Click(object sender, EventArgs e)
        {
            CancelSelection();
            editModeButton.Checked ^= true;
            RenderPanel.Invalidate();
        }

        /*private Cursor GetCursor(string cursorName)
        {
            var buffer = Properties.Resources.ResourceManager.GetObject(cursorName) as byte[];

            using (var m = new MemoryStream(buffer))
            {
                return new Cursor(m);
            }

            System.IO.MemoryStream cursorMemoryStream = new System.IO.MemoryStream(TileShop.Properties.Resources.myCursorFile);
        }*/

    }

    // Class to store a selection of arranger data

    [Serializable]
    public class ArrangerSelectionData
    {
        public string ArrangerName { get; private set; } // Name of the Arranger which holds the data to be copied
        public Point Location { get; private set; } // Upper left location of the selection, in element units
        public Size SelectionSize { get; private set; } // Size of selection in number of elements
        public bool HasSelection { get; private set; }
        public bool InSelection { get; private set; }
        public bool InDragState { get; private set; }
        public bool SelectionChanged { get; private set; } // Lets calling class to check if the selection has changed
        public int Zoom { get; set; }
        public Point BeginPoint { get; private set; } // BeginPoint/EndPoint as provided by the caller in zoomed coordinates
        public Point EndPoint { get; private set; }
        public Rectangle SelectedElements { get; private set; } // Selected elements of the parent arranger in units of elements
        public Rectangle SelectedClientRect { get; private set; }

        public ArrangerElement[,] ElementList { get; private set; }

        public ArrangerSelectionData(string arrangerName)
        {
            ArrangerName = arrangerName;
            ClearSelection();
        }

        public void ClearSelection()
        {
            Location = new Point(0, 0);
            SelectionSize = new Size(0, 0);
            ElementList = null;
            HasSelection = false;
            InSelection = false;
            InDragState = false;
            SelectionChanged = false;
            SelectedElements = new Rectangle(0, 0, 0, 0);
            SelectedClientRect = new Rectangle(0, 0, 0, 0);
            BeginPoint = new Point(0, 0);
            EndPoint = new Point(0, 0);
        }

        // Populates ElementList
        public bool PopulateData()
        {
            if (!HasSelection)
                return false;

            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            ElementList = new ArrangerElement[SelectionSize.Width, SelectionSize.Height];
            for (int ysrc = SelectedElements.Y, ydest = 0; ydest < SelectionSize.Height; ydest++, ysrc++)
            {
                for (int xsrc = SelectedElements.X, xdest = 0; xdest < SelectionSize.Width; xdest++, xsrc++)
                {
                    ElementList[xdest, ydest] = arr.GetElement(xsrc, ysrc).Clone();
                }
            }

            return true;
        }

        public ArrangerElement GetElement(int ElementX, int ElementY)
        {
            return ElementList[ElementX, ElementY];
        }

        public void BeginSelection(Point beginPoint, Point endPoint)
        {
            HasSelection = true;
            InSelection = true;
            SelectionChanged = true;
            BeginPoint = beginPoint;
            EndPoint = endPoint;
            CalculateRectangles();
        }

        // Returns true if the selection was changed (ie. endpoint changed)
        public bool UpdateSelection(Point endPoint)
        {
            if (EndPoint != endPoint)
            {
                EndPoint = endPoint;
                CalculateRectangles();
                return true;
            }
            else // No need to set as the two points are equal
                return false;
        }

        public void EndSelection()
        {
            InSelection = false;
            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);
            Rectangle testBounds = new Rectangle(new Point(0, 0), arr.ArrangerElementSize);

            if(!SelectedElements.IntersectsWith(testBounds)) // No intersection means no selection
            {
                ClearSelection();
                HasSelection = false;
            }

            //PopulateData(); // Clean this up so that PopulateData will not crash from out of ClientRect clicks
        }

        public void BeginDragDrop()
        {
            InDragState = true;
        }

        public void EndDragDrop()
        {
            InDragState = false;
        }

        
        public Point PointToElementLocation(Point Location)
        {
            Point unzoomed = new Point(Location.X / Zoom, Location.Y / Zoom);

            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            // Search list for element
            for(int y = 0; y < arr.ArrangerElementSize.Height; y++)
            {
                for(int x = 0; x < arr.ArrangerElementSize.Width; x++)
                {
                    ArrangerElement el = arr.ElementList[x, y];
                    if (unzoomed.X >= el.X1 && unzoomed.X <= el.X2 && unzoomed.Y >= el.Y1 && unzoomed.Y <= el.Y2)
                        return new Point(x, y);
                }
            }

            throw new ArgumentOutOfRangeException("Location is outside of the range of all ArrangerElements in ElementList");
        }

        private void CalculateRectangles()
        {
            Rectangle zoomed = PointsToRectangle(BeginPoint, EndPoint); // Rectangle in zoomed coordinates
            Rectangle unzoomed = ViewerToArrangerRectangle(zoomed);
            Rectangle unzoomedfull = GetSelectionPixelRect(unzoomed);

            SelectedClientRect = new Rectangle(unzoomedfull.X * Zoom, unzoomedfull.Y * Zoom, unzoomedfull.Width * Zoom, unzoomedfull.Height * Zoom);

            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            SelectedElements = new Rectangle(unzoomedfull.X / arr.ElementPixelSize.Width, unzoomedfull.Y / arr.ElementPixelSize.Height,
                unzoomedfull.Width / arr.ElementPixelSize.Width, unzoomedfull.Height / arr.ElementPixelSize.Height);

            SelectionSize = new Size(SelectedElements.Width, SelectedElements.Height);
        }

        private Rectangle GetSelectionPixelRect(Rectangle r)
        {
            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            int x1 = r.Left;
            int x2 = r.Right;
            int y1 = r.Top;
            int y2 = r.Bottom;

            // Extend rectangle to include the entirety of partially selected tiles
            foreach (ArrangerElement el in arr.ElementList)
            {
                if (x1 > el.X1 && x1 <= el.X2)
                    x1 = el.X1;
                if (y1 > el.Y1 && y1 <= el.Y2)
                    y1 = el.Y1;
                if (x2 < el.X2 && x2 >= el.X1)
                    x2 = el.X2;
                if (y2 < el.Y2 && y2 >= el.Y1)
                    y2 = el.Y2;
            }

            x2++; // Fix edges
            y2++;

            // Clamp selection rectangle to max bounds of the arranger
            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
            if (x2 >= arr.ArrangerPixelSize.Width)
                x2 = arr.ArrangerPixelSize.Width;
            if (y2 >= arr.ArrangerPixelSize.Height)
                y2 = arr.ArrangerPixelSize.Height;

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        private Rectangle ViewerToArrangerRectangle(Rectangle ClientRect)
        {
            int pleft = ClientRect.Left / Zoom;
            int ptop = ClientRect.Top / Zoom;
            int pright = (int)(ClientRect.Left / (float)Zoom + (ClientRect.Right - ClientRect.Left) / (float)Zoom);
            int pbottom = (int)(ClientRect.Top / (float)Zoom + (ClientRect.Bottom - ClientRect.Top) / (float)Zoom);

            Rectangle UnzoomedRect = new Rectangle(pleft, ptop, pright - pleft, pbottom - ptop);

            return UnzoomedRect;
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
    }
}