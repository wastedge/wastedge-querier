using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Form = System.Windows.Forms.Form;

namespace WastedgeQuerier.JavaScript
{
    internal class JavaScriptUi
    {
        public Engine Engine { get; private set; }
        public Form Owner { get; private set; }

        public JavaScriptUi(Engine engine, Form owner)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            Engine = engine;
            Owner = owner;
        }

        public void Alert(string message, string icon)
        {
            if (Owner.InvokeRequired)
            {
                Owner.Invoke(new Action<string, string>(Alert), message, icon);
                return;
            }

            var taskDialog = new TaskDialog
            {
                AllowDialogCancellation = true,
                CommonButtons = TaskDialogCommonButtons.OK,
                MainIcon = GetIcon(icon, TaskDialogIcon.Warning),
                MainInstruction = message,
                PositionRelativeToWindow = true,
                WindowTitle = Owner.Text
            };

            taskDialog.Show(Owner);
        }

        public bool Confirm(string message, string icon)
        {
            if (Owner.InvokeRequired)
                return (bool)Owner.Invoke(new Func<string, string, bool>(Confirm), message, icon);

            var taskDialog = new TaskDialog
            {
                AllowDialogCancellation = true,
                CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No,
                MainIcon = GetIcon(icon, TaskDialogIcon.Warning),
                MainInstruction = message,
                PositionRelativeToWindow = true,
                WindowTitle = Owner.Text
            };

            return (DialogResult)taskDialog.Show(Owner) == DialogResult.Yes;
        }

        private static TaskDialogIcon GetIcon(string icon, TaskDialogIcon defaultIcon)
        {
            if (icon != null)
            {
                switch (icon)
                {
                    case "info":
                    case "information":
                        return TaskDialogIcon.Information;

                    case "warn":
                    case "warning":
                        return TaskDialogIcon.Warning;

                    case "error":
                        return TaskDialogIcon.Error;

                    case "none":
                        return TaskDialogIcon.None;
                }
            }

            return defaultIcon;
        }

        public JsValue ShowForm(string title, JsValue fields, JsValue validate)
        {
            if (Owner.InvokeRequired)
                return (JsValue)Owner.Invoke(new Func<string, JsValue, JsValue, JsValue>(ShowForm), title, fields, validate);

            return UiForm.Show(this, title, fields, validate);
        }

        public IDisposable PushOwner(UiForm owner)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            var previous = Owner;
            Owner = owner;
            return new Disposer(() => Owner = previous);
        }

        public string LoadFile(JsValue options)
        {
            if (Owner.InvokeRequired)
                return (string)Owner.Invoke(new Func<JsValue, string>(LoadFile), options);

            using (var form = new OpenFileDialog())
            {
                if (options.IsObject())
                    InitFileDialog(form, options.AsObject());

                if (form.ShowDialog(Owner) == DialogResult.OK)
                {
                    using (var stream = form.OpenFile())
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            return null;
        }

        public string SaveFile(string content, JsValue options)
        {
            if (Owner.InvokeRequired)
                return (string)Owner.Invoke(new Func<string, JsValue, string>(SaveFile), content, options);

            using (var form = new SaveFileDialog())
            {
                if (options.IsObject())
                    InitFileDialog(form, options.AsObject());

                form.OverwritePrompt = true;

                if (form.ShowDialog(Owner) == DialogResult.OK)
                {
                    if (content != null)
                    {
                        using (var stream = form.OpenFile())
                        using (var writer = new StreamWriter(stream))
                        {
                            writer.Write(content);
                        }
                    }

                    return form.FileName;
                }
            }

            return null;
        }

        private void InitFileDialog(FileDialog form, ObjectInstance obj)
        {
            if (obj.HasOwnProperty("title"))
                form.Title = obj.Get("title").ConvertToString();
            if (obj.HasOwnProperty("filter"))
                form.Filter = obj.Get("filter").ConvertToString();
            if (obj.HasOwnProperty("filename"))
                form.FileName = obj.Get("filename").ConvertToString();
        }

        public void Open(string fileName)
        {
            if (fileName == null)
                return;

            try
            {
                Process.Start(fileName);
            }
            catch (Exception)
            {
                // Ignore exceptions.
            }
        }
    }
}
