using System.Collections.Generic;
using System.Windows.Forms;

namespace TileShop.ExtensionMethods
{
    /// <summary>
    /// Extension methods for TreeView
    /// </summary>
    public static class TreeViewExtensions
    {
        /// <summary>
        /// Gets a flat list of all nodes in the tree
        /// </summary>
        /// <param name="Self"></param>
        /// <returns></returns>
        public static List<TreeNode> GetAllNodes(this TreeView Self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in Self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        /// <summary>
        /// Gets a flat list of all child nodes
        /// </summary>
        /// <param name="Self"></param>
        /// <returns></returns>
        public static List<TreeNode> GetAllNodes(this TreeNode Self)
        {
            List<TreeNode> result = new List<TreeNode> { Self };
            foreach (TreeNode child in Self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }
    }
}
