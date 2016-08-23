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

namespace WastedgeQuerier.EditInExcel
{
    public partial class ValidationErrorsForm : SystemEx.Windows.Forms.Form
    {
        public ValidationErrorsForm(ApiRowErrorsCollection errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            InitializeComponent();

            VisualStyleUtil.StyleListView(_listView);

            foreach (var rowErrors in errors)
            {
                foreach (var error in rowErrors.Errors)
                {
                    _listView.Items.Add(new ListViewItem
                    {
                        Text = (rowErrors.Row + 1).ToString(),
                        SubItems =
                        {
                            error.Field,
                            error.Error
                        }
                    });
                }
            }

            _listView.AutoResizeColumns(
                errors.Count > 0 ? ColumnHeaderAutoResizeStyle.ColumnContent : ColumnHeaderAutoResizeStyle.HeaderSize
            );
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
