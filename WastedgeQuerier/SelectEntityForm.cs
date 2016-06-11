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
using WastedgeQuerier.Util;

namespace WastedgeQuerier
{
    public partial class SelectEntityForm : SystemEx.Windows.Forms.Form
    {
        private readonly Api _api;

        public EntitySchema SelectedEntity => (_entities.SelectedItem as EntityDrawer)?.Entity;

        public SelectEntityForm(Api api)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));

            _api = api;

            InitializeComponent();

            Reload();

            UpdateEnabled();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_filter.ContainsFocus)
            {
                switch (keyData)
                {
                    case Keys.Up:
                        SelectEntity(-1);
                        return true;
                    case Keys.Down:
                        SelectEntity(1);
                        return true;
                    case Keys.PageUp:
                        SelectEntity(-10);
                        return true;
                    case Keys.PageDown:
                        SelectEntity(10);
                        return true;
                    case Keys.Home:
                        SelectEntity(int.MinValue);
                        break;
                    case Keys.End:
                        SelectEntity(int.MaxValue);
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SelectEntity(int offset)
        {
            int index = _entities.SelectedIndex + offset;

            while (index >= 0 && index < _entities.Items.Count)
            {
                if (_entities.Items[index] is EntityDrawer)
                {
                    _entities.SelectedIndex = index;
                    return;
                }

                index += offset;
            }

            if (offset > 0)
            {
                _entities.SelectedIndex = _entities.Items.Count - 1;
            }
            else
            {
                index = 0;

                while (index < _entities.Items.Count)
                {
                    if (_entities.Items[index] is EntityDrawer)
                    {
                        _entities.SelectedIndex = index;
                        return;
                    }

                    index++;
                }
            }
        }

        private void _filter_TextChanged(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            var entities = new List<EntitySchema>();
            string filter = _filter.Text.ToLower();

            foreach (var item in _api.GetSchema().Entities)
            {
                var entity = _api.GetEntitySchema(item);
                bool include = true;

                if (filter.Length > 0)
                {
                    include =
                        entity.Name.ToLower().Contains(filter) ||
                        (entity.Comments != null && entity.Comments.ToLower().Contains(filter));
                }

                if (include)
                    entities.Add(entity);
            }

            entities.Sort(EntitySchemaComparer.Instance);

            _entities.BeginUpdate();

            _entities.Items.Clear();

            string header = null;
            bool hadSelected = false;

            foreach (var entity in entities)
            {
                var name = new EntityName(entity.Name);
                if (header != name.Header)
                {
                    _entities.Items.Add(new HeaderDrawer(HumanText.ToHuman(name.Header)));
                    header = name.Header;
                }

                _entities.Items.Add(new EntityDrawer(entity));
                if (!hadSelected)
                {
                    hadSelected = true;
                    _entities.SelectedIndex = _entities.Items.Count - 1;
                }
            }

            _entities.EndUpdate();
        }

        private void _entities_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _acceptButton.Enabled = _entities.SelectedItem is EntityDrawer;
        }

        private void _entities_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = ((IDrawer)_entities.Items[e.Index]).MeasureItem(_entities.ClientSize.Width);
        }

        private void _entities_DrawItem(object sender, DrawItemEventArgs e)
        {
            ((IDrawer)_entities.Items[e.Index]).DrawItem(e);
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void _entities_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _acceptButton.PerformClick();
        }

        private interface IDrawer
        {
            int MeasureItem(int width);
            void DrawItem(DrawItemEventArgs e);
        }

        private class EntityDrawer : IDrawer
        {
            private static readonly Padding Padding = new Padding(4);
            private static readonly Color CommentsColor = SystemColors.GrayText;
            private static readonly Font Font = SystemFonts.MessageBoxFont;
            private const TextFormatFlags Flags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
            private const TextFormatFlags CommentsFlags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak;
            private const int Separator = 3;

            public EntitySchema Entity { get; }

            private readonly string _name;

            public EntityDrawer(EntitySchema entity)
            {
                Entity = entity;
                _name = HumanText.GetEntityName(Entity);
            }

            public int MeasureItem(int width)
            {
                int height = TextRenderer.MeasureText("W", Font, new Size(int.MaxValue, int.MaxValue), Flags).Height + Padding.Vertical;

                if (!String.IsNullOrEmpty(Entity.Comments))
                {
                    height +=
                        Separator +
                        TextRenderer.MeasureText(Entity.Comments, Font, new Size(width, int.MaxValue), CommentsFlags).Height;
                }

                return height;
            }

            public void DrawItem(DrawItemEventArgs e)
            {
                e.DrawBackground();

                var textBounds = new Rectangle(
                    e.Bounds.Left + Padding.Left,
                    e.Bounds.Top + Padding.Top,
                    e.Bounds.Width - Padding.Horizontal,
                    e.Bounds.Height - Padding.Vertical
                );

                TextRenderer.DrawText(e.Graphics, _name, Font, textBounds, e.ForeColor, e.BackColor, Flags);

                if (!String.IsNullOrEmpty(Entity.Comments))
                {
                    int height = TextRenderer.MeasureText("W", Font, new Size(int.MaxValue, int.MaxValue), Flags).Height;

                    textBounds = new Rectangle(
                        textBounds.Left,
                        textBounds.Top + height + Separator,
                        textBounds.Width,
                        textBounds.Height - (height + Separator)
                    );

                    TextRenderer.DrawText(e.Graphics, Entity.Comments, Font, textBounds, CommentsColor, e.BackColor, CommentsFlags);
                }

                e.DrawFocusRectangle();
            }
        }

        private class HeaderDrawer : IDrawer
        {
            private static readonly Color BackColor = SystemColors.ButtonFace;
            private static readonly Brush BackBrush = SystemBrushes.ButtonFace;
            private static readonly Color ForeColor = SystemColors.ControlText;
            private static readonly Font Font = new Font(SystemFonts.MessageBoxFont.FontFamily, SystemFonts.MessageBoxFont.SizeInPoints * 1.2f, FontStyle.Bold);
            private static readonly Padding Padding = new Padding(4);
            private const TextFormatFlags Flags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;

            private readonly string _header;

            public HeaderDrawer(string header)
            {
                _header = header;
            }

            public int MeasureItem(int width)
            {
                return TextRenderer.MeasureText("W", Font, new Size(int.MaxValue, int.MaxValue), Flags).Height + Padding.Vertical;
            }

            public void DrawItem(DrawItemEventArgs e)
            {
                e.Graphics.FillRectangle(BackBrush, e.Bounds);

                var textBounds = new Rectangle(
                    e.Bounds.Left + Padding.Left,
                    e.Bounds.Top + Padding.Top,
                    e.Bounds.Width - Padding.Horizontal,
                    e.Bounds.Height - Padding.Vertical
                );

                TextRenderer.DrawText(e.Graphics, _header, Font, textBounds, ForeColor, BackColor, Flags);

                e.DrawFocusRectangle();
            }
        }
    }
}
