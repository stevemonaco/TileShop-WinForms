using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileShop
{
    public partial class SwatchControl : UserControl
    {
        Palette pal = null;

        // Palette swatch rendering
        public int swatchScale = 2;
        public Point swatchLoc = new Point(12, 4);
        public Size swatchElementSize = new Size(12, 12);
        public Size swatchElementPadding = new Size(2, 2);

        private int selectedIndex; // 0-based index
        public event EventHandler<EventArgs> SelectedIndexChanged;

        public SwatchControl()
        {

        }

        public SwatchControl(string PaletteName)
        {
            InitializeComponent();
            pal = FileManager.Instance.GetPalette(PaletteName);
        }

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                Invalidate(); // Redraw borders
            }
        }

        public void SetPaletteName(string PaletteName)
        {
            if (String.IsNullOrEmpty(PaletteName))
                pal = null;
            else
                pal = FileManager.Instance.GetPalette(PaletteName);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (pal == null)
                return;

            int palx = 0, paly = 0;
            Rectangle DrawRect = new Rectangle(swatchLoc, new Size(swatchScale * swatchElementSize.Width, swatchScale * swatchElementSize.Height));

            for (int idx = 0; idx < pal.Entries; idx++)
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

            Point unscaledLoc = new Point();
            unscaledLoc.X = ((x - swatchLoc.X) / swatchScale) % (swatchElementSize.Width + swatchElementPadding.Width);
            unscaledLoc.Y = ((y - swatchLoc.Y) / swatchScale) % (swatchElementSize.Height + swatchElementPadding.Height);

            Point palIndex = new Point();
            palIndex.X = ((x - swatchLoc.X) / swatchScale) / (swatchElementSize.Width + swatchElementPadding.Width);
            palIndex.Y = ((y - swatchLoc.Y) / swatchScale) / (swatchElementSize.Height + swatchElementPadding.Height);

            if (unscaledLoc.X < swatchElementSize.Width && unscaledLoc.Y < swatchElementSize.Height)
            {
                int idx = palIndex.X + palIndex.Y * 16;
                if (idx < pal.Entries)
                {
                    if (SelectedIndex != idx)
                    {
                        SelectedIndex = idx;
                        if (SelectedIndexChanged != null)
                            SelectedIndexChanged(this, null);
                        Invalidate();
                    }
                }
            }
        }
    }
}
