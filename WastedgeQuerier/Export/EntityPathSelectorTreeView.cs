using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WastedgeApi;
using WastedgeQuerier.Util;

namespace WastedgeQuerier.Export
{
    internal class EntityPathSelectorTreeView : TreeView
    {
        private readonly Api _api;

        public EntityPathSelectorTreeView(Api api, EntitySchema entity)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _api = api;

            var node = new TreeNode(HumanText.GetEntityName(entity));
            node.Tag = entity;
            node.Nodes.Add(new TreeNode());
            Nodes.Add(node);
            node.Expand();
        }

        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            base.OnBeforeExpand(e);

            if (e.Node.Nodes.Count == 0 || e.Node.Nodes[0].Tag != null)
                return;

            e.Node.Nodes.Clear();

            var entity = e.Node.Tag as EntitySchema;
            if (entity == null)
                entity = _api.GetEntitySchema(((EntityForeign)e.Node.Tag).LinkTable);

            foreach (var foreign in entity.Members.OfType<EntityForeign>().OrderBy(HumanText.GetMemberName))
            {
                var node = new TreeNode(HumanText.GetMemberName(foreign));
                var linkTable = _api.GetEntitySchema(foreign.LinkTable);
                node.Tag = foreign;
                if (linkTable.Members.Any(p => p is EntityForeign))
                    node.Nodes.Add(new TreeNode());
                e.Node.Nodes.Add(node);
            }
        }

        public EntityMemberPath GetSelectedNodePath()
        {
            var result = new List<EntityForeign>();
            BuildPath(result, SelectedNode);
            return new EntityMemberPath(result);
        }

        private void BuildPath(List<EntityForeign> result, TreeNode node)
        {
            var foreign = node.Tag as EntityForeign;
            if (foreign != null)
            {
                BuildPath(result, node.Parent);
                result.Add(foreign);
            }
        }
    }
}
