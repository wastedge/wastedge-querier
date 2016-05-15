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
    internal class FontInstance : InteropInstance
    {
        private static readonly EnumConverter<FontUnderlineType> _underline = new EnumConverter<FontUnderlineType>();

        public XSSFFont Font { get; set; }

        public FontInstance(Engine engine)
            : base(engine)
        {
        }

        public JsValue GetUnderline()
        {
            return _underline.ToString(Font.Underline);
        }

        public void SetUnderline(JsValue value)
        {
            if (value.IsString())
                Font.Underline = _underline.FromString(value.AsString(), FontUnderlineType.None);
            else
                Font.Underline = value.ConvertToBoolean().GetValueOrDefault() ? FontUnderlineType.Single : FontUnderlineType.None;
        }
    }

    internal class FontFactory : InteropFactory<FontInstance>
    {
        private readonly ExcelInterop _interop;

        public FontFactory(Engine engine, ExcelInterop interop)
            : base(engine, "Font")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "name", self => self.Font.FontName, (self, value) => self.Font.FontName = value.ConvertToString());
            AddInstanceProperty(prototype, "bold", self => self.Font.IsBold, (self, value) => self.Font.IsBold = value.ConvertToBoolean().GetValueOrDefault());
            AddInstanceProperty(prototype, "italic", self => self.Font.IsItalic, (self, value) => self.Font.IsItalic = value.ConvertToBoolean().GetValueOrDefault());
            AddInstanceProperty(prototype, "underline", self => self.GetUnderline(), (self, value) => self.SetUnderline(value));
            AddInstanceProperty(prototype, "strikeout", self => self.Font.IsStrikeout, (self, value) => self.Font.IsStrikeout = value.ConvertToBoolean().GetValueOrDefault());
            AddInstanceProperty(prototype, "size", self => self.Font.FontHeightInPoints, (self, value) => self.Font.FontHeightInPoints = (short)value.ConvertToInt32().GetValueOrDefault());
            AddInstanceProperty(prototype, "color", self => _interop.CreateColor(self.Font.GetXSSFColor()), (self, value) => self.Font.SetColor(_interop.UnwrapColor(value)));
        }

        public override FontInstance NewInstance(JsValue[] arguments)
        {
            return new FontInstance(Engine);
        }
    }
}
