using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal static class TreeNodeExtensions
    {
        public static ProjectItem GetProjectItem(this TreeNode self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            return (ProjectItem)self.Tag;
        }
    }
}
