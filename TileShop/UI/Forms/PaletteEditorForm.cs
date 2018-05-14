using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using TileShop.Core;

namespace TileShop
{
    public partial class PaletteEditorForm : EditorDockContent
    {
        Palette pal;

        public Color ActiveColor
        {
            get => _activeColor;
            set
            {
                _activeColor = value;
                activeColorPanel.Invalidate();
            }
        }
        private Color _activeColor = Color.FromArgb(0);

        public byte ForeignRed
        {
            get => _foreignRed;
            set
            {
                if (_foreignRed == value)
                    return;

                _foreignRed = value;

                if (SliderRed.Value != _foreignRed) // Test so the controls don't eventlock
                    SliderRed.Value = _foreignRed;
                if (NudRed.Value != _foreignRed)
                    NudRed.Value = _foreignRed;

                //ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel));
                ForeignColor fc = new ForeignColor(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel);
                ActiveColor = fc.ToColor(pal.ColorModel);
                SavePaletteColors();
            }
        }
        private byte _foreignRed;

        public byte ForeignGreen
        {
            get => _foreignGreen;
            set
            {
                if (_foreignGreen == value)
                    return;

                _foreignGreen = value;
                if (SliderGreen.Value != _foreignGreen) // Test so the controls don't eventlock
                    SliderGreen.Value = _foreignGreen;
                if (NudGreen.Value != _foreignGreen)
                    NudGreen.Value = _foreignGreen;

                //ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel));
                ForeignColor fc = new ForeignColor(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel);
                ActiveColor = fc.ToColor(pal.ColorModel);
                SavePaletteColors();
            }
        }
        private byte _foreignGreen;

        public byte ForeignBlue
        {
            get => _foreignBlue;
            set
            {
                if (_foreignBlue == value)
                    return;

                _foreignBlue = value;
                if (SliderBlue.Value != _foreignBlue) // Test so the controls don't eventlock
                    SliderBlue.Value = _foreignBlue;
                if (NudBlue.Value != _foreignBlue)
                    NudBlue.Value = _foreignBlue;

                //ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel));
                ForeignColor fc = new ForeignColor(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel);
                ActiveColor = fc.ToColor(pal.ColorModel);
                SavePaletteColors();
            }
        }
        private byte _foreignBlue;

        public byte ForeignAlpha
        {
            get => _foreignAlpha;
            set
            {
                if (_foreignAlpha == value)
                    return;

                _foreignAlpha = value;
                if (SliderAlpha.Value != _foreignAlpha) // Test so the controls don't eventlock
                    SliderAlpha.Value = _foreignAlpha;
                if (NudAlpha.Value != _foreignAlpha)
                    NudAlpha.Value = _foreignAlpha;

                //ActiveColor = Color.FromArgb((int)Palette.ForeignToLocalArgb(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel));
                ForeignColor fc = new ForeignColor(ForeignAlpha, ForeignRed, ForeignGreen, ForeignBlue, pal.ColorModel);
                ActiveColor = fc.ToColor(pal.ColorModel);
                SavePaletteColors();
            }
        }
        private byte _foreignAlpha;

        public PaletteEditorForm(string paletteKey)
        {
            InitializeComponent();

            pal = ResourceManager.Instance.GetResource(paletteKey) as Palette;

            ColorFormatBox.Enabled = false;
            List<string> colorList = Palette.GetColorModelNames();
            foreach (string s in colorList)
                ColorFormatBox.Items.Add(s);
            ColorFormatBox.Enabled = true;

            PaletteNameBox.Text = pal.Name;
            ProjectFileBox.Text = pal.DataFileKey;
            PaletteOffsetBox.Text = pal.FileAddress.FileOffset.ToString(); // TODO: Refactor for new FileBitAddress
            NudEntries.Value = pal.Entries;
            SetColorFormatBox(pal.ColorModel);

            SwatchControl.ShowPalette(pal, pal.Entries);

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null,
                activeColorPanel, new object[] { true }); // Enable double buffering

            (byte A, byte R, byte G, byte B) = (0, 0, 0, 0);
            ForeignAlpha = A;
            ForeignRed = R;
            ForeignGreen = G;
            ForeignBlue = B;

            ContentSourceName = pal.Name;
            ContentSourceKey = paletteKey;

            paletteTip.InitialDelay = 1000;
            paletteTip.ReshowDelay = 500;
            paletteTip.SetToolTip(SavePaletteButton, "Permanently saves the palette in memory to underlying source");
            paletteTip.SetToolTip(ReloadPaletteButton, "Reloads palette in memory from underlying source");
        }

        public override bool ReloadContent()
        {
            pal.Reload();
            SwatchControl.SelectedIndex = 0;
            SwatchControl.Invalidate();
            ActiveColor = pal.GetColor(0);

            //(byte A, byte R, byte G, byte B) = pal.SplitForeignColor(SwatchControl.SelectedIndex);
            (byte A, byte R, byte G, byte B) = pal.GetForeignColor(SwatchControl.SelectedIndex).Split(pal.ColorModel);

            ForeignAlpha = A;
            ForeignRed = R;
            ForeignGreen = G;
            ForeignBlue = B;

            ContainsModifiedContent = false;
            OnContentModified(EventArgs.Empty);

            return true;
        }

        public override bool SaveContent()
        {
            pal.SavePalette();
            ContainsModifiedContent = false;

            return true;
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

        private void SetColorFormatBox(ColorModel format)
        {
            int idx = 0;
            foreach(string s in ColorFormatBox.Items)
            {
                if (s == format.ToString())
                {
                    ColorFormatBox.SelectedIndex = idx;
                    return;
                }
                idx++;
            }

            throw new ArgumentException(String.Format("ColorModel {0} cannot be found in colorFormatBox's item collection", format.ToString()));
        }

        private void SetPaletteNumericBounds(ColorModel ColorModel)
        {
            switch(ColorModel)
            {
                case ColorModel.BGR15: case ColorModel.RGB15:
                    SliderAlpha.Visible = false;
                    NudAlpha.Enabled = false;
                    SetPaletteNumericBounds(0, 31, 0, 31, 0, 31, 0, 31);
                    break;
                case ColorModel.RGB24:
                    SliderAlpha.Visible = false;
                    NudAlpha.Enabled = false;
                    SetPaletteNumericBounds(0, 255, 0, 255, 0, 255, 0, 255);
                    break;
                case ColorModel.ARGB32:
                    SliderAlpha.Visible = true;
                    NudAlpha.Enabled = true;
                    SetPaletteNumericBounds(0, 255, 0, 255, 0, 255, 0, 255);
                    break;
                default:
                    throw new ArgumentException(String.Format("ColorModel {0} not supported", ColorModel.ToString()));
            }
        }

        private void SetPaletteNumericBounds(int Rmin, int Rmax, int Gmin, int Gmax, int Bmin, int Bmax, int Amin, int Amax)
        {
            SliderRed.MinValue = Rmin;
            SliderRed.MaxValue = Rmax;
            NudRed.Minimum = Rmin;
            NudRed.Maximum = Rmax;

            SliderGreen.MinValue = Gmin;
            SliderGreen.MaxValue = Gmax;
            NudGreen.Minimum = Gmin;
            NudGreen.Maximum = Gmax;

            SliderBlue.MinValue = Bmin;
            SliderBlue.MaxValue = Bmax;
            NudBlue.Minimum = Bmin;
            NudBlue.Maximum = Bmax;

            SliderAlpha.MinValue = Amin;
            SliderAlpha.MaxValue = Amax;
            NudAlpha.Minimum = Amin;
            NudAlpha.Maximum = Amax;
        }

        private void SliderRed_ValueChanged(object sender, EventArgs e)
        {
            ForeignRed = (byte)SliderRed.Value;
        }

        private void SliderGreen_ValueChanged(object sender, EventArgs e)
        {
            ForeignGreen = (byte)SliderGreen.Value;
        }

        private void SliderBlue_ValueChanged(object sender, EventArgs e)
        {
            ForeignBlue = (byte)SliderBlue.Value;
        }

        private void SliderAlpha_ValueChanged(object sender, EventArgs e)
        {
            ForeignAlpha = (byte)SliderAlpha.Value;
        }

        private void NudRed_ValueChanged(object sender, EventArgs e)
        {
            ForeignRed = (byte)NudRed.Value;
        }

        private void NudGreen_ValueChanged(object sender, EventArgs e)
        {
            ForeignGreen = (byte)NudGreen.Value;
        }

        private void NudBlue_ValueChanged(object sender, EventArgs e)
        {
            ForeignBlue = (byte)NudBlue.Value;
        }

        private void NudAlpha_ValueChanged(object sender, EventArgs e)
        {
            ForeignAlpha = (byte)NudAlpha.Value;
        }

        private void PaletteNameBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void ColorFormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColorModel model = (ColorModel)Enum.Parse(typeof(ColorModel), (string)ColorFormatBox.SelectedItem);

            SetPaletteNumericBounds(model);

            pal.LazyLoadPalette(pal.DataFileKey, pal.FileAddress, model, pal.ZeroIndexTransparent, pal.Entries);
            SwatchControl.Invalidate();
        }

        private void NudEntries_ValueChanged(object sender, EventArgs e)
        {
        }

        private void ActiveColorPanel_Paint(object sender, PaintEventArgs e)
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

        private void SwatchControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveColor = pal.GetColor(SwatchControl.SelectedIndex);
            (byte A, byte R, byte G, byte B) = pal.GetForeignColor(SwatchControl.SelectedIndex).Split(pal.ColorModel);

            ForeignAlpha = A;
            ForeignRed = R;
            ForeignGreen = G;
            ForeignBlue = B;
        }

        private void ReloadPaletteButton_Click(object sender, EventArgs e)
        {
            ReloadContent();
        }

        private void SavePaletteButton_Click(object sender, EventArgs e)
        {
            SaveContent();
            // PaletteContentsModified?.Invoke(this, null); // Not necessary?
        }

        /// <summary>
        /// Save palette colors to memory
        /// Used for temporary saving / previewing
        /// </summary>
        private void SavePaletteColors()
        {
            pal.SetPaletteForeignColor(SwatchControl.SelectedIndex, (byte)SliderAlpha.Value, (byte)SliderRed.Value, (byte)SliderGreen.Value, (byte)SliderBlue.Value);
            SwatchControl.Invalidate();

            ContainsModifiedContent = true;
            OnContentModified(EventArgs.Empty);
        }
    }

}
