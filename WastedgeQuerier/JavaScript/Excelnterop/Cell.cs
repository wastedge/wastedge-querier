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
    internal class CellInstance : InteropInstance
    {
        private readonly ExcelInterop _interop;

        public XSSFCell Cell { get; set; }
        public RowInstance Row { get; set; }

        public CellInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public JsValue GetValue()
        {
            var cellType = Cell.CellType;
            if (cellType == CellType.Formula)
                cellType = Cell.CachedFormulaResultType;

            switch (cellType)
            {
                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(Cell))
                        return Engine.Date.Construct(DateTime.FromOADate(Cell.NumericCellValue));
                    return Cell.NumericCellValue;
                case CellType.String:
                    return Cell.StringCellValue;
                case CellType.Boolean:
                    return Cell.BooleanCellValue;
                default:
                    return JsValue.Null;
            }
        }

        public void SetValue(JsValue arg)
        {
            switch (arg.Type)
            {
                case Types.Boolean:
                    Cell.SetCellValue(arg.AsBoolean());
                    break;
                case Types.String:
                    Cell.SetCellValue(arg.AsString());
                    break;
                case Types.Number:
                    Cell.SetCellValue(arg.AsNumber());
                    break;
                case Types.Object:
                    if (arg.IsDate())
                    {
                        Cell.SetCellValue(arg.AsDate().ToDateTime());
                    }
                    else
                    {
                        double? value = arg.ConvertToDouble();
                        if (value.HasValue)
                            Cell.SetCellValue(value.Value);
                        else
                            Cell.SetCellValue((string)null);
                    }
                    break;
                default:
                    Cell.SetCellValue((string)null);
                    break;
            }
        }

        public JsValue GetCellType()
        {
            var cellType = Cell.CellType;
            if (cellType == CellType.Formula)
                cellType = Cell.CachedFormulaResultType;

            switch (cellType)
            {
                case CellType.Numeric:
                    return "number";
                case CellType.String:
                    return "string";
                case CellType.Boolean:
                    return "boolean";
                default:
                    return "null";
            }
        }

        public JsValue GetComment()
        {
            if (Cell.CellComment == null)
                return JsValue.Null;

            var comment = _interop.Comment.Construct();
            comment.Comment = (XSSFComment)Cell.CellComment;
            return comment;
        }

        public void SetComment(JsValue value)
        {
            Cell.CellComment = null;

            if (value.Is<CommentInstance>())
            {
                Cell.CellComment = value.As<CommentInstance>().Comment;
            }
            else
            {
                string str = value.ConvertToString();
                if (str != null)
                {
                    var comment = (XSSFComment)((XSSFSheet)Cell.Row.Sheet).CreateComment();
                    comment.String = new XSSFRichTextString(str);
                    Cell.CellComment = comment;
                }
            }
        }

        public void SetHyperlink(JsValue value)
        {
            string str = value.ConvertToString();

            if (str == null)
            {
                Cell.Hyperlink = null;
            }
            else
            {
                Cell.Hyperlink = new XSSFHyperlink(HyperlinkType.Url)
                {
                    Address = str
                };
            }
        }

        public JsValue GetStyle()
        {
            return Row.Sheet.Workbook.WrapCellStyle((XSSFCellStyle)Cell.CellStyle);
        }

        public void SetStyle(JsValue value)
        {
            Cell.CellStyle = value.Is<CellStyleInstance>() ? value.As<CellStyleInstance>().CellStyle : null;
        }
    }

    internal class CellFactory : InteropFactory<CellInstance>
    {
        private readonly ExcelInterop _interop;

        public CellFactory(Engine engine, ExcelInterop interop)
            : base(engine, "Cell")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "comment", self => self.GetComment(), (self, value) => self.SetComment(value));
            AddInstanceProperty(prototype, "formula", self => self.Cell.CellFormula, (self, value) => self.Cell.CellFormula = value.ConvertToString());
            AddInstanceProperty(prototype, "hyperlink", self => self.Cell.Hyperlink?.Address, (self, value) => self.SetHyperlink(value));
            AddInstanceProperty(prototype, "type", self => self.GetCellType(), null);
            AddInstanceProperty(prototype, "value", self => self.GetValue(), (self, value) => self.SetValue(value));
            AddInstanceProperty(prototype, "style", self => self.GetStyle(), (self, value) => self.SetStyle(value));
        }

        public override CellInstance NewInstance(JsValue[] arguments)
        {
            return new CellInstance(Engine, _interop);
        }
    }
}
