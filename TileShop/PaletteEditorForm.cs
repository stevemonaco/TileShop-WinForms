using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using WeifenLuo.WinFormsUI.Docking;

namespace TileShop
{
    public partial class PaletteEditorForm : DockContent
    {
        Palette pal;

        public event EventHandler<EventArgs> PaletteContentsModified = null;

        public Color ActiveColor
        {
            get { return activeColor; }
            set
            {
                activeColor = value;
                activeColorPanel.Invalidate();
            }
        }
        private Color activeColor;

        public PaletteEditorForm(string PaletteName)
        {
            InitializeComponent();

            pal = FileManager.Instance.GetPalette(PaletteName);

            colorFormatBox.Enabled = false;
            List<string> colorList = Palette.GetPaletteColorFormatsNameList();
            foreach (string s in colorList)
                colorFormatBox.Items.Add(s);
            colorFormatBox.Enabled = true;

            paletteNameBox.Text = pal.Name;
            projectFileBox.Text = pal.FileName;
            paletteOffsetBox.Text = pal.FileOffset.ToString();
            nudEntries.Value = pal.Entries;
            SetColorFormatBox(pal.ColorFormat);

            swatchControl.SetPaletteName(PaletteName);

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
                activeColorPanel, new object[] { true }); // Enable double buffering

            ActiveColor = pal.GetLocalColor(0);

            paletteTip.InitialDelay = 1000;
            paletteTip.ReshowDelay = 500;
            paletteTip.SetToolTip(savePaletteButton, "Permanently saves the palette in memory to underlying source");
            paletteTip.SetToolTip(saveColorButton, "Saves the current color to the temporary palette in memory");
            paletteTip.SetToolTip(reloadPaletteButton, "Reloads palette in memory from underlying source");
        }

        /*public void AddFileNames(List<string> Filenames)
        {
            projectFileBox.Items.Clear();

            foreach (string s in Filenames)
                projectFileBox.Items.Add(s);

            projectFileBox.SelectedIndex = 0;
        }*/

        private void SetColorFormatBox(PaletteColorFormat format)
        {
            int idx = 0;
            foreach(string s in colorFormatBox.Items)
            {
                if (s == format.ToString())
                {
                    colorFormatBox.SelectedIndex = idx;
                    return;
                }
                idx++;
            }

            throw new ArgumentException(String.Format("PaletteColorFormat {0} cannot be found in colorFormatBox's item collection", format.ToString()));
        }

        private void SetPaletteNumericBounds(PaletteColorFormat palFormat)
        {
            switch(palFormat)
            {
                case PaletteColorFormat.BGR15:
                    sliderAlpha.Visible = false;
                    nudAlpha.Enabled = false;
                    SetPaletteNumericBounds(0, 31, 0, 31, 0, 31, 0, 31);
                    break;
                case PaletteColorFormat.RGB24:
                    sliderAlpha.Visible = false;
                    nudAlpha.Enabled = false;
                    SetPaletteNumericBounds(0, 255, 0, 255, 0, 255, 0, 255);
                    break;
                case PaletteColorFormat.ARGB32:
                    sliderAlpha.Visible = true;
                    nudAlpha.Enabled = true;
                    SetPaletteNumericBounds(0, 255, 0, 255, 0, 255, 0, 255);
                    break;
                default:
                    throw new ArgumentException(String.Format("PaletteColorFormat {0} not supported", palFormat.ToString()));
            }
        }

        private void SetPaletteNumericBounds(int Rmin, int Rmax, int Gmin, int Gmax, int Bmin, int Bmax, int Amin, int Amax)
        {
            sliderRed.MinValue = Rmin;
            sliderRed.MaxValue = Rmax;
            nudRed.Minimum = Rmin;
            nudRed.Maximum = Rmax;

            sliderGreen.MinValue = Gmin;
            sliderGreen.MaxValue = Gmax;
            nudGreen.Minimum = Gmin;
            nudGreen.Maximum = Gmax;

            sliderBlue.MinValue = Bmin;
            sliderBlue.MaxValue = Bmax;
            nudBlue.Minimum = Bmin;
            nudBlue.Maximum = Bmax;

            sliderAlpha.MinValue = Amin;
            sliderAlpha.MaxValue = Amax;
            nudAlpha.Minimum = Amin;
            nudAlpha.Maximum = Amax;
        }

        private void sliderRed_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if (sliderRed.Value != nudRed.Value) // Test so the controls don't eventlock each other
                nudRed.Value = sliderRed.Value;
        }

        private void sliderGreen_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if (sliderGreen.Value != nudGreen.Value) // Test so the controls don't eventlock each other
                nudGreen.Value = sliderGreen.Value;
        }

        private void sliderBlue_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if (sliderBlue.Value != nudBlue.Value) // Test so the controls don't eventlock each other
                nudBlue.Value = sliderBlue.Value;
        }

        private void sliderAlpha_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if (sliderAlpha.Value != nudAlpha.Value) // Test so the controls don't eventlock each other
                nudAlpha.Value = sliderAlpha.Value;
        }

        private void nudRed_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if ((int)nudRed.Value != sliderRed.Value) // Test so the controls don't eventlock each other
                sliderRed.Value = (int)nudRed.Value;
        }

        private void nudGreen_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if ((int)nudGreen.Value != sliderGreen.Value) // Test so the controls don't eventlock each other
                sliderGreen.Value = (int)nudGreen.Value;
        }

        private void nudBlue_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if ((int)nudBlue.Value != sliderBlue.Value) // Test so the controls don't eventlock each other
                sliderBlue.Value = (int)nudBlue.Value;
        }

        private void nudAlpha_ValueChanged(object sender, EventArgs e)
        {
            ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb((byte)nudAlpha.Value, (byte)nudRed.Value, (byte)nudGreen.Value, (byte)nudBlue.Value, pal.ColorFormat));
            if ((int)nudAlpha.Value != sliderAlpha.Value) // Test so the controls don't eventlock each other
                sliderAlpha.Value = (int)nudAlpha.Value;
        }

        private void htmlBox_TextChanged(object sender, EventArgs e)
        {
            int R, G, B;
            int RGB = int.Parse(htmlBox.Text, System.Globalization.NumberStyles.HexNumber);
            R = (RGB & 0xFF0000) >> 16;
            G = (RGB & 0xFF00) >> 8;
            B = RGB & 0xFF;

            sliderRed.Value = R;
            sliderGreen.Value = G;
            sliderBlue.Value = B;
        }

        private void htmlBox_KeyDown(object sender, KeyEventArgs e)
        {
            /*string hexFormat = "0123456789ABCDEF";
            char key = (char)e.KeyValue;

            if (hexFormat.IndexOf(key) == -1) // Not an acceptable hex format character
                return;

            e.Handled = true;*/
        }

        private void htmlBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            string hexFormat = "0123456789ABCDEFabcdef";

            if (hexFormat.IndexOf(e.KeyChar) != -1) // Found character in hexFormat
                return;

            e.Handled = true;
        }

        /*private void DisplayPaletteIndex(int idx)
        {
            uint nativeColor = Palette.GetNativeInArgb32(pal[idx], pal.ColorFormat);
            ActiveColor = pal.GetColor(idx);

            sliderRed.Value = (int)(nativeColor & 0xFF0000) >> 16;
            sliderGreen.Value = (int)(nativeColor & 0xFF00) >> 8;
            sliderBlue.Value = (int)(nativeColor & 0xFF);
            sliderAlpha.Value = (int)(nativeColor & 0xFF000000) >> 24;
        }*/

        private void paletteNameBox_TextChanged(object sender, EventArgs e)
        {
            Text = paletteNameBox.Text; // Update form title
        }

        private void colorFormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PaletteColorFormat format = (PaletteColorFormat)Enum.Parse(typeof(PaletteColorFormat), (string)colorFormatBox.SelectedItem);

            SetPaletteNumericBounds(format);

            pal.LoadPalette(pal.FileName, pal.FileOffset, format, pal.Entries);
            swatchControl.Invalidate();
        }

        private void nudEntries_ValueChanged(object sender, EventArgs e)
        {
            pal.LoadPalette(pal.FileName, pal.FileOffset, pal.ColorFormat, (int)nudEntries.Value);

            swatchControl.Invalidate();
        }

        private void activeColorPanel_Paint(object sender, PaintEventArgs e)
        {
            using (Brush b = new SolidBrush(ActiveColor))
            {
                if (pal.HasAlpha)
                    e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                else
                    e.Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                e.Graphics.FillRectangle(b, activeColorPanel.ClientRectangle);
            }
        }

        private void swatchControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveColor = pal.GetLocalColor(swatchControl.SelectedIndex);
            (byte A, byte R, byte G, byte B) = pal.SplitForeignColor(swatchControl.SelectedIndex);

            sliderRed.Value = R;
            sliderGreen.Value = G;
            sliderBlue.Value = B;
            sliderAlpha.Value = A;

            /*uint nativeColor = Palette.GetNativeInArgb32(pal[swatchControl.SelectedIndex], pal.ColorFormat);
            sliderRed.Value = (int)(nativeColor & 0xFF0000) >> 16;
            sliderGreen.Value = (int)(nativeColor & 0xFF00) >> 8;
            sliderBlue.Value = (int)(nativeColor & 0xFF);
            sliderAlpha.Value = (int)(nativeColor & 0xFF000000) >> 24;*/
        }

        private void reloadPaletteButton_Click(object sender, EventArgs e)
        {
            pal.Reload();
            swatchControl.SelectedIndex = 0;
            swatchControl.Invalidate();
            ActiveColor = pal.GetLocalColor(0);

            PaletteContentsModified?.Invoke(this, null);
        }

        private void saveColorButton_Click(object sender, EventArgs e)
        {
            pal.SetPaletteForeignColor(swatchControl.SelectedIndex, (byte)sliderAlpha.Value, (byte)sliderRed.Value, (byte)sliderGreen.Value, (byte)sliderBlue.Value);
            swatchControl.Invalidate();

            PaletteContentsModified?.Invoke(this, null);
        }

        private void savePaletteButton_Click(object sender, EventArgs e)
        {
            pal.SavePalette();

            // PaletteContentsModified?.Invoke(this, null); // Not necessary?
        }
    }

    class PaletteModifiedArgs : EventArgs
    {
        //bool Memory
    }

}
