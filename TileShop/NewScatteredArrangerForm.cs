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
        public Size ArrangerSize { get; private set; }
        public Size TileSize { get; private set; }
        public string ArrangerName { get; private set; }

        public NewScatteredArrangerForm()
        {
            InitializeComponent();
            ArrangerName = "";
            SetDefaults(8, 8, 16, 8);

            ActiveControl = nameTextBox;
        }

        public void SetDefaults(int TileWidth, int TileHeight, int ArrangerTilesX, int ArrangerTilesY)
        {
            tileWidthBox.Text = TileWidth.ToString();
            tileHeightBox.Text = TileHeight.ToString();
            tilesXBox.Text = ArrangerTilesX.ToString();
            tilesYBox.Text = ArrangerTilesY.ToString();

            ArrangerSize = new Size(ArrangerTilesX, ArrangerTilesY);
            TileSize = new Size(TileWidth, TileHeight);
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
            DialogResult = DialogResult.None;

            if (nameTextBox.Text == "" || tilesXBox.Text == "" || tilesYBox.Text == "" || tileHeightBox.Text == "" || tileWidthBox.Text == "")
            {
                MessageBox.Show("All fields must be completed before adding an arranger to the project");
                return;
            }
            if (FileManager.Instance.HasArranger(nameTextBox.Text))
            {
                MessageBox.Show("Arranger " + nameTextBox.Text + " already exists. Please choose an alternate name.");
                return;
            }

            ArrangerName = nameTextBox.Text;
            ArrangerSize = new Size(int.Parse(tilesXBox.Text), int.Parse(tilesYBox.Text));
            TileSize = new Size(int.Parse(tileWidthBox.Text), int.Parse(tileHeightBox.Text));

            if(TileSize.Width == 0 || TileSize.Height == 0 || ArrangerSize.Width == 0 || ArrangerSize.Height == 0)
            {
                MessageBox.Show("All numeric fields must be nonzero to create a new arranger");
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
