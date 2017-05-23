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

            ActiveControl = NameTextBox;
        }

        public void SetDefaults(int TileWidth, int TileHeight, int ArrangerTilesX, int ArrangerTilesY)
        {
            TileWidthBox.Text = TileWidth.ToString();
            TileHeightBox.Text = TileHeight.ToString();
            TilesXBox.Text = ArrangerTilesX.ToString();
            TilesYBox.Text = ArrangerTilesY.ToString();

            ArrangerSize = new Size(ArrangerTilesX, ArrangerTilesY);
            TileSize = new Size(TileWidth, TileHeight);
        }

        public Size GetTileSize()
        {
            Size s = new Size();
            s.Width = int.Parse(TileWidthBox.Text);
            s.Height = int.Parse(TileHeightBox.Text);
            return s;
        }

        public Size GetArrangerSize()
        {
            Size s = new Size()
            {
                Width = int.Parse(TilesXBox.Text),
                Height = int.Parse(TilesYBox.Text)
            };
            return s;
        }

        public string GetArrangerName()
        {
            return NameTextBox.Text;
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;

            if (NameTextBox.Text == "" || TilesXBox.Text == "" || TilesYBox.Text == "" || TileHeightBox.Text == "" || TileWidthBox.Text == "")
            {
                MessageBox.Show("All fields must be completed before adding an arranger to the project");
                return;
            }
            if (FileManager.Instance.HasArranger(NameTextBox.Text))
            {
                MessageBox.Show("Arranger " + NameTextBox.Text + " already exists. Please choose an alternate name.");
                return;
            }

            ArrangerName = NameTextBox.Text;
            ArrangerSize = new Size(int.Parse(TilesXBox.Text), int.Parse(TilesYBox.Text));
            TileSize = new Size(int.Parse(TileWidthBox.Text), int.Parse(TileHeightBox.Text));

            if(TileSize.Width == 0 || TileSize.Height == 0 || ArrangerSize.Width == 0 || ArrangerSize.Height == 0)
            {
                MessageBox.Show("All numeric fields must be nonzero to create a new arranger");
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
