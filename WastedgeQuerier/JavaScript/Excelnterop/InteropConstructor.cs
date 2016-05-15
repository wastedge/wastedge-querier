using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Native.Date;
using Jint.Native.Function;
using Jint.Native.Object;
using Jint.Runtime;
using Jint.Runtime.Interop;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class InteropConstructor<T> : FunctionInstance, IConstructor
        where T : InteropInstance
    {
        private readonly InteropFactory<T> _factory;

        public InteropPrototype PrototypeObject { get; }

        public InteropConstructor(Engine engine, InteropFactory<T> factory)
            : base(engine, null, null, false)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            _factory = factory;

            Extensible = true;
            Prototype = Engine.Function.PrototypeObject;

            PrototypeObject = factory.NewPrototype();
            PrototypeObject.Prototype = engine.Object.PrototypeObject;
            PrototypeObject.Engine = engine;
            PrototypeObject.FastAddProperty("constructor", this, true, false, true);

            FastAddProperty("prototype", PrototypeObject, false, false, false);
        }

        public override JsValue Call(JsValue thisObject, JsValue[] arguments)
        {
            return JsValue.Undefined;
        }

        public ObjectInstance Construct(JsValue[] arguments)
        {
            var instance = _factory.NewInstance(arguments);
            instance.Prototype = PrototypeObject;
            return instance;
        }
    }
}
