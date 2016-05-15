using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Runtime;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class ColorInstance : InteropInstance
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public ColorInstance(Engine engine)
            : base(engine)
        {
        }

        public ColorInstance(Engine engine, JsValue _0, JsValue _1, JsValue _2)
            : this(engine)
        {
            if (!_2.IsUndefined())
            {
                R = _0.ConvertToInt32().GetValueOrDefault();
                G = _1.ConvertToInt32().GetValueOrDefault();
                B = _2.ConvertToInt32().GetValueOrDefault();
            }
            else if (!_0.IsUndefined())
            {
                string str = _0.ConvertToString();
                if (!String.IsNullOrEmpty(str))
                {
                    if (str[0] == '#')
                        str = str.Substring(1);
                    switch (str.Length)
                    {
                        case 3:
                            R = FromHex(str.Substring(0, 1));
                            R += R * 16;
                            G = FromHex(str.Substring(1, 1));
                            G += G * 16;
                            B = FromHex(str.Substring(2, 1));
                            B += B * 16;
                            break;

                        case 6:
                            R = FromHex(str.Substring(0, 2));
                            G = FromHex(str.Substring(2, 2));
                            B = FromHex(str.Substring(4, 2));
                            break;
                    }
                }
            }
        }

        private int FromHex(string str)
        {
            int result = 0;
            foreach (char c in str)
            {
                result *= 16;

                if (c >= '0' && c <= '9')
                    result += c - '0';
                else if (c >= 'a' && c <= 'f')
                    result += c - 'a' + 10;
                else if (c >= 'A' && c <= 'F')
                    result += c - 'A' + 10;
                else
                    return 0;
            }
            return result;
        }

        public override string ToString()
        {
            return "#" + R.ToString("02X") + G.ToString("02X") + B.ToString("02X");
        }

        public XSSFColor ToColor()
        {
            return new XSSFColor(new[]
            {
                (byte)R,
                (byte)G,
                (byte)B
            });
        }

        public void FromColor(XSSFColor color)
        {
            var values = color.GetRgb();
            R = values[0];
            G = values[1];
            B = values[2];
        }
    }

    internal class ColorFactory : InteropFactory<ColorInstance>
    {
        public ColorFactory(Engine engine)
            : base(engine, "Color")
        {
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "r", self => self.R, (self, value) => self.R = value.ConvertToInt32().GetValueOrDefault(0));
            AddInstanceProperty(prototype, "g", self => self.G, (self, value) => self.G = value.ConvertToInt32().GetValueOrDefault(0));
            AddInstanceProperty(prototype, "b", self => self.B, (self, value) => self.B = value.ConvertToInt32().GetValueOrDefault(0));
        }

        public override ColorInstance NewInstance(JsValue[] arguments)
        {
            return new ColorInstance(Engine, arguments.At(0), arguments.At(1), arguments.At(2));
        }
    }
}
