using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TileShop
{
    public enum ArrangerMoveType { ByteDown = 0, ByteUp, RowDown, RowUp, ColRight, ColLeft, PageDown, PageUp, Home, End };
    public class ArrangerList
    {
        public bool IsSequential;
        public int ListX, ListY;
        public long FileSize;
        public long ArrangerByteSize; // Number of bytes required to be read from file sequentially
        public ArrangerElement[,] ElementList;
        public Size ArrangerSize = new Size(0, 0);

        public ArrangerList(int ElementsX, int ElementsY, string Filename, GraphicsFormat format)
        {
            long offset = 0;
            IsSequential = true;
            FileSize = FileManager.Instance.GetFileStream(Filename).Length;
            NewBlankArranger(ElementsX, ElementsY, format);
            ArrangerByteSize = ElementsX * ElementsY * format.Size();
            int x = 0;
            int y = 0;

            for (int i = 0; i < ElementsY; i++)
            {
                x = 0;
                for (int j = 0; j < ElementsX; j++)
                {
                    ElementList[j, i].FileOffset = offset;
                    if (format.ImageType == "tiled")
                        offset += format.Size();
                    else // Linear
                        offset += (format.Width + format.Stride) * format.ColorDepth / 4;

                    ElementList[j, i].X1 = x;
                    ElementList[j, i].Y1 = y;
                    ElementList[j, i].X2 = x + format.Width - 1;
                    ElementList[j, i].Y2 = y + format.Height - 1;
                    ElementList[j, i].FileName = Filename;
                    ElementList[j, i].Format = format.Name;
                    ElementList[j, i].Palette = "Default";

                    x += format.Width;
                }
                y += format.Height;
            }

            ArrangerElement LastElem = ElementList[ElementsX - 1, ElementsY - 1];
            ArrangerSize = new Size(LastElem.X2, LastElem.Y2);
        }

        void NewBlankArranger(int ElementsX, int ElementsY, GraphicsFormat format)
        {
            ElementList = new ArrangerElement[ElementsX, ElementsY];
            ListX = ElementsX;
            ListY = ElementsY;

            int x = 0;
            int y = 0;

            for (int i = 0; i < ElementsY; i++)
            {
                x = 0;
                for (int j = 0; j < ElementsX; j++)
                {
                    ArrangerElement re = new ArrangerElement();
                    re.X1 = x;
                    re.Y1 = y;
                    re.X2 = x + format.Width - 1;
                    re.Y2 = y + format.Height - 1;
                    re.Format = "Transparent";
                    re.Palette = "Default";
                    ElementList[j, i] = re;

                    x += format.Width;
                }
                y += format.Height;
            }

            ArrangerElement LastElem = ElementList[ElementsX - 1, ElementsY - 1];
            ArrangerSize = new Size(LastElem.X2, LastElem.Y2);
        }

        void Resize(int x, int y, GraphicsCodec codec)
        {
            if (ElementList == null) // Create new list
            {
                ElementList = new ArrangerElement[x, y];
            }
            else // Create new list and merge previous offset list
            {
                ArrangerElement[,] newList = new ArrangerElement[x, y];
            }
        }

        public Rectangle GetSelectionRect(Rectangle r)
        {
            int x1 = r.Left;
            int x2 = r.Right;
            int y1 = r.Top;
            int y2 = r.Bottom;

            // Extend rectangle to include the entirety of partially selected tiles
            foreach(ArrangerElement el in ElementList)
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
            if (x2 >= ArrangerSize.Width)
                x2 = ArrangerSize.Width;
            if (y2 >= ArrangerSize.Height)
                y2 = ArrangerSize.Height;

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        // Returns new fileoffset
        public long Move(ArrangerMoveType MoveType)
        {
            long offset = ElementList[0, 0].FileOffset;
            long delta;

            switch (MoveType)
            {
                case ArrangerMoveType.ByteDown:
                    if (offset + 1 + ArrangerByteSize <= FileSize)
                        offset++;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.ByteUp:
                    if (offset >= 1)
                        offset--;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.RowDown:
                    delta = ListX * FileManager.Instance.GetFormat(ElementList[0, 0].Format).Size();
                    if (offset + delta + ArrangerByteSize <= FileSize)
                        offset += delta;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.RowUp:
                    delta = ListX * FileManager.Instance.GetFormat(ElementList[0, 0].Format).Size();
                    if (offset - delta >= 0)
                        offset -= delta;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.ColRight:
                    delta = FileManager.Instance.GetFormat(ElementList[0, 0].Format).Size();
                    if (offset + delta + ArrangerByteSize <= FileSize)
                        offset += delta;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.ColLeft:
                    delta = FileManager.Instance.GetFormat(ElementList[0, 0].Format).Size();
                    if (offset - delta >= 0)
                        offset -= delta;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.PageDown:
                    delta = ListX * FileManager.Instance.GetFormat(ElementList[0, 0].Format).Size() * ListY / 2;
                    if (offset + delta + ArrangerByteSize <= FileSize)
                        offset += delta;
                    else
                        offset = FileSize - ArrangerByteSize;
                    break;
                case ArrangerMoveType.PageUp:
                    delta = ListX * FileManager.Instance.GetFormat(ElementList[0, 0].Format).Size() * ListY / 2;
                    if (offset - delta >= 0)
                        offset -= delta;
                    else
                        offset = 0;
                    break;
                case ArrangerMoveType.Home:
                    offset = 0;
                    break;
                case ArrangerMoveType.End:
                    offset = FileSize - ArrangerByteSize;
                    break;
            }

            SetNewSequentialFileOffset(offset);

            return offset;
        }

        void SetNewSequentialFileOffset(long FileOffset)
        {
            for (int i = 0; i < ListY; i++)
            {
                for (int j = 0; j < ListX; j++)
                {
                    ElementList[j, i].FileOffset = FileOffset;
                    FileOffset += FileManager.Instance.GetFormat(ElementList[j, i].Format).Size();
                }
            }
        }

    }

    public class ArrangerElement
    {
        public string FileName = "";
        public long FileOffset = 0;
        public string Format = "";
        public string Palette = "";
        public int X1 = 0; // Locations are unzoomed
        public int Y1 = 0;
        public int X2 = 0;
        public int Y2 = 0;
    }
}
