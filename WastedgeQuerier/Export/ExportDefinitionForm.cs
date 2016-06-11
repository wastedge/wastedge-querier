using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using Newtonsoft.Json.Linq;
using WastedgeApi;
using WastedgeQuerier.Formats;
using WastedgeQuerier.JavaScript;
using WastedgeQuerier.Report;
using WastedgeQuerier.Support;
using WastedgeQuerier.Util;
using ListView = System.Windows.Forms.ListView;

namespace WastedgeQuerier.Export
{
    internal partial class ExportDefinitionForm : SystemEx.Windows.Forms.Form
    {
        private readonly Api _api;
        private readonly string _directory;
        private string _fileName;
        private readonly EntitySchema _entity;
        private EntitySchema _selectedEntity;
        private AutoCompleteForm _autoCompleteForm;
        private readonly ListViewGroup _entityGroup;
        private List<Filter> _filters;

        public ExportDefinitionForm(Api api, string directory, string fileName, ExportDefinition export)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));
            if (directory == null)
                throw new ArgumentNullException(nameof(directory));
            if (export == null)
                throw new ArgumentNullException(nameof(export));

            _api = api;
            _directory = directory;
            _fileName = fileName;
            _entity = export.Entity;
            _filters = export.Filters;

            InitializeComponent();

            if (fileName != null)
                Text += " - " + fileName;

            VisualStyleUtil.StyleListView(_availableFields);
            VisualStyleUtil.StyleListView(_reportFields);

            _entityGroup = new ListViewGroup(HumanText.GetEntityName(_entity));
            _reportFields.Groups.Add(_entityGroup);

            var selectPath = new Button
            {
                Image = NeutralResources.navigate_close
            };

            _path.RightButtons.Add(selectPath);

            selectPath.Click += selectPath_Click;

            foreach (var field in export.Fields)
            {
                AddReportField(field);
            }

            SetSelectedEntity(EntityMemberPath.Empty);

            UpdateEnabled();
        }

        private void AddReportField(EntityMemberPath path)
        {
            var item = new ListViewItem(HumanText.GetMemberName(path.Tail))
            {
                Tag = path
            };

            InsertSorted(_reportFields, item, true);
        }

        private void selectPath_Click(object sender, EventArgs e)
        {
            _path.Focus();

            if (_autoCompleteForm == null)
            {
                var treeView = BuildPathSelectorTreeView();

                _autoCompleteForm = new AutoCompleteForm
                {
                    Owner = _path,
                    Font = SystemFonts.MessageBoxFont,
                    Controls =
                    {
                        treeView
                    }
                };

                _autoCompleteForm.CmdKey += _autoCompleteForm_CmdKey;
                _autoCompleteForm.Shown += (s, ea) => treeView.Focus();
            }

            _autoCompleteForm.Show();
        }

        private void _autoCompleteForm_CmdKey(object sender, CmdKeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    var treeView = (EntityPathSelectorTreeView)((System.Windows.Forms.Form)sender).Controls[0];
                    SetSelectedEntity(treeView.GetSelectedNodePath());
                    e.Handled = true;
                    break;

                case Keys.Escape:
                    _autoCompleteForm.Dispose();
                    _autoCompleteForm = null;
                    e.Handled = true;
                    break;
            }
        }

        private TreeView BuildPathSelectorTreeView()
        {
            var treeView = new EntityPathSelectorTreeView(_api, _entity)
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Font = Font
            };

            VisualStyleUtil.StyleTreeView(treeView);

            treeView.LostFocus += treeView_LostFocus;
            treeView.NodeMouseDoubleClick += treeView_NodeMouseDoubleClick;

            return treeView;
        }

        private void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SetSelectedEntity(((EntityPathSelectorTreeView)sender).GetSelectedNodePath());
        }

        private void SetSelectedEntity(EntityMemberPath path)
        {
            if (path.Count == 0)
            {
                _selectedEntity = _entity;
                _path.Text = HumanText.GetEntityName(_selectedEntity);
            }
            else
            {
                _selectedEntity = _api.GetEntitySchema(((EntityForeign)path.Tail).LinkTable);
                _path.Text = HumanText.GetEntityMemberPath(path);
            }

            _availableFields.BeginUpdate();
            _availableFields.Items.Clear();

            foreach (var member in _selectedEntity.Members)
            {
                var memberPath = new EntityMemberPath(path, member);

                if (
                    (member is EntityField || member is EntityCalculatedField) &&
                    _reportFields.Items.Cast<ListViewItem>().All(p => !p.Tag.Equals(memberPath))
                )
                {
                    _availableFields.Items.Add(new ListViewItem(HumanText.GetMemberName(member))
                    {
                        Tag = memberPath
                    });
                }
            }

            _availableFields.EndUpdate();

            UpdateEnabled();

            if (_autoCompleteForm != null)
            {
                _autoCompleteForm.Dispose();
                _autoCompleteForm = null;
            }
        }

        private void UpdateEnabled()
        {
            _moveRight.Enabled = _availableFields.SelectedItems.Count > 0;
            _moveAllRight.Enabled = _availableFields.Items.Count > 0;
            _moveLeft.Enabled = _reportFields.SelectedItems.Count > 0;
            _moveAllLeft.Enabled = _reportFields.Items.Count > 0;
        }

        private void treeView_LostFocus(object sender, EventArgs e)
        {
            if (_autoCompleteForm != null)
            {
                _autoCompleteForm.Dispose();
                _autoCompleteForm = null;
            }
        }

        private void _availableFields_SizeChanged(object sender, EventArgs e)
        {
            SizeFirstListViewColumn(_availableFields);
        }

        private void _reportFields_SizeChanged(object sender, EventArgs e)
        {
            SizeFirstListViewColumn(_reportFields);
        }

        private static void SizeFirstListViewColumn(ListView listView)
        {
            listView.Columns[0].Width = listView.ClientSize.Width - 8;
        }

        private void _availableFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _reportFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _moveRight_Click(object sender, EventArgs e)
        {
            MoveFields(_availableFields, _reportFields, false);
        }

        private void _moveLeft_Click(object sender, EventArgs e)
        {
            MoveFields(_reportFields, _availableFields, false);
        }

        private void _moveAllRight_Click(object sender, EventArgs e)
        {
            MoveFields(_availableFields, _reportFields, true);
        }

        private void _moveAllLeft_Click(object sender, EventArgs e)
        {
            MoveFields(_reportFields, _availableFields, true);
        }

        private void MoveFields(ListView from, ListView to, bool all)
        {
            from.BeginUpdate();
            to.BeginUpdate();

            to.SelectedItems.Clear();

            List<ListViewItem> items;

            if (all)
                items = new List<ListViewItem>(from.Items.Cast<ListViewItem>());
            else
                items = new List<ListViewItem>(from.SelectedItems.Cast<ListViewItem>());

            foreach (var item in items)
            {
                from.Items.Remove(item);

                item.Group = null;
                item.Selected = true;

                if (to == _availableFields)
                {
                    if (_selectedEntity.Members.Any(p => p == ((EntityMemberPath)item.Tag).Tail))
                        InsertSorted(to, item, false);
                }
                else
                {
                    InsertSorted(to, item, true);
                }
            }

            from.EndUpdate();
            to.EndUpdate();

            UpdateEnabled();
        }

        private void InsertSorted(ListView listView, ListViewItem item, bool grouped)
        {
            int index = listView.Items.Count;

            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (String.Compare(listView.Items[i].Text, item.Text, StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    index = i;
                    break;
                }
            }

            listView.Items.Insert(index, item);

            item.Group = grouped ? GetGroup(listView, item) : null;
        }

        private ListViewGroup GetGroup(ListView listView, ListViewItem item)
        {
            var path = (EntityMemberPath)item.Tag;
            if (path.Count == 1)
                return _entityGroup;

            string groupName = HumanText.GetEntityMemberPath(new EntityMemberPath(path.Take(path.Count - 1)));

            var group = listView.Groups.Cast<ListViewGroup>().FirstOrDefault(p => p.Header == groupName);
            if (group != null)
                return group;

            group = new ListViewGroup(groupName);
            int index = _reportFields.Groups.Count;

            for (int i = 1; i < _reportFields.Groups.Count; i++)
            {
                if (String.Compare(_reportFields.Groups[i].Header, group.Header, StringComparison.CurrentCultureIgnoreCase) > 0)
                {
                    index = i;
                    break;
                }
            }

            _reportFields.Groups.Insert(index, group);

            return group;
        }

        private void ExportDefinitionForm_Shown(object sender, EventArgs e)
        {
            SizeFirstListViewColumn(_availableFields);
            SizeFirstListViewColumn(_reportFields);
        }

        private void _editFilters_Click(object sender, EventArgs e)
        {
            using (var form = new EntityFiltersForm(_entity, _filters))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    _filters = form.GetFilters();
            }
        }

        private void _save_Click(object sender, EventArgs e)
        {
            if (_fileName == null)
            {
                using (var form = new SaveForm())
                {
                    form.Path = Path.Combine(_directory, "Export.wqexport");
                    if (form.ShowDialog(this) != DialogResult.OK)
                        return;

                    _fileName = Path.GetFileName(form.Path);

                    Text += " - " + _fileName;
                }
            }

            BuildExport().Save(Path.Combine(_directory, _fileName));
        }

        private ExportDefinition BuildExport()
        {
            var export = new ExportDefinition
            {
                Entity = _entity
            };

            export.Filters.AddRange(_filters);
            var fields = _reportFields.Items.Cast<ListViewItem>().Select(p => (EntityMemberPath)p.Tag).ToList();
            fields.Sort(EntityMemberPathComparer.Instance);
            export.Fields.AddRange(fields);
            return export;
        }

        private void _exportToExcel_Click(object sender, EventArgs e)
        {
            string fileName = "Wastedge Export.xlsx";
            if (_fileName != null)
                fileName = Path.GetFileNameWithoutExtension(_fileName) + ".xlsx";

            using (var form = new SaveFileDialog())
            {
                form.Filter = "Excel (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                form.RestoreDirectory = true;
                form.FileName = fileName;

                if (form.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = form.FileName;
            }

            try
            {
                var results = GetAllResults();

                using (var stream = File.Create(fileName))
                {
                    new ExportExcelExporter().Export(stream, results, BuildExport());
                }
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                    ex = ((TargetInvocationException)ex).InnerException;

                TaskDialogEx.Show(this, "An unexpected error occured" + Environment.NewLine + Environment.NewLine + ex.Message, Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
                return;
            }

            try
            {
                Process.Start(fileName);
            }
            catch
            {
                // Ignore exceptions.
            }
        }

        private List<JObject> GetAllResults()
        {
            var result = new List<JObject>();
            bool success = false;

            var sb = new StringBuilder();

            string expand = BuildExpand();
            if (expand != null)
                sb.Append("$expand=").Append(Uri.EscapeDataString(expand));

            if (_filters.Count > 0)
            {
                if (sb.Length > 0)
                    sb.Append('&');
                sb.Append(ApiUtils.BuildFilters(_filters));
            }

            string parameters = sb.ToString();

            using (var form = new LoadingForm())
            {
                form.LoadingText = "Loading results...";

                form.Shown += async (s, ea) =>
                {
                    int count = 0;
                    string nextResult = null;

                    while (true)
                    {
                        string thisParameters = parameters;
                        if (nextResult != null)
                        {
                            if (thisParameters.Length > 0)
                                thisParameters += "&";
                            thisParameters += "$start=" + Uri.EscapeDataString(nextResult);
                        }

                        var response = await _api.ExecuteRawAsync(_entity.Name, thisParameters, "GET", null);

                        if (form.IsDisposed)
                            return;

                        var resultSet = JObject.Parse(response);

                        var rows = (JArray)resultSet["result"];
                        nextResult = (string)resultSet["next_result"];

                        count += rows.Count;

                        form.LoadingText = $"Loading {count} results...";

                        result.AddRange(rows.Cast<JObject>());

                        if (nextResult == null)
                            break;
                    }

                    success = true;

                    form.Dispose();
                };

                form.ShowDialog(this);
            }

            if (!success)
                return null;

            return result;
        }

        private string BuildExpand()
        {
            var expands = new HashSet<string>();

            foreach (var field in BuildExport().Fields)
            {
                string path = new EntityMemberPath(field.Take(field.Count - 1)).ToString();
                if (path.Length > 0)
                    expands.Add(path);
            }

            if (expands.Count == 0)
                return null;

            return String.Join(",", expands);
        }
    }
}
