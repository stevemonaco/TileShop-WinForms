using System;
using System.Windows.Forms;
using System.Globalization;

namespace TileShop.PopupForms
{
    public partial class JumpToAddressForm : Form
    {
        private long address = 0;

        public long Address
        {
            get { return address; }
            set
            {
                address = value;
                if (HexButton.Checked)
                    NumberBox.Text = String.Format("{0:X}", address);
                else if (DecimalButton.Checked)
                    NumberBox.Text = String.Format("{0}", address);
            }
        }

        public JumpToAddressForm()
        {
            InitializeComponent();
        }

        private void JumpButton_Click(object sender, EventArgs e)
        {
            long tempAddress = 0;
            CultureInfo provider = CultureInfo.InvariantCulture;

            if (HexButton.Checked)
            {
                if(!long.TryParse(NumberBox.Text, System.Globalization.NumberStyles.HexNumber, provider, out tempAddress))
                {
                    MessageBox.Show("Failed to parse " + NumberBox.Text + " as a hexadecimal number");
                    return;
                }
            }
            else if (DecimalButton.Checked)
            {
                if (!long.TryParse(NumberBox.Text, out tempAddress))
                {
                    MessageBox.Show("Failed to parse " + NumberBox.Text + " as a decimal number");
                    return;
                }
            }

            Address = tempAddress;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
