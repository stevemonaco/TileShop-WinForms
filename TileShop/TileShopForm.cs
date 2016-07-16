using System;
using System.Windows.Forms;
using System.IO;

namespace TileShop
{
    public partial class TileShopForm : Form
    {
        private string CodecDirectoryPath = "D:\\Projects\\TileShop\\codecs\\";
        private string PaletteDirectoryPath = "D:\\Projects\\TileShop\\pal\\";
        public TileShopForm()
        {
            InitializeComponent();
            LoadCodecs(CodecDirectoryPath);
            LoadPalettes(PaletteDirectoryPath);
        }

        private void newGraphicsProjectMenu_Click(object sender, EventArgs e)
        {
            GraphicsViewerMdiChild gmc = new GraphicsViewerMdiChild(this);
            gmc.MdiParent = this;
            gmc.Show();
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
                GraphicsViewerMdiChild gmc = new GraphicsViewerMdiChild(this);

                if(gmc.OpenFile(ofd.FileName) == false)
                {
                    gmc.Close();
                    MessageBox.Show("Could not open file " + ofd.FileName);
                    return;
                }
                gmc.MdiParent = this;
                gmc.Show();

                //GraphicsEditorMdiChild gec = new GraphicsEditorMdiChild();
                //gec.MdiParent = this;
                //gec.Show();
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
                    FileManager.Instance.LoadPalette(s);
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            GraphicsViewerMdiChild gmc = new GraphicsViewerMdiChild(this);

            if (gmc.OpenFile("D:\\Projects\\ff2.sfc") == false)
            {
                gmc.Close();
                MessageBox.Show("Could not open file " + "D:\\Projects\\ff2.sfc");
                return;
            }
            gmc.MdiParent = this;
            gmc.WindowState = FormWindowState.Maximized;
            gmc.zoom = 6;
            gmc.Show();
        }
    }
}
