using System;
using System.Windows.Forms;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

namespace TileShop
{
    public partial class TileShopForm : Form
    {
        private string CodecDirectoryPath = "D:\\Projects\\TileShop\\codecs\\";
        private string PaletteDirectoryPath = "D:\\Projects\\TileShop\\pal\\";

        private ProjectExplorerControl gdc = new ProjectExplorerControl();

        public TileShopForm()
        {
            InitializeComponent();
            gdc.Show(dockPanel, DockState.DockLeft);
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
                GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild(this, ArrangerMode.SequentialArranger);

                if(gv.OpenFile(ofd.FileName) == false)
                {
                    gv.Close();
                    MessageBox.Show("Could not open file " + ofd.FileName);
                    return;
                }

                gdc.AddFile(ofd.FileName);
                gv.Show(dockPanel, DockState.Document);
            }
        }

        public void updateOffsetLabel(long offset)
        {
            fileOffsetLabel.Text = offset.ToString("X");
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
            GraphicsViewerMdiChild gmc = new GraphicsViewerMdiChild(this, ArrangerMode.SequentialArranger);

            if (gmc.OpenFile("D:\\Projects\\ff2.sfc") == false)
            {
                gmc.Close();
                MessageBox.Show("Could not open file " + "D:\\Projects\\ff2.sfc");
                return;
            }

            gdc.AddFile("D:\\Projects\\ff2.sfc");
            gmc.WindowState = FormWindowState.Maximized;
            gmc.zoom = 6;
            gmc.Show(dockPanel, DockState.Document);
        }

        private void blankArrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTileArrangerForm ntaf = new NewTileArrangerForm();

            if (ActiveMdiChild != null)
            {
                if (ActiveMdiChild.GetType() == typeof(GraphicsViewerMdiChild))
                {
                    GraphicsViewerMdiChild gmc = (GraphicsViewerMdiChild)ActiveMdiChild;
                    GraphicsFormat fmt = gmc.graphicsFormat;
                    ntaf.SetFormat(fmt.Name);
                    ntaf.SetDefaults(fmt.Width, fmt.Height, 16, 8);
                }
            }

            if(DialogResult.OK == ntaf.ShowDialog())
            {
                GraphicsViewerMdiChild gmc = new GraphicsViewerMdiChild(this, ArrangerMode.MemoryArranger);
                //GraphicsFormat 
                //gmc.LoadTileArranger(ntaf.GetFormatName(), ntaf.GetArrangerSize(), null);
            }
        }
    }
}
