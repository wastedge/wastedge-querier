using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Runtime;
using WastedgeApi;
using WastedgeQuerier.JavaScript.Excelnterop;

namespace WastedgeQuerier.JavaScript
{
    internal class JavaScriptUtil
    {
        private readonly ApiCredentials _credentials;

        public JavaScriptUtil(ApiCredentials credentials)
        {
            if (credentials == null)
                throw new ArgumentNullException(nameof(credentials));

            _credentials = credentials;
        }

        public void Setup(Engine engine, System.Windows.Forms.Form owner)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            engine.SetValue("Api", BuildApi(engine));
            engine.SetValue("UI", BuildUi(engine, owner));
            new ExcelInterop(engine).Setup();
        }

        private object BuildUi(Engine engine, System.Windows.Forms.Form owner)
        {
            var ui = new JavaScriptUi(engine, owner);

            engine.SetValue("alert", new Action<string, string>(ui.Alert));
            engine.SetValue("confirm", new Func<string, string, bool>(ui.Confirm));

            return new
            {
                form = new Func<string, JsValue, JsValue, JsValue>(ui.ShowForm),
                load = new Func<JsValue, string>(ui.LoadFile),
                open = new Func<JsValue, string>(ui.OpenFile),
                save = new Func<string, JsValue, string>(ui.SaveFile),
                start = new Action<string>(ui.Start)
            };
        }

        private object BuildApi(Engine engine)
        {
            var api = new Api(_credentials);

            return new
            {
                query = new Func<string, string, JsValue, JsValue, string, JsValue, JsValue>((entity, weql, offset, count, order, compact) =>
                {
                    var parameters = new ParameterBuilder()
                        .Add("$query", weql)
                        .Add("$offset", offset.ConvertToInt32())
                        .Add("$count", count.ConvertToInt32())
                        .Add("$order", order)
                        .Add("$output", compact.ConvertToBoolean().GetValueOrDefault() ? "compact" : "normal")
                        .ToString();

                    return Execute(engine, () => api.ExecuteRaw(entity, parameters, "GET", null));
                }),
                create = new Func<string, JsValue, JsValue>((path, request) =>
                    Execute(engine, () => api.ExecuteRaw(path, null, "PUT", SerializeRequest(engine, request)))
                ),
                update = new Func<string, JsValue, JsValue>((path, request) =>
                    Execute(engine, () => api.ExecuteRaw(path, null, "POST", SerializeRequest(engine, request)))
                ),
                delete = new Func<string, JsValue, JsValue>((path, request) =>
                    Execute(engine, () => api.ExecuteRaw(path, null, "DELETE", SerializeRequest(engine, request)))
                ),
                schema = new Func<string, JsValue>(path =>
                    Execute(engine, () => api.ExecuteRaw(path, "$meta=true", "GET", null))
                )
            };
        }

        private JsValue Execute(Engine engine, Func<string> func)
        {
            try
            {
                return ParseResponse(engine, func());
            }
            catch (ApiException ex)
            {
                var errors = engine.Array.Construct(Arguments.Empty);

                foreach (var error in ex.Errors)
                {
                    var errorObject = engine.Object.Construct(Arguments.Empty);
                    engine.Array.PrototypeObject.Push(errors, new JsValue[] { errorObject });

                    errorObject.Put("row", new JsValue(error.Row), false);

                    var rowErrorsArray = engine.Array.Construct(Arguments.Empty);
                    errorObject.Put("errors", rowErrorsArray, false);

                    foreach (var rowError in error.Errors)
                    {
                        var rowErrorObject = engine.Object.Construct(Arguments.Empty);
                        engine.Array.PrototypeObject.Push(rowErrorsArray, new JsValue[] { rowErrorObject });

                        if (rowError.Field != null)
                            rowErrorObject.Put("field", new JsValue(rowError.Field), false);
                        rowErrorObject.Put("error", new JsValue(rowError.Error), false);
                    }
                }

                var errorInstance = engine.Error.Construct(new[] { new JsValue(ex.Message) });
                errorInstance.Put("errors", errors, false);

                throw new JavaScriptException(errorInstance);
            }
        }

        private JsValue ParseResponse(Engine engine, string response)
        {
            if (!String.IsNullOrEmpty(response))
                return engine.Json.Parse(engine.Json, new[] { new JsValue(response) });
            return JsValue.Null;
        }

        private string SerializeRequest(Engine engine, JsValue value)
        {
            if (value.IsObject())
                return engine.Json.Stringify(engine.Json, new[] { value }).ConvertToString();
            if (value.IsString())
                return value.ConvertToString();
            return null;
        }

        private class ParameterBuilder
        {
            private readonly StringBuilder _sb = new StringBuilder();

            public ParameterBuilder Add(string name, object value)
            {
                if (value != null)
                {
                    if (_sb.Length > 0)
                        _sb.Append('&');
                    _sb
                        .Append(Uri.EscapeDataString(name))
                        .Append('=')
                        .Append(Uri.EscapeDataString(value.ToString()));
                }

                return this;
            }

            public override string ToString()
            {
                return _sb.ToString();
            }
        }
    }
}
