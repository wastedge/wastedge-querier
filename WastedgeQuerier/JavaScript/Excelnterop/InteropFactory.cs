using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;
using Jint.Runtime.Descriptors;
using Jint.Runtime.Environments;
using Jint.Runtime.Interop;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal abstract class InteropFactory<T>
        where T : InteropInstance
    {
        private InteropConstructor<T> _constructor;

        public Engine Engine { get; }
        public string Name { get; }

        protected InteropFactory(Engine engine, string name)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Engine = engine;
            Name = name;
        }

        public void Setup()
        {
            Setup(Engine.Global);
        }

        public void Setup(ObjectInstance target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            _constructor = NewConstructor();
            ConfigureConstructor(_constructor);
            ConfigurePrototype(_constructor.PrototypeObject);
            target.FastAddProperty(Name, _constructor, true, false, true);
        }

        public T Construct()
        {
            return Construct(Arguments.Empty);
        }

        public T Construct(JsValue[] arguments)
        {
            return (T)_constructor.Construct(arguments);
        }

        public virtual InteropPrototype NewPrototype()
        {
            return new InteropPrototype(Engine);
        }

        public virtual void ConfigurePrototype(InteropPrototype prototype)
        {
        }

        public virtual InteropConstructor<T> NewConstructor()
        {
            return new InteropConstructor<T>(Engine, this);
        }

        public virtual void ConfigureConstructor(InteropConstructor<T> constructor)
        {
        }

        public abstract T NewInstance(JsValue[] arguments);

        protected void AddMethod(ObjectInstance target, string name, Func<JsValue[], JsValue> func, int arguments)
        {
            target.FastAddProperty(name, new ClrFunctionInstance(Engine, (self, args) => func(args), 1), true, false, true);
        }

        protected void AddInstanceMethod(ObjectInstance target, string name, Func<T, JsValue[], JsValue> func, int arguments)
        {
            target.FastAddProperty(name, new ClrFunctionInstance(Engine, (self, args) => func((T)self.AsObject(), args), 1), true, false, true);
        }

        protected void AddInstanceProperty(ObjectInstance target, string name, Func<T, JsValue> get, Action<T, JsValue> set)
        {
            AddInstanceProperty(target, name, get, set, true, true);
        }

        protected void AddInstanceProperty(ObjectInstance target, string name, Func<T, JsValue> get, Action<T, JsValue> set, bool enumerable, bool configurable)
        {
            GetterFunction getter = null;
            if (get != null)
                getter = new GetterFunction(Engine, get);
            SetterFunction setter = null;
            if (set != null)
                setter = new SetterFunction(Engine, set);
            target.DefineOwnProperty(name, new PropertyDescriptor(getter, setter, enumerable, configurable), false);
        }

        private class GetterFunction : FunctionInstance
        {
            private readonly Func<T, JsValue> _func;

            public GetterFunction(Engine engine, Func<T, JsValue> func)
                : base(engine, null, null, false)
            {
                _func = func;
            }

            public override JsValue Call(JsValue thisObject, JsValue[] arguments)
            {
                return _func((T)thisObject.AsObject());
            }
        }

        private class SetterFunction : FunctionInstance
        {
            private readonly Action<T, JsValue> _func;

            public SetterFunction(Engine engine, Action<T, JsValue> func)
                : base(engine, null, null, false)
            {
                _func = func;
            }

            public override JsValue Call(JsValue thisObject, JsValue[] arguments)
            {
                _func((T)thisObject.AsObject(), arguments.At(0));
                return JsValue.Undefined;
            }
        }
    }
}
