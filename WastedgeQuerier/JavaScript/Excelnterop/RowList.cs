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
    internal class RowListInstance : ListInstance<XSSFRow, RowInstance>
    {
        private readonly ExcelInterop _interop;

        public RowListInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public SheetInstance Sheet { get; set; }

        public override int Length => Sheet.Sheet.LastRowNum + 1;

        protected override RowInstance CreateWrapper(XSSFRow obj)
        {
            var row = _interop.Row.Construct();
            row.Row = obj;
            row.Sheet = Sheet;
            return row;
        }

        public override XSSFRow Get(int index)
        {
            return (XSSFRow)Sheet.Sheet.GetRow(index);
        }
    }

    internal class RowListFactory : ListFactory<XSSFRow, RowListInstance, RowInstance>
    {
        private readonly ExcelInterop _interop;

        public RowListFactory(Engine engine, ExcelInterop interop)
            : base(engine, "RowList")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override RowListInstance NewInstance(JsValue[] arguments)
        {
            return new RowListInstance(Engine, _interop);
        }
    }
}
