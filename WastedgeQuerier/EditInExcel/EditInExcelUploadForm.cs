using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WastedgeApi;

namespace WastedgeQuerier.EditInExcel
{
    public partial class EditInExcelUploadForm : SystemEx.Windows.Forms.Form
    {
        private readonly Api _api;
        private readonly EntitySchema _entity;
        private readonly RecordSetChanges _changes;

        public EditInExcelUploadForm(Api api, EntitySchema entity, RecordSetChanges changes)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (changes == null)
                throw new ArgumentNullException(nameof(changes));

            _api = api;
            _entity = entity;
            _changes = changes;

            InitializeComponent();

            _newCheckBox.Text = String.Format(_newCheckBox.Text, _changes.New.Count);
            _newCheckBox.Enabled = entity.CanCreate;
            _modifiedCheckBox.Text = String.Format(_modifiedCheckBox.Text, _changes.Modified.Count);
            _modifiedCheckBox.Enabled = entity.CanUpdate;
            _deletedCheckBox.Text = String.Format(_deletedCheckBox.Text, _changes.Deleted.Count);
            _deletedCheckBox.Enabled = entity.CanDelete;

            UpdateEnabled();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            UploadChanges();

            DialogResult = DialogResult.OK;
        }

        private void _modifiedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _deletedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _acceptButton.Enabled = _newCheckBox.Checked || _modifiedCheckBox.Checked || _deletedCheckBox.Checked;
        }

        private void _newCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UploadChanges()
        {
            try
            {
                LoadingForm.Show(this, async p =>
                {
                    p.LoadingText = "Uploading changes...";

                    await UploadChanges(p);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "One or more requests failed" + Environment.NewLine + Environment.NewLine + ex.Message,
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task UploadChanges(LoadingForm form)
        {
            if (_changes.New.Count > 0 && _entity.CanCreate && _newCheckBox.Checked)
                await UploadNew(form, _changes.New);
            if (_changes.Modified.Count > 0 && _entity.CanUpdate && _modifiedCheckBox.Checked)
                await UploadModified(form, _changes.Modified);
            if (_changes.Deleted.Count > 0 && _entity.CanDelete && _deletedCheckBox.Checked)
                await UploadDeleted(form, _changes.Deleted);
        }

        private async Task UploadNew(LoadingForm form, RecordSet @new)
        {
            form.Text = $"Creating {@new.Count} new records...";

            await _api.CreateCreate(_entity, @new).ExecuteAsync();
        }

        private async Task UploadModified(LoadingForm form, RecordSet modified)
        {
            form.Text = $"Updating {modified.Count} records...";

            await _api.CreateUpdate(_entity, modified).ExecuteAsync();
        }

        private async Task UploadDeleted(LoadingForm form, IList<string> deleted)
        {
            form.Text = $"Deleting {deleted.Count} records...";

            await _api.CreateDelete(_entity, deleted).ExecuteAsync();
        }
    }
}
