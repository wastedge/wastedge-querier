using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;

namespace WastedgeQuerier.JavaScript
{
    internal static class TaskDialogEx
    {
        public static DialogResult Show(IWin32Window owner, string message)
        {
            return Show(owner, message, null);
        }

        public static DialogResult Show(IWin32Window owner, string message, string title)
        {
            return Show(owner, message, title, TaskDialogCommonButtons.None);
        }

        public static DialogResult Show(IWin32Window owner, string message, string title, TaskDialogCommonButtons buttons)
        {
            return Show(owner, message, title, buttons, TaskDialogIcon.None);
        }

        public static DialogResult Show(IWin32Window owner, string message, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon)
        {
            return Show(owner, message, title, buttons, icon, 0);
        }            

        public static DialogResult Show(IWin32Window owner, string message, string title, TaskDialogCommonButtons buttons, TaskDialogIcon icon, int defaultButtonIndex)
        {
            var taskDialog = new TaskDialog
            {
                AllowDialogCancellation = true,
                CommonButtons = buttons,
                MainInstruction = message,
                MainIcon = icon,
                WindowTitle = title,
                PositionRelativeToWindow = true,
                DefaultButton = defaultButtonIndex,
            };

            return (DialogResult)taskDialog.Show(owner);
        }
    }
}
