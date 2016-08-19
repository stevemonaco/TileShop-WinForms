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
    public partial class NewScatteredArrangerForm : Form
    {
        public NewScatteredArrangerForm()
        {
            InitializeComponent();
            SetDefaults(8, 8, 16, 8);
            ActiveControl = nameTextBox;
        }

        public void SetDefaults(int TileWidth, int TileHeight, int ArrangerTilesX, int ArrangerTilesY)
        {

            tileWidthBox.Text = TileWidth.ToString();
            tileHeightBox.Text = TileHeight.ToString();
            tilesXBox.Text = ArrangerTilesX.ToString();
            tilesYBox.Text = ArrangerTilesY.ToString();
        }

        public Size GetTileSize()
        {
            Size s = new Size();
            s.Width = int.Parse(tileWidthBox.Text);
            s.Height = int.Parse(tileHeightBox.Text);
            return s;
        }

        public Size GetArrangerSize()
        {
            Size s = new Size();
            s.Width = int.Parse(tilesXBox.Text);
            s.Height = int.Parse(tilesYBox.Text);
            return s;
        }

        public string GetArrangerName()
        {
            return nameTextBox.Text;
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            if (FileManager.Instance.HasArranger(nameTextBox.Text))
            {
                MessageBox.Show("Arranger " + nameTextBox.Text + " already exists. Please choose an alternate name");
                return;
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
