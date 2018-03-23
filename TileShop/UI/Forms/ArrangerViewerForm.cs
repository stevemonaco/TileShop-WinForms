using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using TileShop.PopupForms;
using TileShop.Core;

namespace TileShop
{
    public partial class ArrangerViewerForm : EditorDockContent
    {
        enum ArrangerViewerCursorMode { ArrangeElements = 0, EditElements = 1, ApplyPalette = 2 }
        ArrangerViewerCursorMode CursorMode;

        public int Zoom
        {
            get { return zoom; }
            private set
            {
                zoom = value;
                selectionData.Zoom = Zoom;
                DisplayRect = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);
                CancelSelection();
                RenderPanel.Invalidate();
            }
        }
        int zoom;

        Size DisplayElements; // The number of elements in the entire display
        Size ElementSize; // The size of each element in unzoomed pixels
        Rectangle DisplayRect; // The zoomed pixel size of the entire display
        RenderManager rm = new RenderManager();
        public Arranger DisplayArranger { get; private set; }
        public Arranger EditArranger { get; private set; } // The subarranger associated with the pixel editor
        ArrangerSelectionData selectionData;

        // Selection in zoomed pixels
        Rectangle ViewSelectionRect = new Rectangle(0, 0, 0, 0);

        Pen ArrangeSelectionPen = new Pen(Brushes.Magenta);
        Brush ArrangeSelectionBrush = new SolidBrush(Color.FromArgb(200, 255, 0, 255));

        Pen EditSelectionPen = new Pen(Brushes.Green);
        Brush EditSelectionBrush = new SolidBrush(Color.FromArgb(200, 0, 200, 0));

        TileCache cache = new TileCache();

        // UI Events
        public event EventHandler<EventArgs> EditArrangerChanged;

        public ArrangerViewerForm(string ArrangerKey)
        {
            InitializeComponent();
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, 
                null, RenderPanel, new object[] { true }); // Enable double buffering of the RenderPanel

            CursorMode = ArrangerViewerCursorMode.ArrangeElements;
            this.GotFocus += GraphicsViewerChild_GotFocus;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            ArrangeSelectionPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            ArrangeSelectionPen.Width = (float)Zoom;

            // Setup arranger variables
            ContentSourceKey = ArrangerKey;
            DisplayArranger = ResourceManager.Instance.GetResource(ArrangerKey) as Arranger;
            ReloadArranger();
            selectionData = new ArrangerSelectionData(ContentSourceKey);
            selectionData.Zoom = 1;

            if (DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                GraphicsFormat graphicsFormat = ResourceManager.Instance.GetGraphicsFormat(DisplayArranger.GetSequentialGraphicsFormat());

                SaveButton.Visible = false;
                ReloadButton.Visible = false;
                SaveLoadSeparator.Visible = false;
                ArrangerPropertiesButton.Visible = false;
                PaletteDropDownButton.Visible = false;

                // Initialize the codec select box
                List<string> formatList = ResourceManager.Instance.GetGraphicsFormatsNameList();
                foreach (string s in formatList)
                    FormatSelectBox.Items.Add(s);

                FormatSelectBox.SelectedIndex = FormatSelectBox.Items.IndexOf(graphicsFormat.Name);
            }
            else if (DisplayArranger.Mode == ArrangerMode.ScatteredArranger)
            {
                JumpButton.Visible = false;
                toolStripSeparator1.Visible = false;
                FormatSelectBox.Visible = false;
                offsetLabel.Visible = false;
                BuildPaletteButtonMenu();
            }
        }

        #region EditorDockContent Implementation
        public override bool ReloadContent()
        {
            throw new NotImplementedException();
            //DisplayArranger = ResourceManager.Instance.ReloadArranger(ContentSourceKey);
            EditArranger = null;
            DisplayElements = DisplayArranger.ArrangerElementSize;
            ElementSize = DisplayArranger.ElementPixelSize;
            ContentSourceName = DisplayArranger.Name;
            selectionData = new ArrangerSelectionData(ContentSourceKey);
            selectionData.Zoom = Zoom;
            DisplayRect = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);

            // Forces the render manager to do a full redraw
            rm.Invalidate();
            // Redraw the viewer graphics
            RenderPanel.Invalidate();

            ContainsModifiedContent = false;
            return true;
        }

        public override bool SaveContent()
        {
            if (DisplayArranger.Mode == ArrangerMode.SequentialArranger) // Sequential arrangers cannot be saved/modified
                return true;

            MessageBox.Show("SaveContent");

            throw new NotImplementedException();
            //ResourceManager.Instance.SaveArranger(ContentSourceKey);

            ContainsModifiedContent = false;
            return true;
        }

        public override bool RefreshContent()
        {
            // Forces a full redraw
            rm.Invalidate();
            RenderPanel.Invalidate();

            return true;
        }
        #endregion

        /// <summary>
        /// Reloads arranger data from underlying source
        /// </summary>
        public void ReloadArranger()
        {
            EditArranger = null;
            DisplayElements = DisplayArranger.ArrangerElementSize;
            ElementSize = DisplayArranger.ElementPixelSize;
            ContentSourceName = DisplayArranger.Name;
            ContainsModifiedContent = false;
            DisplayRect = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);

            // Forces a full redraw
            rm.Invalidate();
            RenderPanel.Invalidate();
        }

        #region Rendering Functions
        private void RenderPanel_Paint(object sender, PaintEventArgs e)
        {
            if (DisplayArranger == null)
                return;

            rm.Render(DisplayArranger);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            //e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy; // No transparency
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver; // Transparency

            Rectangle src = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width, DisplayArranger.ArrangerPixelSize.Height);
            Rectangle dest = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);

            e.Graphics.FillRectangle(SystemBrushes.ControlDarkDark, DisplayRect);
            e.Graphics.DrawImage(rm.Image, dest, src, GraphicsUnit.Pixel);

            // Paint overlays
            e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver; // Enable transparency for overlays
            DrawSelection(e.Graphics);
            DrawGridlines(e.Graphics); // Draw gridlines last so the selection overlays appear to fit cleanly into the grid
        }

        private void DrawGridlines(Graphics g)
        {
            if (ShowGridlinesButton.Checked)
            {
                for (int y = 0; y <= DisplayElements.Height; y++) // Draw horizontal lines
                    g.DrawLine(Pens.White, 0, y * ElementSize.Height * Zoom + 1,
                        DisplayElements.Width * ElementSize.Width * Zoom, y * ElementSize.Height * Zoom + 1);

                for (int x = 0; x <= DisplayElements.Width; x++) // Draw vertical lines
                    g.DrawLine(Pens.White, x * ElementSize.Width * Zoom + 1, 0,
                        x * ElementSize.Width * Zoom + 1, DisplayElements.Height * ElementSize.Height * Zoom);
            }
        }

        /// <summary>
        /// Draws the overlay that indicates the user has selected elements
        /// </summary>
        /// <param name="g"></param>
        private void DrawSelection(Graphics g)
        {
            if (selectionData.HasSelection)
            {
                if (CursorMode == ArrangerViewerCursorMode.ArrangeElements) // Arrange Elements mode
                    g.FillRectangle(ArrangeSelectionBrush, ViewSelectionRect);
                else // Edit Elements mode
                    g.FillRectangle(EditSelectionBrush, ViewSelectionRect);
            }
        }
        #endregion

        #region Mouse Actions and Drag+Drop
        private void RenderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (CursorMode == ArrangerViewerCursorMode.ArrangeElements || CursorMode == ArrangerViewerCursorMode.EditElements))
            {
                if (selectionData.HasSelection && !selectionData.InSelection && ViewSelectionRect.Contains(e.Location)) // Drop and drag for multiple elements
                {
                    selectionData.BeginDragDrop();
                    DoDragDrop(selectionData, DragDropEffects.Copy);
                }
                else // New selection or single drop-and-drag
                {
                    selectionData.BeginSelection(e.Location, e.Location);
                    ViewSelectionRect = selectionData.SelectedClientRect;
                    RenderPanel.Invalidate();
                }
            }
            else if (e.Button == MouseButtons.Left && CursorMode == ArrangerViewerCursorMode.ApplyPalette && DisplayRect.Contains(e.Location))
            {
                Point location = DisplayArranger.PointToElementLocation(e.Location, Zoom);
                DisplayArranger.ElementGrid[location.X, location.Y].PaletteKey = GetSelectedPaletteKey();
                ContainsModifiedContent = true;
                RefreshContent();
            }

            Parent.Focus(); // Fixes the tab highlighting issues with the Documents view not getting focus
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

                    if(CursorMode == ArrangerViewerCursorMode.ArrangeElements)
                    {
                        // Arrange Elements mode leaves the selection visible to be moved around between arrangers
                        ViewSelectionRect = selectionData.SelectedClientRect;
                        RenderPanel.Invalidate();
                    }
                    else if(CursorMode == ArrangerViewerCursorMode.EditElements)
                    {
                        // Edit mode deselects on MouseUp and pushes a subarranger to the pixel editor
                        selectionData.EndSelection();
                        if (selectionData.HasSelection)
                        {
                            Arranger newEditArranger = DisplayArranger.CreateSubArranger("PixelEditArranger", selectionData.SelectedElements.X, selectionData.SelectedElements.Y,
                                selectionData.SelectedElements.Width, selectionData.SelectedElements.Height);

                            if (newEditArranger.ContainsBlankElements())
                                MessageBox.Show("Selection contains unreferenced elements and cannot be edited");
                            else
                            {
                                EditArranger = DisplayArranger.CreateSubArranger("PixelEditArranger", selectionData.SelectedElements.X, selectionData.SelectedElements.Y,
                                    selectionData.SelectedElements.Width, selectionData.SelectedElements.Height);
                                CancelSelection();
                                RenderPanel.Invalidate();

                                EditArrangerChanged?.Invoke(this, null);
                            }
                        }
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

            Point ElementLocation = DisplayArranger.PointToElementLocation(LocalLocation);
            ArrangerSelectionData sel = (ArrangerSelectionData)e.Data.GetData(typeof(ArrangerSelectionData));

            if (ElementLocation.X + sel.SelectionSize.Width > DisplayArranger.ArrangerElementSize.Width)
                return;
                    
            if(ElementLocation.Y + sel.SelectionSize.Height > DisplayArranger.ArrangerElementSize.Height)
                return;

            sel.EndDragDrop();
            sel.PopulateData();

            if(DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                // Deep copy data into arranger from sel
            }
            else if(DisplayArranger.Mode == ArrangerMode.ScatteredArranger)
            {
                // Copy element data only into arranger from sel
                for (int ysrc = 0, ydest = ElementLocation.Y; ysrc < sel.SelectionSize.Height; ysrc++, ydest++)
                {
                    for (int xsrc = 0, xdest = ElementLocation.X; xsrc < sel.SelectionSize.Width; xsrc++, xdest++)
                    {
                        ArrangerElement elsrc = sel.GetElement(xsrc, ysrc);
                        ArrangerElement eldest = DisplayArranger.GetElement(xdest, ydest);
                        ArrangerElement elnew = elsrc.Clone();
                        elnew.X1 = eldest.X1;
                        elnew.X2 = eldest.X2;
                        elnew.Y1 = eldest.Y1;
                        elnew.Y2 = eldest.Y2;
                        DisplayArranger.SetElement(elnew, xdest, ydest);
                    }
                }
            }

            ContainsModifiedContent = true;
            rm.Invalidate();
            RenderPanel.Invalidate();
        }

        private void RenderPanel_DragEnter(object sender, DragEventArgs e)
        {
            Point LocalLocation = RenderPanel.PointToClient(new Point(e.X, e.Y));

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
            Point LocalLocation = RenderPanel.PointToClient(new Point(e.X, e.Y));
            if (e.Data.GetDataPresent(typeof(ArrangerSelectionData)) && DisplayRect.Contains(LocalLocation))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void RenderPanel_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            e.Action = DragAction.Continue;
        }
        #endregion

        #region Keyboard Actions
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Add || keyData == Keys.Oemplus && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.ByteDown);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            if (keyData == Keys.Subtract || keyData == Keys.OemMinus && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.ByteUp);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            if (keyData == Keys.Down && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.RowDown);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Up && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.RowUp);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Right && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.ColRight);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Left && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.ColLeft);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.PageDown && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.PageDown);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.PageUp && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.PageUp);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Home && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.Home);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.End && DisplayArranger.Mode == ArrangerMode.SequentialArranger)
            {
                DisplayArranger.Move(ArrangerMoveType.End);
                UpdateAddressLabel();
                CancelSelection();
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Escape) // Cancel selection
            {
                CancelSelection();
                RenderPanel.Cursor = Cursors.Arrow;
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.OemQuestion && DisplayArranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element wider
            {
                DisplayElements.Width++;
                DisplayArranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateAddressLabel();
                DisplayRect = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.OemPeriod && DisplayArranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element thinner
            {
                DisplayElements.Width--;
                if (DisplayElements.Width < 1)
                    DisplayElements.Width = 1;

                DisplayArranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateAddressLabel();
                DisplayRect = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.L && DisplayArranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element shorter
            {
                DisplayElements.Height--;
                if (DisplayElements.Height < 1)
                    DisplayElements.Height = 1;

                DisplayArranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateAddressLabel();
                DisplayRect = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.OemSemicolon && DisplayArranger.Mode == ArrangerMode.SequentialArranger) // Make arranger one element taller
            {
                DisplayElements.Height++;
                DisplayArranger.ResizeSequentialArranger(DisplayElements.Width, DisplayElements.Height);
                UpdateAddressLabel();
                DisplayRect = new Rectangle(0, 0, DisplayArranger.ArrangerPixelSize.Width * Zoom, DisplayArranger.ArrangerPixelSize.Height * Zoom);
                rm.Invalidate();
                RenderPanel.Invalidate();
                return true;
            }
            else if (keyData == Keys.Z) // Zoom in
            {
                Zoom++;
                return true;
            }
            else if (keyData == Keys.X) // Zoom out
            {
                if (Zoom > 1)
                    Zoom--;
                return true;
            }
            else if (keyData == Keys.A && DisplayArranger.Mode == ArrangerMode.SequentialArranger) // Next codec
            {
                if (FormatSelectBox.SelectedIndex + 1 == FormatSelectBox.Items.Count)
                    FormatSelectBox.SelectedIndex = 0;
                else
                    FormatSelectBox.SelectedIndex++;
                return true;
            }
            else if (keyData == Keys.S && DisplayArranger.Mode == ArrangerMode.SequentialArranger) // Previous codec
            {
                if (FormatSelectBox.SelectedIndex == 0)
                    FormatSelectBox.SelectedIndex = FormatSelectBox.Items.Count - 1;
                else
                    FormatSelectBox.SelectedIndex--;
                return true;
            }
            else if (keyData == Keys.G) // Toggle Gridlines
            {
                ShowGridlinesButton.Checked ^= true;
                RenderPanel.Invalidate();
            }
            else if (keyData == Keys.J) // Show jump dialog
                ShowJumpDialog();

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region User Interface Events
        private void FormatSelectBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DisplayArranger.Mode != ArrangerMode.SequentialArranger)
                return;

            GraphicsFormat format = ResourceManager.Instance.GetGraphicsFormat((string)FormatSelectBox.SelectedItem);
            DisplayArranger.SetGraphicsFormat((string)FormatSelectBox.SelectedItem, new Size(format.DefaultWidth, format.DefaultHeight));
            ElementSize = DisplayArranger.ElementPixelSize;

            UpdateAddressLabel();
            cache.Clear();
            CancelSelection();

            rm.Invalidate();
            RenderPanel.Invalidate();
            RenderPanel.Focus(); // Removes the focus from the FormatSelectBox

            return;
        }

        private void FormatSelectBox_DropDownClosed(object sender, EventArgs e)
        {
            //this.BeginInvoke(new Action(() => { FormatSelectBox.Select(FormatSelectBox.Text.Length, 0); }));
        }

        private void ArrangeModeButton_Click(object sender, EventArgs e)
        {
            CancelSelection();

            ArrangeModeButton.Checked = true;
            EditModeButton.Checked = false;
            foreach (ToolStripItem tsi in PaletteDropDownButton.DropDownItems)
                if(tsi is ToolStripMenuItem tsmi)
                    tsmi.Checked = false;
            PaletteDropDownButton.Checked = false;

            CursorMode = ArrangerViewerCursorMode.ArrangeElements;
            RenderPanel.Invalidate();
        }

        private void EditModeButton_Click(object sender, EventArgs e)
        {
            CancelSelection();

            EditModeButton.Checked = true;
            ArrangeModeButton.Checked = false;
            foreach (ToolStripItem tsi in PaletteDropDownButton.DropDownItems)
                if (tsi is ToolStripMenuItem tsmi)
                    tsmi.Checked = false;
            PaletteDropDownButton.Checked = false;

            CursorMode = ArrangerViewerCursorMode.EditElements;
            RenderPanel.Invalidate();
        }

        private void PaletteMenuItem_Click(object sender, EventArgs e)
        {
            CancelSelection();

            // Uncheck all PaletteDropDownButton items
            foreach (ToolStripItem tsi in PaletteDropDownButton.DropDownItems)
                if (tsi is ToolStripMenuItem tsmi)
                    tsmi.Checked = false;

            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            clickedItem.Checked = true;
            PaletteDropDownButton.Checked = true;

            ArrangeModeButton.Checked = false;
            EditModeButton.Checked = false;

            CursorMode = ArrangerViewerCursorMode.ApplyPalette;
            RenderPanel.Invalidate();
        }

        private void AddPaletteReferenceItem_Click(object sender, EventArgs e)
        {

        }

        private void GraphicsViewerChild_GotFocus(object sender, EventArgs e)
        {
            EditArrangerChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ShowGridlinesButton_Click(object sender, EventArgs e)
        {
            RenderPanel.Invalidate();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            ReloadContent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveContent();
        }

        private void JumpButton_Click(object sender, EventArgs e)
        {
            ShowJumpDialog();
        }

        private void ArrangerPropertiesButton_Click(object sender, EventArgs e)
        {
            ScatteredArrangerPropertiesForm sapf = new ScatteredArrangerPropertiesForm();
            sapf.SetDefaults(false, DisplayArranger.Name, ContentSourceKey, DisplayArranger.ElementPixelSize, DisplayArranger.ArrangerElementSize, DisplayArranger.Layout);

            if (DialogResult.OK == sapf.ShowDialog()) // Modify arranger properties
            {
                Size newArrangerSize = sapf.ArrangerSize;
                string newArrangerName = sapf.ArrangerName;

                if (newArrangerSize.Width < DisplayArranger.ArrangerElementSize.Width || newArrangerSize.Height < DisplayArranger.ArrangerElementSize.Height)
                    if (DialogResult.Cancel == MessageBox.Show("Arranger elements will be lost due to a reduction in size. Continue?", "Resize Arranger", MessageBoxButtons.OKCancel))
                        return;

                DisplayArranger.ResizeScatteredArranger(newArrangerSize.Width, newArrangerSize.Height);

                if (newArrangerName != DisplayArranger.Name) // TODO: Rename arranger, requires refactoring to access ProjectTreeForm...
                {

                }

                ReloadArranger();
            }
        }
        #endregion User Interface Events

        private void BuildPaletteButtonMenu()
        {
            PaletteDropDownButton.DropDownItems.Clear();

            HashSet<string> paletteKeys = DisplayArranger.GetPaletteKeySet();
            paletteKeys.Remove("Default"); // Handle default separately to place it as the first option

            ToolStripMenuItem defaultPaletteItem = new ToolStripMenuItem("Default");
            defaultPaletteItem.Tag = "Default";
            defaultPaletteItem.Click += new System.EventHandler(PaletteMenuItem_Click);
            PaletteDropDownButton.DropDownItems.Add(defaultPaletteItem);

            foreach (string key in paletteKeys)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem(key);
                tsmi.Tag = key;
                tsmi.Click += new System.EventHandler(PaletteMenuItem_Click);
                PaletteDropDownButton.DropDownItems.Add(tsmi);
            }

            //PaletteDropDownButton.DropDownItems.Add(new ToolStripSeparator());

            //ToolStripMenuItem addPaletteReferenceItem = new ToolStripMenuItem("Add new Palette reference");
            //addPaletteReferenceItem.Click += AddPaletteReferenceItem_Click;
            //PaletteDropDownButton.DropDownItems.Add(addPaletteReferenceItem);
        }

        public string GetSelectedPaletteKey()
        {
            foreach (ToolStripItem tsi in PaletteDropDownButton.DropDownItems)
            {
                if (tsi is ToolStripMenuItem tsmi)
                    if (tsmi.Checked)
                        return tsmi.Text;
            }

            return "Default";
        }

        public void ClearEditArranger()
        {
            EditArranger = null;
            EditArrangerChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetZoom(int ZoomLevel)
        {
            Zoom = ZoomLevel;
        }

        private void CancelSelection()
        {
            selectionData.ClearSelection();
            ViewSelectionRect = new Rectangle(0, 0, 0, 0);
        }

        private void UpdateAddressLabel()
        {
            if (DisplayArranger.Mode != ArrangerMode.SequentialArranger)
                throw new InvalidOperationException();

            FileBitAddress address = DisplayArranger.GetInitialSequentialFileAddress();
            string sizestring = DisplayArranger.FileSize.ToString("X");

            int maxdigit = sizestring.Length;
            if (maxdigit % 2 == 1)
                maxdigit++; // Pad out a zero

            if (address.BitOffset == 0) // Byte-aligned offset
                offsetLabel.Text = String.Format("{0:X" + maxdigit.ToString() + "} / {1:X" + maxdigit.ToString() + "}", address.FileOffset, DisplayArranger.FileSize);
            else // Print label with bit display
                offsetLabel.Text = String.Format("{0:X" + maxdigit.ToString() + "}.{1} / {2:X" + maxdigit.ToString() + "}", address.FileOffset, address.BitOffset, DisplayArranger.FileSize);
        }

        private void ShowJumpDialog()
        {
            JumpToAddressForm jtaf = new JumpToAddressForm();
            DialogResult dr = jtaf.ShowDialog();

            if (dr == DialogResult.OK)
            {
                long address = jtaf.Address;
                DisplayArranger.Move(address * 8);
                UpdateAddressLabel();
            }
            ReloadArranger();
        }
    }
}
