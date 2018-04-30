using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TileShop.Core;

namespace TileShop.ExtensionMethods
{
    public static class ResourceTreeExtensions
    {
        public static ProjectResourceBase FindResource(this IDictionary<string, ProjectResourceBase> tree, string searchPath)
        {
            if (String.IsNullOrEmpty(searchPath))
                throw new ArgumentException();

            var paths = searchPath.Split('\\');
            ProjectResourceBase node;

            if (tree.ContainsKey(paths[0]))
                node = tree[paths[0]];
            else
                return null;
                //throw new KeyNotFoundException($"Resource {paths[0]} in {searchPath} not found");

            for(int i = 1; i < paths.Length; i++)
            {
                if (node.ChildResources.ContainsKey(paths[i]))
                    node = node.ChildResources[paths[i]];
                else
                    return null;
                    //throw new KeyNotFoundException($"Resource {paths[i]} in {searchPath} not found");
            }

            return node;
        }

        /*private static ProjectResourceBase FindResource(this ProjectResourceBase resource, string[] paths)
        {
            var resVisitor = resource;

            foreach (string path in paths)
            {
                //if
            }

            return null;
        }*/

        /// <summary>
        /// Allows iteration over a resource tree
        /// </summary>
        /// <param name="tree">The tree.</param>
        /// <returns></returns>
        /// <remarks>Code adapted from https://www.benjamin.pizza/posts/2017-11-13-recursion-without-recursion.html </remarks>
        public static IEnumerable<KeyValuePair<string, ProjectResourceBase>> SelfAndDescendants(this IDictionary<string, ProjectResourceBase> tree)
        {
            foreach (var child in tree)
            {
                yield return child;
                foreach (var descendant in SelfAndDescendants(child.Value.ChildResources))
                    yield return descendant;
            }
        }
    }
}
