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
    internal class Plugin
    {
        public static Plugin Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var project = Project.Open(path);

            return new Plugin(project.Title ?? IOPath.GetFileNameWithoutExtension(path), path, project);
        }

        public string Name { get; }
        public string Path { get; }
        public Project Project { get; }

        private Plugin(string name, string path, Project project)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            Name = name;
            Path = path;
            Project = project;
        }

        public void Run(ApiCredentials credentials, Form owner)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            var content = "Do you want to run the " + Name + " plugin?";

            if (!String.IsNullOrEmpty(Project.Description))
                content += Environment.NewLine + Environment.NewLine + Project.Description;

            var taskDialog = new TaskDialog
            {
                AllowDialogCancellation = true,
                MainInstruction = "Run " + Name,
                CustomMainIcon = NeutralResources.mainicon,
                CommonButtons = TaskDialogCommonButtons.Cancel,
                Content = content,
                WindowTitle = owner.Text,
                PositionRelativeToWindow = true,
                VerificationText = "Run with debugging enabled",
                Buttons = new[]
                {
                    new TaskDialogButton
                    {
                        ButtonText = "&Run",
                        ButtonId = (int)DialogResult.OK
                    }
                }
            };

            bool debugMode;
            var result = (DialogResult)taskDialog.Show(owner, out debugMode);
            if (result == DialogResult.OK)
                new PluginRunner(Path).Run(credentials, owner, debugMode);
        }
    }
}
