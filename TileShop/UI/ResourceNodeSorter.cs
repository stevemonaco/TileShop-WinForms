using System.Collections;

namespace TileShop
{
    class ResourceNodeSorter : IComparer
    {
        public int Compare(object x, object y)
        {
            ResourceNode rx = x as ResourceNode;
            ResourceNode ry = y as ResourceNode;

            if (rx is FolderNode && ry is FolderNode)
                return string.Compare(rx.Text, ry.Text);
            else if (rx is FolderNode)
                return -1;
            else if (ry is FolderNode)
                return 1;
            else
                return string.Compare(rx.Text, ry.Text);
        }
    }
}
