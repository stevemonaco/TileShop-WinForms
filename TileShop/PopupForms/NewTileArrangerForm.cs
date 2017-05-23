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
    public partial class NewTileArrangerForm : Form
    {
        public NewTileArrangerForm()
        {
            InitializeComponent();
            EnumerateFormats();
            SetDefaults(8, 8, 16, 8);
        }

        // Adds all graphic format names
        public void EnumerateFormats()
        {
            List<string> FormatNames = FileManager.Instance.GetGraphicsFormatsNameList();

            foreach (string s in FormatNames)
            {
                formatBox.Items.Add(s);
            }
        }

        public void SetDefaults(int TileWidth, int TileHeight, int ArrangerTilesX, int ArrangerTilesY)
        {
            
            tileWidthBox.Text = TileWidth.ToString();
            tileHeightBox.Text = TileHeight.ToString();
            tilesXBox.Text = ArrangerTilesX.ToString();
            tilesYBox.Text = ArrangerTilesY.ToString();
        }

        public void SetFormat(string FormatName)
        {
            int idx = 0;
            foreach(string s in formatBox.Items)
            {
                if(s == FormatName)
                    formatBox.SelectedIndex = idx;
            }
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

        public string GetFormatName()
        {
            return formatBox.SelectedText;
        }

    }
}
