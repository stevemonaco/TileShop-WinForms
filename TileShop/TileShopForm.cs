using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Generic;
using TileShop.Plugins;

namespace TileShop
{
    public partial class TileShopForm : Form
    {
        string CodecDirectoryPath = "D:\\Projects\\TileShop\\codecs\\";
        string PaletteDirectoryPath = "D:\\Projects\\TileShop\\pal\\";
        string PluginDirectoryPath = "D:\\Projects\\TileShop\\plugins";
        //string PluginDirectoryPath = @"D:\VS Projects\TileShop\TIMParserPlugin\bin\Debug\";

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

        ProjectTreeForm ptf;
        PixelEditorForm pef;
        PluginManager pm = new PluginManager();

        public TileShopForm()
        {
            InitializeComponent();

            MainToolStrip.Visible = false;

            LoadCodecs(CodecDirectoryPath);
            LoadPalettes(PaletteDirectoryPath);
            LoadCursors();
            LoadPlugins();

            this.Text = "TileShop " + Properties.Settings.Default.Version + " - No project loaded";

            ptf = new ProjectTreeForm(this);
            pef = new PixelEditorForm();

            pef.Show(DockPanel, DockState.DockRight);
            ptf.Show(DockPanel, DockState.DockLeft); // Showing this last makes the ProjectExplorerControl focused upon launch
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

            GraphicsViewerForm gv = new GraphicsViewerForm(arrangerName);
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

        /// <summary>
        /// Load default palettes from the palettes directory
        /// </summary>
        /// <param name="path">Path to the palettes directory</param>
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

        private void LoadPlugins()
        {
            pm.LoadPlugins(PluginDirectoryPath);

            // Create menu options for each loaded file parser plugin
            foreach(Lazy<IFileParserContract, IFileParserData> plugin in pm.ParserPlugins)
            {
                // TODO: Error checking for valid variable string characters in name [A-Z][a-z][0-9]
                string strippedName = plugin.Metadata.Name.Replace(" ", "");

                ToolStripMenuItem nameItem = new ToolStripMenuItem(plugin.Metadata.Name)
                {
                    Name = strippedName + "MenuItem",
                    Tag = plugin.Metadata.Name,
                    Visible = true
                };
                pluginsToolStripMenuItem.DropDownItems.Add(nameItem);

                ToolStripMenuItem runItem = new ToolStripMenuItem("Run")
                {
                    Name = strippedName + "RunPluginMenuItem",
                    Tag = plugin.Metadata.Name
                };
                runItem.Click += RunFileParserPlugin_Click;
                runItem.Visible = true;
                nameItem.DropDownItems.Add(runItem);

                ToolStripMenuItem viewItem = new ToolStripMenuItem("View Plugin Info")
                {
                    Name = strippedName + "ViewInfoMenuItem",
                    Tag = plugin.Metadata.Name,
                    Visible = true
                };
                viewItem.Click += ViewFileParserPlugin_Click;
                nameItem.DropDownItems.Add(viewItem);
            }
        }

        private void RunFileParserPlugin_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem viewItem = (ToolStripMenuItem)sender;
            string pluginName = (string)viewItem.Tag;

            foreach (Lazy<IFileParserContract, IFileParserData> plugin in pm.ParserPlugins)
            {
                if (plugin.Metadata.Name != pluginName)
                    continue;

                if (!plugin.Value.DisplayPluginInterface()) // If no arrangers/palettes to add
                    break;

                List<Arranger> arrangers = plugin.Value.RetrieveArrangers();
                List<Palette> palettes = plugin.Value.RetrievePalettes();
                List<DataFile> dataFiles = plugin.Value.RetrieveDataFiles();

                if (arrangers == null)
                {
                    MessageBox.Show("Plugin returned null for RetrieveArrangers");
                    return;
                }
                if (palettes == null)
                {
                    MessageBox.Show("Plugin returned null for RetrievePalettes");
                    return;
                }

                foreach (DataFile df in dataFiles)
                    ptf.AddDataFile(df, pluginName);

                foreach (Palette pal in palettes)
                    ptf.AddPalette(pal.Clone(), pluginName);

                foreach (Arranger arr in arrangers)
                    ptf.AddArranger(arr.Clone(), pluginName);
            }
        }

        private void ViewFileParserPlugin_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem viewItem = (ToolStripMenuItem)sender;
            string pluginName = (string)viewItem.Tag;

            foreach (Lazy<IFileParserContract, IFileParserData> plugin in pm.ParserPlugins)
            {
                if(plugin.Metadata.Name == pluginName)
                {
                    MessageBox.Show("Name: " + plugin.Metadata.Name +
                        "\nAuthor: " + plugin.Metadata.Author +
                        "\nVersion: " + plugin.Metadata.Version +
                        "\nDescription: " + plugin.Metadata.Description,
                        "Plugin Information");
                    break;
                }
            }
        }

        private void BlankArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTileArrangerForm ntaf = new NewTileArrangerForm();

            if (ActiveMdiChild is GraphicsViewerForm gv)
            {
                Size ElementSize = gv.DisplayArranger.ElementPixelSize;
                ntaf.SetDefaults(ElementSize.Width, ElementSize.Height, 16, 8);

                if (gv.DisplayArranger.Mode == ArrangerMode.SequentialArranger)
                {
                    GraphicsFormat fmt = FileManager.Instance.GetGraphicsFormat(gv.DisplayArranger.GetSequentialGraphicsFormat());
                    ntaf.SetFormat(fmt.Name);
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

            ProjectFileName = "D:\\Projects\\ff2newxml.xml";
            GameDescriptor gd = new GameDescriptor();
            gd.LoadProject(ptf, ProjectFileName);
        }

        private void SaveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save all EditorDockContents
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if(dc is EditorDockContent)
                        ((EditorDockContent)dc).SaveContent();
                }
            }

            if (String.IsNullOrEmpty(ProjectFileName)) // First save, need a filename
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    AddExtension = true,
                    DefaultExt = ".xml",
                    ValidateNames = true,
                    Filter = "Xml Project File|*.xml",
                    Title = "Save Project"
                };

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ProjectFileName = sfd.FileName;
                    ptf.SaveProject(ProjectFileName);
                }
            }
            else
                ptf.SaveProject(ProjectFileName);
        }

        private void SaveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save all EditorDockContents
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc is EditorDockContent)
                        ((EditorDockContent)dc).SaveContent();
                }
            }

            SaveFileDialog sfd = new SaveFileDialog()
            {
                AddExtension = true,
                DefaultExt = ".xml",
                ValidateNames = true,
                Filter = "Xml Project File|*.xml",
                Title = "Save Project"
            };
            if (sfd.
                ShowDialog() == DialogResult.OK)
            {
                ProjectFileName = sfd.FileName;
                ptf.SaveProject(ProjectFileName);
            }
        }

        private void NewPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPaletteForm npf = new NewPaletteForm();
            npf.AddFileNames(ptf.GetFileNameList());

            if(DialogResult.OK == npf.ShowDialog())
            {
                Palette pal = new Palette(npf.PaletteName);
                pal.LoadPalette(npf.FileName, new FileBitAddress(npf.FileOffset, 0), npf.ColorFormat, true, npf.Entries); // TODO: Refactor for new FileBitAddress
                ptf.AddPalette(pal, "");
            }
        }

        private void NewScatteredArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScatteredArrangerPropertiesForm sapf = new ScatteredArrangerPropertiesForm();

            if (ActiveMdiChild is GraphicsViewerForm gv) // Initialize with defaults from the active MDI window
            {
                Size ElementSize = gv.DisplayArranger.ElementPixelSize;
                sapf.SetDefaults(true, "", "", new Size(ElementSize.Width, ElementSize.Height), new Size(16, 8), ArrangerLayout.TiledArranger);
            }

            if (DialogResult.OK == sapf.ShowDialog())
            {
                Size ArrSize = sapf.ArrangerSize;
                Size ElementSize = sapf.ElementPixelSize;

                Arranger arr = Arranger.NewScatteredArranger(sapf.ArrangerLayout, ArrSize.Width, ArrSize.Height, ElementSize.Width, ElementSize.Height);
                arr.Name = sapf.ArrangerName;
                ptf.AddArranger(arr, "", true);
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

            GraphicsViewerForm gv = (GraphicsViewerForm)sender;
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
                    if (dc is EditorDockContent)
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
                    if (dc is EditorDockContent && dc != sender)
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
                    if (dc is EditorDockContent && dc != sender)
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
                    GameDescriptor gd = new GameDescriptor();
                    gd.LoadProject(ptf, ofd.FileName);
                    ProjectFileName = ofd.FileName;
                }
                else
                {
                    DataFile df = new DataFile(Path.GetFileNameWithoutExtension(ofd.FileName));
                    if (!ptf.AddDataFile(df, "", true))
                    {
                        MessageBox.Show("Could not open file " + ofd.FileName);
                        return;
                    }
                }
            }
        }

        private void CloseProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*if(pec.IsProjectModified)
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
            }*/

            CloseEditors();

            ProjectFileName = "";
            ptf.CloseProject();
            LoadPalettes(PaletteDirectoryPath);
        }

        private void CloseEditors()
        {
            List<EditorDockContent> CloseList = new List<EditorDockContent>();

            // Find all EditorDockContents within all Panes and populate the CloseList
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc is EditorDockContent)
                        CloseList.Add((EditorDockContent)dc);
                }
            }

            foreach (EditorDockContent edc in CloseList)
                edc.Close();
        }

        public List<EditorDockContent> GetActiveEditors()
        {
            List<EditorDockContent> editorList = new List<EditorDockContent>();

            // Find all EditorDockContents within all Panes and populate the CloseList
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc is EditorDockContent && !(dc is PixelEditorForm))
                        editorList.Add((EditorDockContent)dc);
                }
            }

            return editorList;
        }
    }
}
