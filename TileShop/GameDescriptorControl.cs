using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TileShop
{
    public partial class GameDescriptorControl : DockContent
    {
        public GameDescriptorControl()
        {
            InitializeComponent();
        }

        // Full filename with path
        public bool AddFile(string Filename)
        {
            TreeNode tn = new TreeNode();
            tn.Text = Path.GetFileName(Filename);
            tn.Tag = Filename;

            filesTreeView.Nodes.Add(tn);
            return true;
        }

        // Full filename with path
        public bool RemoveFile(string Filename)
        {
            foreach(TreeNode tn in filesTreeView.Nodes)
            {
                if((string)tn.Tag == Filename)
                {
                    tn.Remove();
                    break;
                }
            }

            return true;
        }
    }
}
