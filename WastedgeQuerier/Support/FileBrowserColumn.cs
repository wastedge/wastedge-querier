using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.Support
{
    public class FileBrowserColumn
    {
        public string ColumnHeader { get; }
        public int ColumnWidth { get; }
        public HorizontalAlignment Alignment { get; }

        public FileBrowserColumn(string columnHeader, int columnWidth, HorizontalAlignment alignment)
        {
            ColumnHeader = columnHeader;
            ColumnWidth = columnWidth;
            Alignment = alignment;
        }
    }
}
