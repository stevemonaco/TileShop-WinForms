using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using TileShop.Core;
using TileShop.ExtensionMethods;

namespace TileShop
{
    public partial class ProjectTreeForm : DockContent
    {
        TileShopForm tsf = null;
        ContextMenu contextMenu = new ContextMenu();
        const int DefaultArrangerWidth = 8;
        const int DefaultArrangerHeight = 16;

        public ProjectTreeForm(TileShopForm tileShopForm)
        {
            tsf = tileShopForm ?? throw new ArgumentNullException();

            InitializeComponent();
            ProjectTreeView.TreeViewNodeSorter = new ResourceNodeSorter();
        }

        /// <summary>
        /// Adds a DataFile to the ProjectTreeView and to FileManager
        /// </summary>
        /// <param name="Filename">Absolute path of the file to add</param>
        /// <param name="nodeKey">Key to store the DataFile</param>
        /// <param name="show">Optionally displays the file using a sequential arranger immediately</param>
        /// <returns></returns>
        public bool AddDataFile(string filename, string nodeKey, bool show = false)
        {
            if (String.IsNullOrWhiteSpace(filename) || nodeKey is null)
                throw new ArgumentNullException();

            DataFile df = new DataFile(Path.GetFileNameWithoutExtension(filename), filename);

            // Ensure the file has not been previously added
            if(ProjectTreeView.Nodes.TryGetNode(df.Name, out _))// File has already been added
                return false;

            DataFileNode fileNode = new DataFileNode()
            {
                Text = df.Name,
                Tag = df.Name
            };

            if (!ResourceManager.AddResource(Path.Combine(nodeKey, df.Name), df))
                return false;

            AddSequentialArranger(df);

            if (show)
            {
                ProjectTreeView.SelectedNode = fileNode;
                return ShowArranger(Path.Combine(nodeKey, df.Name, df.Name + ".SequentialArranger"));
            }

            return true;
        }

        internal void OnResourceAdded(object sender, ResourceEventArgs e)
        {
            // Ensure the resource wasn't already added
            // This is a workaround for DataFiles loaded via XML projects having
            // their SequentialArranger nodes created twice otherwise.

            if (!ProjectTreeView.Nodes.TryGetNode(e.ResourceKey, out _))
            {
                ProjectResourceBase res = ResourceManager.GetResource<ProjectResourceBase>(e.ResourceKey);
                AddResourceAsNode(e.ResourceKey, res);

                if (res is DataFile df)
                {
                    AddSequentialArranger(df);
                }
            }
        }

        internal void AddSequentialArranger(DataFile df)
        {
            FileTypeLoader ftl = new FileTypeLoader();
            var arr = new SequentialArranger(8, 16, df.ResourceKey, ftl.GetDefaultFormat(df.Location));
            string arrangerName = df.Name + ".SequentialArranger";
            arr.Rename(arrangerName);
            arr.Parent = df;
            ResourceManager.AddResource(Path.Combine(df.ResourceKey, arrangerName), arr);
        }

        #region Remove* functions to refactor elsewhere later
        /// <summary>
        /// Removes a file from the TreeView
        /// </summary>
        /// <param name="Filename">Fully qualified filename</param>
        /// <param name="NodePath">Folder node path into the TreeView</param>
        /// <returns></returns>
        /*public bool RemoveFile(string Filename, string NodePath)
        {
            TreeNode tn = FindNode(Filename, NodePath);

            if (tn is null) // Not found
                return false;

            if (!(tn is DataFileNode))
                throw new InvalidOperationException("Attempted to RemoveFile on a TreeView node that is not a FileNode");

            tn.Remove();
            //ResourceManager.Instance.RemoveFile(Filename);

            return true;
        }

        /// <summary>
        /// Renames a file's node, its entry in FileManager, filename on disk, and remaps all project references
        /// </summary>
        /// <param name="Filename">File to be renamed</param>
        /// <param name="NodePath">Path to the file node</param>
        /// <param name="NewFilename">Name that the file will be renamed to</param>
        /// <returns></returns>
        public bool RenameFile(string Filename, string NodePath, string NewFilename)
        {
            TreeNode tn = FindNode(Filename, NodePath);

            if (tn is null) // Not found
                return false;

            if (!(tn is DataFileNode))
                throw new InvalidOperationException("Attempted to RenameFile on a TreeView node that is not a FileNode");

            if (!ResourceManager.RenameFile(Filename, NewFilename))
                return false;

            tn.Text = NewFilename;

            return true;
        }

        /// <summary>
        /// Renames a Palettes's node, its entry in FileManager, and remaps all project references
        /// </summary>
        /// <param name="PaletteName">Palette to be renamed</param>
        /// <param name="NodePath">Path to the palette node</param>
        /// <param name="NewPaletteName">Name that the palette will be renamed to</param>
        /// <returns></returns>
        public bool RenamePalette(string PaletteName, string NodePath, string NewPaletteName)
        {
            TreeNode tn = FindNode(PaletteName, NodePath);

            if (tn is null) // Not found
                return false;

            if (!(tn is PaletteNode))
                throw new InvalidOperationException("Attempted to RenamePalette on a TreeView node that is not a PaletteNode");

            if (!ResourceManager.RenamePalette(PaletteName, NewPaletteName))
                return false;

            tn.Text = NewPaletteName;

            return true;
        }

        /// <summary>
        /// Removes an Arranger from the TreeView
        /// </summary>
        /// <param name="ArrangerName">Name of the arranger to remove</param>
        /// <param name="NodePath">Folder node path into the TreeView</param>
        /// <returns></returns>
        public bool RemoveArranger(string ArrangerName, string NodePath)
        {
            TreeNode tn = FindNode(ArrangerName, NodePath);

            if (tn is null)
                return false;

            if (!(tn is ArrangerNode))
                throw new InvalidOperationException("Attempted to RemoveArranger on a TreeView node that is not an ArrangerNode");

            return false;
        }

        /// <summary>
        /// Removes a Palette from the TreeView
        /// </summary>
        /// <param name="PaletteName">Name of the palette to remove</param>
        /// <param name="NodePath">Folder node path into the TreeView</param>
        /// <returns></returns>
        public bool RemovePalette(string PaletteName, string NodePath)
        {
            TreeNode tn = FindNode(PaletteName, NodePath);

            if (tn is null)
                return false;

            if (!(tn is ArrangerNode))
                throw new InvalidOperationException("Attempted to RemovePalette on a TreeView node that is not an PaletteNode");

            return false;
        }*/
        #endregion

        /// <summary>
        /// Shows an Arranger
        /// </summary>
        /// <param name="arrangerKey">Key of Arranger in FileManager to show</param>
        /// <returns></returns>
        public bool ShowArranger(string arrangerKey)
        {
            IEnumerable<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            // If the Arranger has an active viewer, switch to it. Do not create a new one.
            var openedEditor = activeEditors.OfType<ArrangerViewerForm>().FirstOrDefault(x => x.ContentSourceKey == arrangerKey);
            if(openedEditor != null)
            {
                openedEditor.Show();
                return false;
            }

            // Create a new viewer
            ArrangerViewerForm gv = new ArrangerViewerForm(arrangerKey);
            gv.WindowState = FormWindowState.Maximized;
            gv.SetZoom(6);
            gv.Show(tsf.DockPanel, DockState.Document);

            gv.ContentModified += tsf.ViewerContentModified;
            gv.ContentSaved += tsf.ContentSaved;
            gv.EditArrangerChanged += tsf.EditArrangerChanged;
            gv.ClearEditArranger();

            return true;
        }

        /// <summary>
        /// Shows a Palette editor
        /// </summary>
        /// <param name="paletteKey">Key of Palette in FileManager to show</param>
        /// <returns>False if the Palette cannot be shown</returns>
        public bool ShowPaletteEditor(string paletteKey)
        {
            if (!ResourceManager.HasResource(paletteKey))
                return false;

            IEnumerable<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            var openedEditor = activeEditors.OfType<PaletteEditorForm>().FirstOrDefault(x => x.ContentSourceKey == paletteKey);
            if (openedEditor != null) // If the Palette has an active editor, show it. Do not create a new one.
            {
                openedEditor.Show();
            }
            else // Create a new editor and show it.
            {
                PaletteEditorForm pef = new PaletteEditorForm(paletteKey);
                pef.ContentModified += tsf.PaletteContentModified;
                pef.ContentSaved += tsf.ContentSaved;
                pef.Show(tsf.DockPanel, DockState.Document);
            }

            return true;
        }

        /// <summary>
        /// Used to remove all nodes in the project tree and removes everything loaded into the FileManager
        /// Sets IsProjectModified to false
        /// </summary>
        /// <returns></returns>
        public bool CloseProject()
        {
            ProjectTreeView.Nodes.Clear();
            ResourceManager.ClearResources();

            return true;
        }

        private bool AddResourceAsNode(string key, ProjectResourceBase Resource)
        {
            ResourceNode rn = null;
            if (Resource is DataFile df)
                rn = new DataFileNode(df);
            else if (Resource is Palette pal)
                rn = new PaletteNode(pal);
            else if (Resource is Arranger arr)
                rn = new ArrangerNode(arr);
            else if (Resource is ResourceFolder rf)
                rn = new FolderNode(rf.Name);

            string keyParent;
            int index = key.LastIndexOf(Path.DirectorySeparatorChar);
            if (index == -1)
                keyParent = "";
            else
                keyParent = key.Substring(0, index);

            TreeNode parentNode;
            if (ProjectTreeView.Nodes.TryGetParentNode(key, out parentNode))
                parentNode.Nodes.Add(rn);
            else
                ProjectTreeView.Nodes.Add(rn); // Add to the root

            return true;
        }

        public IEnumerable<string> GetFileNameList()
        {
            var nameList = new List<string>();

            var fileNodes = ProjectTreeView.Nodes.SelfAndDescendants().OfType<DataFileNode>();
            fileNodes.ForEach(x => { nameList.Add(x.NodeKey); });

            return nameList;
        }

        private void ProjectTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //if (e.Node is DataFileNode)
            //{
            //    ShowSequentialArranger(e.Node.FullPath);
            //}

            if (e.Node is PaletteNode)
            {
                ShowPaletteEditor(e.Node.FullPath);
            }
            else if (e.Node is ArrangerNode)
            {
                ShowArranger(e.Node.FullPath);
            }
        }

        private void ProjectTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // Show context menu
            {
                ResourceNode ptn = e.Node as ResourceNode;
                ptn.BuildContextMenu(contextMenu);
                contextMenu.Show(ProjectTreeView, e.Location);
            }
        }

        private void ProjectTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void ProjectTreeView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ProjectTreeView_DragDrop(object sender, DragEventArgs e)
        {
            ResourceNode dragNode = null;
            bool moveChildren = false;

            if (e.Data.GetDataPresent(typeof(DataFileNode)))
                dragNode = e.Data.GetData(typeof(DataFileNode)) as DataFileNode;
            if (e.Data.GetDataPresent(typeof(FolderNode)))
                dragNode = e.Data.GetData(typeof(FolderNode)) as FolderNode;
            if (e.Data.GetDataPresent(typeof(PaletteNode)))
            {
                dragNode = e.Data.GetData(typeof(PaletteNode)) as PaletteNode;
                moveChildren = true;
            }
            if (e.Data.GetDataPresent(typeof(ArrangerNode)))
                dragNode = e.Data.GetData(typeof(ArrangerNode)) as ArrangerNode;
            else
                return;

            Point p = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            ResourceNode dropNode = ((TreeView)sender).GetNodeAt(p) as ResourceNode;

            if (moveChildren) // FolderNode dragdrop
            {
                if (!(dropNode is FolderNode))
                    return;

                dragNode.Remove();
                dropNode.Nodes.Add(dragNode);
            }
            else // ArrangerNode, FileNode, PaletteNode all must be moved into a folder node (or root)
            {
                if (!(dropNode is FolderNode)) // All nodes must be attached to a folder node
                    return;

                dragNode.Remove();
                dropNode.Nodes.Add(dragNode);
            }
        }

        private void ProjectTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (!(e.Node is FolderNode) && !(e.Node is DataFileNode))
                throw new InvalidOperationException("ProjectTreeView attempted to collapse a node that isn't a FolderNode or DataFileNode");

            if (e.Node is FolderNode fn)
            {
                fn.ImageIndex = 0;
                fn.SelectedImageIndex = 0;
            }
        }

        private void ProjectTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!(e.Node is FolderNode) && !(e.Node is DataFileNode))
                throw new InvalidOperationException("ProjectTreeView attempted to expand a node that isn't a FolderNode or DataFileNode");

            if (e.Node is FolderNode fn)
            {
                fn.ImageIndex = 1;
                fn.SelectedImageIndex = 1;
            }
        }

        private void ProjectTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Check for naming conflicts
        }
    }

    public class GameDescriptorSettings
    {
        public enum FileLocationNumberFormat { Decimal = 0, Hexadecimal = 1 }

        public FileLocationNumberFormat NumberFormat { set; get; }
    }
}
