using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileShop.Core
{
    /// <summary>
    /// Singleton class that manages file and editor resources
    /// Files loaded here are kept open always until CloseProject is called
    /// </summary>

    // TODO - Consider future implementation of lazy instantiation for some objects, especially for projects that have many, many files
    public class ResourceManager
    {
        public static readonly ResourceManager Instance = new ResourceManager();

        /// <summary>
        /// List of graphics format codecs
        /// </summary>
        private Dictionary<string, GraphicsFormat> FormatList = new Dictionary<string, GraphicsFormat>();

        /// <summary>
        /// List of in-use files on disk that can be read or written to by subeditors such as the arranger or palette editor
        /// </summary>
        private Dictionary<string, DataFile> DataFileList = new Dictionary<string, DataFile>();

        /// <summary>
        /// List of arrangers that may be edited by the user
        /// </summary>
        private Dictionary<string, Arranger> ArrangerList = new Dictionary<string, Arranger>();

        /// <summary>
        /// List of palettes that may be edited by the user
        /// </summary>
        private Dictionary<string, Palette> PaletteList = new Dictionary<string, Palette>();

        /// <summary>
        /// List of cursors loaded
        /// </summary>
        private Dictionary<string, Cursor> CursorList = new Dictionary<string, Cursor>();

        /// <summary>
        /// FileTypeLoader which is used to match file extensions with the default graphics codec upon opening for sequential arranger
        /// </summary>
        private FileTypeLoader Loader = new FileTypeLoader();

        /// <summary>
        /// List of arrangers that contains arrangers that will be persisted to storage
        /// </summary>
        private Dictionary<string, Arranger> PersistentArrangerList = new Dictionary<string, Arranger>();

        /// <summary>
        /// List of arrangers that contains palettes that will be persisted to storage
        /// </summary>
        private Dictionary<string, Palette> PersistentPaletteList = new Dictionary<string, Palette>();

        const int DefaultElementsX = 16;
        const int DefaultElementsY = 16;

        #region GraphicsFormat Management
        public void AddGraphicsFormat(GraphicsFormat format)
        {
            FormatList.Add(format.Name, format);
        }

        public bool LoadFormat(string Filename)
        {
            GraphicsFormat fmt = new GraphicsFormat();
            if (!fmt.LoadFromXml(Filename))
                return false;

            AddGraphicsFormat(fmt);
            return true;
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
            Dictionary<string, GraphicsFormat>.KeyCollection keys = ResourceManager.Instance.FormatList.Keys;
            List<string> keyList = keys.ToList<string>();
            keyList.Sort();

            return keyList;
        }
        #endregion

        #region DataFile Management
        /// <summary>
        /// Adds a data file to FileManager
        /// </summary>
        /// <param name="dataFile"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool AddDataFile(DataFile dataFile, string key)
        {
            if (dataFile == null || key == null)
                throw new ArgumentNullException();

            if (!HasDataFile(key))
            {
                DataFileList[key] = dataFile;
            }

            return true;
        }

        /// <summary>
        /// Retrieves a DataFile by its key
        /// </summary>
        /// <param name="dataFileKey"></param>
        /// <returns></returns>
        public DataFile GetDataFile(string dataFileKey)
        {
            if (DataFileList.ContainsKey(dataFileKey))
                return DataFileList[dataFileKey];
            else
                throw new KeyNotFoundException();
        }

        public bool HasDataFile(string Filename)
        {
            return DataFileList.ContainsKey(Filename);
        }

        public void RemoveFile(string FileName)
        {
            if (DataFileList.ContainsKey(FileName))
            {
                DataFileList[FileName].Close();
                DataFileList.Remove(FileName);
            }
        }

        /// <summary>
        /// Renames a file that is currently loaded into the FileManager, renames it on disk, and remaps all references to it in the project
        /// </summary>
        /// <param name="FileName">FileManager file to be renamed</param>
        /// <param name="NewFileName">Name that the file will be renamed to</param>
        /// <returns>Success state</returns>
        public bool RenameFile(string FileName, string NewFileName)
        {
            if (String.IsNullOrEmpty(FileName) || String.IsNullOrEmpty(NewFileName))
                throw new ArgumentException("");

            // Must contain FileName and must not contain NewFileName
            if (HasDataFile(NewFileName) || !HasDataFile(FileName) || File.Exists(NewFileName))
                return false;

            // File must not already exist
            if (File.Exists(NewFileName))
                return false;

            DataFile df = GetDataFile(FileName);
            string name = df.Stream.Name;
            df.Stream.Close();

            DataFileList.Remove(FileName);

            File.Move(name, NewFileName);
            df.Open(NewFileName);
            DataFileList.Add(NewFileName, df);

            // Rename references
            foreach (Arranger arr in ArrangerList.Values)
            {
                foreach (ArrangerElement el in arr.ElementGrid)
                {
                    if (el.DataFileKey == FileName)
                        el.DataFileKey = NewFileName;
                }
            }

            foreach (Arranger arr in PersistentArrangerList.Values)
            {
                foreach (ArrangerElement el in arr.ElementGrid)
                {
                    if (el.DataFileKey == FileName)
                        el.DataFileKey = NewFileName;
                }
            }

            foreach (Palette pal in PaletteList.Values)
            {
                if (pal.DataFileKey == FileName)
                    pal.SetFileKey(NewFileName);
            }

            foreach (Palette pal in PersistentPaletteList.Values)
            {
                if (pal.DataFileKey == FileName)
                    pal.SetFileKey(NewFileName);
            }

            return true;
        }

        #endregion

        #region Arranger Management
        public void AddArranger(Arranger arr, string key)
        {
            if (arr == null || key == null)
                throw new ArgumentNullException();

            if (!HasArranger(key))
            {
                PersistentArrangerList.Add(key, arr);
                ArrangerList.Add(key, arr.Clone());
            }
        }

        /// <summary>
        /// Creates a sequential arranger from a data file
        /// </summary>
        /// <param name="dataFileKey">Key of a previously loaded data file</param>
        /// <returns></returns>
        public bool LoadSequentialArranger(string dataFileKey)
        {
            string formatname = Loader.GetDefaultFormatName(dataFileKey);
            Arranger arranger = Arranger.NewSequentialArranger(DefaultElementsX, DefaultElementsY, dataFileKey, GetGraphicsFormat(formatname));
            AddArranger(arranger, dataFileKey); // TODO: Ensure that Filename is local to the XML project file

            return true;
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

        public bool HasArranger(string ArrangerName)
        {
            return PersistentArrangerList.ContainsKey(ArrangerName);
        }

        public void RemoveArranger(string ArrangerName)
        {
            if (ArrangerList.ContainsKey(ArrangerName))
                ArrangerList.Remove(ArrangerName);

            if (PersistentArrangerList.ContainsKey(ArrangerName))
                PersistentArrangerList.Remove(ArrangerName);
        }

        /// <summary>
        /// Renames an arranger that is currently loaded into the FileManager
        /// </summary>
        /// <param name="ArrangerName">Arranger to be renamed</param>
        /// <param name="NewArrangerName">Name that the arranger will be renamed to</param>
        /// <returns></returns>
        public bool RenameArranger(string ArrangerName, string NewArrangerName)
        {
            if (!HasArranger(ArrangerName) || HasArranger(NewArrangerName))
                return false;

            Arranger arr = GetArranger(ArrangerName);
            arr.Rename(NewArrangerName);
            ArrangerList.Remove(ArrangerName);
            ArrangerList.Add(NewArrangerName, arr);

            Arranger arr2 = GetPersistentArranger(ArrangerName);
            arr2.Rename(NewArrangerName);
            PersistentArrangerList.Remove(ArrangerName);
            PersistentArrangerList.Add(NewArrangerName, arr2);

            return true;
        }
        #endregion

        #region Palette Management
        public void AddPalette(Palette pal, string key)
        {
            if (pal == null || key == null)
                throw new ArgumentNullException();

            if (String.IsNullOrEmpty(pal.Name))
                throw new ArgumentException();

            if (!HasPalette(key))
            {
                PersistentPaletteList.Add(key, pal);
                PaletteList.Add(key, pal.Clone());
            }
        }

        /// <summary>
        /// Loads and associates a palette within FileManager
        /// </summary>
        /// <param name="Filename">Filename to palette to be loaded</param>
        /// <param name="PaletteName">Name associated to the palette within FileManager </param>
        /// <returns></returns>
        public bool LoadPalette(string Filename, string PaletteName)
        {
            if (Filename == null || PaletteName == null)
                throw new ArgumentNullException();

            Palette pal = new Palette(PaletteName);
            if (!pal.LoadPalette(Filename))
                return false;

            AddPalette(pal, pal.Name);
            return true;
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

        public bool HasPalette(string PaletteName)
        {
            return PersistentPaletteList.ContainsKey(PaletteName);
        }

        public void RemovePalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName))
                PaletteList.Remove(PaletteName);

            if (PersistentPaletteList.ContainsKey(PaletteName))
                PersistentPaletteList.Remove(PaletteName);
        }

        /// <summary>
        /// Renames an palette that is currently loaded into the FileManager and remaps all references to it in the project
        /// </summary>
        /// <param name="PaletteName">Palette to be renamed</param>
        /// <param name="NewPaletteName">Name that the palette will be renamed to</param>
        /// <returns></returns>
        public bool RenamePalette(string PaletteName, string NewPaletteName)
        {
            if (!HasPalette(PaletteName) || HasPalette(NewPaletteName))
                return false;

            Palette pal = GetPalette(PaletteName);
            pal.SetFileKey(NewPaletteName);
            PaletteList.Remove(NewPaletteName);
            PaletteList.Add(NewPaletteName, pal);

            Palette pal2 = GetPersistentPalette(PaletteName);
            pal2.SetFileKey(NewPaletteName);
            PersistentPaletteList.Remove(NewPaletteName);
            PersistentPaletteList.Add(NewPaletteName, pal2);

            // Rename references
            foreach (Arranger arr in ArrangerList.Values)
            {
                foreach (ArrangerElement el in arr.ElementGrid)
                {
                    if (el.PaletteKey == PaletteName)
                        el.PaletteKey = NewPaletteName;
                }
            }

            foreach (Arranger arr in PersistentArrangerList.Values)
            {
                foreach (ArrangerElement el in arr.ElementGrid)
                {
                    if (el.PaletteKey == PaletteName)
                        el.PaletteKey = NewPaletteName;
                }
            }

            return true;
        }
        #endregion

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

        #region Cursor Management
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
        #endregion

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
            foreach (DataFile df in DataFileList.Values)
                df.Close();
            DataFileList.Clear();

            PaletteList.Clear();
            ArrangerList.Clear();
            PersistentArrangerList.Clear();
            PersistentPaletteList.Clear();

            return true;
        }

    }
}
