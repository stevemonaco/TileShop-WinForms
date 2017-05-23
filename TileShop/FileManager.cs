using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileShop
{
    /// <summary>
    /// Singleton class that manages file and editor resources
    /// Files loaded here are kept open always until ClearAll is called
    /// </summary>
    
    // TODO - Consider future implementation of lazy instantiation for some objects, especially for projects that have many, many files
    public class FileManager
    {
       public static readonly FileManager Instance = new FileManager();

        /// <summary>
        /// List of in-use files that can be read or written to by subeditors such as the arranger or palette editor
        /// </summary>
        private Dictionary<string, FileStream> FileList = new Dictionary<string, FileStream>();

        /// <summary>
        /// List of arrangers that may be edited by the user
        /// </summary>
        private Dictionary<string, Arranger> ArrangerList = new Dictionary<string, Arranger>();

        /// <summary>
        /// List of arrangers that contains arrangers that will be persisted to storage
        /// </summary>
        private Dictionary<string, Arranger> PersistentArrangerList = new Dictionary<string, Arranger>();

        /// <summary>
        /// List of arrangers that contains palettes that will be persisted to storage
        /// </summary>
        private Dictionary<string, Palette> PersistentPaletteList = new Dictionary<string, Palette>();

        /// <summary>
        /// List of palettes that may be edited by the user
        /// </summary>
        private Dictionary<string, Palette> PaletteList = new Dictionary<string, Palette>();

        /// <summary>
        /// List of graphics format codecs
        /// </summary>
        private Dictionary<string, GraphicsFormat> FormatList = new Dictionary<string, GraphicsFormat>();

        /// <summary>
        /// List of cursors loaded
        /// </summary>
        private Dictionary<string, Cursor> CursorList = new Dictionary<string, Cursor>();

        /// <summary>
        /// FileTypeLoader which is used to match file extensions with the default graphics codec upon opening for sequential arranger
        /// </summary>
        private FileTypeLoader Loader = new FileTypeLoader();

        const int DefaultElementsX = 16;
        const int DefaultElementsY = 16;

        public void AddFileStream(string FileName, FileStream fs)
        {
            FileList.Add(FileName, fs);
        }

        public void AddGraphicsFormat(GraphicsFormat format)
        {
            FormatList.Add(format.Name, format);
        }

        public void AddArranger(Arranger arr)
        {
            if (arr == null)
                throw new ArgumentNullException();

            if (!HasArranger(arr.Name))
            {
                PersistentArrangerList.Add(arr.Name, arr);
                ArrangerList.Add(arr.Name, arr.Clone());
            }
        }

        public void AddPalette(Palette pal)
        {
            if (pal == null)
                throw new ArgumentNullException();

            if (String.IsNullOrEmpty(pal.Name))
                throw new ArgumentException();

            if (!HasPalette(pal.Name))
            {
                PersistentPaletteList.Add(pal.Name, pal);
                PaletteList.Add(pal.Name, pal.Clone());
            }
        }

        public FileStream GetFileStream(string FileName)
        {
            if (FileList.ContainsKey(FileName))
                return FileList[FileName];
            else
                throw new KeyNotFoundException();
        }

        /// <summary>
        /// Gets the editable palette associated with the specified name
        /// </summary>
        /// <param name="PaletteName"></param>
        /// <returns></returns>
        public Palette GetPalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName))
                return PaletteList[PaletteName];
            else
                throw new KeyNotFoundException(String.Format("Palette {0} was not found in the PaletteList", PaletteName));
        }

        /// <summary>
        /// Gets the editable arranger associated with the specified name
        /// </summary>
        /// <param name="ArrangerName"></param>
        /// <returns></returns>
        public Arranger GetArranger(string ArrangerName)
        {
            if (ArrangerList.ContainsKey(ArrangerName))
                return ArrangerList[ArrangerName];
            else
                throw new KeyNotFoundException(String.Format("Arranger {0} was not found in the ArrangerList", ArrangerName));
        }

        /// <summary>
        /// Gets the persistent palette associated with the specified name
        /// </summary>
        /// <param name="PaletteName"></param>
        /// <returns></returns>
        public Palette GetPersistentPalette(string PaletteName)
        {
            if (PersistentPaletteList.ContainsKey(PaletteName))
                return PersistentPaletteList[PaletteName];
            else
                throw new KeyNotFoundException(String.Format("Palette {0} was not found in the PersistentPaletteList", PaletteName));
        }

        /// <summary>
        /// Gets the persistent arranger associated with the specified name
        /// </summary>
        /// <param name="ArrangerName"></param>
        /// <returns></returns>
        public Arranger GetPersistentArranger(string ArrangerName)
        {
            if (PersistentArrangerList.ContainsKey(ArrangerName))
                return PersistentArrangerList[ArrangerName];
            else
                throw new KeyNotFoundException(String.Format("Arranger {0} was not found in the PersistentArrangerList", ArrangerName));
        }

        public bool HasFile(string Filename)
        {
            return FileList.ContainsKey(Filename);
        }

        public bool HasArranger(string ArrangerName)
        {
            return PersistentArrangerList.ContainsKey(ArrangerName);
        }

        public bool HasPalette(string PaletteName)
        {
            return PersistentPaletteList.ContainsKey(PaletteName);
        }

        public GraphicsFormat GetGraphicsFormat(string FormatName)
        {
            if (FormatList.ContainsKey(FormatName))
                return FormatList[FormatName];
            else
                throw new KeyNotFoundException();
        }

        public List<string> GetGraphicsFormatsNameList()
        {
            Dictionary<string, GraphicsFormat>.KeyCollection keys = FileManager.Instance.FormatList.Keys;
            List<string> keyList = keys.ToList<string>();
            keyList.Sort();

            return keyList;
        }

        public bool LoadFile(string Filename)
        {
            // TODO: Better error handling
            try
            {
                if (!FileList.ContainsKey(Filename))
                {
                    FileStream fs = File.Open(Filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    AddFileStream(Filename, fs);
                }
            }
            catch(FileNotFoundException ex)
            {
                return false;
            }
            catch(IOException ex)
            {
                return false;
            }

            return true;
        }

        public bool LoadSequentialArrangerFromFilename(string Filename)
        {
            if (!LoadFile(Filename))
                return false;

            string formatname = Loader.GetDefaultFormatName(Filename);
            Arranger arranger = Arranger.NewSequentialArranger(DefaultElementsX, DefaultElementsY, Filename, GetGraphicsFormat(formatname));
            AddArranger(arranger);

            return true;
        }

        public bool LoadFormat(string Filename)
        {
            GraphicsFormat fmt = new GraphicsFormat();
            if (!fmt.LoadFromXml(Filename))
                return false;

            AddGraphicsFormat(fmt);
            return true;
        }

        public bool LoadPalette(string Filename, string PaletteName)
        {
            if (Filename == null || PaletteName == null)
                throw new ArgumentNullException();

            Palette pal = new Palette(PaletteName);
            if (!pal.LoadPalette(Filename))
                return false;

            string palname = Path.GetFileNameWithoutExtension(Filename);

            AddPalette(pal);
            return true;
        }

        public bool AddCursor(string CursorName, Cursor cursor)
        {
            if (!CursorList.ContainsKey(CursorName))
            {
                CursorList.Add(CursorName, cursor);
                return true;
            }
            return false;
        }

        public Cursor GetCursor(string CursorName)
        {
            if (CursorList.ContainsKey(CursorName))
                return CursorList[CursorName];
            else
                throw new KeyNotFoundException();
        }

        public void RemoveFile(string FileName)
        {
            if(FileList.ContainsKey(FileName))
            {
                FileList[FileName].Close();
                FileList.Remove(FileName);
            }
        }

        public void RemoveArranger(string ArrangerName)
        {
            if(ArrangerList.ContainsKey(ArrangerName))
                ArrangerList.Remove(ArrangerName);

            if (PersistentArrangerList.ContainsKey(ArrangerName))
                PersistentArrangerList.Remove(ArrangerName);
        }

        public void RemovePalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName))
                PaletteList.Remove(PaletteName);

            if (PersistentPaletteList.ContainsKey(PaletteName))
                PersistentPaletteList.Remove(PaletteName);
        }

        /// <summary>
        /// Reloads the specified arranger from its underlying source
        /// </summary>
        /// <param name="ArrangerName">Name of the Arranger to reload</param>
        /// <returns>The reloaded arranger</returns>
        public Arranger ReloadArranger(string ArrangerName)
        {
            if (ArrangerList.ContainsKey(ArrangerName) && PersistentArrangerList.ContainsKey(ArrangerName))
            {
                Arranger arr = PersistentArrangerList[ArrangerName].Clone();
                ArrangerList[ArrangerName] = arr;
                return arr;
            }
            else
                throw new KeyNotFoundException();
        }

        public void ReloadPalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName) && PersistentPaletteList.ContainsKey(PaletteName))
                PaletteList[PaletteName] = PersistentPaletteList[PaletteName].Clone();
            else
                throw new KeyNotFoundException();
        }

        public void SaveArranger(string ArrangerName)
        {
            if (ArrangerList.ContainsKey(ArrangerName) && PersistentArrangerList.ContainsKey(ArrangerName))
                PersistentArrangerList[ArrangerName] = ArrangerList[ArrangerName].Clone();
            else
                throw new KeyNotFoundException();
        }

        public void SavePalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName) && PersistentPaletteList.ContainsKey(PaletteName))
                PersistentPaletteList[PaletteName] = PaletteList[PaletteName].Clone();
            else
                throw new KeyNotFoundException();
        }

        /// <summary>
        /// Closes project-based memory objects
        /// Does not remove graphic formats, cursors, or file loaders
        /// </summary>
        /// <returns></returns>
        public bool CloseProject()
        {
            foreach (FileStream fs in FileList.Values)
                fs.Close();
            FileList.Clear();

            PaletteList.Clear();
            ArrangerList.Clear();
            PersistentArrangerList.Clear();
            PersistentPaletteList.Clear();

            return true;
        }

    }
}
