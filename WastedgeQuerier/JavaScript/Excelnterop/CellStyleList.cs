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
    internal class CellStyleListInstance : ListInstance<XSSFCellStyle, CellStyleInstance>
    {
        private readonly ExcelInterop _interop;

        public CellStyleListInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public WorkbookInstance Workbook { get; set; }

        public override int Length => Workbook.Workbook.NumCellStyles;

        protected override CellStyleInstance CreateWrapper(XSSFCellStyle obj)
        {
            var cellStyle = _interop.CellStyle.Construct();
            cellStyle.CellStyle = obj;
            cellStyle.Workbook = Workbook;
            return cellStyle;
        }

        public override XSSFCellStyle Get(int index)
        {
            return (XSSFCellStyle)Workbook.Workbook.GetCellStyleAt((short)index);
        }
    }

    internal class CellStyleListFactory : ListFactory<XSSFCellStyle, CellStyleListInstance, CellStyleInstance>
    {
        private readonly ExcelInterop _interop;

        public CellStyleListFactory(Engine engine, ExcelInterop interop)
            : base(engine, "CellStyleList")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override CellStyleListInstance NewInstance(JsValue[] arguments)
        {
            return new CellStyleListInstance(Engine, _interop);
        }
    }
}
