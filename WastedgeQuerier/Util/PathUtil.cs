using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.Util
{
    internal class PathUtil
    {
        public static PathContains ContainsPath(string root, string path)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            root = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar);
            path = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);

            if (String.Equals(root, path, StringComparison.OrdinalIgnoreCase))
                return PathContains.Equals;
            if (path.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                return PathContains.Contains;
            return PathContains.Not;
        }

        public static string RemoveSubPath(string root, string path)
        {
            if (root == null)
                throw new ArgumentNullException(nameof(root));
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            root = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar);
            path = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);

            if (String.Equals(root, path, StringComparison.OrdinalIgnoreCase))
                return "";
            if (path.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                return path.Substring(root.Length + 1);

            throw new ArgumentException("Path does not start with root");
        }

        public static void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            if (sourceDirectory == null)
                throw new ArgumentNullException(nameof(sourceDirectory));
            if (targetDirectory == null)
                throw new ArgumentNullException(nameof(targetDirectory));

            CopyDirectory(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
        }

        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            Directory.CreateDirectory(target.FullName);

            foreach (var fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            foreach (var diSourceSubDir in source.GetDirectories())
            {
                CopyDirectory(diSourceSubDir, target.CreateSubdirectory(diSourceSubDir.Name));
            }
        }
    }
}
