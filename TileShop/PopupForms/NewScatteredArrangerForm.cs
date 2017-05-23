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
        public Size ElementPixelSize { get; private set; }
        public string ArrangerName { get; private set; }

        public NewScatteredArrangerForm()
        {
            InitializeComponent();
            ArrangerName = "";
            SetDefaults(8, 8, 16, 8);

            ActiveControl = NameTextBox;
        }

        public void SetDefaults(int ElementWidth, int ElementHeight, int ArrangerElementsWidth, int ArrangerElementsHeight)
        {
            if (ElementWidth < 0 || ElementHeight < 0 || ArrangerElementsWidth < 0 || ArrangerElementsHeight < 0)
                throw new ArgumentOutOfRangeException();

            ArrangerWidthBox.Value = ArrangerElementsWidth;
            ArrangerHeightBox.Value = ArrangerElementsHeight;
            ElementWidthBox.Value = ElementWidth;
            ElementHeightBox.Value = ElementHeight;

            ArrangerSize = new Size(ArrangerElementsWidth, ArrangerElementsHeight);
            ElementPixelSize = new Size(ElementWidth, ElementHeight);
        }

        public Size GetElementSize()
        {
            Size s = new Size()
            {
                Width = (int)ElementWidthBox.Value,
                Height = (int)ElementHeightBox.Value
            };

            return s;
        }

        public Size GetArrangerSize()
        {
            Size s = new Size()
            {
                Width = (int)ArrangerWidthBox.Value,
                Height = (int)ArrangerHeightBox.Value
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

            if (NameTextBox.Text == "")
            {
                MessageBox.Show("The arranger must be named before adding it to the project.");
                return;
            }
            if (FileManager.Instance.HasArranger(NameTextBox.Text))
            {
                MessageBox.Show("Arranger " + NameTextBox.Text + " already exists. Please choose an alternate name.");
                return;
            }

            ArrangerName = NameTextBox.Text;
            ArrangerSize = GetArrangerSize();
            ElementPixelSize = GetElementSize();

            if(ElementPixelSize.Width == 0 || ElementPixelSize.Height == 0 || ArrangerSize.Width == 0 || ArrangerSize.Height == 0)
            {
                MessageBox.Show("All numeric fields must be nonzero to create a new arranger");
                return;
            }

            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
