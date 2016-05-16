using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jint;
using Jint.Native;
using Jint.Runtime;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class WorkbookInstance : InteropInstance
    {
        private readonly ExcelInterop _interop;
        private readonly SheetListInstance _sheetList;
        private readonly CellStyleListInstance _cellStyleList;
        private readonly Dictionary<XSSFFont, FontInstance> _fonts = new Dictionary<XSSFFont, FontInstance>(IdentityComparer<XSSFFont>.Instance);

        public XSSFWorkbook Workbook { get; }

        public WorkbookInstance(Engine engine, XSSFWorkbook workbook, ExcelInterop interop)
            : base(engine)
        {
            if (workbook == null)
                throw new ArgumentNullException(nameof(workbook));

            Workbook = workbook;

            _interop = interop;

            _sheetList = interop.SheetList.Construct();
            _sheetList.Workbook = this;
            FastAddProperty("sheets", _sheetList, true, false, true);
            _cellStyleList = interop.CellStyleList.Construct();
            _cellStyleList.Workbook = this;
            FastAddProperty("cellStyles", _cellStyleList, true, false, true);
        }

        public JsValue Save(JsValue[] arguments)
        {
            using (var stream = File.Create(arguments.At(0).ConvertToString()))
            {
                Workbook.Write(stream);
            }

            return JsValue.Undefined;
        }

        public JsValue CreateSheet(JsValue[] arguments)
        {
            return _sheetList.Wrap((XSSFSheet)Workbook.CreateSheet(arguments.At(0).ConvertToString()));
        }

        public JsValue CreateFont()
        {
            return WrapFont((XSSFFont)Workbook.CreateFont());
        }

        public FontInstance WrapFont(XSSFFont font)
        {
            if (font == null)
                return null;

            FontInstance result;
            if (!_fonts.TryGetValue(font, out result))
            {
                result = _interop.Font.Construct();
                result.Font = font;
                _fonts.Add(font, result);
            }
            return result;
        }

        public JsValue WrapCellStyle(XSSFCellStyle cellStyle)
        {
            return _cellStyleList.Wrap(cellStyle);
        }

        public JsValue CreateCellStyle()
        {
            return _cellStyleList.Wrap((XSSFCellStyle)Workbook.CreateCellStyle());
        }
    }

    internal class WorkbookFactory : InteropFactory<WorkbookInstance>
    {
        private readonly ExcelInterop _interop;

        public WorkbookFactory(Engine engine, ExcelInterop interop)
            : base(engine, "Workbook")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceMethod(prototype, "save", (self, arguments) => self.Save(arguments), 1);
            AddInstanceMethod(prototype, "createSheet", (self, arguments) => self.CreateSheet(arguments), 1);
            AddInstanceMethod(prototype, "createFont", (self, arguments) => self.CreateFont(), 0);
            AddInstanceMethod(prototype, "createCellStyle", (self, arguments) => self.CreateCellStyle(), 0);
        }

        public override WorkbookInstance NewInstance(JsValue[] arguments)
        {
            XSSFWorkbook workbook;

            var fileName = arguments.At(0);

            if (fileName.IsString())
            {
                using (var stream = File.OpenRead(fileName.ConvertToString()))
                {
                    workbook = new XSSFWorkbook(stream);
                }
            }
            else
            {
                workbook = new XSSFWorkbook();
            }

            return new WorkbookInstance(Engine, workbook, _interop);
        }
    }
}
