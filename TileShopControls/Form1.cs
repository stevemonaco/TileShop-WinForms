using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TileShopControls
{
    public partial class Form1 : Form
    {
        Slider slider;
        public Form1()
        {
            InitializeComponent();
            slider = new Slider(10, new Size(100, 30));
            this.Width = 500;
            slider.Location = new Point(50, 50);
            slider.Visible = true;
            this.Controls.Add(slider);
        }
    }
}
