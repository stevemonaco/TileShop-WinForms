using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

// Code adopted from http://stackoverflow.com/questions/463299/how-do-i-make-a-textbox-that-only-accepts-numbers

namespace TileShop
{
    public partial class IntegerTextBox : TextBox
    {
        public IntegerTextBox()
        {
            InitializeComponent();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo fi = CultureInfo.CurrentCulture.NumberFormat;

            string c = e.KeyChar.ToString();
            if (char.IsDigit(c, 0))
                return;

            if ((SelectionStart == 0) && (c.Equals(fi.NegativeSign)))
                return;

            // copy/paste
            if ((((int)e.KeyChar == 22) || ((int)e.KeyChar == 3))
                && ((ModifierKeys & Keys.Control) == Keys.Control))
                return;

            if (e.KeyChar == '\b')
                return;

            e.Handled = true;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_PASTE = 0x0302;
            if (m.Msg == WM_PASTE)
            {
                string text = Clipboard.GetText();
                if (string.IsNullOrEmpty(text))
                    return;

                if ((text.IndexOf('+') >= 0) && (SelectionStart != 0))
                    return;

                int i;
                if (!int.TryParse(text, out i)) // change this for other integer types
                    return;

                if ((i < 0) && (SelectionStart != 0))
                    return;
            }
            base.WndProc(ref m);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}