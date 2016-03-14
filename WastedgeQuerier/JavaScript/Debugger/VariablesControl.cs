using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using Jint.Debugger;
using Jint.Native;
using WeifenLuo.WinFormsUI.Docking;

namespace WastedgeQuerier.JavaScript.Debugger
{
    internal partial class VariablesControl : DockContent
    {
        private DebugInformation _displayedLocalsSource;

        public VariablesControl()
        {
            Font = SystemFonts.MessageBoxFont;

            InitializeComponent();

            VisualStyleUtil.StyleListView(_listView);
        }

        public void ResetVariables()
        {
            _listView.Enabled = false;

            _displayedLocalsSource = null;
        }

        public void LoadVariables(DebugInformation debug, VariablesMode mode)
        {
            if (_displayedLocalsSource == debug)
                return;

            _displayedLocalsSource = debug;

            _listView.Enabled = true;

            _listView.BeginUpdate();

            _listView.Items.Clear();

            if (mode == VariablesMode.Locals && debug.Scopes.Count > 1)
            {
                var globals = new Dictionary<JsInstance, bool>();

                foreach (var item in debug.Scopes.Peek().Global)
                {
                    globals[item.Value] = true;
                }

                foreach (var variable in debug.Locals)
                {
                    if (globals.ContainsKey(variable.Value))
                        continue;

                    AddVariable(variable);
                }
            }
            else
            {
                foreach (var variable in debug.Scopes.Peek().Global)
                {
                    AddVariable(variable);
                }
            }

            _listView.EndUpdate();
        }

        private void AddVariable(KeyValuePair<string, JsInstance> variable)
        {
            _listView.Items.Add(new ListViewItem
            {
                Text = variable.Key,
                SubItems =
                {
                    variable.Value.ToSource(),
                    variable.Value.Type
                }
            });
        }

        private void _listView_SizeChanged(object sender, EventArgs e)
        {
            int width = _listView.ClientSize.Width - 6;

            foreach (ColumnHeader column in _listView.Columns)
            {
                if (column != _valueHeader)
                    width -= column.Width;
            }

            _valueHeader.Width = Math.Max(width, 100);
        }
    }
}
