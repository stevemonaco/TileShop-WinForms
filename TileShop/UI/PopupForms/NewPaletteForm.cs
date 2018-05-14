using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TileShop.Core;

namespace TileShop
{
    public partial class NewPaletteForm : Form
    {
        public string PaletteName { get; private set; }
        public string FileName { get; private set; }
        public ColorModel ColorModel { get; private set; }
        public int Entries { get; private set; }
        public long FileOffset { get; private set; }

        public NewPaletteForm()
        {
            InitializeComponent();

            // Populate the dropdown with all supported ColorModels
            colorFormatBox.Items.Clear();
            List<string> formatlist = Palette.GetColorModelNames();

            foreach (string s in formatlist)
                colorFormatBox.Items.Add(s);

            colorFormatBox.SelectedIndex = 0;
        }

        private void addPaletteButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;

            if(paletteNameBox.Text == "" || entriesBox.Text == "" || offsetBox.Text == "")
            {
                MessageBox.Show("All fields must be completed before adding a palette to the project");
                return;
            }
            if(ResourceManager.HasResource(paletteNameBox.Text))
            {
                MessageBox.Show("Palette " + paletteNameBox.Text + " already exists. Please choose an alternate name.");
                return;
            }

            PaletteName = paletteNameBox.Text;
            FileName = (string)projectFileBox.SelectedItem;
            Entries = int.Parse(entriesBox.Text);
            FileOffset = long.Parse(offsetBox.Text);
            ColorModel = Palette.StringToColorModel((string)colorFormatBox.Text);

            DialogResult = DialogResult.OK;
            this.Close();
        }

        public void AddFileNames(IEnumerable<string> filenames)
        {
            projectFileBox.Items.Clear();

            foreach (string s in filenames)
                projectFileBox.Items.Add(s);

            projectFileBox.SelectedIndex = 0;
        }

        private void NewPaletteForm_Shown(object sender, EventArgs e)
        {
            // Populate the dropdown with all supported ColorModels
            colorFormatBox.Items.Clear();
            List<string> formatlist = Palette.GetColorModelNames();

            if (formatlist.Count == 0)
            {
                MessageBox.Show("Please add files to the project before adding palettes");
                Close();
            }
            foreach (string s in formatlist)
                colorFormatBox.Items.Add(s);

            colorFormatBox.SelectedIndex = 0;
        }
    }
}
