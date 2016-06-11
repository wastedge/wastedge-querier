using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WastedgeQuerier.Support
{
    public class FilesDragEventArgs : EventArgs
    {
        public FilesDropData DropData { get; }
        public Point Location { get; }
        public int X => Location.X;
        public int Y => Location.Y;
        public DragDropEffects Effect { get; set; }
        public DragDropEffects AllowedEffect { get; }
        public int KeyState { get; }

        public FilesDragEventArgs(FilesDropData dropData, Point location, DragDropEffects allowedEffect, int keyState)
        {
            if (dropData == null)
                throw new ArgumentNullException(nameof(dropData));

            DropData = dropData;
            Location = location;
            AllowedEffect = allowedEffect;
            KeyState = keyState;
        }
    }

    public delegate void FilesDragEventHandler(object sender, FilesDragEventArgs e);
}
