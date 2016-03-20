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

namespace WastedgeQuerier
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
            _modifiedCheckBox.Text = String.Format(_modifiedCheckBox.Text, _changes.Modified.Count);
            _deletedCheckBox.Text = String.Format(_deletedCheckBox.Text, _changes.Deleted.Count);

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
            Exception exception = null;

            using (var form = new LoadingForm())
            {
                form.LoadingText = "Uploading changes...";

                form.Shown += async (s, ea) =>
                {
                    try
                    {
                        await UploadChanges(form);
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                    }

                    form.Dispose();
                };

                form.ShowDialog(this);
            }

            if (exception != null)
            {
                MessageBox.Show(
                    this,
                    "One or more requests failed" + Environment.NewLine + Environment.NewLine + exception.Message,
                    Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private async Task UploadChanges(LoadingForm form)
        {
            if (_changes.New.Count > 0)
                await UploadNew(form, _changes.New);
            if (_changes.Modified.Count > 0)
                await UploadModified(form, _changes.Modified);
            if (_changes.Deleted.Count > 0)
                await UploadDeleted(form, _changes.Deleted);
        }

        private async Task UploadNew(LoadingForm form, RecordSet @new)
        {
            form.Text = $"Creating {@new.Count} new records...";

            await _api.CreateAsync(_entity, @new);
        }

        private async Task UploadModified(LoadingForm form, RecordSet modified)
        {
            form.Text = $"Updating {modified.Count} records...";

            await _api.UpdateAsync(_entity, modified);
        }

        private async Task UploadDeleted(LoadingForm form, IList<string> deleted)
        {
            form.Text = $"Deleting {deleted.Count} records...";

            await _api.DeleteAsync(_entity, deleted);
        }
    }
}
