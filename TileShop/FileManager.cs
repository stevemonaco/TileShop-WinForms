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
                return null;
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
                return null;
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
                return null;
        }

        public bool LoadFile(string Filename)
        {
            // TODO: Error handling
            FileStream fs = File.Open(Filename, FileMode.Open, FileAccess.ReadWrite);

            AddFileStream(Path.GetFileNameWithoutExtension(Filename), fs);
            return true;
        }

        public bool LoadFormat(string Filename)
        {
            GraphicsFormat fmt = new GraphicsFormat();
            if (!fmt.Load(Filename))
                return false;

            AddGraphicsFormat(fmt);
            return true;
        }

        public bool LoadPalette(string Filename)
        {
            Palette pal = new Palette();
            if (!pal.LoadPalette(Filename))
                return false;

            string palname = Path.GetFileNameWithoutExtension(Filename);

            AddPalette(palname, pal);
            return true;
        }

    }
}
