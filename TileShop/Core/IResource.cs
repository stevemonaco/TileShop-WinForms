namespace TileShop.Core
{
    /// <summary>
    /// Interface specifying how TileShop Resource objects must be implemented
    /// </summary>
    public interface IResource
    {
        /// <summary>
        /// Identifying name of the IResource
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Rename an IResource with a new name
        /// </summary>
        /// <param name="name"></param>
        void Rename(string name);
    }
}
