using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WastedgeApi;

namespace WastedgeQuerier
{
    public class EntityMemberPath : IEnumerable<EntityMember>, IEquatable<EntityMemberPath>
    {
        public static readonly EntityMemberPath Empty = new EntityMemberPath();

        private readonly EntityMember[] _members;

        public int Count => _members.Length;

        public EntityMember this[int index] => _members[index];

        public EntityMember Tail => this[Count - 1];

        public EntityMemberPath(IEnumerable<EntityMember> members)
        {
            if (members == null)
                throw new ArgumentNullException(nameof(members));

            _members = members.ToArray();
        }

        public EntityMemberPath(IEnumerable<EntityMember> members, EntityMember member)
        {
            var list = members.ToList();
            list.Add(member);
            _members = list.ToArray();
        }

        public EntityMemberPath(params EntityMember[] members)
        {
            _members = new EntityMember[members.Length];
            Array.Copy(members, _members, members.Length);
        }

        public IEnumerator<EntityMember> GetEnumerator()
        {
            return ((IEnumerable<EntityMember>)_members).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            return Equals(obj as EntityMemberPath);
        }

        public bool Equals(EntityMemberPath other)
        {
            if (ReferenceEquals(this, other))
                return true;
            if (other == null)
                return false;

            if (Count != other.Count)
                return false;

            for (int i = 0; i < Count; i++)
            {
                if (this[i] != other[i])
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;

            foreach (var member in _members)
            {
                hashCode *= 31;
                hashCode += member.GetHashCode();
            }

            return hashCode;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _members.Length; i++)
            {
                if (i > 0)
                    sb.Append('.');
                sb.Append(_members[i].Name);
            }

            return sb.ToString();
        }

        public static EntityMemberPath Parse(Api api, EntitySchema entity, string path)
        {
            if (api == null)
                throw new ArgumentNullException(nameof(api));
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var result = new List<EntityMember>();
            if (!Parse(result, api, entity, path))
                return null;

            return new EntityMemberPath(result);
        }

        private static bool Parse(List<EntityMember> result, Api api, EntitySchema entity, string path)
        {
            int pos = path.IndexOf('.');
            string name;

            if (pos == -1)
            {
                name = path;
                path = null;
            }
            else
            {
                name = path.Substring(0, pos);
                path = path.Substring(pos + 1);
            }

            EntityMember member;
            if (!entity.Members.TryGetValue(name, out member))
                return false;

            result.Add(member);

            if (path == null)
                return true;

            var foreign = member as EntityForeign;
            if (foreign == null)
                return false;

            entity = api.GetEntitySchema(foreign.LinkTable);
            if (entity == null)
                return false;

            return Parse(result, api, entity, path);
        }
    }
}
