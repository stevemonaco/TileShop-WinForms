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
        //FileFolderNode filesNode = new FileFolderNode();
        //PaletteFolderNode palettesNode = new PaletteFolderNode();
        //ArrangerFolderNode arrangersNode = new ArrangerFolderNode();
        ContextMenu contextMenu = new ContextMenu();

        public ProjectExplorerControl(TileShopForm tileShopForm)
        {
            tsf = tileShopForm ?? throw new ArgumentNullException();

            InitializeComponent();

            /*filesNode.Text = "Files";
            palettesNode.Text = "Palettes";
            arrangersNode.Text = "Arrangers";

            ProjectTreeView.Nodes.Add(filesNode);
            ProjectTreeView.Nodes.Add(palettesNode);
            ProjectTreeView.Nodes.Add(arrangersNode);*/
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

            FolderNode folderNode = AddFolderNode(NodePath);

            if (folderNode == null) // Add to root
                ProjectTreeView.Nodes.Add(fileNode);
            else // Add to subfolder
                folderNode.Nodes.Add(fileNode);

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

            FolderNode folderNode = AddFolderNode(NodePath);
            folderNode.Nodes.Add(an);

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

            FolderNode folderNode = AddFolderNode(NodePath);
            
            folderNode.Nodes.Add(pn);
            FileManager.Instance.AddPalette(pal);

            return true;
        }

        public bool RemovePalette(string PaletteName, string NodePath)
        {
            TreeNode tn = FindNode(PaletteName, NodePath);

            if (tn == null)
                return false;

            if (tn.GetType() != typeof(PaletteNode))
                throw new InvalidOperationException("Attempted to RemovePalette on a TreeView node that is not an PaletteNode");

            return false;
        }

        public bool ShowSequentialArranger(string Filename)
        {
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
            XElement root = new XElement("gdf");

            // Save settings
            XElement settings = new XElement("settings");
            //XElement numberformat = new XElement("filelocationnumberformat");
            //numberformat.SetValue("hexadecimal");
            //settings.Add(numberformat);

            root.Add(settings);

            // Save each data file
            XElement datafiles = new XElement("datafiles");
            XElement palettes = new XElement("palettes");
            XElement arrangers = new XElement("arrangers");

            foreach (TreeNode tn in ProjectTreeView.Nodes)
            {
                if(tn.GetType() == typeof(FileNode))
                {
                    XElement el = new XElement("file");
                    el.SetAttributeValue("location", tn.Text);
                    datafiles.Add(el);
                }
                else if(tn.GetType() == typeof(PaletteNode))
                {
                    PaletteNode pn = (PaletteNode)tn;
                    Palette pal = FileManager.Instance.GetPersistentPalette(pn.Text);
                    XElement xmlpal = new XElement("palette");
                    xmlpal.SetAttributeValue("name", pal.Name);
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
                    XElement xmlarr = new XElement("arranger");
                    xmlarr.SetAttributeValue("name", arr.Name);
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
                            XElement graphic = new XElement("graphic");
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
            //tsf.openExistingArranger(e.Node.Text);
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
                        idx++;
                        break;
                    }
                }

                if (!nodeExists)
                    break;
            }

            for (int i = idx; i < nodepaths.Length; i++)
            {
                FolderNode fnadd = new FolderNode();
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
    }

    public abstract class ProjectTreeNode : TreeNode
    {
        public abstract void BuildContextMenu(ContextMenu Menu);
    }

    public class FolderNode : ProjectTreeNode
    {
        public FolderNode()
        {
        }

        public override void BuildContextMenu(ContextMenu Menu)
        {
            //Menu.MenuItems.Clear();

            //Menu.MenuItems.Add(new MenuItem("Add Existing File to Project", AddFile_Click));
        }

        public void AddFile_Click(object sender, System.EventArgs e)
        {

        }
    }

    public class FileFolderNode : ProjectTreeNode
    {
        public FileFolderNode()
        {
        }

        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();

            Menu.MenuItems.Add(new MenuItem("Add Existing File to Project", AddFile_Click));
        }

        public void AddFile_Click(object sender, System.EventArgs e)
        {

        }
    }

    public class PaletteFolderNode : ProjectTreeNode
    {
        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();

            Menu.MenuItems.Add(new MenuItem("Add Palette from File to Project", AddPalette_Click));
        }

        public void AddPalette_Click(object sender, System.EventArgs e)
        {

        }
    }

    public class ArrangerFolderNode : ProjectTreeNode
    {
        public override void BuildContextMenu(ContextMenu Menu)
        {
            Menu.MenuItems.Clear();

            Menu.MenuItems.Add(new MenuItem("Add New Scattered Arranger to Project", AddScatteredArranger_Click));
        }

        public void AddScatteredArranger_Click(object sender, System.EventArgs e)
        {

        }
    }

    public class FileNode : ProjectTreeNode
    {
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

        }
    }

    public class PaletteNode : ProjectTreeNode
    {
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

        }
    }

    public class ArrangerNode : ProjectTreeNode
    {
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

        }
    }

    public class GameDescriptorSettings
    {
        public enum FileLocationNumberFormat { Decimal = 0, Hexadecimal = 1 }

        public FileLocationNumberFormat NumberFormat { set; get; }
    }
}
