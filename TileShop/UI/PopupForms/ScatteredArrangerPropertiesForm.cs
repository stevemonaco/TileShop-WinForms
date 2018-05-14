using System;
using System.Drawing;
using System.Windows.Forms;
using TileShop.Core;

namespace TileShop
{
    public partial class ScatteredArrangerPropertiesForm : Form
    {
        public Size ArrangerSize { get; private set; }
        public Size ElementPixelSize { get; private set; }
        public string ArrangerName { get; private set; }
        public ArrangerLayout ArrangerLayout { get; private set; }
        public string ArrangerKey { get; private set; }

        private bool CreateMode;

        public ScatteredArrangerPropertiesForm()
        {
            InitializeComponent();
            ArrangerName = "";
            SetDefaults(true, "", "", new Size(8, 8), new Size(16, 8), ArrangerLayout.TiledArranger);

            ActiveControl = NameTextBox;
        }

        /// <summary>
        /// Sets the default parameters for the form before the user sees the form
        /// </summary>
        /// <param name="createMode">True if the form is intended to create a new arranger; else false</param>
        /// <param name="name">Name of the arranger</param>
        /// <param name="key">Key of the arranger to be renamed or blank if to be created</param>
        /// <param name="elementSize">Size of each element in pixels</param>
        /// <param name="arrangerSize">Size of the arranger in elements</param>
        /// <param name="layoutMode">Layout mode of the arranger</param>
        public void SetDefaults(bool createMode, string name, string key, Size elementSize, Size arrangerSize, ArrangerLayout layoutMode)
        {
            if (key is null)
                throw new ArgumentNullException();
            if(elementSize.Width <= 0 || elementSize.Height <= 0 || arrangerSize.Width <= 0 || arrangerSize.Height <= 0)
                throw new ArgumentOutOfRangeException();

            CreateMode = createMode;

            if(CreateMode)
            {
                Text = "Create New Scattered Arranger";
                ConfirmButton.Text = "Create";
            }
            else // Modify an existing arranger
            {
                this.Text = "Modify Scattered Arranger Properties";
                TiledLayoutButton.Enabled = false;
                LinearLayoutButton.Enabled = false;
                ElementWidthBox.Enabled = false;
                ElementHeightBox.Enabled = false;
                ConfirmButton.Text = "Modify";
            }

            ArrangerWidthBox.Value = arrangerSize.Width;
            ArrangerHeightBox.Value = arrangerSize.Height;
            ElementWidthBox.Value = elementSize.Width;
            ElementHeightBox.Value = elementSize.Height;
            NameTextBox.Text = name;

            ArrangerSize = arrangerSize;
            ElementPixelSize = elementSize;
            ArrangerName = name;
            ArrangerKey = key;
        }

        private Size GetElementSize()
        {
            Size s = new Size()
            {
                Width = (int)ElementWidthBox.Value,
                Height = (int)ElementHeightBox.Value
            };

            return s;
        }

        private Size GetArrangerSize()
        {
            Size s = new Size()
            {
                Width = (int)ArrangerWidthBox.Value,
                Height = (int)ArrangerHeightBox.Value
            };
            return s;
        }

        private string GetArrangerName()
        {
            return NameTextBox.Text;
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;

            if (NameTextBox.Text == "")
            {
                MessageBox.Show("The arranger must be named.");
                return;
            }
            if (CreateMode && ResourceManager.HasResource(NameTextBox.Text))
            {
                MessageBox.Show("Arranger " + NameTextBox.Text + " already exists. Please choose an alternate name.");
                return;
            }

            ArrangerName = NameTextBox.Text;

            if(ArrangerName == "")
            {
                MessageBox.Show("Please choose a name for the arranger");
                return;
            }

            if(CreateMode && ResourceManager.HasResource(ArrangerName))
            {
                MessageBox.Show("Arranger " + ArrangerName + " already exists. Please choose another name.");
                return;
            }

            ArrangerSize = GetArrangerSize();
            ElementPixelSize = GetElementSize();
            if (TiledLayoutButton.Checked)
                ArrangerLayout = ArrangerLayout.TiledArranger;
            else if (LinearLayoutButton.Checked)
                ArrangerLayout = ArrangerLayout.LinearArranger;

            if(ElementPixelSize.Width <= 0 || ElementPixelSize.Height <= 0 || ArrangerSize.Width <= 0 || ArrangerSize.Height <= 0)
            {
                MessageBox.Show("All numeric fields must be greater than zero to create a new arranger");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
