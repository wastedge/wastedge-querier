using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WastedgeApi;

namespace WastedgeQuerier.Util
{
    internal static class HumanText
    {
        public static string ToHuman(string text)
        {
            if (String.IsNullOrEmpty(text))
                return text;

            text = text.Replace('_', ' ');

            return text.Substring(0, 1).ToUpper() + text.Substring(1);
        }

        public static string GetEntityName(EntitySchema entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return ToHuman(new EntityName(entity.Name).Name);
        }

        public static string GetMemberName(EntityMember field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return ToHuman(field.Name);
        }

        public static string GetEntityMemberPath(EntityMemberPath path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var sb = new StringBuilder();

            for (int i = 0; i < path.Count; i++)
            {
                if (i > 0)
                    sb.Append(" » ");
                sb.Append(GetMemberName(path[i]));
            }

            return sb.ToString();
        }
    }
}
