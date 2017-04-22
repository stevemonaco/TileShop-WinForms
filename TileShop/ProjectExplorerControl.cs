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
        FileFolderNode filesNode = new FileFolderNode();
        PaletteFolderNode palettesNode = new PaletteFolderNode();
        ArrangerFolderNode arrangersNode = new ArrangerFolderNode();
        ContextMenu contextMenu = new ContextMenu();

        /// <summary>
        /// Gets the state expressing if the project file structure has been modified since last save
        /// This includes changes to the arrangers but not changes to the underlying source files
        /// </summary>
        public bool IsProjectModified
        {
            get { return isProjectModified; }
            private set { isProjectModified = value; }
        }
        private bool isProjectModified;

        public ProjectExplorerControl(TileShopForm tileShopForm)
        {
            tsf = tileShopForm ?? throw new ArgumentNullException();

            InitializeComponent();

            filesNode.Text = "Files";
            palettesNode.Text = "Palettes";
            arrangersNode.Text = "Arrangers";

            ProjectTreeView.Nodes.Add(filesNode);
            ProjectTreeView.Nodes.Add(palettesNode);
            ProjectTreeView.Nodes.Add(arrangersNode);
        }

        // Full filename with path
        public bool AddFile(string Filename, bool Show)
        {
            // Ensure the file has not been previously added
            foreach(FileNode node in filesNode.Nodes)
            {
                if ((string)node.Tag == Filename)
                    return false;
            }

            FileNode fn = new FileNode()
            {
                Text = Filename,
                Tag = Filename
            };
            if (!FileManager.Instance.LoadFile(Filename))
                return false;

            filesNode.Nodes.Add(fn);

            if (Show)
            {
                ProjectTreeView.SelectedNode = fn;
                return ShowSequentialArranger(Filename);
            }

            return true;
        }

        // Full filename with path
        public bool RemoveFile(string Filename)
        {
            foreach(FileNode fn in filesNode.Nodes)
            {
                if((string)fn.Tag == Filename)
                {
                    fn.Remove();
                    return true;
                }
            }

            return false;
        }

        public bool AddArranger(Arranger arr, bool Show)
        {
            ArrangerNode an = new ArrangerNode()
            {
                Text = arr.Name,
                Tag = arr.Name
            };
            arrangersNode.Nodes.Add(an);

            FileManager.Instance.AddArranger(arr);

            if(Show)
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

        public bool RemoveArranger(string ArrangerName)
        {
            foreach (ArrangerNode an in arrangersNode.Nodes)
            {
                if ((string)an.Tag == ArrangerName)
                {
                    an.Remove();
                    return true;
                }
            }

            return false;
        }

        public bool AddPalette(Palette pal)
        {
            PaletteNode pn = new PaletteNode();
            pn.Text = pal.Name;
            pn.Tag = pal.Name;

            palettesNode.Nodes.Add(pn);
            FileManager.Instance.AddPalette(pal);

            return true;
        }

        public bool RemovePalette(string PaletteName)
        {
            foreach (PaletteNode tn in palettesNode.Nodes)
            {
                if ((string)tn.Tag == PaletteName)
                {
                    tn.Remove();
                    return true;
                }
            }

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
            //projectTreeView.Nodes.Clear();
            filesNode.Nodes.Clear();
            palettesNode.Nodes.Clear();
            arrangersNode.Nodes.Clear();
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

            var settings = xe.Descendants("settings")
                .Select(e => new
                {
                    numberformat = e.Descendants("filelocationnumberformat").First().Value
                });

            var datafiles = xe.Descendants("file")
                .Select(e => new
                {
                    location = e.Attribute("location").Value,
                });

            foreach (var datafile in datafiles)
                AddFile(datafile.location, false);

            var palettes = xe.Descendants("palette")
                .Select(e => new
                {
                    name = e.Attribute("name").Value,
                    fileoffset = long.Parse(e.Attribute("fileoffset").Value, System.Globalization.NumberStyles.HexNumber),
                    bitoffset = e.Attribute("bitoffset"),
                    datafile = e.Attribute("datafile").Value,
                    entries = int.Parse(e.Attribute("entries").Value),
                    format = e.Attribute("format").Value
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

                pal.LoadPalette(palette.datafile, address, format, palette.entries);
                AddPalette(pal);
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
                    graphiclist = e.Descendants("graphic")
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

                AddArranger(arr, false);
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
            XElement numberformat = new XElement("filelocationnumberformat");
            numberformat.SetValue("hexadecimal");
            settings.Add(numberformat);

            root.Add(settings);

            // Save each data file
            XElement datafiles = new XElement("datafiles");

            foreach(FileNode fn in filesNode.Nodes)
            {
                XElement el = new XElement("file");
                el.SetAttributeValue("location", fn.Text);
                datafiles.Add(el);
            }

            root.Add(datafiles);

            // Save Palettes
            XElement palettes = new XElement("palettes");

            foreach(PaletteNode pn in palettesNode.Nodes)
            {
                Palette pal = FileManager.Instance.GetPersistentPalette(pn.Text);
                XElement el = new XElement("palette");
                el.SetAttributeValue("name", pal.Name);
                el.SetAttributeValue("fileoffset", String.Format("{0:X}", pal.FileAddress.FileOffset));
                el.SetAttributeValue("bitoffset", String.Format("{0:X}", pal.FileAddress.BitOffset));
                el.SetAttributeValue("datafile", pal.FileName);
                el.SetAttributeValue("format", Palette.ColorFormatToString(pal.ColorFormat));
                el.SetAttributeValue("entries", pal.Entries);
                palettes.Add(el);
            }

            root.Add(palettes);

            // Save each arranger
            XElement arrangers = new XElement("arrangers");
            
            foreach(ArrangerNode an in arrangersNode.Nodes)
            {
                Arranger arr = FileManager.Instance.GetPersistentArranger(an.Text);
                XElement el = new XElement("arranger");
                el.SetAttributeValue("name", arr.Name);
                el.SetAttributeValue("elementsx", arr.ArrangerElementSize.Width);
                el.SetAttributeValue("elementsy", arr.ArrangerElementSize.Height);
                el.SetAttributeValue("width", arr.ElementPixelSize.Width);
                el.SetAttributeValue("height", arr.ElementPixelSize.Height);

                string DefaultPalette = FindMostFrequentValue(arr, "PaletteName");
                string DefaultFile = FindMostFrequentValue(arr, "FileName");
                string DefaultFormat = FindMostFrequentValue(arr, "FormatName");

                el.SetAttributeValue("defaultformat", DefaultFormat);
                el.SetAttributeValue("defaultfile", DefaultFile);
                el.SetAttributeValue("defaultpalette", DefaultPalette);

                for(int y = 0; y < arr.ArrangerElementSize.Height; y++)
                {
                    for(int x = 0; x < arr.ArrangerElementSize.Width; x++)
                    {
                        XElement graphic = new XElement("graphic");
                        ArrangerElement arrel = arr.GetElement(x, y);

                        graphic.SetAttributeValue("fileoffset", String.Format("{0:X}", arrel.FileAddress.FileOffset));
                        graphic.SetAttributeValue("bitoffset", String.Format("{0:X}", arrel.FileAddress.BitOffset));
                        graphic.SetAttributeValue("posx", x);
                        graphic.SetAttributeValue("posy", y);
                        if (arrel.FormatName != DefaultFormat)
                            graphic.SetAttributeValue("format", arrel.FormatName);
                        if (arrel.FileName != DefaultFile)
                            graphic.SetAttributeValue("file", arrel.FileName);
                        if (arrel.PaletteName != DefaultPalette)
                            graphic.SetAttributeValue("palette", arrel.PaletteName);

                        el.Add(graphic);
                    }
                }

                arrangers.Add(el);
            }

            root.Add(arrangers);

            root.Save(XmlFileName);

            return true;
        }

        // Used to determine property defaults for XML
        private string FindMostFrequentValue(Arranger arr, string attributeName)
        {
            Dictionary<string, int> freq = new Dictionary<string, int>();
            Type T = typeof(ArrangerElement);
            PropertyInfo P  = T.GetProperty(attributeName);

            foreach (ArrangerElement el in arr.ElementList)
            {
                string s = (string) P.GetValue(el);

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

            foreach (FileNode fn in filesNode.Nodes)
                list.Add((string)fn.Tag);

            return list;
        }

        private void ProjectTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //tsf.openExistingArranger(e.Node.Text);
            if(e.Node.GetType() == typeof(FileNode))
            {
                ShowSequentialArranger((string)e.Node.Tag);
            }
            else if(e.Node.GetType() == typeof(PaletteNode))
            {
                ShowPaletteEditor((string)e.Node.Tag);
            }
            else if(e.Node.GetType() == typeof(ArrangerNode))
            {
                ShowScatteredArranger((string)e.Node.Tag);
            }
        }

        private void ProjectTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Right) // Show context menu
            {
                ProjectTreeNode ptn = (ProjectTreeNode) e.Node;
                ptn.BuildContextMenu(contextMenu);
                contextMenu.Show(ProjectTreeView, e.Location);
            }
        }
    }

    public abstract class ProjectTreeNode : TreeNode
    {
        public abstract void BuildContextMenu(ContextMenu Menu);

        public bool IsModified { get; set; }
    }

    public class FileFolderNode : ProjectTreeNode
    {
        public FileFolderNode()
        {
            IsModified = false;
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
