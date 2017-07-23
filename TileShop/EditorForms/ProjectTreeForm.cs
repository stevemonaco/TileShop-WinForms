using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TileShop
{
    public partial class ProjectTreeForm : DockContent
    {
        TileShopForm tsf = null;
        ContextMenu contextMenu = new ContextMenu();

        public ProjectTreeForm(TileShopForm tileShopForm)
        {
            tsf = tileShopForm ?? throw new ArgumentNullException();

            InitializeComponent();
        }

        /// <summary>
        /// Adds a DataFile to the ProjectTreeView and to FileManager
        /// </summary>
        /// <param name="Filename">Filename of the file to add</param>
        /// <param name="NodePath">Folder node path into the TreeView</param>
        /// <param name="Show">Optionally displays the file using a sequential arranger immediately</param>
        /// <returns></returns>
        public bool AddDataFile(DataFile df, string NodePath, bool Show = false)
        {
            if (df == null || NodePath == null)
                throw new ArgumentNullException();

            // Ensure the file has not been previously added
            TreeNode tn = FindNode(df.Name, NodePath); // Refactor to HasDataFile checks

            if (tn != null) // File has already been added
                return false;

            DataFileNode fileNode = new DataFileNode()
            {
                Text = df.Name,
                Tag = df.Name
            };

            string fileKey = Path.Combine(NodePath, df.Name);
            if (!ResourceManager.Instance.AddDataFile(df, fileKey))
                return false;

            // TODO: Refactor

            if (NodePath == "") // Add to root
                ProjectTreeView.Nodes.Add(fileNode);
            else // Add to folder
            {
                FolderNode folderNode = AddFolderNode(NodePath);
                folderNode.Nodes.Add(fileNode);
            }

            if (Show)
            {
                ProjectTreeView.SelectedNode = fileNode;
                return ShowSequentialArranger(fileKey);
            }

            return true;
        }

        /// <summary>
        /// Removes a file from the TreeView
        /// </summary>
        /// <param name="Filename">Fully qualified filename</param>
        /// <param name="NodePath">Folder node path into the TreeView</param>
        /// <returns></returns>
        public bool RemoveFile(string Filename, string NodePath)
        {
            TreeNode tn = FindNode(Filename, NodePath);

            if (tn == null) // Not found
                return false;

            if (!(tn is DataFileNode))
                throw new InvalidOperationException("Attempted to RemoveFile on a TreeView node that is not a FileNode");

            tn.Remove();
            ResourceManager.Instance.RemoveFile(Filename);

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

            if (tn == null) // Not found
                return false;

            if (!(tn is DataFileNode))
                throw new InvalidOperationException("Attempted to RenameFile on a TreeView node that is not a FileNode");

            if (!ResourceManager.Instance.RenameFile(Filename, NewFilename))
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

            if (tn == null) // Not found
                return false;

            if (!(tn is PaletteNode))
                throw new InvalidOperationException("Attempted to RenamePalette on a TreeView node that is not a PaletteNode");

            if (!ResourceManager.Instance.RenamePalette(PaletteName, NewPaletteName))
                return false;

            tn.Text = NewPaletteName;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ArrangerName"></param>
        /// <param name="NodePath"></param>
        /// <param name="NewArrangerName"></param>
        /// <returns></returns>
        public bool RenameArranger(string ArrangerName, string NodePath, string NewArrangerName)
        {
            TreeNode tn = FindNode(ArrangerName, NodePath);

            if (tn == null) // Not found
                return false;

            if (!(tn is ArrangerNode))
                throw new InvalidOperationException("Attempted to RenameArranger on a TreeView node that is not an ArrangerNode");

            if (!ResourceManager.Instance.RenameArranger(ArrangerName, NewArrangerName))
                return false;

            tn.Text = NewArrangerName;

            return true;
        }

        /// <summary>
        /// Adds an Arranger to the TreeView and to FileManager
        /// </summary>
        /// <param name="arr">Arranger to add</param>
        /// <param name="NodePath">Folder node path into the TreeView</param>
        /// <param name="Show">Optionally shows the arranger immediately</param>
        /// <returns></returns>
        public bool AddArranger(Arranger arr, string NodePath, bool Show = false)
        {
            if (arr == null || NodePath == null)
                throw new ArgumentNullException();

            if(arr.Mode != ArrangerMode.ScatteredArranger)
                throw new NotSupportedException("AddArranger does not support arranger types other than ScatteredArranger");

            // Ensure arranger of the same name and path has not been previously added
            TreeNode tn = FindNode(arr.Name, NodePath);

            if (tn != null) // File has already been added
                return false;

            ArrangerNode an = new ArrangerNode()
            {
                Text = arr.Name,
                Tag = arr.Name
            };

            if (NodePath == "") // Add to root
                ProjectTreeView.Nodes.Add(an);
            else
            {
                FolderNode folderNode = AddFolderNode(NodePath);
                folderNode.Nodes.Add(an);
            }

            string arrangerKey = Path.Combine(NodePath, arr.Name);
            ResourceManager.Instance.AddArranger(arr, arrangerKey);

            if (Show)
                return ShowScatteredArranger(arrangerKey);

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

            if (tn == null)
                return false;

            if (!(tn is ArrangerNode))
                throw new InvalidOperationException("Attempted to RemoveArranger on a TreeView node that is not an ArrangerNode");

            return false;
        }

        /// <summary>
        /// Adds an Arranger to the TreeView and to FileManager
        /// </summary>
        /// <param name="pal">Palette to be added</param>
        /// <param name="NodePath">Folder node path into the TreeView</param>
        /// <returns></returns>
        public bool AddPalette(Palette pal, string NodePath)
        {
            PaletteNode pn = new PaletteNode();
            pn.Text = pal.Name;
            pn.Tag = pal.Name;

            if (NodePath == "") // Add to root
                ProjectTreeView.Nodes.Add(pn);
            else
            {
                FolderNode folderNode = AddFolderNode(NodePath);
                folderNode.Nodes.Add(pn);
            }

            string key = Path.Combine(NodePath, pal.Name);
            ResourceManager.Instance.AddPalette(pal, key);

            return true;
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

            if (tn == null)
                return false;

            if (!(tn is ArrangerNode))
                throw new InvalidOperationException("Attempted to RemovePalette on a TreeView node that is not an PaletteNode");

            return false;
        }

        /// <summary>
        /// Shows a Sequential Arranger as a docked document tab if not already opened
        /// </summary>
        /// <param name="dataFileKey">Filename of a datafile previously added to FileManager</param>
        /// <returns></returns>
        public bool ShowSequentialArranger(string dataFileKey)
        {
            List<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            foreach (EditorDockContent dc in activeEditors) // Return if an editor is already opened
            {
                if (dc.ContentSourceName == dataFileKey && dc is ArrangerViewerForm)
                    return false;
            }

            if (ResourceManager.Instance.LoadSequentialArranger(dataFileKey))
            {
                ArrangerViewerForm avf = new ArrangerViewerForm(dataFileKey);
                avf.WindowState = FormWindowState.Maximized;
                avf.SetZoom(6);
                avf.Show(tsf.DockPanel, DockState.Document);

                avf.ContentModified += tsf.ViewerContentModified;
                avf.ContentSaved += tsf.ContentSaved;
                avf.EditArrangerChanged += tsf.EditArrangerChanged;
                avf.ClearEditArranger();

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Shows a Scattered Arranger
        /// </summary>
        /// <param name="ArrangerKey">Key of Arranger in FileManager to show</param>
        /// <returns></returns>
        public bool ShowScatteredArranger(string ArrangerKey)
        {
            List<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            foreach (EditorDockContent dc in activeEditors) // Return if an editor is already opened
            {
                if (dc.ContentSourceKey == ArrangerKey && dc is ArrangerViewerForm)
                    return false;
            }

            ArrangerViewerForm gv = new ArrangerViewerForm(ArrangerKey);
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
        /// <param name="PaletteKey">Key of Palette in FileManager to show</param>
        /// <returns></returns>
        public bool ShowPaletteEditor(string PaletteKey)
        {
            if (!ResourceManager.Instance.HasPalette(PaletteKey))
                return false;

            List<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            foreach (EditorDockContent dc in activeEditors) // Return if an editor is already opened
            {
                if (dc.ContentSourceKey == PaletteKey && dc is PaletteEditorForm)
                    return false;
            }

            PaletteEditorForm pef = new PaletteEditorForm(PaletteKey);
            pef.ContentModified += tsf.PaletteContentModified;
            pef.ContentSaved += tsf.ContentSaved;
            pef.Show(tsf.DockPanel, DockState.Document);

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
            ResourceManager.Instance.CloseProject();

            return true;
        }

        public bool LoadProject(string XmlFileName)
        {
            GameDescriptorSerializer gds = new GameDescriptorSerializer();
            return gds.LoadProject(ProjectTreeView.Nodes, XmlFileName);
        }

        /// <summary>
        /// Iterates over tree nodes and saves project settings to XML
        /// </summary>
        /// <param name="XmlFileName"></param>
        /// <returns></returns>
        public bool SaveProject(string XmlFileName)
        {
            var root = new XElement("gdf");

            // Save settings
            var settings = new XElement("settings");
            //XElement numberformat = new XElement("filelocationnumberformat");
            //numberformat.SetValue("hexadecimal");
            //settings.Add(numberformat);

            root.Add(settings);

            // Save each data file
            var datafiles = new XElement("datafiles");
            var palettes = new XElement("palettes");
            var arrangers = new XElement("arrangers");

            foreach (TreeNode tn in ProjectTreeView.GetAllNodes())
            {
                if (tn is DataFileNode fn)
                {
                    var xmlfile = new XElement("file");
                    xmlfile.SetAttributeValue("location", fn.Text);
                    xmlfile.SetAttributeValue("folder", fn.GetNodePath());
                    datafiles.Add(xmlfile);
                }
                else if (tn is PaletteNode pn)
                {
                    Palette pal = ResourceManager.Instance.GetPersistentPalette(pn.Text);
                    var xmlpal = new XElement("palette");
                    xmlpal.SetAttributeValue("name", pal.Name);
                    xmlpal.SetAttributeValue("folder", pn.GetNodePath());
                    xmlpal.SetAttributeValue("fileoffset", String.Format("{0:X}", pal.FileAddress.FileOffset));
                    xmlpal.SetAttributeValue("bitoffset", String.Format("{0:X}", pal.FileAddress.BitOffset));
                    xmlpal.SetAttributeValue("datafile", pal.DataFileKey);
                    xmlpal.SetAttributeValue("format", Palette.ColorFormatToString(pal.ColorFormat));
                    xmlpal.SetAttributeValue("entries", pal.Entries);
                    xmlpal.SetAttributeValue("zeroindextransparent", pal.ZeroIndexTransparent);
                    palettes.Add(xmlpal);
                }
                else if (tn is ArrangerNode an)
                {
                    Arranger arr = ResourceManager.Instance.GetPersistentArranger(an.Text);
                    var xmlarr = new XElement("arranger");
                    xmlarr.SetAttributeValue("name", arr.Name);
                    xmlarr.SetAttributeValue("folder", an.GetNodePath());
                    xmlarr.SetAttributeValue("elementsx", arr.ArrangerElementSize.Width);
                    xmlarr.SetAttributeValue("elementsy", arr.ArrangerElementSize.Height);
                    xmlarr.SetAttributeValue("width", arr.ElementPixelSize.Width);
                    xmlarr.SetAttributeValue("height", arr.ElementPixelSize.Height);

                    if (arr.Layout == ArrangerLayout.TiledArranger)
                        xmlarr.SetAttributeValue("layout", "tiled");
                    else if (arr.Layout == ArrangerLayout.LinearArranger)
                        xmlarr.SetAttributeValue("layout", "linear");

                    string DefaultPalette = FindMostFrequentValue(arr, "PaletteName");
                    string DefaultFile = FindMostFrequentValue(arr, "FileName");
                    string DefaultFormat = FindMostFrequentValue(arr, "FormatName");

                    xmlarr.SetAttributeValue("defaultformat", DefaultFormat);
                    xmlarr.SetAttributeValue("defaultfile", DefaultFile);
                    xmlarr.SetAttributeValue("defaultpalette", DefaultPalette);

                    for (int y = 0; y < arr.ArrangerElementSize.Height; y++)
                    {
                        for (int x = 0; x < arr.ArrangerElementSize.Width; x++)
                        {
                            var graphic = new XElement("graphic");
                            ArrangerElement el = arr.GetElement(x, y);

                            graphic.SetAttributeValue("fileoffset", String.Format("{0:X}", el.FileAddress.FileOffset));
                            graphic.SetAttributeValue("bitoffset", String.Format("{0:X}", el.FileAddress.BitOffset));
                            graphic.SetAttributeValue("posx", x);
                            graphic.SetAttributeValue("posy", y);
                            if (el.FormatName != DefaultFormat)
                                graphic.SetAttributeValue("format", el.FormatName);
                            if (el.DataFileKey != DefaultFile)
                                graphic.SetAttributeValue("file", el.DataFileKey);
                            if (el.PaletteKey != DefaultPalette)
                                graphic.SetAttributeValue("palette", el.PaletteKey);

                            xmlarr.Add(graphic);
                        }
                    }

                    arrangers.Add(xmlarr);
                }
            }

            root.Add(datafiles);
            root.Add(palettes);
            root.Add(arrangers);

            root.Save(XmlFileName);

            return true;
        }

        /// <summary>
        /// Find most frequent of an attribute within an arranger's elements
        /// </summary>
        /// <param name="arr">Arranger to search</param>
        /// <param name="attributeName">Name of the attribute to find most frequent value of</param>
        /// <returns></returns>
        private string FindMostFrequentValue(Arranger arr, string attributeName)
        {
            Dictionary<string, int> freq = new Dictionary<string, int>();
            Type T = typeof(ArrangerElement);
            PropertyInfo P = T.GetProperty(attributeName);

            foreach (ArrangerElement el in arr.ElementGrid)
            {
                string s = (string)P.GetValue(el);

                if (s == "")
                    continue;

                if (freq.ContainsKey(s))
                    freq[s]++;
                else
                    freq.Add(s, 1);
            }

            var max = freq.FirstOrDefault(x => x.Value == freq.Values.Max()).Key;

            return max;
        }

        /// <summary>
        /// Gets a list of all filenames loaded into the FileManager
        /// </summary>
        /// <returns></returns>
        public List<string> GetFileNameList()
        {
            List<string> list = new List<string>();

            foreach (TreeNode tn in ProjectTreeView.Nodes)
            {
                if (tn is FolderNode)
                {
                    List<string> nodeFileNameList = GetChildFileNames(tn);
                    foreach (string s in nodeFileNameList)
                        list.Add(s);
                }
                else if (tn is DataFileNode)
                    list.Add(tn.Text);
            }

            return list;
        }

        /// <summary>
        /// Gets a list of all filenames from all children nodes. Recursive method.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        private List<string> GetChildFileNames(TreeNode parentNode)
        {
            List<string> ret = new List<string>();
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                if (childNode is FolderNode)
                {
                    List<string> childFileNames = GetChildFileNames(childNode);
                    foreach (string s in childFileNames)
                        ret.Add(s);
                }
                else if (childNode is DataFileNode)
                    ret.Add(childNode.Text);
            }

            return ret;
        }

        private void ProjectTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node is DataFileNode)
            {
                ShowSequentialArranger(e.Node.FullPath);

            }
            else if (e.Node is PaletteNode)
            {
                ShowPaletteEditor(e.Node.FullPath);
            }
            else if (e.Node is ArrangerNode)
            {
                ShowScatteredArranger(e.Node.FullPath);
            }
        }

        private void ProjectTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // Show context menu
            {
                ProjectTreeNode ptn = (ProjectTreeNode)e.Node;
                ptn.BuildContextMenu(contextMenu);
                contextMenu.Show(ProjectTreeView, e.Location);
            }
        }

        /// <summary>
        /// Adds a folder node (and any necessary parent folder nodes) to the TreeView
        /// </summary>
        /// <param name="FolderNodePath">Full path of the folder node to add</param>
        /// <returns>The FolderNode that was added or found on success. Null on failure.</returns>
        public FolderNode AddFolderNode(string FolderNodePath)
        {
            if (FolderNodePath == null)
                throw new ArgumentNullException();

            if (FolderNodePath == "")
                return null;

            string[] nodepaths = FolderNodePath.Split(new char[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            // Find deepest node collection that matches as much of the path as possible
            TreeNodeCollection deepestCollection = ProjectTreeView.Nodes;
            FolderNode fn = null;
            int idx = 0; // Index of nodepath to start adding nodes with
            string currentPath = "";

            foreach (string path in nodepaths)
            {
                bool nodeExists = false;
                foreach (TreeNode tn in deepestCollection)
                {
                    if (tn.Text == path && tn is FolderNode) // Found next segment of path
                    {
                        deepestCollection = tn.Nodes;
                        fn = (FolderNode)tn;
                        nodeExists = true;
                        currentPath = currentPath + nodepaths[idx] + "\\";
                        idx++;
                        break;
                    }
                }

                if (!nodeExists)
                    break;
            }

            if (currentPath != "")
                currentPath.Remove(currentPath.Length - 1, 1);

            for (int i = idx; i < nodepaths.Length; i++)
            {
                FolderNode fnadd = new FolderNode();
                currentPath = currentPath + "\\" + nodepaths[i];
                fnadd.Text = nodepaths[i];
                deepestCollection.Add(fnadd);
                deepestCollection = fnadd.Nodes;
                fn = fnadd;
            }

            return fn;
        }

        /// <summary>
        /// Finds a folder node associated with a path
        /// </summary>
        /// <param name="NodePath">Path to folder node</param>
        /// <returns>A FolderNode on success, null on failure</returns>
        private FolderNode FindFolderNode(string NodePath)
        {
            if (NodePath == null)
                throw new ArgumentNullException();

            if (NodePath == "")
                return null;

            string[] nodePaths = NodePath.Split('\\');

            TreeNodeCollection nodeLevel = ProjectTreeView.Nodes;
            FolderNode matchedNode = null;

            foreach (string path in nodePaths)
            {
                bool nodeExists = false;
                foreach (TreeNode tn in nodeLevel)
                {
                    if (tn.Text == path && tn is FolderNode) // Found next segment of path
                    {
                        matchedNode = (FolderNode)tn;
                        nodeLevel = tn.Nodes;
                        nodeExists = true;
                        break;
                    }
                }

                if (!nodeExists)
                    return null;
            }

            return matchedNode;
        }

        /// <summary>
        /// Finds a node within the ProjectTreeView
        /// </summary>
        /// <param name="NodeName">Name of the node</param>
        /// <param name="NodePath">Path to the node</param>
        /// <returns>The found TreeNode on success, null on failure</returns>
        private TreeNode FindNode(string NodeName, string NodePath)
        {
            FolderNode fn = FindFolderNode(NodePath);

            if (fn == null)
                return null;

            foreach (TreeNode tn in fn.Nodes)
            {
                if (tn.Text == NodeName)
                    return tn;
            }

            return null;
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
            ProjectTreeNode dragNode = null;
            bool moveChildren = false;

            if (e.Data.GetDataPresent(typeof(DataFileNode)))
                dragNode = (DataFileNode)e.Data.GetData(typeof(DataFileNode));
            if (e.Data.GetDataPresent(typeof(FolderNode)))
                dragNode = (FolderNode)e.Data.GetData(typeof(FolderNode));
            if (e.Data.GetDataPresent(typeof(PaletteNode)))
            {
                dragNode = (PaletteNode)e.Data.GetData(typeof(PaletteNode));
                moveChildren = true;
            }
            if (e.Data.GetDataPresent(typeof(ArrangerNode)))
                dragNode = (ArrangerNode)e.Data.GetData(typeof(ArrangerNode));
            else
                return;

            Point p = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            ProjectTreeNode dropNode = (ProjectTreeNode)((TreeView)sender).GetNodeAt(p);

            if (moveChildren) // FolderNode dragdrop
            {
                if (!(dropNode is FolderNode))
                    return;

                dragNode.Remove();
                dropNode.Nodes.Add((TreeNode)dragNode);
            }
            else // ArrangerNode, FileNode, PaletteNode all must be moved into a folder node (or root)
            {
                if (!(dropNode is FolderNode)) // All nodes must be attached to a folder node
                    return;

                dragNode.Remove();
                dropNode.Nodes.Add((TreeNode)dragNode);
            }
        }

        private void ProjectTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (!(e.Node is FolderNode))
                throw new InvalidOperationException("ProjectTreeView attempted to collapse a node that isn't a FolderNode");

            FolderNode fn = (FolderNode)e.Node;
            fn.ImageIndex = 0;
            fn.SelectedImageIndex = 0;
        }

        private void ProjectTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!(e.Node is FolderNode))
                throw new InvalidOperationException("ProjectTreeView attempted to expand a node that isn't a FolderNode");

            FolderNode fn = (FolderNode)e.Node;
            fn.ImageIndex = 1;
            fn.SelectedImageIndex = 1;
        }

        private void ProjectTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // Check for naming conflicts
        }
    }

    /// <summary>
    /// Extension methods for TreeView
    /// </summary>
    public static class TreeViewExtensions
    {
        /// <summary>
        /// Gets a flat list of all nodes in the tree
        /// </summary>
        /// <param name="Self"></param>
        /// <returns></returns>
        public static List<TreeNode> GetAllNodes(this TreeView Self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in Self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        /// <summary>
        /// Gets a flat list of all child nodes
        /// </summary>
        /// <param name="Self"></param>
        /// <returns></returns>
        public static List<TreeNode> GetAllNodes(this TreeNode Self)
        {
            List<TreeNode> result = new List<TreeNode> { Self };
            foreach (TreeNode child in Self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }
    }

    /// <summary>
    /// Extension methods for TreeNode
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// Builds a node path for the specified node
        /// </summary>
        /// <param name="node">Specified node</param>
        /// <returns>Node path</returns>
        public static string GetNodePath(this TreeNode node)
        {
            string path = "";
            TreeNode currentNode = node.Parent;

            while (currentNode != null) // Traverse up parent nodes to build the node path
            {
                if (path == "")
                    path = currentNode.Text;
                else
                    path = currentNode.Text + "\\" + path;

                currentNode = currentNode.Parent;
            }

            return path;
        }
    }

    public class GameDescriptorSettings
    {
        public enum FileLocationNumberFormat { Decimal = 0, Hexadecimal = 1 }

        public FileLocationNumberFormat NumberFormat { set; get; }
    }
}
