using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

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
            pec.Show(dockPanel, DockState.DockLeft);
            LoadCodecs(CodecDirectoryPath);
            LoadPalettes(PaletteDirectoryPath);
            LoadCursors();

            pef = new PixelEditorForm();
            pef.Show(dockPanel, DockState.DockRight);
        }

        public void RefreshTitle()
        {
            if (String.IsNullOrEmpty(ProjectFileName))
                this.Text = "TileShop " + Properties.Settings.Default.Version + " - No project loaded";
            else
                this.Text = "TileShop " + Properties.Settings.Default.Version + " - " + ProjectFileName;
        }

        public bool openExistingArranger(string arrangerName)
        {
            // Check if the arranger is already an opened Document
            foreach(Control c in dockPanel.Documents)
            {
                if (c.Text == arrangerName)
                    return false;
            }

            GraphicsViewerChild gv = new GraphicsViewerChild(arrangerName);
            gv.Show(dockPanel, DockState.Document);

            return true;
        }

        public void updateOffsetLabel(string offset)
        {
            fileOffsetLabel.Text = offset;
        }

        public void updateSelectionLabel(string text)
        {
            selectionLabel.Text = text;
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

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            string s = @"D:\Projects\ff2.sfc";

            if(!pec.AddFile(s, true))
                MessageBox.Show("Could not open file " + s);
        }

        private void blankArrangerToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void debugXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            ProjectFileName = "D:\\Projects\\ff2.xml";
            pec.LoadFromXml(ProjectFileName);

            /*GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(this, "Font");
            gv.WindowState = FormWindowState.Maximized;
            gv.SetZoom(6);
            gv.Show(dockPanel, DockState.Document);*/
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(ProjectFileName)) // First save, need a filename
            {

            }
            else
                pec.SaveToXml(ProjectFileName);
        }

        private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newPaletteToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void newScatteredArrangerToolStripMenuItem_Click(object sender, EventArgs e)
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

        public void editArrangerChanged(object sender, EventArgs e)
        {
            GraphicsViewerChild gv = (GraphicsViewerChild)sender;
            pef.SetEditArranger(gv.EditArranger);
        }

        public void paletteContentsModified(object sender, EventArgs e)
        {
            // Minor bug: Can sometimes reload arranger of some DockContents twice
            // Example: A floating GraphicsViewerChild window (with multiple docks?)
            foreach(DockPane dp in dockPanel.Panes)
            {
                foreach(DockContent dc in dp.Contents)
                {
                    if (dc.GetType() == typeof(GraphicsViewerChild))
                        ((GraphicsViewerChild)dc).ReloadArranger();
                    else if (dc.GetType() == typeof(PixelEditorForm))
                        ((PixelEditorForm)dc).ReloadArranger();
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            ofd.Title = "File Location";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(ofd.FileName) == ".xml") // Load an XML project
                {
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
                    pec.LoadFromXml(ofd.FileName);
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

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(pec.IsProjectModified)
            {
                DialogResult dr = MessageBox.Show("The project has been modified. Save?", "Save Project", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes && !String.IsNullOrEmpty(ProjectFileName)) // Project has a filename due to being previously saved or opened
                {
                    pec.SaveToXml(ProjectFileName);
                }
                if(dr == DialogResult.Yes && String.IsNullOrEmpty(ProjectFileName)) // Project has no filename because it has never been saved
                {
                    OpenFileDialog ofd = new OpenFileDialog();

                    ofd.AddExtension = true;
                    ofd.CheckFileExists = true;
                    ofd.CheckPathExists = true;
                    ofd.Multiselect = false;
                    ofd.Title = "File Location";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        pec.SaveToXml(ofd.FileName);

                    }
                }
                else if (dr == DialogResult.No)
                {

                }
                else if (dr == DialogResult.Cancel)
                    return;
            }

            ProjectFileName = "";
            pec.ClearAll();
        }
    }
}
