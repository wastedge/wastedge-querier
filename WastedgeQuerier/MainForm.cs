using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WastedgeApi;
using WastedgeQuerier.JavaScript;
using WastedgeQuerier.Plugins;
using WastedgeQuerier.Report;

namespace WastedgeQuerier
{
    public partial class MainForm : SystemEx.Windows.Forms.Form
    {
        private readonly string[] _args;
        private readonly Api _api;
        private EntitySchema _schema;
        private readonly Dictionary<string, EntitySchema> _schemas = new Dictionary<string, EntitySchema>();
        private JavaScriptForm _javaScriptForm;
        private readonly PluginManager _pluginManager;

        public MainForm(ApiCredentials credentials, string[] args)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));
            if (args == null)
                throw new ArgumentNullException(nameof(args));

            _args = args;

            InitializeComponent();

            _pluginManager = new PluginManager(Path.Combine(Program.DataPath, "Plugins"), _pluginsMenuItem, _pluginsPluginManagerSeparatorMenuItem);
            _pluginManager.Reload();
            _pluginManager.PluginOpened += _pluginManager_PluginOpened;

            _filters.Visible = false;

            _api = new Api(credentials);

            _filter.GotFocus += (s, e) => AcceptButton = _add;
            _filter.LostFocus += (s, e) => AcceptButton = null;

            UpdateEnabled();

            _container.Enabled = false;
        }

        private void _pluginManager_PluginOpened(object sender, PluginEventArgs e)
        {
            e.Plugin.Run(_api.Credentials, this);
        }

        private void _fileExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            await _api.CacheFullEntitySchemaAsync();

            _tables.BeginUpdate();
            _tables.Items.Clear();
            _tables.Items.Add("");
            _tables.Items.AddRange(_api.GetSchema().Entities.OrderBy(p => p).Cast<object>().ToArray());
            _tables.EndUpdate();

            _container.Enabled = true;

            ProcessArguments();

#if DEBUG
            _tables.SelectedItem = "customer/service";
#endif
        }

        private void ProcessArguments()
        {
            foreach (string arg in _args)
            {
                if (File.Exists(arg) && Path.HasExtension(arg))
                {
                    switch (Path.GetExtension(arg).ToLowerInvariant())
                    {
                        case ".js":
                            EnsureJavaScriptForm();
                            _javaScriptForm.OpenEditor(arg);
                            break;

                        case ".weproj":
                            EnsureJavaScriptForm();
                            _javaScriptForm.OpenProject(arg);
                            break;

                        case ".wqpkg":
                            using (var form = new PluginManagerForm(_api.Credentials))
                            {
                                form.LoadPackage(arg);
                                form.ShowDialog(this);
                                _pluginManager.Reload();
                            }
                            return;
                    }
                }
            }
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

#if DEBUG
            _report.PerformClick();
#endif
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
            EnsureJavaScriptForm();
        }

        private void EnsureJavaScriptForm()
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

        private void _pluginsPluginManagerMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new PluginManagerForm(_api.Credentials))
            {
                form.ShowDialog(this);

                _pluginManager.Reload();
            }
        }

        private void _helpOpenHelpMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/wastedge/wastedge-querier/wiki");
            }
            catch
            {
                // Ignore exceptions.
            }
        }

        private void _helpAboutMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/wastedge/wastedge-querier");
            }
            catch
            {
                // Ignore exceptions.
            }
        }

        private void _report_Click(object sender, EventArgs e)
        {
            var filters = _filterControls.Controls.OfType<FilterControl>().Select(p => p.GetFilter()).ToList();

            using (var form = new ReportForm(_api, _schema, filters))
            {
                form.ShowDialog(this);
            }
        }
    }
}
