using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal abstract class ProjectItem
    {
        public abstract ProjectItemType Type { get; }

        public string Name => Node.Text;
        public string Path { get; }
        public TreeNode Node { get; }

        protected ProjectItem(TreeNode node, string path)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Node = node;
            Path = path;
        }
    }
}
