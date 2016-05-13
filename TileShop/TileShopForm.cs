using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileShop
{
    public partial class TileShopForm : Form
    {
        public TileShopForm()
        {
            InitializeComponent();
        }

        private void newGraphicsProjectMenu_Click(object sender, EventArgs e)
        {
            GraphicsMdiChild gmc = new GraphicsMdiChild(this);
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
                GraphicsMdiChild gmc = new GraphicsMdiChild(this);

                if(gmc.OpenFile(ofd.FileName) == false)
                {
                    gmc.Close();
                    MessageBox.Show("Could not open file " + ofd.FileName);
                    return;
                }
                gmc.MdiParent = this;
                gmc.Show();
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
    }
}
