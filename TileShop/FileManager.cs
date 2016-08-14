using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileShop
{
    public class FileManager
    {
        public static readonly FileManager Instance = new FileManager();

        public Dictionary<string, FileStream> FileList = new Dictionary<string, FileStream>();
        public Dictionary<string, Arranger> ArrangerList = new Dictionary<string, Arranger>();
        public Dictionary<string, Palette> PaletteList = new Dictionary<string, Palette>();
        public Dictionary<string, GraphicsFormat> FormatList = new Dictionary<string, GraphicsFormat>();

        public void AddFileStream(string FileName, FileStream fs)
        {
            FileList.Add(FileName, fs);
        }

        public FileStream GetFileStream(string FileName)
        {
            if (FileList.ContainsKey(FileName))
                return FileList[FileName];
            else
                throw new KeyNotFoundException();
        }

        public void AddPalette(string PaletteName, Palette pal)
        {
            PaletteList.Add(PaletteName, pal);
        }

        public Palette GetPalette(string PaletteName)
        {
            if (PaletteList.ContainsKey(PaletteName))
                return PaletteList[PaletteName];
            else
                throw new KeyNotFoundException();
        }

        public void AddGraphicsFormat(GraphicsFormat format)
        {
            FormatList.Add(format.Name, format);
        }

        public GraphicsFormat GetFormat(string FormatName)
        {
            if (FormatList.ContainsKey(FormatName))
                return FormatList[FormatName];
            else
                throw new KeyNotFoundException();
        }

        public bool LoadFile(string Filename)
        {
            // TODO: Error handling
            try
            {
                FileStream fs = File.Open(Filename, FileMode.Open, FileAccess.ReadWrite);
                AddFileStream(Path.GetFileNameWithoutExtension(Filename), fs);
            }
            catch(FileNotFoundException ex)
            {
                return false;
            }

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

    }
}
