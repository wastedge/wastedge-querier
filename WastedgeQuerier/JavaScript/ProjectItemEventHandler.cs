using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WastedgeQuerier.JavaScript
{
    internal class ProjectItemEventArgs : EventArgs
    {
        public ProjectItem ProjectItem { get; }

        public ProjectItemEventArgs(ProjectItem projectItem)
        {
            if (projectItem == null)
                throw new ArgumentNullException(nameof(projectItem));

            ProjectItem = projectItem;
        }
    }

    internal delegate void ProjectItemEventHandler(object sender, ProjectItemEventArgs e);
}
