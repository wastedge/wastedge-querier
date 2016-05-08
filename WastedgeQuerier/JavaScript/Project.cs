using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Zip;
using IOPath = System.IO.Path;

namespace WastedgeQuerier.JavaScript
{
    internal class Project
    {
        private static readonly XNamespace Ns = "https://github.com/wastedge/wastedge-querier/2016/04/project";

        private Project(string fileName)
        {
            FileName = IOPath.GetFileName(fileName);
            Path = IOPath.GetDirectoryName(fileName);
        }

        public string Path { get; }
        public string FileName { get; }
        public string Title { get; set; }
        public string Description { get; set; }

        public static Project Create(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            var project = new Project(fileName);

            project.Save();

            using (var source = typeof(Project).Assembly.GetManifestResourceStream(typeof(Project).Namespace + ".main.js"))
            using (var target = File.Create(IOPath.Combine(project.Path, "main.js")))
            {
                source.CopyTo(target);
            }

            return project;
        }

        public static Project Open(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException(nameof(fileName));

            fileName = IOPath.GetFullPath(fileName);

            var project = new Project(fileName);

            XDocument doc = null;

            if (IOPath.GetExtension(fileName) == ".wqpkg")
            {
                using (var zipFile = new ZipFile(fileName))
                {
                    // Find the project item.

                    foreach (ZipEntry item in zipFile)
                    {
                        if (IOPath.GetExtension(item.Name) == ".weproj")
                        {
                            using (var stream = zipFile.GetInputStream(item))
                            {
                                doc = XDocument.Load(stream);
                            }

                            break;
                        }
                    }
                }

                if (doc == null)
                    throw new IOException("Invalid package file");
            }
            else
            {
                doc = XDocument.Load(fileName);
            }

            var title = doc.Root.Element(Ns + "Title");
            if (title != null)
                project.Title = title.Value;
            var description = doc.Root.Element(Ns + "Description");
            if (description != null)
                project.Description = description.Value;

            return project;
        }

        public void Save()
        {
            var project = new XElement(Ns + "Project");
            if (Title != null)
                project.Add(new XElement(Ns + "Title", Title));
            if (Description != null)
                project.Add(new XElement(Ns + "Description", Description));

            new XDocument(project).Save(IOPath.Combine(Path, FileName));
        }

        public string GetTitle()
        {
            if (String.IsNullOrEmpty(Title))
                return IOPath.GetFileNameWithoutExtension(FileName);
            return Title;
        }
    }
}
