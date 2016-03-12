using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WastedgeApi
{
    public abstract class EntityPhysicalField : EntityMember
    {
        public EntityDataType DataType { get; }
        public bool Mandatory { get; }

        protected EntityPhysicalField(string name, string comments, EntityDataType dataType, bool mandatory)
            : base(name, comments)
        {
            if (dataType == null)
                throw new ArgumentNullException(nameof(dataType));

            DataType = dataType;
            Mandatory = mandatory;
        }
    }
}
