using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using WastedgeApi;
using WastedgeQuerier.JavaScript;
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    public partial class EntityFiltersForm : SystemEx.Windows.Forms.Form
    {
        private readonly EntitySchema _entity;

        public EntityFiltersForm(EntitySchema entity, IEnumerable<Filter> filters)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            _entity = entity;

            InitializeComponent();

            _filter.GotFocus += (s, e) => AcceptButton = _add;
            _filter.LostFocus += (s, e) => AcceptButton = _acceptButton;

            _filter.BeginUpdate();

            _filter.Items.Clear();
            _filter.Items.AddRange(_entity.Members.OfType<EntityPhysicalField>().Select(p => new FilterField(p)).ToArray());

            _filter.EndUpdate();

            foreach (var filter in filters)
            {
                var control = AddFilter(filter.Field);
                control.FilterType = filter.Type;
                control.SetValue(filter.Value);
            }

            UpdateEnabled();
        }

        private void _filter_SizeChanged(object sender, EventArgs e)
        {
            _add.Height = _filter.Height;
        }

        private void _add_Click(object sender, EventArgs e)
        {
            var member = _filter.SelectedItem as FilterField;
            if (member == null)
            {
                TaskDialogEx.Show(this, "Unknown field", Text, TaskDialogCommonButtons.OK, TaskDialogIcon.Error);
                return;
            }

            AddFilter(member.Field);
            _filter.Text = null;
        }

        private FilterControl AddFilter(EntityPhysicalField member)
        {
            var filterControl = new FilterControl(member)
            {
                Dock = DockStyle.Top
            };

            _filterControls.Controls.Add(filterControl);

            filterControl.BringToFront();

            filterControl.SelectNextControl(filterControl, true, true, true, false);

            return filterControl;
        }

        private void _filter_TextChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _add.Enabled = _filter.Text.Length > 0;
        }

        public List<Filter> GetFilters()
        {
            return _filterControls.Controls.Cast<FilterControl>().Select(p => p.GetFilter()).ToList();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private class FilterField
        {
            public EntityPhysicalField Field { get; }

            public FilterField(EntityPhysicalField field)
            {
                Field = field;
            }

            public override string ToString()
            {
                return HumanText.GetMemberName(Field);
            }
        }
    }
}
