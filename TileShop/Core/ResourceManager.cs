﻿using System;
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
    /// Files loaded here are kept open always until ClearResources is called
    /// A lease model is used for managing resources that are currently being edited
    /// A leased Resource is a resource that is intended to be edited and saved
    /// GetResource will return the edited copy of the Resource if is leased or the original copy if it is not
    /// This is so that Resources will be able to be previewed with unsaved changes in referenced Resources
    /// </summary>

    // TODO - Consider future implementation of lazy instantiation for some objects, especially for projects that have many, many files
    public sealed class ResourceManager
    {
        /// <summary>
        /// Maps resource keys to resources
        /// </summary>
        private Dictionary<string, IProjectResource> ResourceMap = new Dictionary<string, IProjectResource>();
        /// <summary>
        /// Maps resource keys to resources that are currently leased
        /// </summary>
        private Dictionary<string, IProjectResource> LeasedResourceMap = new Dictionary<string, IProjectResource>();

        /// <summary>
        /// Events to notify UI components when resources have been added or renamed
        /// </summary>
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
        /// List of cursors loaded
        /// </summary>
        private Dictionary<string, Cursor> CursorList = new Dictionary<string, Cursor>();

        /// <summary>
        /// FileTypeLoader which is used to match file extensions with the default graphics codec upon opening for sequential arranger
        /// </summary>
        private FileTypeLoader Loader = new FileTypeLoader();

        const int DefaultElementsX = 16;
        const int DefaultElementsY = 16;

        #region Lazy Singleton implementation
        public static readonly Lazy<ResourceManager> lazySingleton = new Lazy<ResourceManager>(() => new ResourceManager());

        public static ResourceManager Instance { get { return lazySingleton.Value; } }

        private ResourceManager()
        {
        }
        #endregion

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

            throw new KeyNotFoundException($"Key '{ResourceKey}' not found in ResourceManager");
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
        /// Leases a resource by key
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns>A leased resource</returns>
        public IProjectResource LeaseResource(string ResourceKey)
        {
            if (ResourceKey == null)
                throw new ArgumentException("Null name argument passed into LeaseResource");

            if (LeasedResourceMap.ContainsKey(ResourceKey))
                throw new InvalidOperationException($"Key {ResourceKey} is already being leased");

            if(ResourceMap.ContainsKey(ResourceKey))
            {
                IProjectResource resource = ResourceMap[ResourceKey];
                LeasedResourceMap.Add(ResourceKey, resource);
                return resource;
            }

            throw new KeyNotFoundException($"Key '{ResourceKey}' not found in ResourceManager");
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
                throw new KeyNotFoundException($"Key '{ResourceKey}' not found in ResourceManager");

            if (LeasedResourceMap.ContainsKey(ResourceKey))
                return true;
            return false;
        }

        // TODO: Attempt DataFile -> HasA relationship instead
        public void ReturnLease(string ResourceKey, bool Save)
        {
            if (!LeasedResourceMap.ContainsKey(ResourceKey))
                throw new KeyNotFoundException($"Key '{ResourceKey}' attempted to return its lease but one is not active");

            if (!ResourceMap.ContainsKey(ResourceKey))
                throw new KeyNotFoundException($"Key '{ResourceKey}' not found in ResourceManager");

            // TODO: Save leased object

            ResourceMap[ResourceKey] = LeasedResourceMap[ResourceKey];
            LeasedResourceMap.Remove(ResourceKey);
        }

        /// <summary>
        /// Creates a SequentialArranger created from a DataFile
        /// </summary>
        /// <param name="DataFileKey">Key of a loaded DataFile</param>
        /// <param name="ResourceKey">Key of the SequentialArranger Resource to create as a leased resource</param>
        /// <returns></returns>
        public bool LeaseDataFileAsArranger(string DataFileKey, string ResourceKey)
        {
            if (!HasResource(DataFileKey) || HasResource(ResourceKey) || LeasedResourceMap.ContainsKey(ResourceKey))
                return false;

            string formatname = Loader.GetDefaultFormatName(DataFileKey);
            Arranger arranger = Arranger.NewSequentialArranger(DefaultElementsX, DefaultElementsY, DataFileKey, GetGraphicsFormat(formatname));

            LeasedResourceMap.Add(ResourceKey, arranger);

            return true;
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

        /// <summary>
        /// Clears all project resources
        /// Does not remove graphic formats, cursors, or file loaders
        /// </summary>
        /// <returns></returns>
        public void ClearResources()
        {
            foreach (KeyValuePair<string, IProjectResource> resource in ResourceMap)
            {
                if (resource.Value is DataFile df)
                    df.Close();
            }
            LeasedResourceMap.Clear();
            ResourceMap.Clear();
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

        #region Palette Management
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

    }
}
