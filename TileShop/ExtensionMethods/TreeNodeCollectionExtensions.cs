using System;
using System.Windows.Forms;
using TileShop.Core;

namespace TileShop.ExtensionMethods
{
    /// <summary>
    /// TileShop-specific extension methods to TreeNodeCollections
    /// </summary>
    public static class TreeNodeCollectionExtensions
    {
        public static bool AddResource(this TreeNodeCollection tnc, IResource Resource, string ParentFolderPath)
        {
            switch(Resource)
            {
                case Arranger arranger:
                    break;
                case DataFile dataFile:
                    break;
                case Palette palette:
                    break;
                default:
                    throw new ArgumentException("AddResource attempted to add an unsupported resource type of " + Resource.GetType().Name);
            }

            return false;
        }

        public static bool RemoveResource(this TreeNodeCollection tnc, string ParentFolderPath)
        {
            return false;
        }
    }
}
