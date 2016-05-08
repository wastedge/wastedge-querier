using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal class ProjectFolder : ProjectItem
    {
        public override ProjectItemType Type => ProjectItemType.Folder;

        public ProjectFolder(TreeNode node, string path)
            : base(node, path)
        {
        }
    }
}
