using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace TileShop
{
    public partial class SwatchControl : UserControl
    {
        Palette pal = null;  // Palette to show
        int MaxColors = 0; // Maximum colors of pal to show

        // Palette swatch rendering
        public int swatchScale = 2;
        public Point swatchLoc = new Point(12, 4);
        public Size swatchElementSize = new Size(12, 12);
        public Size swatchElementPadding = new Size(2, 2);

        public event EventHandler<EventArgs> SelectedIndexChanged;

        public SwatchControl()
        {
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, this, new object[] { true }); // Enable double buffering
        }

        /// <summary>
        /// Constructs a new instance of SwatchControl
        /// </summary>
        /// <param name="PaletteName">Palette to show</param>
        /// <param name="MaxColorsDisplayed">Maximum palette entries to display. Default shows all entries</param>
        public SwatchControl(string PaletteName, int MaxColorsDisplayed = 0)
        {
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, this, new object[] { true }); // Enable double buffering

            pal = FileManager.Instance.GetPalette(PaletteName);
            if (MaxColorsDisplayed == 0)
                MaxColors = pal.Entries;
            else
                MaxColors = MaxColorsDisplayed;
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    SelectedIndexChanged?.Invoke(this, null);
                    Invalidate(); // Redraw borders
                }
            }
        }
        private int selectedIndex; // 0-based index

        /// <summary>
        /// Shows the specified palette inside of the SwatchControl
        /// </summary>
        /// <param name="palette">Palette to show</param>
        /// <param name="MaxColorsDisplayed">Maximum palette entries to display</param>
        public void ShowPalette(Palette palette, int MaxColorsDisplayed)
        {
            pal = palette ?? throw new ArgumentNullException();

            if (MaxColorsDisplayed > pal.Entries && MaxColorsDisplayed < 1)
                throw new ArgumentOutOfRangeException();

            MaxColors = MaxColorsDisplayed;

            Invalidate();
        }

        /*public void SetPaletteName(string PaletteName)
        {
            if (String.IsNullOrEmpty(PaletteName))
                throw new ArgumentException();
            else
                pal = FileManager.Instance.GetPalette(PaletteName);
            Invalidate();
        }*/

        protected override void OnPaint(PaintEventArgs e)
        {
            if (pal == null)
                return;

            int palx = 0, paly = 0;
            Rectangle DrawRect = new Rectangle(swatchLoc, new Size(swatchScale * swatchElementSize.Width, swatchScale * swatchElementSize.Height));

            for (int idx = 0; idx < MaxColors; idx++)
            {
                //e.Graphics.DrawRectangle(Pens.Black, DrawRect);
                Rectangle HighlightRect = DrawRect;
                HighlightRect.Inflate(1, 1);

                Brush b = new SolidBrush(Color.FromArgb((int)(pal[idx] | 0xFF000000))); // Draw the color with no transparency
                e.Graphics.FillRectangle(b, DrawRect);

                // Draw border
                if (idx == SelectedIndex)
                    ControlPaint.DrawBorder3D(e.Graphics, HighlightRect, Border3DStyle.Raised, Border3DSide.Left | Border3DSide.Top | Border3DSide.Right | Border3DSide.Bottom);
                else
                    ControlPaint.DrawBorder3D(e.Graphics, HighlightRect, Border3DStyle.Sunken, Border3DSide.Left | Border3DSide.Top | Border3DSide.Right | Border3DSide.Bottom);

                palx++;
                DrawRect.X += (swatchElementSize.Width + swatchElementPadding.Width) * swatchScale;

                if (palx >= 16)
                {
                    paly++;
                    DrawRect.Y += (swatchElementSize.Height + swatchElementPadding.Height) * swatchScale;

                    palx = 0;
                    DrawRect.X = swatchLoc.X;
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (pal == null)
                return;

            int x = e.Location.X;
            int y = e.Location.Y;

            Point unscaledLoc = new Point()
            {
                X = ((x - swatchLoc.X) / swatchScale) % (swatchElementSize.Width + swatchElementPadding.Width),
                Y = ((y - swatchLoc.Y) / swatchScale) % (swatchElementSize.Height + swatchElementPadding.Height)
            };
            Point palIndex = new Point()
            {
                X = ((x - swatchLoc.X) / swatchScale) / (swatchElementSize.Width + swatchElementPadding.Width),
                Y = ((y - swatchLoc.Y) / swatchScale) / (swatchElementSize.Height + swatchElementPadding.Height)
            };

            if (unscaledLoc.X < swatchElementSize.Width && unscaledLoc.Y < swatchElementSize.Height)
            {
                int idx = palIndex.X + palIndex.Y * 16;
                if (idx < MaxColors)
                    SelectedIndex = idx;
            }
        }
    }
}
