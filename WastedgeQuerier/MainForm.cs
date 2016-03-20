using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WastedgeApi;
using WastedgeQuerier.JavaScript;

namespace WastedgeQuerier
{
    public partial class MainForm : SystemEx.Windows.Forms.Form
    {
        private readonly Api _api;
        private EntitySchema _schema;
        private readonly Dictionary<string, EntitySchema> _schemas = new Dictionary<string, EntitySchema>();
        private JavaScriptForm _javaScriptForm;

        public MainForm(ApiCredentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            InitializeComponent();

            _filters.Visible = false;

            _api = new Api(credentials);

            _filter.GotFocus += (s, e) => AcceptButton = _add;
            _filter.LostFocus += (s, e) => AcceptButton = null;

            UpdateEnabled();

            _container.Enabled = false;
        }

        private void _fileExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            var schema = await _api.GetSchemaAsync();

            _tables.BeginUpdate();
            _tables.Items.Clear();
            _tables.Items.Add("");
            _tables.Items.AddRange(schema.Entities.OrderBy(p => p).Cast<object>().ToArray());
            _tables.EndUpdate();

#if DEBUG
            _tables.SelectedIndex = _tables.Items.IndexOf("system/customer");
#endif

            _container.Enabled = true;

            if (Environment.GetCommandLineArgs().Skip(1).Any(p => p.EndsWith(".js")))
                _toolsJavaScriptConsoleMenuItem.PerformClick();
        }

        private async void _tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterControls.Controls.Clear();

            if (_tables.SelectedIndex == 0)
            {
                _footerPanel.Visible = false;
                _filters.Visible = false;
                return;
            }

            if (!_schemas.TryGetValue(_tables.Text, out _schema))
            {
                _container.Enabled = false;

                _schema = await _api.GetEntitySchemaAsync(_tables.Text);
                _schemas.Add(_tables.Text, _schema);

                _container.Enabled = true;
            }

            _filter.BeginUpdate();
            _filter.Items.Clear();
            _filter.Items.AddRange(_schema.Members.OfType<EntityPhysicalField>().Select(p => p.Name).Cast<object>().ToArray());
            _filter.EndUpdate();

            _footerPanel.Visible = true;
            _filters.Visible = true;
        }

        private void _filter_SizeChanged(object sender, EventArgs e)
        {
            _add.Height = _filter.Height;
        }

        private void _add_Click(object sender, EventArgs e)
        {
            if (!_schema.Members.Contains(_filter.Text))
            {
                MessageBox.Show(this, "Unknown field", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddFilter((EntityPhysicalField)_schema.Members[_filter.Text]);
            _filter.Text = null;
        }

        private void AddFilter(EntityPhysicalField member)
        {
            var filterControl = new FilterControl(member)
            {
                Dock = DockStyle.Top
            };

            _filterControls.Controls.Add(filterControl);

            filterControl.BringToFront();

            filterControl.SelectNextControl(filterControl, true, true, true, false);
        }

        private void _reset_Click(object sender, EventArgs e)
        {
            _filterControls.Controls.Clear();
        }

        private void _filter_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _add.Enabled = _filter.Text.Length > 0;
        }

        private void _export_Click(object sender, EventArgs e)
        {
            var filters = _filterControls.Controls.OfType<FilterControl>().Select(p => p.GetFilter()).ToList();

            using (var form = new ResultForm(_api, _schema, filters))
            {
                form.ShowDialog(this);
            }
        }

        private void _toolsJavaScriptConsoleMenuItem_Click(object sender, EventArgs e)
        {
            if (_javaScriptForm != null)
            {
                _javaScriptForm.BringToFront();
                return;
            }

            _javaScriptForm = new JavaScriptForm(_api);
            _javaScriptForm.Disposed += (s, ea) => _javaScriptForm = null;

            _javaScriptForm.Show();
        }
    }
}
