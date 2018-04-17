using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace TileShop.Core
{
    /// <summary>
    /// DataFile manages access to user-modifiable files
    /// </summary>
    public class DataFile : ProjectResource
    {
        public string Location { get; private set; } = null;
        public FileStream Stream { get; private set; } = null;

        public DataFile(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Renames a DataFile to a new name
        /// </summary>
        /// <param name="name"></param>
        public override void Rename(string name)
        {
            Name = name;
        }

        public override ProjectResource Clone()
        {
            DataFile df = new DataFile(Name);
            df.Open(Location);
            return df;
        }

        /// <summary>
        /// Opens a file on disk and makes the underlying stream accessible
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool Open(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException();

            if (!File.Exists(filename))
                throw new FileNotFoundException("Could not find " + filename);

            Location = filename;
            Stream = File.Open(filename, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return true;
        }

        public void Close()
        {
            if (Stream != null)
                Stream.Close();
        }

        public override XElement Serialize()
        {
            throw new NotImplementedException();
        }

        public override bool Deserialize(XElement element)
        {
            throw new NotImplementedException();
        }
    }
}
