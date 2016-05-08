using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemEx.Windows.Forms;
using WastedgeApi;
using WastedgeQuerier.JavaScript;
using Form = System.Windows.Forms.Form;
using IOPath = System.IO.Path;

namespace WastedgeQuerier.Plugins
{
    internal class Plugin : PluginNode
    {
        public static PluginNode Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var project = Project.Open(path);

            return new Plugin(project.Title ?? IOPath.GetFileNameWithoutExtension(path), path, project);
        }

        public Project Project { get; }

        private Plugin(string name, string path, Project project)
            : base(name, path)
        {
            Project = project;
        }

        public void Run(ApiCredentials credentials, Form owner)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            var taskDialog = new TaskDialog
            {
                AllowDialogCancellation = true,
                MainInstruction = "Run " + Name,
                CustomMainIcon = NeutralResources.mainicon,
                CommonButtons = TaskDialogCommonButtons.Cancel,
                Content = "Do you want to run the " + Name + " plugin?",
                WindowTitle = owner.Text,
                PositionRelativeToWindow = true,
                Buttons = new[]
                {
                    new TaskDialogButton
                    {
                        ButtonText = "&Run",
                        ButtonId = (int)DialogResult.OK
                    }
                }
            };

            var result = (DialogResult)taskDialog.Show(owner);
            if (result == DialogResult.OK)
                new PluginRunner(Path).Run(credentials, owner);
        }
    }
}
