using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Native.Array;
using Jint.Runtime;
using Jint.Runtime.Descriptors;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal abstract class ListInstance<TWrapped, TWrapper> : InteropInstance
        where TWrapped : class
        where TWrapper : InteropInstance
    {
        private readonly Dictionary<TWrapped, TWrapper> _cache = new Dictionary<TWrapped, TWrapper>(IdentityComparer<TWrapped>.Instance);

        public abstract int Length { get; }

        protected ListInstance(Engine engine)
            : base(engine)
        {
        }

        public JsValue Wrap(TWrapped obj)
        {
            if (obj == null)
                return JsValue.Null;

            TWrapper cache;
            if (!_cache.TryGetValue(obj, out cache))
            {
                cache = CreateWrapper(obj);
                _cache.Add(obj, cache);
            }

            return cache;
        }

        protected abstract TWrapper CreateWrapper(TWrapped obj);

        public abstract TWrapped Get(int index);

        public virtual void Set(int index, TWrapper obj)
        {
            throw new JavaScriptException("Cannot set");
        }

        public override IEnumerable<KeyValuePair<string, PropertyDescriptor>> GetOwnProperties()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return new KeyValuePair<string, PropertyDescriptor>(
                    i.ToString(),
                    new PropertyDescriptor(Wrap(Get(i)), false, true, false)
                );
            }

            foreach (var item in base.GetOwnProperties())
            {
                yield return item;
            }
        }

        public override bool HasOwnProperty(string propertyName)
        {
            uint index;
            if (ArrayInstance.IsArrayIndex(propertyName, out index))
                return index < Length;
            return base.HasOwnProperty(propertyName);
        }

        public override PropertyDescriptor GetOwnProperty(string propertyName)
        {
            uint index;
            if (ArrayInstance.IsArrayIndex(propertyName, out index) && index < Length)
                return new PropertyDescriptor(Wrap(Get((int)index)), false, true, false);
            return base.GetOwnProperty(propertyName);
        }

        protected override void SetOwnProperty(string propertyName, PropertyDescriptor desc)
        {
            uint index;
            if (ArrayInstance.IsArrayIndex(propertyName, out index) && index < Length)
            {
                TWrapper wrapper = null;
                if (desc.Value.HasValue && !desc.Value.Value.IsNullOrUndefined())
                    wrapper = (TWrapper)desc.Value.Value.AsObject();

                Set((int)index, wrapper);
            }
            else
            {
                base.SetOwnProperty(propertyName, desc);
            }
        }
    }

    internal abstract class ListFactory<TWrapped, TList, TWrapper> : InteropFactory<TList>
        where TWrapped : class
        where TWrapper : InteropInstance
        where TList : ListInstance<TWrapped, TWrapper>
    {
        protected ListFactory(Engine engine, string name)
            : base(engine, name)
        {
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "length", self => self.Length, null, false, false);
        }
    }
}
