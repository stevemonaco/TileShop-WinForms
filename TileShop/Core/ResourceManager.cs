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
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool AddResource(string resourceKey, ProjectResourceBase resource)
        {
            if (resource is null)
                throw new ArgumentNullException("Null argument passed into AddResource");
            if (resource.Name is null)
                throw new ArgumentException("Argument with null Name property passed into AddResource");

            if (Instance.ResourceTree.ContainsResource(resourceKey))
                return false;

            Instance.ResourceTree.AddResource(resourceKey, resource);
            Instance.ResourceAdded?.Invoke(Instance, new ResourceEventArgs(resourceKey));
            return true;
        }

        /// <summary>
        /// Remove a resource from ResourceManager by key
        /// </summary>
        /// <param name="resourceKey">Name of the resource to be removed</param>
        /// <returns>True if removed or no key exists, false if the resource is leased</returns>
        public static bool RemoveResource(string resourceKey)
        {
            if (resourceKey is null)
                throw new ArgumentException("Null name argument passed into RemoveResource");
            if (Instance.LeasedResourceMap.ContainsKey(resourceKey)) // Resource still in use
                return false;

            if (Instance.ResourceTree.ContainsResource(resourceKey))
                Instance.ResourceTree.Remove(resourceKey);

            return true;
        }

        /// <summary>
        /// Gets a resource by key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns>A leased resource if available, otherwise the original resource</returns>
        /*public ProjectResourceBase GetResource(string resourceKey)
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
        }*/

        /// <summary>
        /// Gets a resource by key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resourceKey">The resource key.</param>
        /// <returns>A leased resource if available, otherwise the original resource</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static T GetResource<T>(string resourceKey) where T : ProjectResourceBase
        {
            ProjectResourceBase res;

            if (resourceKey is null)
                throw new ArgumentException($"Null name argument passed into {nameof(GetResource)}");

            if (Instance.LeasedResourceMap.ContainsKey(resourceKey))
                res = Instance.LeasedResourceMap[resourceKey];
            else
                Instance.ResourceTree.TryGetResource(resourceKey, out res);
                //res = Instance.GetResource(resourceKey);

            if (res is T tRes)
                return tRes;

            throw new KeyNotFoundException($"Key '{resourceKey}' of requested type not found in ResourceManager");
        }

        /// <summary>
        /// Determines if the ResourceManager has a resource associated with the specified key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns>True if the key is in use</returns>
        public static bool HasResource(string resourceKey)
        {
            if (resourceKey is null)
                throw new ArgumentException("Null name argument passed into HasResource");

            return Instance.ResourceTree.ContainsResource(resourceKey);
        }

        /// <summary>
        /// Leases a resource by key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns>A leased resource</returns>
        public static ProjectResourceBase LeaseResource(string resourceKey)
        {
            if (resourceKey is null)
                throw new ArgumentException("Null name argument passed into LeaseResource");

            if (Instance.LeasedResourceMap.ContainsKey(resourceKey))
                throw new InvalidOperationException($"Key {resourceKey} is already being leased");

            ProjectResourceBase resource;

            if(Instance.ResourceTree.TryGetResource(resourceKey, out resource))
            {
                ProjectResourceBase clonedResource = resource.Clone();
                Instance.LeasedResourceMap.Add(resourceKey, clonedResource);
                return clonedResource;
            }

            throw new KeyNotFoundException($"Key '{resourceKey}' not found in ResourceManager");
        }

        /// <summary>
        /// Returns the lease status of a resource key
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <returns>True if the resource is leased</returns>
        public static bool IsResourceLeased(string resourceKey)
        {
            if (resourceKey is null)
                throw new ArgumentNullException("Null name argument passed into IsResourceLeased");
            if (!Instance.ResourceTree.ContainsResource(resourceKey))
                throw new KeyNotFoundException($"Key '{resourceKey}' not found in ResourceManager");

            if (Instance.LeasedResourceMap.ContainsKey(resourceKey))
                return true;
            return false;
        }

        // TODO: Attempt DataFile -> HasA relationship instead
        public static void ReturnLease(string resourceKey, bool save)
        {
            if (!Instance.LeasedResourceMap.ContainsKey(resourceKey))
                throw new KeyNotFoundException($"Key '{resourceKey}' attempted to return its lease but one is not active");

            if (!Instance.ResourceTree.ContainsResource(resourceKey))
                throw new KeyNotFoundException($"Key '{resourceKey}' not found in ResourceManager");

            // TODO: Save leased object

            //ResourceMap[ResourceKey] = LeasedResourceMap[ResourceKey];
            Instance.LeasedResourceMap.Remove(resourceKey);
        }

        /// <summary>
        /// Creates a SequentialArranger created from a DataFile
        /// </summary>
        /// <param name="dataFileKey">Key of a loaded DataFile</param>
        /// <param name="resourceKey">Key of the SequentialArranger Resource to create as a leased resource</param>
        /// <returns></returns>
        public bool LeaseDataFileAsArranger(string dataFileKey, string resourceKey, int arrangerWidth, int arrangerHeight)
        {
            if (!HasResource(dataFileKey) || HasResource(resourceKey) || LeasedResourceMap.ContainsKey(resourceKey))
                return false;

            string formatname = Loader.GetDefaultFormatName(dataFileKey);
            var arranger = new SequentialArranger(arrangerWidth, arrangerHeight, dataFileKey, GetGraphicsFormat(formatname));

            LeasedResourceMap.Add(resourceKey, arranger);

            return true;
        }

        public static bool MoveResource(string oldResourceKey, string newResourceKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears all project resources
        /// Does not remove graphic formats, cursors, or file loaders
        /// </summary>
        /// <returns></returns>
        public static void ClearResources()
        {
            Instance.ResourceTree.SelfAndDescendants().ForEach((x) =>
            {
                if (x is DataFile df)
                    df.Close();
            });

            Instance.LeasedResourceMap.Clear();
            Instance.ResourceTree.Clear();
        }

        public static IEnumerable<string> ResourceKeys { get => Instance.ResourceTree.SelfAndDescendants().Select(x => x.ResourceKey); }
        #endregion

        #region XML Management        
        /// <summary>
        /// Loads the project from an XML source
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="baseDirectory">The base directory of the project</param>
        /// <returns></returns>
        public static void LoadProject(Stream stream, string baseDirectory)
        {
            var tree = GameDescriptorSerializer.DeserializeProject(stream, baseDirectory);

            // Add root-level nodes to the ResourceTree
            foreach (var item in tree)
                Instance.ResourceTree.AddResource(item.Key, item.Value);

            // Invoke ResourceAdded event for every deserialized node
            foreach (var item in tree.SelfAndDescendants())
                Instance.ResourceAdded?.Invoke(Instance, new ResourceEventArgs(item.ResourceKey));

            return;
        }

        public static void SaveProject(Stream stream)
        {
            GameDescriptorSerializer.SerializeProject(Instance.ResourceTree, stream);
        }

        #endregion

        #region GraphicsFormat Management
        public static void AddGraphicsFormat(GraphicsFormat format)
        {
            Instance.FormatList.Add(format.Name, format);
        }

        public static bool LoadFormat(string filename)
        {
            GraphicsFormat fmt = new GraphicsFormat();
            if (!fmt.LoadFromXml(filename))
                return false;

            AddGraphicsFormat(fmt);
            return true;
        }

        public static GraphicsFormat GetGraphicsFormat(string formatName)
        {
            if (Instance.FormatList.ContainsKey(formatName))
                return Instance.FormatList[formatName];
            else
                throw new KeyNotFoundException();
        }

        public static IEnumerable<string> GetGraphicsFormatsNameList()
        {
            var keyList = Instance.FormatList.Keys.ToList();
            keyList.Sort();

            return keyList;
        }
        #endregion

        #region DataFile Management

        /// <summary>
        /// Renames a file that is currently loaded into the FileManager, renames it on disk, and remaps all references to it in the project
        /// </summary>
        /// <param name="fileName">FileManager file to be renamed</param>
        /// <param name="newFileName">Name that the file will be renamed to</param>
        /// <returns>Success state</returns>
        public static bool RenameFile(string fileName, string newFileName)
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
        /// <param name="filename">Filename to palette to be loaded</param>
        /// <param name="paletteName">Name associated to the palette within FileManager </param>
        /// <returns></returns>
        public static bool LoadPalette(string filename, string paletteName)
        {
            if (filename is null || paletteName is null)
                throw new ArgumentNullException();

            Palette pal = new Palette(paletteName);
            if (!pal.LoadPalette(filename))
                return false;

            pal.ShouldBeSerialized = false;

            AddResource(pal.Name, pal);

            return true;
        }

        /// <summary>
        /// Renames an palette that is currently loaded into the FileManager and remaps all references to it in the project
        /// </summary>
        /// <param name="paletteName">Palette to be renamed</param>
        /// <param name="newPaletteName">Name that the palette will be renamed to</param>
        /// <returns></returns>
        public static bool RenamePalette(string paletteName, string newPaletteName)
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
        public static bool AddCursor(string cursorName, Cursor cursor)
        {
            if (!Instance.CursorList.ContainsKey(cursorName))
            {
                Instance.CursorList.Add(cursorName, cursor);
                return true;
            }
            return false;
        }

        public static Cursor GetCursor(string cursorName)
        {
            if (Instance.CursorList.ContainsKey(cursorName))
                return Instance.CursorList[cursorName];
            else
                throw new KeyNotFoundException();
        }
        #endregion

    }
}
