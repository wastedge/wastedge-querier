using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal class ProjectRoot : ProjectItem
    {
        public override ProjectItemType Type => ProjectItemType.Root;

        public ProjectRoot(TreeNode node, string path)
            : base(node, path)
        {
        }
    }
}
