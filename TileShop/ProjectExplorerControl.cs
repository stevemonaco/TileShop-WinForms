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
    public partial class ProjectExplorerControl : DockContent
    {
        TileShopForm tsf = null;

        public ProjectExplorerControl(TileShopForm tileShopForm)
        {
            tsf = tileShopForm;
            InitializeComponent();
        }

        // Full filename with path
        public bool AddFile(string Filename)
        {
            TreeNode tn = new TreeNode();
            tn.Text = Filename;
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

        public bool AddArranger(string ArrangerName)
        {
            TreeNode tn = new TreeNode();
            tn.Text = ArrangerName;
            tn.Tag = ArrangerName;

            arrangersTreeView.Nodes.Add(tn);
            return true;
        }

        public bool RemoveArranger(string Filename)
        {
            foreach (TreeNode tn in arrangersTreeView.Nodes)
            {
                if ((string)tn.Tag == Filename)
                {
                    tn.Remove();
                    break;
                }
            }

            return true;
        }

        public bool ClearAll()
        {
            filesTreeView.Nodes.Clear();
            arrangersTreeView.Nodes.Clear();
            palettesTreeView.Nodes.Clear();

            return true;
        }

        private void arrangersTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            GraphicsViewerMdiChild gv = new GraphicsViewerMdiChild((TileShopForm)this.Parent, (string)e.Node.Tag);
        }

        private void filesTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tsf.openExistingArranger(e.Node.Text);
        }
    }
}
