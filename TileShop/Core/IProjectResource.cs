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
    }
}
