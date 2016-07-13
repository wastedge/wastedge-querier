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
using Jint;
using Jint.Native;
using Jint.Native.Array;
using Jint.Native.Date;
using Jint.Runtime;
using DateTimePicker = SystemEx.Windows.Forms.DateTimePicker;
using Form = System.Windows.Forms.Form;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;

namespace WastedgeQuerier.JavaScript
{
    internal partial class UiForm : SystemEx.Windows.Forms.Form
    {
        private UiForm()
        {
            InitializeComponent();
        }

        public static JsValue Show(JavaScriptUi ui, string title, JsValue fields, JsValue validate)
        {
            if (ui == null)
                throw new ArgumentNullException(nameof(ui));

            using (var form = new UiForm())
            {
                if (String.IsNullOrEmpty(title))
                    form.Text = ui.Owner.Text;
                else
                    form.Text = title;

                form.Icon = ui.Owner.Icon;

                if (!fields.IsArray())
                    throw new JavaScriptException("fields must be an array");

                var controls = new List<Field>();

                fields.AsArray().ForEach((index, value) =>
                {
                    var container = form._container;

                    while (container.RowStyles.Count <= index)
                    {
                        container.RowCount++;
                        container.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    }

                    var field = value.AsObject();
                    string name = field.Get("name").ConvertToString();
                    string label = field.Get("label").ConvertToString();
                    var type = ParseType(field.Get("type").ConvertToString());
                    ArrayInstance list = null;
                    if (field.HasOwnProperty("list"))
                        list = field.Get("list").AsArray();

                    Field control;
                    switch (type)
                    {
                        case FieldType.Text:
                            control = new TextField(name, label);
                            break;
                        case FieldType.CheckBox:
                            control = new CheckBoxField(name, label);
                            break;
                        case FieldType.Numeric:
                            control = new NumericField(name, label);
                            break;
                        case FieldType.Date:
                            control = new DateField(name, label);
                            break;
                        case FieldType.DateTime:
                            control = new DateTimeField(name, label);
                            break;
                        case FieldType.ComboBox:
                            control = new ComboBoxField(name, label, list);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    controls.Add(control);

                    if (field.HasOwnProperty("value"))
                        control.SetValue(field.Get("value"));

                    if (control.ShowLabel)
                    {
                        var labelControl = new Label
                        {
                            AutoSize = true,
                            Text = label,
                            Dock = DockStyle.Fill,
                            TextAlign = ContentAlignment.MiddleLeft
                        };

                        container.SetRow(labelControl, index);
                        container.Controls.Add(labelControl);
                    }

                    control.Control.Dock = DockStyle.Fill;
                    control.Control.AutoSize = true;
                    container.SetRow(control.Control, index);
                    container.SetColumn(control.Control, 1);
                    container.Controls.Add(control.Control);
                });

                form._acceptButton.Click += (s, e) =>
                {
                    try
                    {
                        if (validate.IsObject())
                        {
                            var values = BuildValues(ui.Engine, controls);
                            var result = validate.Invoke(values);
                            if (!result.ConvertToBoolean().GetValueOrDefault())
                                return;
                        }
                        form.DialogResult = DialogResult.OK;
                    }
                    catch (JavaScriptException exception)
                    {
                        JintDebugger.ExceptionForm.Show(form, exception);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(
                            form,
                            new StringBuilder()
                                .AppendLine("An exception occurred while executing the script:")
                                .AppendLine()
                                .Append(exception.Message).Append(" (").Append(exception.GetType().FullName).AppendLine(")")
                                .AppendLine()
                                .AppendLine(exception.StackTrace)
                                .ToString(),
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                };

                var owner = ui.Owner;

                using (ui.PushOwner(form))
                {
                    if (form.ShowDialog(owner) == DialogResult.OK)
                        return BuildValues(ui.Engine, controls);
                }
            }

            return JsValue.Null;
        }

        private static JsValue BuildValues(Engine engine, List<Field> controls)
        {
            var results = engine.Object.Construct(Arguments.Empty);

            foreach (var control in controls)
            {
                results.Put(control.Name, control.GetValue(engine), true);
            }

            return results;
        }

        private static FieldType ParseType(string type)
        {
            switch (type)
            {
                case "text":
                    return FieldType.Text;
                case "check":
                case "checkbox":
                    return FieldType.CheckBox;
                case "number":
                case "numeric":
                    return FieldType.Numeric;
                case "date":
                    return FieldType.Date;
                case "datetime":
                case "date-time":
                    return FieldType.DateTime;
                case "list":
                case "combobox":
                    return FieldType.ComboBox;
                default:
                    throw new JavaScriptException("field type must be one of text, checkbox, numeric, date, datetime or combobox");
            }
        }

        private enum FieldType
        {
            Text,
            CheckBox,
            Numeric,
            Date,
            DateTime,
            ComboBox
        }

        private abstract class Field
        {
            public string Name { get; }
            public string Label { get; }
            public FieldType Type { get; }
            public Control Control { get; }

            public virtual bool ShowLabel => true;

            protected Field(string name, string label, FieldType type, Control control)
            {
                Name = name;
                Label = label;
                Type = type;
                Control = control;
            }

            public abstract JsValue GetValue(Engine engine);

            public abstract void SetValue(JsValue value);
        }

        private class TextField : Field
        {
            public TextField(string name, string label)
                : base(name, label, FieldType.Text, new TextBox())
            {
            }

            public override JsValue GetValue(Engine engine)
            {
                string text = Control.Text;
                return text.Length == 0 ? JsValue.Null : new JsValue(text);
            }

            public override void SetValue(JsValue value)
            {
                Control.Text = value.ConvertToString();
            }
        }

        private class NumericField : Field
        {
            public NumericField(string name, string label)
                : base(name, label, FieldType.Numeric, new SimpleNumericTextBox())
            {
            }

            public override JsValue GetValue(Engine engine)
            {
                var textBox = (SimpleNumericTextBox)Control;
                return textBox.Value.HasValue ? new JsValue((double)textBox.Value) : JsValue.Null;
            }

            public override void SetValue(JsValue value)
            {
                ((SimpleNumericTextBox)Control).Value = (decimal)value.ConvertToDouble();
            }
        }

        private class CheckBoxField : Field
        {
            public override bool ShowLabel => false;

            public CheckBoxField(string name, string label)
                : base(name, label, FieldType.CheckBox, new CheckBox())
            {
                ((CheckBox)Control).FlatStyle = FlatStyle.System;
                Control.Text = label;
            }

            public override JsValue GetValue(Engine engine)
            {
                return new JsValue(((CheckBox)Control).Checked);
            }

            public override void SetValue(JsValue value)
            {
                ((CheckBox)Control).Checked = value.ConvertToBoolean().GetValueOrDefault();
            }
        }

        private class DateField : Field
        {
            public DateField(string name, string label)
                : base(name, label, FieldType.DateTime, new DateTimePicker())
            {
                ((DateTimePicker)Control).Format = DateTimePickerFormat.Short;
            }

            public override JsValue GetValue(Engine engine)
            {
                var dateBox = (DateTimePicker)Control;
                return dateBox.Value.HasValue ? engine.Date.Construct(dateBox.Value.Value) : JsValue.Null;
            }

            public override void SetValue(JsValue value)
            {
                var dateBox = (DateTimePicker)Control;
                var obj = value.IsObject() ? value.AsObject() as DateInstance : null;
                if (obj != null)
                    dateBox.Value = obj.ToDateTime();
                else
                    dateBox.Value = null;
            }
        }

        private class DateTimeField : Field
        {
            public DateTimeField(string name, string label)
                : base(name, label, FieldType.DateTime, new DateTimePickerEx())
            {
                ((DateTimePickerEx)Control).DateFormat = DateTimePickerFormat.Short;
            }

            public override JsValue GetValue(Engine engine)
            {
                var dateTimeBox = (DateTimePickerEx)Control;
                return dateTimeBox.SelectedDateTime.HasValue ? engine.Date.Construct(dateTimeBox.SelectedDateTime.Value) : JsValue.Null;
            }

            public override void SetValue(JsValue value)
            {
                var dateBox = (DateTimePickerEx)Control;
                var obj = value.IsObject() ? value.AsObject() as DateInstance : null;
                if (obj != null)
                    dateBox.SelectedDateTime = obj.ToDateTime();
                else
                    dateBox.SelectedDateTime = null;
            }
        }

        private class ComboBoxField : Field
        {
            public ComboBoxField(string name, string label, ArrayInstance list)
                : base(name, label, FieldType.ComboBox, new ComboBox())
            {
                var comboBox = (ComboBox)Control;
                comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox.FlatStyle = FlatStyle.System;

                comboBox.Items.Add("");

                list.ForEach((index, value) => comboBox.Items.Add(value.ConvertToString()));
            }

            public override JsValue GetValue(Engine engine)
            {
                var comboBox = (ComboBox)Control;

                if (comboBox.SelectedIndex == 0)
                    return JsValue.Null;

                return new JsValue(comboBox.SelectedIndex - 1);
            }

            public override void SetValue(JsValue value)
            {
                ((ComboBox)Control).SelectedIndex = value.ConvertToInt32().Map(0, p => p + 1);
            }
        }
    }
}
