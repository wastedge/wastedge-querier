using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class SheetListInstance : ListInstance<XSSFSheet, SheetInstance>
    {
        private readonly ExcelInterop _interop;

        public SheetListInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public WorkbookInstance Workbook { get; set; }

        public override int Length => Workbook.Workbook.NumberOfSheets;

        protected override SheetInstance CreateWrapper(XSSFSheet obj)
        {
            var sheet = _interop.Sheet.Construct();
            sheet.Sheet = obj;
            sheet.Workbook = Workbook;
            return sheet;
        }

        public override XSSFSheet Get(int index)
        {
            return (XSSFSheet)Workbook.Workbook.GetSheetAt(index);
        }
    }

    internal class SheetListFactory : ListFactory<XSSFSheet, SheetListInstance, SheetInstance>
    {
        private readonly ExcelInterop _interop;

        public SheetListFactory(Engine engine, ExcelInterop interop)
            : base(engine, "SheetList")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override SheetListInstance NewInstance(JsValue[] arguments)
        {
            return new SheetListInstance(Engine, _interop);
        }
    }
}
