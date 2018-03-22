using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace TileShop
{
    public abstract class ResourceNode : TreeNode
    {
        public abstract void BuildContextMenu(ContextMenu Menu);

        public string GetNodeKey()
        {
            return this.FullPath + this.Name;
        }

        public string GetNodePath()
        {
            return this.FullPath;
        }

        /// <summary>
        /// Path to folder where the node is contained
        /// </summary>
        /*protected string folderPath;

        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                MoveNodeToPath();
            }
        }

        private void MoveNodeToPath()
        {
            TreeView parentTree = this.TreeView;
        }*/
    }

    public class FolderNode : ResourceNode
    {
        public FolderNode()
        {
            ImageIndex = 0;
            SelectedImageIndex = 0;
        }

        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();
            Menu.MenuItems.Add(new MenuItem("Add New Folder", AddFolder_Click));
            Menu.MenuItems.Add(new MenuItem("Remove Folder", RemoveFolder_Click));
        }

        public void AddFolder_Click(object sender, System.EventArgs e)
        {

        }

        public void RemoveFolder_Click(object sender, System.EventArgs e)
        {
            if (this.Nodes.Count > 0) // This folder contains child nodes that will also need to be removed; warn user
            {
                DialogResult dr = MessageBox.Show("Folder contains subitems that will also be removed. Continue?", "Remove Folder", MessageBoxButtons.YesNo);
                if (dr == DialogResult.No)
                    return;

                MessageBox.Show("Feature not implemented");
                // TODO: Remove subitems
            }
            else
                this.Remove();
        }
    }

    public class DataFileNode : ResourceNode
    {
        public DataFileNode()
        {
            ImageIndex = 2;
            SelectedImageIndex = 2;
        }

        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();

            Menu.MenuItems.Add(new MenuItem("Open File in Viewer", OpenFile_Click));
            Menu.MenuItems.Add(new MenuItem("Remove File from Project", RemoveFile_Click));
        }

        public void OpenFile_Click(object sender, System.EventArgs e)
        {

        }

        public void RemoveFile_Click(object sender, System.EventArgs e)
        {
            this.Remove();
        }
    }

    public class PaletteNode : ResourceNode
    {
        public PaletteNode()
        {
            ImageIndex = 4;
            SelectedImageIndex = 4;
        }

        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();

            Menu.MenuItems.Add(new MenuItem("Edit Palette", EditPalette_Click));
            Menu.MenuItems.Add(new MenuItem("Remove Palette from Project", RemovePalette_Click));
        }

        public void EditPalette_Click(object sender, System.EventArgs e)
        {
            //PaletteEditor pe = new PaletteEditor("CharMapPal1");


        }

        public void RemovePalette_Click(object sender, System.EventArgs e)
        {

            this.Remove();
        }
    }

    public class ArrangerNode : ResourceNode
    {
        public ArrangerNode()
        {
            ImageIndex = 3;
            SelectedImageIndex = 3;
        }

        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();

            Menu.MenuItems.Add(new MenuItem("Open Arranger", OpenArranger_Click));
            Menu.MenuItems.Add(new MenuItem("Resize Arranger", ResizeArranger_Click));
            Menu.MenuItems.Add(new MenuItem("Export Arranger", ExportArranger_Click));
            Menu.MenuItems.Add("-");
            Menu.MenuItems.Add(new MenuItem("Remove Arranger", RemoveArranger_Click));
        }

        public void OpenArranger_Click(object sender, System.EventArgs e)
        {

        }

        public void ResizeArranger_Click(object sender, System.EventArgs e)
        {

        }

        public void ExportArranger_Click(object sender, System.EventArgs e)
        {

        }

        public void RemoveArranger_Click(object sender, System.EventArgs e)
        {
            this.Remove();
        }
    }
}
