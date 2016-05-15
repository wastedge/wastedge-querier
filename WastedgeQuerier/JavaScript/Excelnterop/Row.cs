using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Runtime;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class RowInstance : InteropInstance
    {
        private readonly CellListInstance _cellList;

        public XSSFRow Row { get; set; }
        public SheetInstance Sheet { get; set; }

        public RowInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _cellList = interop.CellList.Construct();
            _cellList.Row = this;
            FastAddProperty("cells", _cellList, true, false, true);
        }

        public JsValue CreateCell(JsValue[] arguments)
        {
            var cell = _cellList.Wrap((XSSFCell)Row.CreateCell(arguments.At(0).ConvertToInt32().GetValueOrDefault()));
            if (arguments.Length > 1)
                cell.As<CellInstance>().SetValue(arguments[1]);
            return cell;
        }
    }

    internal class RowFactory : InteropFactory<RowInstance>
    {
        private readonly ExcelInterop _interop;

        public RowFactory(Engine engine, ExcelInterop interop)
            : base(engine, "Row")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceMethod(prototype, "createCell", (self, arguments) => self.CreateCell(arguments), 1);
            AddInstanceProperty(prototype, "height", self => self.Row.HeightInPoints, (self, value) => self.Row.HeightInPoints = (float)value.ConvertToDouble().GetValueOrDefault(self.Row.Sheet.DefaultRowHeightInPoints));
        }

        public override RowInstance NewInstance(JsValue[] arguments)
        {
            return new RowInstance(Engine, _interop);
        }
    }
}
