using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileShop
{
    public class FileManager
    {
        public static readonly FileManager Instance = new FileManager();

        private Dictionary<string, FileStream> FileList = new Dictionary<string, FileStream>();
        private Dictionary<string, Arranger> ArrangerList = new Dictionary<string, Arranger>();
        private Dictionary<string, Palette> PaletteList = new Dictionary<string, Palette>();
        private Dictionary<string, GraphicsFormat> FormatList = new Dictionary<string, GraphicsFormat>();
        private Dictionary<string, Cursor> CursorList = new Dictionary<string, Cursor>();
        private FileTypeLoader Loader = new FileTypeLoader();

        const int DefaultElementsX = 8;
        const int DefaultElementsY = 16;

        public void AddFileStream(string FileName, FileStream fs)
        {
            FileList.Add(FileName, fs);
        }

        public void AddGraphicsFormat(GraphicsFormat format)
        {
            FormatList.Add(format.Name, format);
        }

        public void AddArranger(Arranger arr)
        {
            if(!HasArranger(arr.Name))
                ArrangerList.Add(arr.Name, arr);
        }

        public void AddPalette(string PaletteName, Palette pal)
        {
            PaletteList.Add(PaletteName, pal);
        }

        public FileStream GetFileStream(string FileName)
        {
            if (FileList.ContainsKey(FileName))
                return FileList[FileName];
            else
                throw new KeyNotFoundException();
        }

        public Palette GetPalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName))
                return PaletteList[PaletteName];
            else
                throw new KeyNotFoundException();
        }
        
        public Arranger GetArranger(string ArrangerName)
        {
            if (HasArranger(ArrangerName))
                return ArrangerList[ArrangerName];
            else
                throw new KeyNotFoundException(String.Format("Arranger {0} was not found in the ArrangerList", ArrangerName));
        }

        public bool HasFile(string Filename)
        {
            return FileList.ContainsKey(Filename);
        }

        public bool HasArranger(string ArrangerName)
        {
            return ArrangerList.ContainsKey(ArrangerName);
        }

        public bool HasPalette(string PaletteName)
        {
            return PaletteList.ContainsKey(PaletteName);
        }

        public GraphicsFormat GetGraphicsFormat(string FormatName)
        {
            if (FormatList.ContainsKey(FormatName))
                return FormatList[FormatName];
            else
                throw new KeyNotFoundException();
        }

        public List<string> GetGraphicsFormatsNameList()
        {
            Dictionary<string, GraphicsFormat>.KeyCollection keys = FileManager.Instance.FormatList.Keys;
            List<string> keyList = keys.ToList<string>();
            keyList.Sort();

            return keyList;
        }

        public bool LoadFile(string Filename)
        {
            // TODO: Error handling
            try
            {
                if (!FileList.ContainsKey(Filename))
                {
                    FileStream fs = File.Open(Filename, FileMode.Open, FileAccess.ReadWrite);
                    AddFileStream(Filename, fs);
                }
            }
            catch(FileNotFoundException ex)
            {
                return false;
            }

            return true;
        }

        public bool LoadSequentialArrangerFromFilename(string Filename)
        {
            if (!LoadFile(Filename))
                return false;

            string formatname = Loader.GetDefaultFormatName(Filename);
            Arranger arranger = Arranger.NewSequentialArranger(DefaultElementsX, DefaultElementsY, Filename, GetGraphicsFormat(formatname));
            AddArranger(arranger);

            return true;
        }

        public bool LoadFormat(string Filename)
        {
            GraphicsFormat fmt = new GraphicsFormat();
            if (!fmt.LoadFromXml(Filename))
                return false;

            AddGraphicsFormat(fmt);
            return true;
        }

        public bool LoadPalette(string Filename, string PaletteName)
        {
            if (Filename == null || PaletteName == null)
                throw new ArgumentNullException();

            Palette pal = new Palette(PaletteName);
            if (!pal.LoadPalette(Filename))
                return false;

            string palname = Path.GetFileNameWithoutExtension(Filename);

            AddPalette(palname, pal);
            return true;
        }

        public bool AddCursor(string CursorName, Cursor cursor)
        {
            if (!CursorList.ContainsKey(CursorName))
            {
                CursorList.Add(CursorName, cursor);
                return true;
            }
            return false;
        }

        public Cursor GetCursor(string CursorName)
        {
            if (CursorList.ContainsKey(CursorName))
                return CursorList[CursorName];
            else
                throw new KeyNotFoundException();
        }

        public bool RemoveArranger(string ArrangerName)
        {
            if(ArrangerList.ContainsKey(ArrangerName))
            {
                ArrangerList.Remove(ArrangerName);
                return true;
            }

            return false;
        }

        public bool ClearAll()
        {
            FileList.Clear();
            PaletteList.Clear();
            ArrangerList.Clear();

            return true;
        }

    }
}
