using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace TileShop
{
    public partial class ProjectExplorerControl : DockContent
    {
        TileShopForm tsf = null;
        ContextMenu contextMenu = new ContextMenu();

        public ProjectExplorerControl(TileShopForm tileShopForm)
        {
            tsf = tileShopForm ?? throw new ArgumentNullException();

            InitializeComponent();
        }

        /// <summary>
        /// Adds a file to the TreeView and to FileManager
        /// </summary>
        /// <param name="Filename">Filename of the file to add</param>
        /// <param name="Show">Optionally displays the file using a sequential arranger immediately</param>
        /// <returns></returns>
        public bool AddFile(string Filename, string NodePath, bool Show = false)
        {
            // Ensure the file has not been previously added
            TreeNode tn = FindNode(Filename, NodePath);

            if (tn != null) // File has already been added
                return false;

            FileNode fileNode = new FileNode()
            {
                Text = Filename,
                Tag = Filename
            };
            if (!FileManager.Instance.LoadFile(Filename))
                return false;

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
                return ShowSequentialArranger(Filename);
            }

            return true;
        }

        /// <summary>
        /// Removes a file from the TreeView
        /// </summary>
        /// <param name="Filename">Fully qualified filename</param>
        /// <returns></returns>
        public bool RemoveFile(string Filename, string NodePath)
        {
            TreeNode tn = FindNode(Filename, NodePath);

            if (tn == null) // Not found
                return false;

            if (tn.GetType() != typeof(FileNode))
                throw new InvalidOperationException("Attempted to RemoveFile on a TreeView node that is not a FileNode");

            tn.Remove();
            FileManager.Instance.RemoveFile(Filename);

            return true;
        }

        /// <summary>
        /// Adds an arranger to the TreeView and to FileManager
        /// </summary>
        /// <param name="arr">Arranger to add</param>
        /// <param name="Show">Optionally shows the arranger immediately</param>
        /// <returns></returns>
        public bool AddArranger(Arranger arr, string NodePath, bool Show = false)
        {
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

            FileManager.Instance.AddArranger(arr);

            if (Show)
            {
                if (arr.Mode == ArrangerMode.SequentialArranger)
                    throw new InvalidOperationException("AddArranger is not meant for SequentialArrangers");
                else if (arr.Mode == ArrangerMode.ScatteredArranger)
                    return ShowScatteredArranger(arr.Name);
                else
                    throw new NotSupportedException("AddArranger does not support arranger types other than ScatteredArranger");
            }

            return true;
        }

        /// <summary>
        /// Removes an arranger from the TreeView
        /// </summary>
        /// <param name="ArrangerName">Name of the arranger to remove</param>
        /// <returns></returns>
        public bool RemoveArranger(string ArrangerName, string NodePath)
        {
            TreeNode tn = FindNode(ArrangerName, NodePath);

            if (tn == null)
                return false;

            if(tn.GetType() != typeof(ArrangerNode))
                throw new InvalidOperationException("Attempted to RemoveArranger on a TreeView node that is not an ArrangerNode");

            return false;
        }

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

            FileManager.Instance.AddPalette(pal);

            return true;
        }

        public bool RemovePalette(PaletteNode pn)
        {
            if (pn == null)
                throw new ArgumentNullException("PaletteNode cannot be null");

            FileManager.Instance.RemovePalette(pn.Text);
            pn.Remove();

            return true;
        }

        public bool ShowSequentialArranger(string Filename)
        {
            List<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            foreach(EditorDockContent dc in activeEditors) // Return if an editor is already opened
            {
                if (dc.ContentSourceName == Filename && dc.GetType() == typeof(GraphicsViewerChild))
                    return false;
            }

            if (FileManager.Instance.LoadSequentialArrangerFromFilename(Filename))
            {
                GraphicsViewerChild gv = new GraphicsViewerChild(Filename);
                gv.WindowState = FormWindowState.Maximized;
                gv.SetZoom(6);
                gv.Show(tsf.DockPanel, DockState.Document);
                return true;
            }
            else
                return false;
        }

        public bool ShowScatteredArranger(string ArrangerName)
        {
            List<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            foreach (EditorDockContent dc in activeEditors) // Return if an editor is already opened
            {
                if (dc.ContentSourceName == ArrangerName && dc.GetType() == typeof(GraphicsViewerChild))
                    return false;
            }

            GraphicsViewerChild gv = new GraphicsViewerChild(ArrangerName);
            gv.WindowState = FormWindowState.Maximized;

            gv.SetZoom(6);
            gv.Show(tsf.DockPanel, DockState.Document);
            gv.ContentModified += tsf.ContentModified;
            gv.ContentSaved += tsf.ContentSaved;
            gv.EditArrangerChanged += tsf.EditArrangerChanged;
            gv.ClearEditArranger();

            return true;
        }

        public bool ShowPaletteEditor(string PaletteName)
        {
            if (!FileManager.Instance.HasPalette(PaletteName))
                return false;

            List<EditorDockContent> activeEditors = tsf.GetActiveEditors();

            foreach (EditorDockContent dc in activeEditors) // Return if an editor is already opened
            {
                if (dc.ContentSourceName == PaletteName && dc.GetType() == typeof(PaletteEditorForm))
                    return false;
            }

            PaletteEditorForm pef = new PaletteEditorForm(PaletteName);
            pef.ContentModified += tsf.ContentModified;
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
            FileManager.Instance.CloseProject();

            return true;
        }

        /// <summary>
        /// Loads data files, palettes, and arrangers from XML
        /// Sets up the nodes in the project tree
        /// The project should be previously cleared before calling
        /// </summary>
        /// <param name="XmlFileName"></param>
        /// <returns></returns>
        public bool LoadProject(string XmlFileName)
        {
            XElement xe = XElement.Load(XmlFileName);

            string path = Path.GetDirectoryName(XmlFileName);

            Directory.SetCurrentDirectory(path);

            /*var settings = xe.Descendants("settings")
                .Select(e => new
                {
                    numberformat = e.Descendants("filelocationnumberformat").First().Value
                });*/

            var datafiles = xe.Descendants("file")
                .Select(e => new
                {
                    location = e.Attribute("location").Value,
                    folder = e.Attribute("folder").Value
                });

            foreach (var datafile in datafiles)
                AddFile(datafile.location, datafile.folder);

            var palettes = xe.Descendants("palette")
                .Select(e => new
                {
                    name = e.Attribute("name").Value,
                    fileoffset = long.Parse(e.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber),
                    bitoffset = e.Attribute("bitoffset"),
                    datafile = e.Attribute("datafile").Value,
                    entries = int.Parse(e.Attribute("entries").Value),
                    format = e.Attribute("format").Value,
                    zeroindextransparent = bool.Parse(e.Attribute("zeroindextransparent").Value),
                    folder = e.Attribute("folder").Value
                });

            foreach (var palette in palettes)
            {
                Palette pal = new Palette(palette.name);
                PaletteColorFormat format = Palette.StringToColorFormat(palette.format);
                FileBitAddress address = new FileBitAddress();
                address.FileOffset = palette.fileoffset;
                if (palette.bitoffset != null)
                    address.BitOffset = int.Parse(palette.bitoffset.Value);
                else
                    address.BitOffset = 0;

                pal.LoadPalette(palette.datafile, address, format, palette.zeroindextransparent, palette.entries);
                AddPalette(pal, palette.folder);
            }

            var arrangers = xe.Descendants("arranger")
                .Select(e => new
                {
                    name = e.Attribute("name").Value,
                    elementsx = int.Parse(e.Attribute("elementsx").Value),
                    elementsy = int.Parse(e.Attribute("elementsy").Value),
                    height = int.Parse(e.Attribute("height").Value),
                    width = int.Parse(e.Attribute("width").Value),
                    defaultformat = e.Attribute("defaultformat").Value,
                    defaultfile = e.Attribute("defaultfile").Value,
                    defaultpalette = e.Attribute("defaultpalette").Value,
                    folder = e.Attribute("folder").Value,
                    graphiclist = e.Descendants("graphic"),
                });

            foreach (var arranger in arrangers)
            {
                Arranger arr = Arranger.NewScatteredArranger(arranger.elementsx, arranger.elementsy, arranger.width, arranger.height);
                arr.Name = arranger.name;

                var graphics = arranger.graphiclist.Select(e => new
                {
                    fileoffset = long.Parse(e.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber),
                    bitoffset = e.Attribute("bitoffset"),
                    posx = int.Parse(e.Attribute("posx").Value),
                    posy = int.Parse(e.Attribute("posy").Value),
                    format = e.Attribute("format"),
                    palette = e.Attribute("palette"),
                    file = e.Attribute("file")
                });

                foreach (var graphic in graphics)
                {
                    ArrangerElement el = arr.GetElement(graphic.posx, graphic.posy);

                    el.FileName = graphic.file?.Value ?? arranger.defaultfile;
                    el.PaletteName = graphic.palette?.Value ?? arranger.defaultpalette;
                    el.FormatName = graphic.format?.Value ?? arranger.defaultformat;

                    if (graphic.bitoffset != null)
                        el.FileAddress = new FileBitAddress(graphic.fileoffset, int.Parse(graphic.bitoffset.Value));
                    else
                        el.FileAddress = new FileBitAddress(graphic.fileoffset, 0);

                    el.Height = arranger.height;
                    el.Width = arranger.width;
                    el.X1 = graphic.posx * el.Width;
                    el.Y1 = graphic.posy * el.Height;
                    el.X2 = el.X1 + el.Width - 1;
                    el.Y2 = el.Y1 + el.Height - 1;

                    el.AllocateBuffers();

                    arr.SetElement(el, graphic.posx, graphic.posy);
                }

                AddArranger(arr, arranger.folder);
            }

            return true;
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
                if(tn.GetType() == typeof(FileNode))
                {
                    FileNode fn = (FileNode)tn;
                    var xmlfile = new XElement("file");
                    xmlfile.SetAttributeValue("location", fn.Text);
                    xmlfile.SetAttributeValue("folder", fn.GetNodePath());
                    datafiles.Add(xmlfile);
                }
                else if(tn.GetType() == typeof(PaletteNode))
                {
                    PaletteNode pn = (PaletteNode)tn;
                    Palette pal = FileManager.Instance.GetPersistentPalette(pn.Text);
                    var xmlpal = new XElement("palette");
                    xmlpal.SetAttributeValue("name", pal.Name);
                    xmlpal.SetAttributeValue("folder", pn.GetNodePath());
                    xmlpal.SetAttributeValue("fileoffset", String.Format("{0:X}", pal.FileAddress.FileOffset));
                    xmlpal.SetAttributeValue("bitoffset", String.Format("{0:X}", pal.FileAddress.BitOffset));
                    xmlpal.SetAttributeValue("datafile", pal.FileName);
                    xmlpal.SetAttributeValue("format", Palette.ColorFormatToString(pal.ColorFormat));
                    xmlpal.SetAttributeValue("entries", pal.Entries);
                    xmlpal.SetAttributeValue("zeroindextransparent", pal.ZeroIndexTransparent);
                    palettes.Add(xmlpal);
                }
                else if(tn.GetType() == typeof(ArrangerNode))
                {
                    ArrangerNode an = (ArrangerNode)tn;
                    Arranger arr = FileManager.Instance.GetPersistentArranger(an.Text);
                    var xmlarr = new XElement("arranger");
                    xmlarr.SetAttributeValue("name", arr.Name);
                    xmlarr.SetAttributeValue("folder", an.GetNodePath());
                    xmlarr.SetAttributeValue("elementsx", arr.ArrangerElementSize.Width);
                    xmlarr.SetAttributeValue("elementsy", arr.ArrangerElementSize.Height);
                    xmlarr.SetAttributeValue("width", arr.ElementPixelSize.Width);
                    xmlarr.SetAttributeValue("height", arr.ElementPixelSize.Height);

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
                            if (el.FileName != DefaultFile)
                                graphic.SetAttributeValue("file", el.FileName);
                            if (el.PaletteName != DefaultPalette)
                                graphic.SetAttributeValue("palette", el.PaletteName);

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

        // Used to determine property defaults for XML
        private string FindMostFrequentValue(Arranger arr, string attributeName)
        {
            Dictionary<string, int> freq = new Dictionary<string, int>();
            Type T = typeof(ArrangerElement);
            PropertyInfo P = T.GetProperty(attributeName);

            foreach (ArrangerElement el in arr.ElementList)
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

        public List<string> GetFileNameList()
        {
            List<string> list = new List<string>();

            foreach (TreeNode tn in ProjectTreeView.Nodes)
            {
                if (tn.GetType() == typeof(FolderNode))
                {
                    List<string> nodeFileNameList = GetChildFileNames(tn);
                    foreach (string s in nodeFileNameList)
                        list.Add(s);
                }
                else if (tn.GetType() == typeof(FileNode))
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
            foreach(TreeNode childNode in parentNode.Nodes)
            {
                if (childNode.GetType() == typeof(FolderNode))
                {
                    List<string> childFileNames = GetChildFileNames(childNode);
                    foreach (string s in childFileNames)
                        ret.Add(s);
                }
                else if (childNode.GetType() == typeof(FileNode))
                    ret.Add(childNode.Text);
            }

            return ret;
        }

        private void ProjectTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.GetType() == typeof(FileNode))
            {
                ShowSequentialArranger((string)e.Node.Tag);
            }
            else if (e.Node.GetType() == typeof(PaletteNode))
            {
                ShowPaletteEditor((string)e.Node.Tag);
            }
            else if (e.Node.GetType() == typeof(ArrangerNode))
            {
                ShowScatteredArranger((string)e.Node.Tag);
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
        /// <param name="NodePath"></param>
        /// <returns>The FolderNode that was added or found on success. Null on failure.</returns>
        private FolderNode AddFolderNode(string FolderNodePath)
        {
            if (FolderNodePath == null)
                throw new ArgumentNullException();

            if (FolderNodePath == "")
                return null;

            string[] nodepaths = FolderNodePath.Split('\\');

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
                    if (tn.Text == path && tn.GetType() == typeof(FolderNode)) // Found next segment of path
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
                    if (tn.Text == path && tn.GetType() == typeof(FolderNode)) // Found next segment of path
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

            foreach(TreeNode tn in fn.Nodes)
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

            if(e.Data.GetDataPresent(typeof(FileNode)))
                dragNode = (FileNode)e.Data.GetData(typeof(FileNode));
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
                if (dropNode.GetType() != typeof(FolderNode))
                    return;

                dragNode.Remove();
                dropNode.Nodes.Add((TreeNode)dragNode);
            }
            else // ArrangerNode, FileNode, PaletteNode all must be moved into a folder node (or root)
            {
                if (dropNode.GetType() != typeof(FolderNode)) // All nodes must be attached to a folder node
                    return;

                dragNode.Remove();
                dropNode.Nodes.Add((TreeNode)dragNode);
            }
        }

        private void ProjectTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.GetType() != typeof(FolderNode))
                throw new InvalidOperationException("ProjectTreeView attempted to collapse a node that isn't a FolderNode");

            FolderNode fn = (FolderNode)e.Node;
            fn.ImageIndex = 0;
            fn.SelectedImageIndex = 0;
        }

        private void ProjectTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.GetType() != typeof(FolderNode))
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
        public static List<TreeNode> GetAllNodes(this TreeView _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in _self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        public static List<TreeNode> GetAllNodes(this TreeNode _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            result.Add(_self);
            foreach (TreeNode child in _self.Nodes)
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

    public abstract class ProjectTreeNode : TreeNode
    {
        public abstract void BuildContextMenu(ContextMenu Menu);

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

    public class FolderNode : ProjectTreeNode
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


            }
            else
                this.Remove();
        }
    }

    public class FileNode : ProjectTreeNode
    {
        public FileNode()
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

    public class PaletteNode : ProjectTreeNode
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

    public class ArrangerNode : ProjectTreeNode
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

    public class GameDescriptorSettings
    {
        public enum FileLocationNumberFormat { Decimal = 0, Hexadecimal = 1 }

        public FileLocationNumberFormat NumberFormat { set; get; }
    }
}
