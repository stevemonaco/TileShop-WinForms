using System.Xml.Linq;

namespace TileShop.Core
{
    /// <summary>
    /// Interface specifying how TileShop Project Resource objects must be implemented
    /// </summary>
    public interface IProjectResource
    {
        /// <summary>
        /// Identifying name of the resource
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Rename a resource with a new name
        /// </summary>
        /// <param name="name"></param>
        void Rename(string name);

        /// <summary>
        /// Deep-clone copy of the object
        /// </summary>
        IProjectResource Clone();

        /// <summary>
        /// Serializes resource into an XElement
        /// </summary>
        /// <returns></returns>
        //XElement Serialize();

        /// <summary>
        /// Deserialize XElement into IProjectResource
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        //bool Deserialize(XElement element);
    }
}
