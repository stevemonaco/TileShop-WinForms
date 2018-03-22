using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TileShop.Core
{
    /// <summary>
    /// DataFile manages access to user-modifiable files
    /// </summary>
    public class DataFile : IResource
    {
        public string Name { get; private set; }
        public string Location { get; private set; }
        public FileStream Stream { get; private set; }

        public DataFile(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Renames a DataFile to a new name
        /// </summary>
        /// <param name="name"></param>
        public void Rename(string name)
        {
            Name = name;
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
    }
}
