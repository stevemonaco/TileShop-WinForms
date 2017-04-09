using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Generic;

namespace TileShop
{
    public partial class TileShopForm : Form
    {
        string CodecDirectoryPath = "D:\\Projects\\TileShop\\codecs\\";
        string PaletteDirectoryPath = "D:\\Projects\\TileShop\\pal\\";

        public string ProjectFileName
        {
            get { return projectFileName; }
            private set
            {
                projectFileName = value;
                RefreshTitle();
            }
        }
        private string projectFileName = "";

        ProjectExplorerControl pec;
        PixelEditorForm pef;

        public TileShopForm()
        {
            InitializeComponent();

            this.Text = "TileShop " + Properties.Settings.Default.Version + " - No project loaded";

            pec = new ProjectExplorerControl(this);
            pec.Show(DockPanel, DockState.DockLeft);
            LoadCodecs(CodecDirectoryPath);
            LoadPalettes(PaletteDirectoryPath);
            LoadCursors();

            pef = new PixelEditorForm();
            pef.Show(DockPanel, DockState.DockRight);
        }

        public void RefreshTitle()
        {
            if (String.IsNullOrEmpty(ProjectFileName))
                this.Text = "TileShop " + Properties.Settings.Default.Version + " - No project loaded";
            else
                this.Text = "TileShop " + Properties.Settings.Default.Version + " - " + ProjectFileName;
        }

        public bool OpenExistingArranger(string arrangerName)
        {
            // Check if the arranger is already an opened Document
            foreach(Control c in DockPanel.Documents)
            {
                if (c.Text == arrangerName)
                    return false;
            }

            GraphicsViewerChild gv = new GraphicsViewerChild(arrangerName);
            gv.Show(DockPanel, DockState.Document);

            return true;
        }

        public void UpdateOffsetLabel(string offset)
        {
            FileOffsetLabel.Text = offset;
        }

        public void UpdateSelectionLabel(string text)
        {
            SelectionLabel.Text = text;
        }

        private void LoadCodecs(string path)
        {
            string[] filenames = Directory.GetFiles(path);

            foreach(string s in filenames)
            {
                if(Path.GetExtension(s) == ".xml")
                    FileManager.Instance.LoadFormat(s);
            }
        }

        private void LoadPalettes(string path)
        {
            string[] filenames = Directory.GetFiles(path);

            foreach (string s in filenames)
            {
                if (Path.GetExtension(s) == ".pal")
                    FileManager.Instance.LoadPalette(s, Path.GetFileNameWithoutExtension(s));
            }
        }

        private void LoadCursors()
        {
            Cursor PencilCursor = CustomCursor.LoadCursorFromBitmap(Properties.Resources.PencilCursor, new Point(0, 15));
            FileManager.Instance.AddCursor("PencilCursor", PencilCursor);

            Cursor PickerCursor = CustomCursor.LoadCursorFromBitmap(Properties.Resources.PickerCursor, new Point(2, 19));
            FileManager.Instance.AddCursor("PickerCursor", PickerCursor);
        }

        private void DebugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            string s = @"D:\Projects\ff2.sfc";

            if(!pec.AddFile(s, true))
                MessageBox.Show("Could not open file " + s);
        }

        private void BlankArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTileArrangerForm ntaf = new NewTileArrangerForm();

            if (ActiveMdiChild != null)
            {
                if (ActiveMdiChild.GetType() == typeof(GraphicsViewerChild))
                {
                    GraphicsViewerChild gv = (GraphicsViewerChild)ActiveMdiChild;

                    Size ElementSize = gv.arranger.ElementPixelSize;
                    ntaf.SetDefaults(ElementSize.Width, ElementSize.Height, 16, 8);

                    if (gv.arranger.Mode == ArrangerMode.SequentialArranger)
                    {
                        GraphicsFormat fmt = FileManager.Instance.GetGraphicsFormat(gv.arranger.GetSequentialGraphicsFormat());
                        ntaf.SetFormat(fmt.Name);
                    }
                }
            }

            if(DialogResult.OK == ntaf.ShowDialog())
            {
                //GraphicsViewerMdiChild gmc = new GraphicsViewerMdiChild(this, ArrangerMode.MemoryArranger);
                //GraphicsFormat 
                //gmc.LoadTileArranger(ntaf.GetFormatName(), ntaf.GetArrangerSize(), null);
            }
        }

        private void DebugXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            ProjectFileName = "D:\\Projects\\ff2.xml";
            pec.LoadProject(ProjectFileName);

            /*GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(this, "Font");
            gv.WindowState = FormWindowState.Maximized;
            gv.SetZoom(6);
            gv.Show(dockPanel, DockState.Document);*/
        }

        private void SaveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(ProjectFileName)) // First save, need a filename
            {

            }
            else
                pec.SaveProject(ProjectFileName);
        }

        private void SaveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void NewPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPaletteForm npf = new NewPaletteForm();
            npf.AddFileNames(pec.GetFileNameList());

            if(DialogResult.OK == npf.ShowDialog())
            {
                Palette pal = new Palette(npf.PaletteName);
                pal.LoadPalette(npf.FileName, npf.FileOffset, npf.ColorFormat, npf.Entries);
                pec.AddPalette(pal);
            }
        }

        private void NewScatteredArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewScatteredArrangerForm nsaf = new NewScatteredArrangerForm();

            if (ActiveMdiChild != null)
            {
                if (ActiveMdiChild.GetType() == typeof(GraphicsViewerChild))
                {
                    GraphicsViewerChild gv = (GraphicsViewerChild)ActiveMdiChild;
                    Size ElementSize = gv.arranger.ElementPixelSize;
                    nsaf.SetDefaults(ElementSize.Width, ElementSize.Height, 16, 8);
                }
            }

            if (DialogResult.OK == nsaf.ShowDialog())
            {
                Size ArrSize = nsaf.GetArrangerSize();
                Size TileSize = nsaf.GetTileSize();

                Arranger arr = Arranger.NewScatteredArranger(ArrSize.Width, ArrSize.Height, TileSize.Width, TileSize.Height);
                arr.Name = nsaf.GetArrangerName();
                pec.AddArranger(arr, true);
            }
        }

        public void EditArrangerChanged(object sender, EventArgs e)
        {
            if (pef.IsClosed)
            {
                pef = new PixelEditorForm();
                pef.Show(DockPanel, DockState.DockRight);
            }

            if (!pef.Visible)
                pef.Show();

            GraphicsViewerChild gv = (GraphicsViewerChild)sender;
            pef.SetEditArranger(gv.EditArranger);
        }

        /// <summary>
        /// Called in the event that a palette subeditor has made changes that must be propagated to sibling subeditors
        /// Each subeditor must be refreshed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PaletteContentsModified(object sender, EventArgs e)
        {
            // Minor bug: Can sometimes reload arranger of some DockContents twice
            // Example: A floating GraphicsViewerChild window (with multiple docks?)
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc.GetType().IsSubclassOf(typeof(EditorDockContent)))
                        ((EditorDockContent)dc).RefreshContent();
                }
            }
        }

        /// <summary>
        /// Called upon an editor having its content modified
        /// </summary>
        /// <param name="sender">Editor which invoked the event</param>
        /// <param name="e"></param>
        public void ContentModified(object sender, EventArgs e)
        {
            // Minor bug: Can sometimes refresh some DockContents twice
            // Example: A floating GraphicsViewerChild window (with multiple docks?)
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc.GetType().IsSubclassOf(typeof(EditorDockContent)) && dc != sender)
                        ((EditorDockContent)dc).RefreshContent();
                }
            }
        }

        /// <summary>
        /// Called upon an editor having its content saved
        /// </summary>
        /// <param name="sender">Editor which invoked the event</param>
        /// <param name="e"></param>
        public void ContentSaved(object sender, EventArgs e)
        {
            // Minor bug: Can sometimes refresh some DockContents twice
            // Example: A floating GraphicsViewerChild window (with multiple docks?)
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc.GetType().IsSubclassOf(typeof(EditorDockContent)) && dc != sender)
                        ((EditorDockContent)dc).ReloadContent();
                }
            }
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
                Title = "File Location"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(ofd.FileName) == ".xml") // Load an XML project
                {
                    // TODO: Handle opening a new XML project while one is already loaded

                    // Clear all files/arrangers/palettes
                    /*FileManager.Instance.ClearAll();
                    pec.ClearAll();

                    // Add saving for modified viewers here
                    foreach (Control c in this.Controls)
                    {
                        if (c.GetType() == typeof(GraphicsViewerChild))
                        {
                            c.Dispose();
                        }
                    }*/

                    // Load new XML project file
                    pec.LoadProject(ofd.FileName);
                    ProjectFileName = ofd.FileName;
                }
                else
                {
                    if (!pec.AddFile(ofd.FileName, true))
                    {
                        MessageBox.Show("Could not open file " + ofd.FileName);
                        return;
                    }
                }
            }
        }

        private void CloseProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(pec.IsProjectModified)
            {
                DialogResult dr = MessageBox.Show("The project has been modified. Save?", "Save Project", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes && !String.IsNullOrEmpty(ProjectFileName)) // Project has a filename due to being previously saved or opened
                {
                    pec.SaveProject(ProjectFileName);
                }
                if(dr == DialogResult.Yes && String.IsNullOrEmpty(ProjectFileName)) // Project has no filename because it has never been saved
                {
                    OpenFileDialog ofd = new OpenFileDialog()
                    {
                        AddExtension = true,
                        CheckFileExists = true,
                        CheckPathExists = true,
                        Multiselect = false,
                        Title = "File Location"
                    };

                    if (ofd.ShowDialog() == DialogResult.OK)
                        pec.SaveProject(ofd.FileName);
                    else // Cancelled
                        return;
                }
                else if (dr == DialogResult.No)
                {

                }
                else if (dr == DialogResult.Cancel)
                    return;
            }

            List<EditorDockContent> CloseList = new List<EditorDockContent>();

            // Find all EditorDockContents within all Panes and populate the CloseList
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc.GetType().IsSubclassOf(typeof(EditorDockContent)))
                        CloseList.Add((EditorDockContent)dc);
                }
            }

            foreach (EditorDockContent edc in CloseList)
                edc.Close();

            ProjectFileName = "";
            pec.CloseProject();
            LoadPalettes(PaletteDirectoryPath);
        }
    }
}
