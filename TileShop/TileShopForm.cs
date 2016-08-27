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

        ProjectExplorerControl pec;

        public TileShopForm()
        {
            InitializeComponent();

            pec = new ProjectExplorerControl(this);
            pec.Show(dockPanel, DockState.DockLeft);
            LoadCodecs(CodecDirectoryPath);
            LoadPalettes(PaletteDirectoryPath);
        }

        private void newGraphicsProjectMenu_Click(object sender, EventArgs e)
        {
            //GraphicsViewerMdiChild gmc = new GraphicsViewerMdiChild(this);
            //gmc.Show(dockPanel, DockState.Document);
        }

        private void openFileMenu_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.AddExtension = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            ofd.Title = "File Location";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(ofd.FileName) == ".xml")
                {
                    // Clear all files/arrangers/palettes
                    FileManager.Instance.ClearAll();
                    pec.ClearAll();

                    // Add saving for modified viewers here
                    foreach(Control c in this.Controls)
                    {
                        if(c.GetType() == typeof(GraphicsViewerMdiChild))
                        {
                            c.Dispose();
                        }
                    }

                    // Read new XML file
                    pec.LoadFromXml(ofd.FileName);
                }
                else
                {
                    if (FileManager.Instance.LoadSequentialArrangerFromFilename(ofd.FileName))
                    {
                        GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(this, ofd.FileName);
                        pec.AddFile(ofd.FileName, true);
                        gv.Show(dockPanel, DockState.Document);
                    }
                    else
                    {
                        MessageBox.Show("Could not open file " + ofd.FileName);
                        return;
                    }
                }
            }
        }

        public bool openExistingArranger(string arrangerName)
        {
            // Check if the arranger is already an opened Document
            foreach(Control c in dockPanel.Documents)
            {
                if (c.Text == arrangerName)
                    return false;
            }

            GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(this, arrangerName);
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

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;

            if(!pec.AddFile("D:\\Projects\\ff2.sfc", true))
                MessageBox.Show("Could not open file " + "D:\\Projects\\ff2.sfc");
        }

        private void blankArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTileArrangerForm ntaf = new NewTileArrangerForm();

            if (ActiveMdiChild != null)
            {
                if (ActiveMdiChild.GetType() == typeof(GraphicsViewerMdiChild))
                {
                    GraphicsViewerMdiChild gv = (GraphicsViewerMdiChild)ActiveMdiChild;

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

            pec.LoadFromXml("D:\\Projects\\ff2test.xml");
            GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(this, "Font");
            gv.WindowState = FormWindowState.Maximized;
            gv.SetZoom(6);
            gv.Show(dockPanel, DockState.Document);
        }

        private void scatteredArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewScatteredArrangerForm nsaf = new NewScatteredArrangerForm();

            if (ActiveMdiChild != null)
            {
                if (ActiveMdiChild.GetType() == typeof(GraphicsViewerMdiChild))
                {
                    GraphicsViewerMdiChild gv = (GraphicsViewerMdiChild)ActiveMdiChild;
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
                FileManager.Instance.AddArranger(arr);

                GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(this, arr.Name);
                pec.AddArranger(arr, true);
                gv.WindowState = FormWindowState.Maximized;
                gv.SetZoom(6);
                gv.Show(dockPanel, DockState.Document);
            }
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pec.SaveToXml(@"D:\Projects\ff2test.xml");
        }
    }
}
