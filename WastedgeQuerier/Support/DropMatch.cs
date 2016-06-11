using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WastedgeQuerier.Util;
using IOPath = System.IO.Path;

namespace WastedgeQuerier.Support
{
    internal class DropMatch
    {
        public static List<DropMatch> FromDropData(string rootPath, string targetPath, FileBrowserManager fileBrowserManager, FilesDropData dropData)
        {
            if (rootPath == null)
                throw new ArgumentNullException(nameof(rootPath));
            if (targetPath == null)
                throw new ArgumentNullException(nameof(targetPath));
            if (dropData == null)
                throw new ArgumentNullException(nameof(dropData));

            var matches = new List<DropMatch>();

            for (int i = 0; i < dropData.FileNames.Length; i++)
            {
                string fileName = dropData.FileNames[i];

                if (!IOPath.IsPathRooted(fileName))
                {
                    // Normal drag/drop operations have the full path. Drag/drop operations
                    // without a full path generally are Ole objects that don't point to
                    // physical files, like drag/dropping from an Outlook email or a Windows
                    // compressed folder. To get the data, 
                    matches.Add(new DropMatch(fileName, i, DropMatchKind.FileVirtual));
                    continue;
                }

                var fileInfo = new FileInfo(fileName);

                if (fileInfo.Attributes.IsHidden())
                    continue;

                bool contained = PathUtil.ContainsPath(rootPath, fileInfo.FullName) != PathContains.Not;

                if (PathUtil.ContainsPath(targetPath, fileInfo.DirectoryName) == PathContains.Equals)
                    continue;

                if (fileInfo.Attributes.IsDirectory())
                {
                    if (Directory.Exists(IOPath.Combine(targetPath, fileInfo.Name)))
                        continue;
                    if (PathUtil.ContainsPath(fileInfo.FullName, targetPath) != PathContains.Not)
                        continue;

                    matches.Add(new DropMatch(fileInfo.FullName, i, contained ? DropMatchKind.DirectoryMove : DropMatchKind.DirectoryCopy));
                }
                else if (fileBrowserManager != null && fileBrowserManager.Matches(fileInfo.FullName))
                {
                    matches.Add(new DropMatch(fileInfo.FullName, i, contained ? DropMatchKind.FileMove : DropMatchKind.FileCopy));
                }
            }

            return matches;
        }

        public string Path { get; }
        public int Index { get; }
        public DropMatchKind Kind { get; }

        private DropMatch(string path, int index, DropMatchKind kind)
        {
            Path = path;
            Index = index;
            Kind = kind;
        }
    }

    internal enum DropMatchKind
    {
        DirectoryMove,
        DirectoryCopy,
        FileMove,
        FileCopy,
        FileVirtual
    }
}
