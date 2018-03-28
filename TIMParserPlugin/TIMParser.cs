using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using System.IO;
using TileShop.Core;
using TileShop.Plugins;

namespace TIMParserPlugin
{
    /// <summary>
    /// Class to create arrangers and palettes for the PSX TIM format
    /// </summary>
    [Export(typeof(IFileParserContract))]
    [ExportMetadata("Name", "TIM Parser Plugin")]
    [ExportMetadata("Author", "Klarth")]
    [ExportMetadata("Version", "0.1")]
    [ExportMetadata("Description", "Parses file(s) for TIMs and automatically creates the appropriate arrangers and palettes")]
    public class TIMParser : IFileParserContract
    {
        List<IProjectResource> resources = new List<IProjectResource>();

        const uint TimTag = 0x00000010;

        /// <summary>
        /// Implements DisplayPluginInterface by using an OpenFileDialog with multiselect
        /// </summary>
        /// <returns></returns>
        public bool DisplayPluginInterface()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                Title = "Select one or more files to parse for embedded TIM files"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
                return false;

            // Clear data for multiple runs
            resources.Clear();

            foreach (string fname in ofd.FileNames)
            {
                int count = SearchTIMFile(fname);
                if (count > 0) // Found at least one TIM in the file
                {
                    DataFile df = new DataFile(Path.GetFileName(fname));
                    df.Open(fname); // TODO: Refactor not opening these files plugin-side; do it TileShop-side instead
                    resources.Add(df);
                }
            }

            return true;
        }

        private int SearchTIMFile(string FileName)
        {
            if (!File.Exists(FileName))
                throw new FileNotFoundException();

            int TimCount = 0;

            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 65536, FileOptions.SequentialScan))
                {
                    BinaryReader br = new BinaryReader(fs);

                    for (int i = 0; i < fs.Length / 4; i++)
                    {
                        if (fs.Position + 8 > fs.Length) // Ensure enough bytes remaining for header tags
                            break;

                        if (br.ReadUInt32() != TimTag)
                            continue;

                        uint ImageTag = br.ReadUInt32();

                        if (ImageTag > 15) // Value too large to be a valid image tag
                            continue;

                        TimData td = new TimData();

                        // Detect color depth
                        if ((ImageTag & 0b111) == 0b000)
                            td.ColorDepth = 4;
                        else if ((ImageTag & 0b111) == 0b001)
                            td.ColorDepth = 8;
                        else if ((ImageTag & 0b111) == 0b010)
                            td.ColorDepth = 16;
                        else if ((ImageTag & 0b111) == 0b011)
                            td.ColorDepth = 24;
                        else
                            continue; // Invalid color depth

                        // Read CLUT header if present
                        if ((ImageTag & 0b1000) == 0b1000) // Clut flag enabled
                        {
                            if (fs.Position + 12 > fs.Length) // Ensure enough bytes remaining for CLUT header
                                break;

                            td.ClutBlockSize = br.ReadInt32() - 12;
                            td.PaletteOrgX = br.ReadUInt16();
                            td.PaletteOrgY = br.ReadUInt16();
                            td.ClutColors = br.ReadUInt16();
                            td.ClutCount = br.ReadUInt16();

                            int ClutSize = td.ClutColors * 2;

                            if (td.ClutColors * td.ClutCount * 2 != td.ClutBlockSize) // CLUT size verification
                                continue;

                            for (int j = 0; j < td.ClutCount; j++)
                                td.ClutOffsets.Add(br.BaseStream.Position + ClutSize * j);

                            if (fs.Position + td.ClutBlockSize > fs.Length) // Ensure enough bytes remaining for CLUT
                                break;

                            fs.Seek(td.ClutBlockSize, SeekOrigin.Current); // Skip reading the CLUT
                        }

                        if (fs.Position + 12 > fs.Length) // Ensure enough bytes remaining for image header
                            break;

                        td.ImageDataSize = br.ReadInt32() - 12;
                        td.ImageOrgX = br.ReadUInt16();
                        td.ImageOrgY = br.ReadUInt16();
                        td.ImageWidth = 16 * br.ReadUInt16() / td.ColorDepth;
                        td.ImageHeight = br.ReadUInt16();

                        if (td.ImageDataSize != td.ImageWidth * td.ImageHeight * td.ColorDepth / 8)
                            continue;

                        if (fs.Position + td.ImageDataSize > fs.Length) // Ensure enough bytes remaining for image data
                            break;

                        td.ImageDataOffset = fs.Position;
                        fs.Seek(td.ImageDataSize, SeekOrigin.Current);

                        Arrange(td, FileName, String.Format("{0}.{1}", FileName, TimCount));
                        TimCount++;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return TimCount;
        }

        public List<IProjectResource> RetrieveResources()
        {
            return resources;
        }

        void Arrange(TimData td, string TimFileName, string BaseName)
        {
            if (td == null)
                throw new ArgumentNullException();

            string InitialPaletteName = String.Format("{0}.CLUT.{1}", BaseName, 0);

            for (int i = 0; i < td.ClutCount; i++)
            {
                string palName = String.Format("{0}.CLUT.{1}", BaseName, i);
                Palette pal = new Palette(palName);
                pal.LoadPaletteFromFileName(TimFileName, new FileBitAddress(td.ClutOffsets[i], 0), PaletteColorFormat.BGR15, false, td.ClutColors);
                resources.Add(pal);

                Arranger arr = Arranger.NewScatteredArranger(ArrangerLayout.LinearArranger, 1, 1, td.ImageWidth, td.ImageHeight);
                arr.Rename(String.Format("{0}.{1}", BaseName, i));

                ArrangerElement el = arr.GetElement(0, 0);

                el.DataFileKey = TimFileName;
                el.PaletteKey = palName;
                el.FileAddress = new FileBitAddress(td.ImageDataOffset, 0);
                el.Height = td.ImageHeight;
                el.Width = td.ImageWidth;

                el.X1 = 0;
                el.X2 = td.ImageWidth - 1;
                el.Y1 = 0;
                el.Y2 = td.ImageHeight - 1;

                switch (td.ColorDepth)
                {
                    case 4:
                        el.FormatName = "PSX 4bpp";
                        break;
                    case 8:
                        el.FormatName = "PSX 8bpp";
                        break;
                    case 16:
                        el.FormatName = "PSX 16bpp";
                        break;
                    case 24:
                        el.FormatName = "PSX 24bpp";
                        break;
                    default:
                        throw new InvalidOperationException(String.Format("ColorDepth {0} is not supported", td.ColorDepth));
                }

                el.AllocateBuffers();
                arr.SetElement(el, 0, 0);
                resources.Add(arr);
            }
        }

        public class TimData
        {
            public int ClutBlockSize;
            public uint PaletteOrgX;
            public uint PaletteOrgY;
            public int ColorDepth;
            public int ClutColors;
            public int ClutCount;
            public int ImageDataSize;
            public uint ImageOrgX;
            public uint ImageOrgY;
            public int ImageWidth;
            public int ImageHeight;

            public List<long> ClutOffsets = new List<long>();
            public long ImageDataOffset;
        }
    }

}
