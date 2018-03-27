using System.Windows.Forms;

namespace TileShop.ExtensionMethods
{
    /// <summary>
    /// Extension methods for TreeNode
    /// </summary>
    public static class TreeNodeExtensions
    {
        /// <summary>
        /// Builds a node path for the specified node
        /// </summary>
        /// <param name="node">Specified node</param>
        /// <returns>Node path</returns>
        public static string GetNodePath(this TreeNode node)
        {
            string path = "";
            TreeNode currentNode = node.Parent;

            while (currentNode != null) // Traverse up parent nodes to build the node path
            {
                if (path == "")
                    path = currentNode.Text;
                else
                    path = currentNode.Text + "\\" + path;

                currentNode = currentNode.Parent;
            }

            return path;
        }
    }
}
