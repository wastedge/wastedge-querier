using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Support
{
    public abstract class FileBrowserManager
    {
        public IList<string> Filters { get; }

        public IList<FileBrowserColumn> Columns { get; }

        public FileBrowserManager()
        {
            Filters = new List<string>();
            Columns = new List<FileBrowserColumn>();
        }

        public bool Matches(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            string fileExtension = Path.GetExtension(fileName);

            foreach (string extension in Filters)
            {
                if (String.Equals(extension, fileExtension, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public abstract string[] GetValues(string path);
    }
}
