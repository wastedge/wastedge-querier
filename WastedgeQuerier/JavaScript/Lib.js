sys = (function () {
    return {
        log: function (message) {
            if (message === undefined)
                message = '';

            System.Console.WriteLine(message.replace(/\r?\n/g, '\r\n'));
        },

        dump: function (o, indent) {
            if (indent == null)
                indent = 0;

            var result = '';

            var dumpLine = function (text, indent) {
                for (var i = 0; i < indent; i++) {
                    result += '  ';
                }

                result += String(text);
                result += '\r\n';
            }

            var dumpObj = function (o, indent) {
                switch (typeof (o)) {
                    case 'object':
                        if (o === null) {
                            dumpLine('null', indent);
                        } else if (o instanceof Array) {
                            for (var i = 0; i < o.length; i++) {
                                dumpLine(i + ':', indent);
                                dumpObj(o[i], indent + 1);
                            }
                        } else {
                            for (var key in o) {
                                dumpLine(key + ':', indent);
                                dumpObj(o[key], indent + 1);
                            }
                        }
                        break;

                    case 'boolean':
                        if (o)
                            dumpLine('true', indent);
                        else
                            dumpLine('false', indent);
                        break;

                    case 'number':
                        dumpLine(o, indent);
                        break;

                    case 'string':
                        dumpLine("'" + o + "'", indent);
                        break;

                    default:
                        dumpLine(typeof (o), indent);
                        break;
                }
            };

            dumpObj(o, indent);

            sys.log(result);
        }
    };
})();

console = {
    log: function (log) {
        sys.log(log + '\n');
    }
};

weapi = (function (api) {
    function run(path, parameters, method, request) {
        return api.ExecuteRaw(path, parameters, method, request);
    }

    function buildParameters(parameters) {
        var result = '';

        for (var key in parameters) {
            if (parameters.hasOwnProperty(key) && parameters[key] !== undefined) {
                if (result !== '') {
                    result += '&';
                }
                result += encodeURIComponent(key) + '=' + encodeURIComponent(parameters[key]);
            }
        }

        return result;
    }

    return {
        run: run,
        query: function (entity, weql, offset, count) {
            var parameters = buildParameters({
                $query: weql,
                $offset: offset,
                $count: count
            });

            return JSON.parse(run(entity, parameters, 'GET', null));
        }
    };
})(api);
