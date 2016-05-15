using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class CellStyleInstance : InteropInstance
    {
        private static readonly EnumConverter<HorizontalAlignment> _alignment = new EnumConverter<HorizontalAlignment>();
        private static readonly EnumConverter<VerticalAlignment> _verticalAlignment = new EnumConverter<VerticalAlignment>();
        private static readonly EnumConverter<BorderStyle> _borderStyle = new EnumConverter<BorderStyle>();
        private static readonly EnumConverter<FillPattern> _fillPattern = new EnumConverter<FillPattern>();

        private readonly ExcelInterop _interop;

        public XSSFCellStyle CellStyle { get; set; }
        public WorkbookInstance Workbook { get; set; }

        public CellStyleInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public JsValue GetAlignment()
        {
            return _alignment.ToString(CellStyle.Alignment);
        }

        public void SetAlignment(JsValue value)
        {
            CellStyle.Alignment = _alignment.FromString(value.ConvertToString(), HorizontalAlignment.Left);
        }

        public JsValue GetVerticalAlignment()
        {
            return _verticalAlignment.ToString(CellStyle.VerticalAlignment);
        }

        public void SetVerticalAlignment(JsValue value)
        {
            CellStyle.VerticalAlignment = _verticalAlignment.FromString(value.ConvertToString(), VerticalAlignment.Bottom);
        }

        public JsValue GetBorderLeft()
        {
            return _borderStyle.ToString(CellStyle.BorderLeft);
        }

        public void SetBorderLeft(JsValue value)
        {
            CellStyle.BorderLeft = _borderStyle.FromString(value.ConvertToString(), BorderStyle.None);
        }

        public JsValue GetBorderColorLeft()
        {
            var style = CellStyle;
            var color = _interop.Color.Construct();
            color.FromColor(style.LeftBorderXSSFColor);
            return color;
        }

        public void SetBorderColorLeft(JsValue value)
        {
            CellStyle.SetLeftBorderColor(value.Is<ColorInstance>() ? value.As<ColorInstance>().ToColor() : null);
        }

        public JsValue GetBorderTop()
        {
            return _borderStyle.ToString(CellStyle.BorderTop);
        }

        public void SetBorderTop(JsValue value)
        {
            CellStyle.BorderTop = _borderStyle.FromString(value.ConvertToString(), BorderStyle.None);
        }

        public JsValue GetBorderColorTop()
        {
            var style = CellStyle;
            var color = _interop.Color.Construct();
            color.FromColor(style.TopBorderXSSFColor);
            return color;
        }

        public void SetBorderColorTop(JsValue value)
        {
            CellStyle.SetTopBorderColor(value.Is<ColorInstance>() ? value.As<ColorInstance>().ToColor() : null);
        }

        public JsValue GetBorderRight()
        {
            return _borderStyle.ToString(CellStyle.BorderRight);
        }

        public void SetBorderRight(JsValue value)
        {
            CellStyle.BorderRight = _borderStyle.FromString(value.ConvertToString(), BorderStyle.None);
        }

        public JsValue GetBorderColorRight()
        {
            var style = CellStyle;
            var color = _interop.Color.Construct();
            color.FromColor(style.RightBorderXSSFColor);
            return color;
        }

        public void SetBorderColorRight(JsValue value)
        {
            CellStyle.SetRightBorderColor(value.Is<ColorInstance>() ? value.As<ColorInstance>().ToColor() : null);
        }

        public JsValue GetBorderBottom()
        {
            return _borderStyle.ToString(CellStyle.BorderBottom);
        }

        public void SetBorderBottom(JsValue value)
        {
            CellStyle.BorderBottom = _borderStyle.FromString(value.ConvertToString(), BorderStyle.None);
        }

        public JsValue GetBorderColorBottom()
        {
            return _interop.CreateColor(CellStyle.BottomBorderXSSFColor);
        }

        public void SetBorderColorBottom(JsValue value)
        {
            CellStyle.SetBottomBorderColor(_interop.UnwrapColor(value));
        }

        public JsValue GetDataFormat()
        {
            return CellStyle.GetDataFormatString();
        }

        public void SetDataFormat(JsValue value)
        {
            string str = value.ConvertToString();
            if (str == null)
                CellStyle.DataFormat = 0;
            else
                CellStyle.DataFormat = Workbook.Workbook.CreateDataFormat().GetFormat(str);
        }

        public JsValue GetFillPattern()
        {
            return _fillPattern.ToString(CellStyle.FillPattern);
        }

        public void SetFillPattern(JsValue value)
        {
            CellStyle.FillPattern = _fillPattern.FromString(value.ConvertToString(), FillPattern.NoFill);
        }

        public JsValue GetFillForegroundColor()
        {
            return _interop.CreateColor(CellStyle.FillForegroundXSSFColor);
        }

        public void SetFillForegroundColor(JsValue value)
        {
            CellStyle.FillForegroundXSSFColor = _interop.UnwrapColor(value);
        }

        public JsValue GetFillBackgroundColor()
        {
            return _interop.CreateColor(CellStyle.FillBackgroundXSSFColor);
        }

        public void SetFillBackgroundColor(JsValue value)
        {
            CellStyle.FillBackgroundXSSFColor = _interop.UnwrapColor(value);
        }

        public JsValue GetFont()
        {
            return Workbook.WrapFont(CellStyle.GetFont());
        }

        public void SetFont(JsValue value)
        {
            CellStyle.SetFont(value.Is<FontInstance>() ? value.As<FontInstance>().Font : null);
        }

        public JsValue SetDateFormat()
        {
            CellStyle.DataFormat = Workbook.Workbook.CreateDataFormat().GetFormat(ExcelUtil.GetDateFormat());
            return JsValue.Undefined;
        }

        public JsValue SetDateTimeFormat()
        {
            CellStyle.DataFormat = Workbook.Workbook.CreateDataFormat().GetFormat(ExcelUtil.GetDateTimeFormat());
            return JsValue.Undefined;
        }
    }

    internal class CellStyleFactory : InteropFactory<CellStyleInstance>
    {
        private readonly ExcelInterop _interop;

        public CellStyleFactory(Engine engine, ExcelInterop interop)
            : base(engine, "CellStyle")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "alignment", self => self.GetAlignment(), (self, value) => self.SetAlignment(value));
            AddInstanceProperty(prototype, "borderBottom", self => self.GetBorderBottom(), (self, value) => self.SetBorderBottom(value));
            AddInstanceProperty(prototype, "borderColorBottom", self => self.GetBorderColorBottom(), (self, value) => self.SetBorderColorBottom(value));
            AddInstanceProperty(prototype, "borderLeft", self => self.GetBorderLeft(), (self, value) => self.SetBorderLeft(value));
            AddInstanceProperty(prototype, "borderColorLeft", self => self.GetBorderColorLeft(), (self, value) => self.SetBorderColorLeft(value));
            AddInstanceProperty(prototype, "borderRight", self => self.GetBorderRight(), (self, value) => self.SetBorderRight(value));
            AddInstanceProperty(prototype, "borderColorRight", self => self.GetBorderColorRight(), (self, value) => self.SetBorderColorRight(value));
            AddInstanceProperty(prototype, "borderTop", self => self.GetBorderTop(), (self, value) => self.SetBorderTop(value));
            AddInstanceProperty(prototype, "borderColorTop", self => self.GetBorderColorTop(), (self, value) => self.SetBorderColorTop(value));
            AddInstanceProperty(prototype, "format", self => self.GetDataFormat(), (self, value) => self.SetDataFormat(value));
            AddInstanceProperty(prototype, "fillBackgroundColor", self => self.GetFillBackgroundColor(), (self, value) => self.SetFillBackgroundColor(value));
            AddInstanceProperty(prototype, "fillForegroundColor", self => self.GetFillForegroundColor(), (self, value) => self.SetFillForegroundColor(value));
            AddInstanceProperty(prototype, "fillPattern", self => self.GetFillPattern(), (self, value) => self.SetFillPattern(value));
            AddInstanceProperty(prototype, "font", self => self.GetFont(), (self, value) => self.SetFont(value));
            AddInstanceProperty(prototype, "verticalAlignment", self => self.GetVerticalAlignment(), (self, value) => self.SetVerticalAlignment(value));
            AddInstanceProperty(prototype, "wrapText", self => self.CellStyle.WrapText, (self, value) => self.CellStyle.WrapText = value.ConvertToBoolean().GetValueOrDefault());
            AddInstanceMethod(prototype, "setDateFormat", (self, arguments) => self.SetDateFormat(), 0);
            AddInstanceMethod(prototype, "setDateTimeFormat", (self, arguments) => self.SetDateTimeFormat(), 0);
        }

        public override CellStyleInstance NewInstance(JsValue[] arguments)
        {
            return new CellStyleInstance(Engine, _interop);
        }
    }
}
