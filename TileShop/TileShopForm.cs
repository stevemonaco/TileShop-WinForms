using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using MoreLinq;
using WeifenLuo.WinFormsUI.Docking;
using TileShop.Core;
using TileShop.Plugins;

namespace TileShop
{
    public partial class TileShopForm : Form
    {
        string CodecDirectoryPath = @"D:\Projects\TileShop\codecs\";
        string PaletteDirectoryPath = @"D:\Projects\TileShop\pal\";
        string PluginDirectoryPath = @"D:\Projects\TileShop\plugins\";

        public string ProjectFileName
        {
            get => _projectFileName;
            private set
            {
                _projectFileName = value;
                if (ProjectFileName == "")
                    Text = "TileShop " + Properties.Settings.Default.Version + " - No project loaded";
                else
                    Text = "TileShop " + Properties.Settings.Default.Version + " - " + ProjectFileName;
            }
        }
        private string _projectFileName;

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
            //LoadPlugins();

            ProjectFileName = "";

            ptf = new ProjectTreeForm(this);
            pef = new PixelEditorForm();
            ResourceManager.Instance.ResourceAdded += ptf.OnResourceAdded;

            pef.Show(DockPanel, DockState.DockRight);
            ptf.Show(DockPanel, DockState.DockLeft); // Showing this last makes the ProjectExplorerControl focused upon launch
        }

        public bool OpenExistingArranger(string arrangerName)
        {
            if(DockPanel.Documents.Any(c => (c as DockContent).Text == arrangerName))
                return false;

            ArrangerViewerForm avf = new ArrangerViewerForm(arrangerName);
            avf.Show(DockPanel, DockState.Document);

            return true;
        }

        public void UpdateOffsetLabel(string offset) => FileOffsetLabel.Text = offset;

        public void UpdateSelectionLabel(string text) => SelectionLabel.Text = text;

        #region Startup loading functions
        private void LoadCodecs(string path)
        {
            var codecs = Directory.GetFiles(path).Where(s => Path.GetExtension(s) == ".xml");
            codecs.ForEach((x) => ResourceManager.LoadFormat(x));
        }

        /// <summary>
        /// Load default palettes from the palettes directory
        /// </summary>
        /// <param name="path">Path to the palettes directory</param>
        private void LoadPalettes(string path)
        {
            var palettes = Directory.GetFiles(path).Where(s => Path.GetExtension(s) == ".pal");
            palettes.ForEach((x) => ResourceManager.LoadPalette(x, Path.GetFileNameWithoutExtension(x)));
        }

        private void LoadCursors()
        {
            Cursor PencilCursor = CustomCursor.LoadCursorFromBitmap(Properties.Resources.PencilCursor, new Point(0, 15));
            ResourceManager.AddCursor("PencilCursor", PencilCursor);

            Cursor PickerCursor = CustomCursor.LoadCursorFromBitmap(Properties.Resources.PickerCursor, new Point(2, 19));
            ResourceManager.AddCursor("PickerCursor", PickerCursor);
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
        #endregion

        #region UI Events
        private void RunFileParserPlugin_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem viewItem = sender as ToolStripMenuItem;
            string pluginName = viewItem.Tag as string;

            foreach (Lazy<IFileParserContract, IFileParserData> plugin in pm.ParserPlugins)
            {
                if (plugin.Metadata.Name != pluginName)
                    continue;

                if (!plugin.Value.DisplayPluginInterface()) // If no arrangers/palettes to add
                    break;

                var resources = plugin.Value.RetrieveResourceMap();

                if (resources is null)
                {
                    MessageBox.Show($"Plugin '{pluginName}' returned no resources to add");
                    return;
                }

                foreach (string key in resources.Keys)
                {
                    // TODO: Add checks to ensure resources were added
                    ProjectResourceBase res = resources[key];
                    if (res is DataFile df)
                        ResourceManager.AddResource(key, res);
                    else
                        ResourceManager.AddResource(key, res.Clone());
                }
            }
        }

        private void ViewFileParserPlugin_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem viewItem = sender as ToolStripMenuItem;
            string pluginName = viewItem.Tag as string;

            var plugin = pm.ParserPlugins.FirstOrDefault(x => x.Metadata.Name == pluginName);

            if (plugin != null)
            {
                MessageBox.Show($"Plugin Information\n" +
                    $"Name: {plugin.Metadata.Name}\n" +
                    $"\nAuthor: {plugin.Metadata.Author}\n" +
                    $"\nVersion:  {plugin.Metadata.Version}\n" +
                    $"\nDescription: {plugin.Metadata.Description}\n");
            }
        }

        private void DebugXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            ProjectFileName = "D:\\Projects\\ff2newxml.xml";
            using (FileStream fs = File.OpenRead(ProjectFileName))
            {
                ResourceManager.LoadProject(fs, Path.GetDirectoryName(ProjectFileName));
            }
        }

        private void SaveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Test equiv below
            // DockPanel.Panes.Select(x => x.Contents).OfType<EditorDockContent>().ForEach(x => x.SaveContent());
            
            // Save all EditorDockContents
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if(dc is EditorDockContent edc)
                        edc.SaveContent();
                }
            }

            if (String.IsNullOrWhiteSpace(ProjectFileName)) // First save, need a filename
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
                    using (FileStream fs = File.Open(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        ResourceManager.SaveProject(fs);
                        ProjectFileName = sfd.FileName;
                    }
                }
            }
            else
            {
                using (FileStream fs = File.Open(ProjectFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    ResourceManager.SaveProject(fs);
                }
            }
        }

        private void SaveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save all EditorDockContents
            foreach (DockPane dp in DockPanel.Panes)
            {
                foreach (DockContent dc in dp.Contents)
                {
                    if (dc is EditorDockContent edc)
                        edc.SaveContent();
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
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = File.Open(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    ResourceManager.SaveProject(fs);
                    ProjectFileName = sfd.FileName;
                }
            }
        }

        private void NewPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewPaletteForm npf = new NewPaletteForm();
            npf.AddFileNames(ptf.GetFileNameList());

            if(DialogResult.OK == npf.ShowDialog())
            {
                Palette pal = new Palette(npf.PaletteName);
                pal.LazyLoadPalette(npf.FileName, new FileBitAddress(npf.FileOffset, 0), npf.ColorModel, true, npf.Entries); // TODO: Refactor for new FileBitAddress
                ResourceManager.AddResource(pal.Name, pal);
            }
        }

        private void NewScatteredArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScatteredArrangerPropertiesForm sapf = new ScatteredArrangerPropertiesForm();

            if (ActiveMdiChild is ArrangerViewerForm avf) // Initialize with defaults from the active MDI window
            {
                Size elementSize = avf.DisplayArranger.ElementPixelSize;
                sapf.SetDefaults(true, "", "", new Size(elementSize.Width, elementSize.Height), new Size(16, 8), ArrangerLayout.TiledArranger);
            }

            if (DialogResult.OK == sapf.ShowDialog())
            {
                Size arrSize = sapf.ArrangerSize;
                Size elementSize = sapf.ElementPixelSize;

                var arr = new ScatteredArranger(sapf.ArrangerLayout, arrSize.Width, arrSize.Height, elementSize.Width, elementSize.Height);
                arr.Rename(sapf.ArrangerName);
                ResourceManager.AddResource(arr.Name, arr);
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

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

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
                ProjectFileName = ofd.FileName;
                using (FileStream fs = File.OpenRead(ofd.FileName))
                {
                    ResourceManager.LoadProject(File.OpenRead(ofd.FileName), Path.GetDirectoryName(ofd.FileName));
                }
            }
            else if (!ptf.AddDataFile(ofd.FileName, "", true)) // Add a new file to the project
            {
                MessageBox.Show("Could not open file " + ofd.FileName);
                return;
            }
        }

        private void CloseProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*if(pec.IsProjectModified)
            {
                DialogResult dr = MessageBox.Show("The project has been modified. Save?", "Save Project", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes && !String.IsNullOrWhiteSpace(ProjectFileName)) // Project has a filename due to being previously saved or opened
                {
                    pec.SaveProject(ProjectFileName);
                }
                if(dr == DialogResult.Yes && String.IsNullOrWhiteSpace(ProjectFileName)) // Project has no filename because it has never been saved
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
        #endregion

        #region EditorDockContent Events
        public void EditArrangerChanged(object sender, EventArgs e)
        {
            if (pef.IsClosed)
            {
                pef = new PixelEditorForm();
                pef.Show(DockPanel, DockState.DockRight);
            }

            if (!pef.Visible)
                pef.Show();

            pef.ContentModified += PixelContentModified;

            ArrangerViewerForm avf = (ArrangerViewerForm)sender;
            pef.SetEditArranger(avf.EditArranger);
        }

        /// <summary>
        /// Invoked when a PixelEditorForm has made changes that must be propagated to sibling subeditors
        /// Only ArrangerViewerForms need to be refreshed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PixelContentModified(object sender, EventArgs e)
        {
            var arrangerViewers = DockPanel.Panes.SelectMany(x => x.Contents).OfType<ArrangerViewerForm>();
            arrangerViewers.ForEach(x => { x.RefreshContent(); });
        }

        /// <summary>
        /// Invoked when a PaletteEditorForm has made changes that must be propagated to sibling subeditors
        /// Each subeditor type must be refreshed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PaletteContentModified(object sender, EventArgs e)
        {
            // Minor bug: Can sometimes reload arranger of some DockContents twice
            // Example: A floating GraphicsViewerChild window (with multiple docks?)

            var palEditors = DockPanel.Panes.SelectMany(x => x.Contents).OfType<EditorDockContent>().Where(x => x != sender);
            palEditors.ForEach(x => x.RefreshContent());
        }

        /// <summary>
        /// Invoked when a GraphicsViewerForm has made changes that must be propagated to sibling subeditors
        /// </summary>
        /// <param name="sender">Editor which invoked the event</param>
        /// <param name="e"></param>
        public void ViewerContentModified(object sender, EventArgs e)
        {
            // Currently no need for the GraphicsViewerForm to make changes to propagate
            return;
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

            var editorList = DockPanel.Panes.SelectMany(x => x.Contents).OfType<EditorDockContent>().Where(x => x != sender);
            editorList.ForEach(x => x.RefreshContent());
        }
        #endregion

        private void CloseEditors()
        {
            var closeList = DockPanel.Panes.SelectMany(x => x.Contents).OfType<EditorDockContent>();
            closeList.ForEach(x => x.Close());
        }

        public IEnumerable<EditorDockContent> GetActiveEditors()
        {
            var editorList = DockPanel.Panes.SelectMany(x => x.Contents).OfType<EditorDockContent>().Where(y => !(y is PixelEditorForm));
            return editorList;
        }
    }
}
