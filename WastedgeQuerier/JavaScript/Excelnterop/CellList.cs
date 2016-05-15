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
    internal class CellListInstance : ListInstance<XSSFCell, CellInstance>
    {
        private readonly ExcelInterop _interop;

        public CellListInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public RowInstance Row { get; set; }

        public override int Length => Row.Row.LastCellNum;

        protected override CellInstance CreateWrapper(XSSFCell obj)
        {
            var cell = _interop.Cell.Construct();
            cell.Cell = obj;
            cell.Row = Row;
            return cell;
        }

        public override XSSFCell Get(int index)
        {
            return (XSSFCell)Row.Row.GetCell(index);
        }
    }

    internal class CellListFactory : ListFactory<XSSFCell, CellListInstance, CellInstance>
    {
        private readonly ExcelInterop _interop;

        public CellListFactory(Engine engine, ExcelInterop interop)
            : base(engine, "CellList")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override CellListInstance NewInstance(JsValue[] arguments)
        {
            return new CellListInstance(Engine, _interop);
        }
    }
}
