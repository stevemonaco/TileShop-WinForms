using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using MoreLinq;
using TileShop.ExtensionMethods;

namespace TileShop.Core
{
    /// <summary>
    /// Singleton class that manages file and editor resources
    /// A lease model is used for managing resources that are currently being edited
    /// A leased Resource is a resource that is intended to be edited and saved
    /// GetResource will return the edited copy of the Resource if is leased or the original copy if it is not
    /// This is so that Resources will be able to be previewed with unsaved changes in referenced Resources
    /// </summary>
    public sealed class ResourceManager
    {
        private Dictionary<string, ProjectResourceBase> ResourceTree = new Dictionary<string, ProjectResourceBase>();

        /// <summary>
        /// Maps resource keys to resources
        /// </summary>
        //private Dictionary<string, ProjectResourceBase> ResourceMap = new Dictionary<string, ProjectResourceBase>();
        /// <summary>
        /// Maps resource keys to copies of resources that are currently leased
        /// </summary>
        private Dictionary<string, ProjectResourceBase> LeasedResourceMap = new Dictionary<string, ProjectResourceBase>();

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
        /// List of cursors loaded
        /// </summary>
        private Dictionary<string, Cursor> CursorList = new Dictionary<string, Cursor>();

        /// <summary>
        /// FileTypeLoader which is used to match file extensions with the default graphics codec upon opening for sequential arranger
        /// </summary>
        private FileTypeLoader Loader = new FileTypeLoader();

        #region Lazy Singleton implementation
        private static readonly Lazy<ResourceManager> lazySingleton = new Lazy<ResourceManager>(() => new ResourceManager());

        public static ResourceManager Instance { get { return lazySingleton.Value; } }

        private ResourceManager()
        {
        }
        #endregion

        #region ProjectResourceBase Management
        /// <summary>
        /// Add a resource to ResourceManager by key
        /// </summary>
        /// <param name="Resource"></param>
        /// <returns></returns>
        public bool AddResource(string ResourceKey, ProjectResourceBase Resource)
        {
            if (Resource is null)
                throw new ArgumentNullException("Null argument passed into AddResource");
            if (Resource.Name is null)
                throw new ArgumentException("Argument with null Name property passed into AddResource");

            if (ResourceTree.ContainsResource(ResourceKey))
                return false;

            ResourceTree.AddResource(ResourceKey, Resource);
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
            if (ResourceKey is null)
                throw new ArgumentException("Null name argument passed into RemoveResource");
            if (LeasedResourceMap.ContainsKey(ResourceKey)) // Resource still in use
                return false;

            if (ResourceTree.ContainsResource(ResourceKey))
                ResourceTree.Remove(ResourceKey);

            return true;
        }

        /// <summary>
        /// Gets a resource by key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns>A leased resource if available, otherwise the original resource</returns>
        public ProjectResourceBase GetResource(string resourceKey)
        {
            if (resourceKey is null)
                throw new ArgumentException("Null name argument passed into GetResource");
            if (LeasedResourceMap.ContainsKey(resourceKey))
                return LeasedResourceMap[resourceKey];

            ProjectResourceBase res;

            if (ResourceTree.TryGetResource(resourceKey, out res))
                return res;

            throw new KeyNotFoundException($"Key '{resourceKey}' not found in ResourceManager");

            //if (ResourceMap.ContainsKey(ResourceKey))
            //    return ResourceMap[ResourceKey];

            //throw new KeyNotFoundException($"Key '{ResourceKey}' not found in ResourceManager");
        }

        /// <summary>
        /// Determines if the ResourceManager has a resource associated with the specified key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns>True if the key is in use</returns>
        public bool HasResource(string resourceKey)
        {
            if (resourceKey is null)
                throw new ArgumentException("Null name argument passed into HasResource");

            return ResourceTree.ContainsResource(resourceKey);
        }

        /// <summary>
        /// Leases a resource by key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns>A leased resource</returns>
        public ProjectResourceBase LeaseResource(string resourceKey)
        {
            if (resourceKey is null)
                throw new ArgumentException("Null name argument passed into LeaseResource");

            if (LeasedResourceMap.ContainsKey(resourceKey))
                throw new InvalidOperationException($"Key {resourceKey} is already being leased");

            ProjectResourceBase resource;

            if(ResourceTree.TryGetResource(resourceKey, out resource))
            {
                ProjectResourceBase clonedResource = resource.Clone();
                LeasedResourceMap.Add(resourceKey, clonedResource);
                return clonedResource;
            }

            throw new KeyNotFoundException($"Key '{resourceKey}' not found in ResourceManager");
        }

        /// <summary>
        /// Returns the lease status of a resource key
        /// </summary>
        /// <param name="ResourceKey"></param>
        /// <returns>True if the resource is leased</returns>
        public bool IsResourceLeased(string ResourceKey)
        {
            if (ResourceKey is null)
                throw new ArgumentNullException("Null name argument passed into IsResourceLeased");
            if (!ResourceTree.ContainsResource(ResourceKey))
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

            if (!ResourceTree.ContainsResource(ResourceKey))
                throw new KeyNotFoundException($"Key '{ResourceKey}' not found in ResourceManager");

            // TODO: Save leased object

            //ResourceMap[ResourceKey] = LeasedResourceMap[ResourceKey];
            LeasedResourceMap.Remove(ResourceKey);
        }

        /// <summary>
        /// Creates a SequentialArranger created from a DataFile
        /// </summary>
        /// <param name="DataFileKey">Key of a loaded DataFile</param>
        /// <param name="ResourceKey">Key of the SequentialArranger Resource to create as a leased resource</param>
        /// <returns></returns>
        public bool LeaseDataFileAsArranger(string DataFileKey, string ResourceKey, int ArrangerWidth, int ArrangerHeight)
        {
            if (!HasResource(DataFileKey) || HasResource(ResourceKey) || LeasedResourceMap.ContainsKey(ResourceKey))
                return false;

            string formatname = Loader.GetDefaultFormatName(DataFileKey);
            var arranger = new SequentialArranger(ArrangerWidth, ArrangerHeight, DataFileKey, GetGraphicsFormat(formatname));

            LeasedResourceMap.Add(ResourceKey, arranger);

            return true;
        }

        public bool MoveResource(string OldResourceKey, string NewResourceKey)
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
            ResourceTree.SelfAndDescendants().ForEach((x) =>
            {
                if (x is DataFile df)
                    df.Close();
            });

            LeasedResourceMap.Clear();
            ResourceTree.Clear();
        }

        public IEnumerable<string> ResourceKeys { get => ResourceTree.SelfAndDescendants().Select(x => x.ResourceKey); }
        #endregion

        #region XML Management        
        /// <summary>
        /// Loads the project from an XML source
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="baseDirectory">The base directory of the project</param>
        /// <returns></returns>
        public void LoadProject(Stream stream, string baseDirectory)
        {
            var tree = GameDescriptorSerializer.DeserializeProject(stream, baseDirectory);

            // Add root-level nodes to the ResourceTree
            foreach (var item in tree)
                ResourceTree.AddResource(item.Key, item.Value);

            // Invoke ResourceAdded event for every deserialized node
            foreach (var item in tree.SelfAndDescendants())
                ResourceAdded?.Invoke(this, new ResourceEventArgs(item.ResourceKey));

            return;
        }

        public void SaveProject(Stream stream)
        {
            GameDescriptorSerializer.SerializeProject(ResourceTree, stream);
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
            /*if (String.IsNullOrWhiteSpace(FileName) || String.IsNullOrWhiteSpace(NewFileName))
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
            if (Filename is null || PaletteName is null)
                throw new ArgumentNullException();

            Palette pal = new Palette(PaletteName);
            if (!pal.LoadPalette(Filename))
                return false;

            pal.ShouldBeSerialized = false;

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
