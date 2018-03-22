using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace TileShop.Controls
{
    public class CheckedDropDownButton : ToolStripDropDownButton
    {
        [Category("Appearance")]
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                Invalidate();
            }
        }
        private bool _checked;

        protected override void OnPaint(PaintEventArgs e)
        {
            if(this.Checked)
            {
                using (Pen p = new Pen(Color.FromArgb(144, 200, 246), 1.0f))
                {
                    e.Graphics.Clear(Color.FromArgb(192, 220, 243)); // Draw highlighted background

                    RectangleF rect = e.Graphics.VisibleClipBounds;

                    // Height of ToolStripButtons seem to not extend all the way to the edge
                    e.Graphics.DrawRectangle(p, 0, 0, this.Size.Width - 1, this.Size.Height - 2);
                    base.OnPaint(e);
                }
            }

            base.OnPaint(e); // Let the base renderer draw the image icon and drop down arrow
        }
    }

}
