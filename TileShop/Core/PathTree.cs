using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileShop.Core
{
    /*class PathTree<T>
    {
        PathTreeNode<T> Root;
        IComparer<T> Comparer;

        public PathTree() : this(Comparer<T>.Default)
        { }

        public PathTree(IComparer<T> nodeComparer)
        {
            Comparer = nodeComparer;
            //Children = new SortedSet<PathTreeNode<T>>(nodeComparer);
        }

        /// <summary>
        /// Adds the item to the specified path if the parent exists
        /// </summary>
        /// <param name="path">The path associated with the item</param>
        /// <param name="item">The item</param>
        public void Add(string path, T item)
        {
            string[] paths = path.Split(Path.PathSeparator);

            //var listVisitor = Children;

            //foreach (PathTreeNode<T> node in listVisitor)
            //{
                
            //}
        }

        public bool TryGetValue(string path, out T value)
        {
            if (String.IsNullOrWhiteSpace(path))
                throw new ArgumentException();

            value = default(T);

            var paths = path.Split('\\');
            var nodeVisitor = Root;

            for (int i = 0; i < paths.Length; i++)
            {
                if (!nodeVisitor.TryGetChildNode(paths[i], out nodeVisitor))
                    return false;
            }

            //value = nodeVisitor;

            //resource = node;
            return false;
        }

        private class PathTreeNode<V>
        {
            public Dictionary<string, PathTreeNode<V>> Children { get; private set; }
            public PathTreeNode<V> Parent { get; set; }

            public PathTreeNode()
            {

            }

            public bool TryGetChildNode(string name, out PathTreeNode<V> value)
            {
                if (Children.TryGetValue(name, out value))
                    return true;

                return false;
            }
        }
    }

    //class PathTreeNodeComparer<T> : where T is PathTreeNode<>
    //{
    //}*/
}
