using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TileShop.ExtensionMethods
{
    /// <summary>
    /// Extension methods for TreeView
    /// </summary>
    public static class TreeViewExtensions
    {
        public static bool TryGetNode(this TreeNodeCollection tree, string nodeKey, out TreeNode value)
        {
            if (String.IsNullOrWhiteSpace(nodeKey))
            {
                value = null;
                return false;
            }

            var paths = nodeKey.Split('\\');
            var nodeVisitor = tree;
            TreeNode node = null;

            foreach (var name in paths)
            {
                var index = nodeVisitor.IndexOfKey(name);
                if (index != -1) // Found
                {
                    node = nodeVisitor[index];
                    nodeVisitor = node.Nodes;
                }
                else // Not found
                {
                    value = null;
                    return false;
                }
            }

            value = node;
            return true;
        }

        public static bool TryGetParentNode(this TreeNodeCollection tree, string nodeKey, out TreeNode value)
        {
            if (String.IsNullOrWhiteSpace(nodeKey))
                throw new ArgumentException();

            return tree.TryGetNode(Path.GetDirectoryName(nodeKey), out value);
        }

        /// <summary>
        /// Allows depth-first iteration over a resource tree
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns></returns>
        /// <remarks>Idea adapted from https://www.benjamin.pizza/posts/2017-11-13-recursion-without-recursion.html 
        /// Implementation adapted from https://blogs.msdn.microsoft.com/wesdyer/2007/03/23/all-about-iterators/
        /// </remarks>
        public static IEnumerable<TreeNode> SelfAndDescendants(this TreeNodeCollection tree)
        {
            Stack<TreeNode> nodeStack = new Stack<TreeNode>();

            foreach (TreeNode node in tree)
                nodeStack.Push(node);

            while (nodeStack.Count > 0)
            {
                var node = nodeStack.Pop();
                yield return node;
                foreach (TreeNode child in node.Nodes)
                    nodeStack.Push(child);
            }
        }

        /// <summary>
        /// Ancestors of the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static IEnumerable<TreeNode> Ancestors(this TreeNode node)
        {
            var parentVisitor = node.Parent;

            while (parentVisitor != null)
            {
                yield return parentVisitor;
                parentVisitor = parentVisitor.Parent;
            }
        }
    }
}
