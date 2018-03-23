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
        /// Maps resource keys to resources
        /// </summary>
        private Dictionary<string, IProjectResource> ResourceMap = new Dictionary<string, IProjectResource>();
        /// <summary>
        /// Maps resource keys to resources that are currently leased
        /// </summary>
        private Dictionary<string, IProjectResource> LeasedResourceMap = new Dictionary<string, IProjectResource>();

        public EventHandler<ResourceEventArgs> ResourceAdded;
        public EventHandler<ResourceEventArgs> ResourceRenamed;

        /// <summary>
        /// List of graphics format codecs
        /// </summary>
        private Dictionary<string, GraphicsFormat> FormatList = new Dictionary<string, GraphicsFormat>();

        /// <summary>
        /// List of in-use files on disk that can be read or written to by subeditors such as the arranger or palette editor
        /// </summary>
        //private Dictionary<string, DataFile> DataFileList = new Dictionary<string, DataFile>();

        /// <summary>
        /// List of arrangers that may be edited by the user
        /// </summary>
        //private Dictionary<string, Arranger> ArrangerList = new Dictionary<string, Arranger>();

        /// <summary>
        /// List of palettes that may be edited by the user
        /// </summary>
        //private Dictionary<string, Palette> PaletteList = new Dictionary<string, Palette>();

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

        #region IProjectResource Management
        /// <summary>
        /// Add a resource to ResourceManager by key
        /// </summary>
        /// <param name="Resource"></param>
        /// <returns></returns>
        public bool AddResource(string ResourceKey, IProjectResource Resource)
        {
            if (Resource == null)
                throw new ArgumentNullException("Null argument passed into AddResource");
            if (Resource.Name == null)
                throw new ArgumentException("Argument with null Name property passed into AddResource");
            if (ResourceMap.ContainsKey(ResourceKey))
                return false;

            ResourceMap.Add(ResourceKey, Resource);
            ResourceAdded?.Invoke(this, new ResourceEventArgs(ResourceKey));
            return true;
        }

        /// <summary>
        /// Remove a resource from ResourceManager by key
        /// </summary>
        /// <param name="ResourceKey">Name of the resource to be removed</param>
        /// <returns>True if removed or no key exists, false if the resource is leased</returns>
        public bool RemoveResource(string ResourceKey)
        {
            if (ResourceKey == null)
                throw new ArgumentException("Null name argument passed into RemoveResource");
            if (LeasedResourceMap.ContainsKey(ResourceKey)) // Resource still in use
                return false;

            if (ResourceMap.ContainsKey(ResourceKey))
                ResourceMap.Remove(ResourceKey);

            return true;
        }

        /// <summary>
        /// Gets a resource by key
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns>A leased resource if available, otherwise the original resource</returns>
        public IProjectResource GetResource(string ResourceKey)
        {
            if (ResourceKey == null)
                throw new ArgumentException("Null name argument passed into GetResource");
            if (LeasedResourceMap.ContainsKey(ResourceKey))
                return LeasedResourceMap[ResourceKey];

            if (ResourceMap.ContainsKey(ResourceKey))
                return ResourceMap[ResourceKey];

            throw new KeyNotFoundException("Key '{ResourceKey}' not found in ResourceManager");
        }

        /// <summary>
        /// Determines if the ResourceManager has a resource associated with the specified key
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns>True if the key is in use</returns>
        public bool HasResource(string ResourceKey)
        {
            if (ResourceKey == null)
                throw new ArgumentException("Null name argument passed into HasResource");

            if (ResourceMap.ContainsKey(ResourceKey))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the ResourceMap
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, IProjectResource> GetResourceMap()
        {
            return ResourceMap;
        }

        /// <summary>
        /// Leases a resource by key
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns>A leased resource</returns>
        public IProjectResource LeaseResource(string ResourceKey)
        {
            if (ResourceKey == null)
                throw new ArgumentException("Null name argument passed into LeaseResource");

            if (LeasedResourceMap.ContainsKey(ResourceKey))
                throw new InvalidOperationException("Key {ResourceKey} is already being leased");

            if(ResourceMap.ContainsKey(ResourceKey))
            {
                IProjectResource resource = ResourceMap[ResourceKey];
                LeasedResourceMap.Add(ResourceKey, resource);
                return resource;
            }

            throw new KeyNotFoundException("Key '{ResourceKey}' not found in ResourceManager");
        }

        /// <summary>
        /// Returns the lease status of a resource key
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns>True if the resource is leased</returns>
        public bool IsResourceLeased(string ResourceKey)
        {
            if (ResourceKey == null)
                throw new ArgumentNullException("Null name argument passed into IsResourceLeased");
            if (!ResourceMap.ContainsKey(ResourceKey))
                throw new KeyNotFoundException("Key '{ResourceKey}' not found in ResourceManager");

            if (LeasedResourceMap.ContainsKey(ResourceKey))
                return true;
            return false;
        }

        public void ReturnLease(string ResourceKey)
        {
            if (!LeasedResourceMap.ContainsKey(ResourceKey))
                throw new KeyNotFoundException("Key '{ResourceKey}' attempted to return its lease but one is not active");

            if (!ResourceMap.ContainsKey(ResourceKey))
                throw new KeyNotFoundException("Key '{ResourceKey}' not found in ResourceManager");

            ResourceMap[ResourceKey] = LeasedResourceMap[ResourceKey];
            LeasedResourceMap.Remove(ResourceKey);
        }

        /// <summary>
        /// Rename a resource in ResourceManager
        /// </summary>
        /// <param name="OldResourceName"></param>
        /// <param name="NewResourceName"></param>
        /// <returns></returns>
        public bool RenameResource(string OldResourceName, string NewResourceName)
        {
            throw new NotImplementedException();
        }
        #endregion

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
        /*public bool AddDataFile(DataFile dataFile, string key)
        {
            if (dataFile == null || key == null)
                throw new ArgumentNullException();

            if (!HasDataFile(key))
            {
                DataFileList[key] = dataFile;
            }

            return true;
        }*/

        /// <summary>
        /// Retrieves a DataFile by its key
        /// </summary>
        /// <param name="dataFileKey"></param>
        /// <returns></returns>
        /*public DataFile GetDataFile(string dataFileKey)
        {
            if (DataFileList.ContainsKey(dataFileKey))
                return DataFileList[dataFileKey];
            else
                throw new KeyNotFoundException();
        }*/

        public void RemoveFile(string FileName)
        {
            /*if (DataFileList.ContainsKey(FileName))
            {
                DataFileList[FileName].Close();
                DataFileList.Remove(FileName);
            }*/
        }

        /// <summary>
        /// Renames a file that is currently loaded into the FileManager, renames it on disk, and remaps all references to it in the project
        /// </summary>
        /// <param name="FileName">FileManager file to be renamed</param>
        /// <param name="NewFileName">Name that the file will be renamed to</param>
        /// <returns>Success state</returns>
        public bool RenameFile(string FileName, string NewFileName)
        {
            /*if (String.IsNullOrEmpty(FileName) || String.IsNullOrEmpty(NewFileName))
                throw new ArgumentException("");

            // Must contain FileName and must not contain NewFileName
            if (HasDataFile(NewFileName) || !HasDataFile(FileName) || File.Exists(NewFileName))
                return false;

            // File must not already exist
            if (File.Exists(NewFileName))
                return false;

            DataFile df = GetResource(FileName) as DataFile;
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

            foreach (Arranger arr in ArrangerList.Values)
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
            }*/

            return true;
        }

        #endregion

        #region Arranger Management
        /*public void AddArranger(Arranger arr, string key)
        {
            if (arr == null || key == null)
                throw new ArgumentNullException();

            if (!HasArranger(key))
            {
                PersistentArrangerList.Add(key, arr);
                ArrangerList.Add(key, arr.Clone());
            }
        }*/

        /// <summary>
        /// Creates a sequential arranger from a data file
        /// </summary>
        /// <param name="dataFileKey">Key of a previously loaded data file</param>
        /// <returns></returns>
        public bool LoadSequentialArranger(string dataFileKey)
        {
            string formatname = Loader.GetDefaultFormatName(dataFileKey);
            Arranger arranger = Arranger.NewSequentialArranger(DefaultElementsX, DefaultElementsY, dataFileKey, GetGraphicsFormat(formatname));

            // TODO: FIX ASAP
            //AddArranger(arranger, dataFileKey); // TODO: Ensure that Filename is local to the XML project file

            return true;
        }

        /// <summary>
        /// Renames an arranger that is currently loaded into the FileManager
        /// </summary>
        /// <param name="ArrangerName">Arranger to be renamed</param>
        /// <param name="NewArrangerName">Name that the arranger will be renamed to</param>
        /// <returns></returns>
        /*public bool RenameArranger(string ArrangerName, string NewArrangerName)
        {
            if (!HasArranger(ArrangerName) || HasArranger(NewArrangerName))
                return false;

            Arranger arr = GetResource(ArrangerName) as Arranger;
            arr.Rename(NewArrangerName);
            ArrangerList.Remove(ArrangerName);
            ArrangerList.Add(NewArrangerName, arr);

            return true;
        }*/
        #endregion

        #region Palette Management
        /*public void AddPalette(Palette pal, string key)
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
        }*/

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

            AddResource(pal.Name, pal);

            return true;
        }

        /// <summary>
        /// Gets the editable palette associated with the specified name
        /// </summary>
        /// <param name="PaletteName"></param>
        /// <returns></returns>
        /*public Palette GetPalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName))
                return PaletteList[PaletteName];
            else
                throw new KeyNotFoundException(String.Format("Palette {0} was not found in the PaletteList", PaletteName));
        }*/

        public bool HasPalette(string PaletteName)
        {
            return false;
            //return PersistentPaletteList.ContainsKey(PaletteName);
        }

        /// <summary>
        /// Renames an palette that is currently loaded into the FileManager and remaps all references to it in the project
        /// </summary>
        /// <param name="PaletteName">Palette to be renamed</param>
        /// <param name="NewPaletteName">Name that the palette will be renamed to</param>
        /// <returns></returns>
        public bool RenamePalette(string PaletteName, string NewPaletteName)
        {
            /*if (!HasPalette(PaletteName) || HasPalette(NewPaletteName))
                return false;

            Palette pal = GetResource(PaletteName) as Palette;
            pal.SetFileKey(NewPaletteName);
            PaletteList.Remove(NewPaletteName);
            PaletteList.Add(NewPaletteName, pal);

            // Rename references
            foreach (Arranger arr in ArrangerList.Values)
            {
                foreach (ArrangerElement el in arr.ElementGrid)
                {
                    if (el.PaletteKey == PaletteName)
                        el.PaletteKey = NewPaletteName;
                }
            }*/

            return true;
        }
        #endregion

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
        /// Clears all project resources
        /// Does not remove graphic formats, cursors, or file loaders
        /// </summary>
        /// <returns></returns>
        public void ClearResources()
        {
            foreach(KeyValuePair<string, IProjectResource> resource in ResourceMap)
            {
                if (resource.Value is DataFile df)
                    df.Close();
            }
            LeasedResourceMap.Clear();
            ResourceMap.Clear();
        }

    }
}
