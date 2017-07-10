using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;

// Code adopted from http://stackoverflow.com/questions/463299/how-do-i-make-a-textbox-that-only-accepts-numbers

namespace TileShop
{
    public partial class IntegerTextBox : TextBox
    {
        internal static class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern bool GetCaretPos(out System.Drawing.Point lpPoint);
        }

        public int Minimum { set; get; }
        public int Maximum { set; get; }

        public IntegerTextBox()
        {
            InitializeComponent();
            Minimum = int.MinValue;
            Maximum = int.MaxValue;
        }

        /*protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            NumberFormatInfo fi = CultureInfo.CurrentCulture.NumberFormat;

            string c = e.KeyChar.ToString();
            if (char.IsDigit(c, 0))
                if (TestRange(c))
                    return;

            if ((SelectionStart == 0) && (c.Equals(fi.NegativeSign))) // Allow a negative sign only at the start of the input
                if(Text.IndexOf('-') == -1) // Allow only one negative in a number
                    if(TestRange(c))
                        return;

            // copy/paste
            if ((((int)e.KeyChar == 22) || ((int)e.KeyChar == 3))
                && ((ModifierKeys & Keys.Control) == Keys.Control))
                return;

            if (e.KeyChar == '\b')
                return;

            e.Handled = true;
        }*/

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            string c;

            // copy/paste
            if ((((int)e.KeyChar == 22) || ((int)e.KeyChar == 3))
                && ((ModifierKeys & Keys.Control) == Keys.Control))
                c = Clipboard.GetText();
            else // Single input
                c = e.KeyChar.ToString();

            if (TestInput(c))
            {
                if(c != "-")
                    return;

                // Toggle negative sign
                if (Text[0] == '-')
                    Text = Text.Remove(0, 1);
                else
                    Text = Text.Insert(0, "-");
            }

            e.Handled = true;
        }

        /*protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_PASTE = 0x0302;
            if (m.Msg == WM_PASTE)
            {
                string text = Clipboard.GetText();
                if (string.IsNullOrEmpty(text))
                    return;

                if (TestInput(text))
                    return; // Our code

                if ((text.IndexOf('+') >= 0) && (SelectionStart != 0))
                    return;

                int i;
                if (!int.TryParse(text, out i)) // change this for other integer types
                    return;

                if ((i < 0) && (SelectionStart != 0))
                    return;
            }
            base.WndProc(ref m);
        }*/

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        // Tests if the current input will put the control within the min/max range
        private bool TestRange(string s)
        {
            string teststr = Text;
            if (SelectionLength >= 1)
            {
                teststr = teststr.Remove(SelectionStart, SelectionLength);
                teststr = teststr.Insert(SelectionStart, s);
            }
            else
                teststr = teststr.Insert(SelectionStart, s);

            int testval = int.Parse(teststr);

            if (testval >= Minimum && testval <= Maximum)
                return true;

            return false;
        }

        // Tests if the current input will put the control within the min/max range or is allowed
        private bool TestInput(string s)
        {
            string teststr = Text;
            int testval;

            if (string.IsNullOrEmpty(s))
                return false;

            if (s == "\b")
                return true;

            if(s == "-") // Toggle negative/positive
            {
                if (teststr.Length > 0) // All negative input must already have a number entered
                {
                    if (teststr[0] == '-')
                        teststr = teststr.Remove(0, 1);
                    else
                        teststr = teststr.Insert(0, "-");

                    testval = int.Parse(teststr);

                    if (testval >= Minimum && testval <= Maximum)
                        return true;
                }
            }
            else
            {
                if (SelectionLength >= 1) // Test an insertion over a current selection
                {
                    teststr = teststr.Remove(SelectionStart, SelectionLength);
                    teststr = teststr.Insert(SelectionStart, s);
                }
                else
                    teststr = teststr.Insert(SelectionStart, s);

                if(int.TryParse(teststr, out testval)) // Let TryParse determine if we have a valid number
                {
                    if (testval >= Minimum && testval <= Maximum)
                        return true;
                }
            }

            return false; // The caller should reject the erroneous input
        }
    }
}