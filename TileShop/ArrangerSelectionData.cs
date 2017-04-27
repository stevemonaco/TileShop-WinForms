using System;
using System.Drawing;

namespace TileShop
{
    // Class to store a selection of arranger data
    public class ArrangerSelectionData
    {
        public string ArrangerName { get; private set; } // Name of the Arranger which holds the data to be copied
        public Point Location { get; private set; } // Upper left location of the selection, in element units
        public Size SelectionSize { get; private set; } // Size of selection in number of elements
        public bool HasSelection { get; private set; }
        public bool InSelection { get; private set; }
        public bool InDragState { get; private set; }
        public bool SelectionChanged { get; private set; } // Lets calling class to check if the selection has changed
        public int Zoom { get; set; }
        public Point BeginPoint { get; private set; } // BeginPoint/EndPoint as provided by the caller in zoomed coordinates
        public Point EndPoint { get; private set; }
        public Rectangle SelectedElements { get; private set; } // Selected elements of the parent arranger in units of elements
        public Rectangle SelectedClientRect { get; private set; }

        public ArrangerElement[,] ElementList { get; private set; }

        public ArrangerSelectionData(string arrangerName)
        {
            ArrangerName = arrangerName;
            ClearSelection();
        }

        public void ClearSelection()
        {
            Location = new Point(0, 0);
            SelectionSize = new Size(0, 0);
            ElementList = null;
            HasSelection = false;
            InSelection = false;
            InDragState = false;
            SelectionChanged = false;
            SelectedElements = new Rectangle(0, 0, 0, 0);
            SelectedClientRect = new Rectangle(0, 0, 0, 0);
            BeginPoint = new Point(0, 0);
            EndPoint = new Point(0, 0);
        }

        // Populates ElementList
        public bool PopulateData()
        {
            if (!HasSelection)
                return false;

            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            ElementList = new ArrangerElement[SelectionSize.Width, SelectionSize.Height];
            for (int ysrc = SelectedElements.Y, ydest = 0; ydest < SelectionSize.Height; ydest++, ysrc++)
            {
                for (int xsrc = SelectedElements.X, xdest = 0; xdest < SelectionSize.Width; xdest++, xsrc++)
                {
                    ElementList[xdest, ydest] = arr.GetElement(xsrc, ysrc).Clone();
                }
            }

            return true;
        }

        public ArrangerElement GetElement(int ElementX, int ElementY)
        {
            return ElementList[ElementX, ElementY];
        }

        public void BeginSelection(Point beginPoint, Point endPoint)
        {
            HasSelection = true;
            InSelection = true;
            SelectionChanged = true;
            BeginPoint = beginPoint;
            EndPoint = endPoint;
            CalculateRectangles();
        }

        // Returns true if the selection was changed (ie. endpoint changed)
        public bool UpdateSelection(Point endPoint)
        {
            if (EndPoint != endPoint)
            {
                EndPoint = endPoint;
                CalculateRectangles();
                return true;
            }
            else // No need to set as the two points are equal
                return false;
        }

        public void EndSelection()
        {
            InSelection = false;
            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);
            Rectangle testBounds = new Rectangle(new Point(0, 0), arr.ArrangerElementSize);

            if (!SelectedElements.IntersectsWith(testBounds)) // No intersection means no selection
            {
                ClearSelection();
                HasSelection = false;
            }

            //PopulateData(); // Clean this up so that PopulateData will not crash from out of ClientRect clicks
        }

        public void BeginDragDrop()
        {
            InDragState = true;
        }

        public void EndDragDrop()
        {
            InDragState = false;
        }


        public Point PointToElementLocation(Point Location)
        {
            Point unzoomed = new Point(Location.X / Zoom, Location.Y / Zoom);

            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            // Search list for element
            for (int y = 0; y < arr.ArrangerElementSize.Height; y++)
            {
                for (int x = 0; x < arr.ArrangerElementSize.Width; x++)
                {
                    ArrangerElement el = arr.ElementList[x, y];
                    if (unzoomed.X >= el.X1 && unzoomed.X <= el.X2 && unzoomed.Y >= el.Y1 && unzoomed.Y <= el.Y2)
                        return new Point(x, y);
                }
            }

            throw new ArgumentOutOfRangeException("Location is outside of the range of all ArrangerElements in ElementList");
        }

        private void CalculateRectangles()
        {
            Rectangle zoomed = PointsToRectangle(BeginPoint, EndPoint); // Rectangle in zoomed coordinates
            Rectangle unzoomed = ViewerToArrangerRectangle(zoomed);
            Rectangle unzoomedfull = GetSelectionPixelRect(unzoomed);

            SelectedClientRect = new Rectangle(unzoomedfull.X * Zoom, unzoomedfull.Y * Zoom, unzoomedfull.Width * Zoom, unzoomedfull.Height * Zoom);

            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            SelectedElements = new Rectangle(unzoomedfull.X / arr.ElementPixelSize.Width, unzoomedfull.Y / arr.ElementPixelSize.Height,
                unzoomedfull.Width / arr.ElementPixelSize.Width, unzoomedfull.Height / arr.ElementPixelSize.Height);

            SelectionSize = new Size(SelectedElements.Width, SelectedElements.Height);
        }

        private Rectangle GetSelectionPixelRect(Rectangle r)
        {
            Arranger arr = FileManager.Instance.GetArranger(ArrangerName);

            int x1 = r.Left;
            int x2 = r.Right;
            int y1 = r.Top;
            int y2 = r.Bottom;

            // Extend rectangle to include the entirety of partially selected tiles
            foreach (ArrangerElement el in arr.ElementList)
            {
                if (x1 > el.X1 && x1 <= el.X2)
                    x1 = el.X1;
                if (y1 > el.Y1 && y1 <= el.Y2)
                    y1 = el.Y1;
                if (x2 < el.X2 && x2 >= el.X1)
                    x2 = el.X2;
                if (y2 < el.Y2 && y2 >= el.Y1)
                    y2 = el.Y2;
            }

            x2++; // Fix edges
            y2++;

            // Clamp selection rectangle to max bounds of the arranger
            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
            if (x2 >= arr.ArrangerPixelSize.Width)
                x2 = arr.ArrangerPixelSize.Width;
            if (y2 >= arr.ArrangerPixelSize.Height)
                y2 = arr.ArrangerPixelSize.Height;

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        private Rectangle ViewerToArrangerRectangle(Rectangle ClientRect)
        {
            int pleft = ClientRect.Left / Zoom;
            int ptop = ClientRect.Top / Zoom;
            int pright = (int)(ClientRect.Left / (float)Zoom + (ClientRect.Right - ClientRect.Left) / (float)Zoom);
            int pbottom = (int)(ClientRect.Top / (float)Zoom + (ClientRect.Bottom - ClientRect.Top) / (float)Zoom);

            Rectangle UnzoomedRect = new Rectangle(pleft, ptop, pright - pleft, pbottom - ptop);

            return UnzoomedRect;
        }

        private Rectangle PointsToRectangle(Point beginPoint, Point endPoint)
        {
            int top = beginPoint.Y < endPoint.Y ? beginPoint.Y : endPoint.Y;
            int bottom = beginPoint.Y > endPoint.Y ? beginPoint.Y : endPoint.Y;
            int left = beginPoint.X < endPoint.X ? beginPoint.X : endPoint.X;
            int right = beginPoint.X > endPoint.X ? beginPoint.X : endPoint.X;

            Rectangle rect = new Rectangle(left, top, (right - left), (bottom - top));
            return rect;
        }
    }
}
