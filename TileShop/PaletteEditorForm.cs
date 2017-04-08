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
    public partial class PaletteEditorForm : EditorDockContent
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
        private Color activeColor = Color.FromArgb(0);

        public byte ForeignRed
        {
            get { return foreignRed; }
            set
            {
                if (foreignRed == value)
                    return;

                foreignRed = value;

                if (sliderRed.Value != foreignRed) // Test so the controls don't eventlock
                    sliderRed.Value = foreignRed;
                if (nudRed.Value != foreignRed)
                    nudRed.Value = foreignRed;

                ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorFormat));
                SavePaletteColors();
            }
        }
        private byte foreignRed;

        public byte ForeignGreen
        {
            get { return foreignGreen; }
            set
            {
                if (foreignGreen == value)
                    return;

                foreignGreen = value;
                if (sliderGreen.Value != foreignGreen) // Test so the controls don't eventlock
                    sliderGreen.Value = foreignGreen;
                if (nudGreen.Value != foreignGreen)
                    nudGreen.Value = foreignGreen;

                ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorFormat));
                SavePaletteColors();
            }
        }
        private byte foreignGreen;

        public byte ForeignBlue
        {
            get { return foreignBlue; }
            set
            {
                if (foreignBlue == value)
                    return;

                foreignBlue = value;
                if (sliderBlue.Value != foreignBlue) // Test so the controls don't eventlock
                    sliderBlue.Value = foreignBlue;
                if (nudBlue.Value != foreignBlue)
                    nudBlue.Value = foreignBlue;

                ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorFormat));
                SavePaletteColors();
            }
        }
        private byte foreignBlue;

        public byte ForeignAlpha
        {
            get { return foreignAlpha; }
            set
            {
                if (foreignAlpha == value)
                    return;

                foreignAlpha = value;
                if (sliderAlpha.Value != foreignAlpha) // Test so the controls don't eventlock
                    sliderAlpha.Value = foreignAlpha;
                if (nudAlpha.Value != foreignAlpha)
                    nudAlpha.Value = foreignAlpha;

                ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorFormat));
                SavePaletteColors();
            }
        }
        private byte foreignAlpha;

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

            swatchControl.ShowPalette(FileManager.Instance.GetPalette(PaletteName));

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
                activeColorPanel, new object[] { true }); // Enable double buffering

            (byte A, byte R, byte G, byte B) = pal.SplitForeignColor(0);
            ForeignAlpha = A;
            ForeignRed = R;
            ForeignGreen = G;
            ForeignBlue = B;

            //ActiveColor = pal.GetLocalColor(0);

            paletteTip.InitialDelay = 1000;
            paletteTip.ReshowDelay = 500;
            paletteTip.SetToolTip(savePaletteButton, "Permanently saves the palette in memory to underlying source");
            paletteTip.SetToolTip(reloadPaletteButton, "Reloads palette in memory from underlying source");
        }

        public override bool ReloadContent()
        {
            return false;
        }

        public override bool SaveContent()
        {
            return false;
        }

        public override bool RefreshContent()
        {
            return false;
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
            ForeignRed = (byte)sliderRed.Value;
        }

        private void sliderGreen_ValueChanged(object sender, EventArgs e)
        {
            ForeignGreen = (byte)sliderGreen.Value;
        }

        private void sliderBlue_ValueChanged(object sender, EventArgs e)
        {
            ForeignBlue = (byte)sliderBlue.Value;
        }

        private void sliderAlpha_ValueChanged(object sender, EventArgs e)
        {
            ForeignAlpha = (byte)sliderAlpha.Value;
        }

        private void nudRed_ValueChanged(object sender, EventArgs e)
        {
            ForeignRed = (byte)nudRed.Value;
        }

        private void nudGreen_ValueChanged(object sender, EventArgs e)
        {
            ForeignGreen = (byte)nudGreen.Value;
        }

        private void nudBlue_ValueChanged(object sender, EventArgs e)
        {
            ForeignBlue = (byte)nudBlue.Value;
        }

        private void nudAlpha_ValueChanged(object sender, EventArgs e)
        {
            ForeignAlpha = (byte)nudAlpha.Value;
        }

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
            //pal.LoadPalette(pal.FileName, pal.FileOffset, pal.ColorFormat, (int)nudEntries.Value);

            //swatchControl.Invalidate();
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

            ForeignAlpha = A;
            ForeignRed = R;
            ForeignGreen = G;
            ForeignBlue = B;
        }

        private void reloadPaletteButton_Click(object sender, EventArgs e)
        {
            pal.Reload();
            swatchControl.SelectedIndex = 0;
            swatchControl.Invalidate();
            ActiveColor = pal.GetLocalColor(0);

            (byte A, byte R, byte G, byte B) = pal.SplitForeignColor(swatchControl.SelectedIndex);

            ForeignAlpha = A;
            ForeignRed = R;
            ForeignGreen = G;
            ForeignBlue = B;

            PaletteContentsModified?.Invoke(this, null);
        }

        /// <summary>
        /// Save palette colors to memory
        /// </summary>
        private void SavePaletteColors()
        {
            pal.SetPaletteForeignColor(swatchControl.SelectedIndex, (byte)sliderAlpha.Value, (byte)sliderRed.Value, (byte)sliderGreen.Value, (byte)sliderBlue.Value);
            swatchControl.Invalidate();

            ContainsModifiedContent = true;

            PaletteContentsModified?.Invoke(this, null);
        }

        private void savePaletteButton_Click(object sender, EventArgs e)
        {
            pal.SavePalette();

            // PaletteContentsModified?.Invoke(this, null); // Not necessary?
        }
    }

}
