using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal class ProjectFile : ProjectItem
    {
        public override ProjectItemType Type => ProjectItemType.File;

        public ProjectFile(TreeNode node, string path)
            : base(node, path)
        {
        }
    }
}
