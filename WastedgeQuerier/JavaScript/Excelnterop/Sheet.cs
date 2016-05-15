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
    internal class SheetInstance : InteropInstance
    {
        private readonly ExcelInterop _interop;
        private readonly RowListInstance _rowList;
        private readonly MergeRegionListInstance _mergeRegionList;

        public XSSFSheet Sheet { get; set; }
        public WorkbookInstance Workbook { get; set; }

        public SheetInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;

            _rowList = interop.RowList.Construct();
            _rowList.Sheet = this;
            FastAddProperty("rows", _rowList, true, false, true);
            _mergeRegionList = interop.MergeRegionList.Construct();
            _mergeRegionList.Sheet = this;
            FastAddProperty("mergeRegions", _mergeRegionList, true, false, true);
        }

        public JsValue CreateRow(JsValue[] arguments)
        {
            return _rowList.Wrap((XSSFRow)Sheet.CreateRow(arguments.At(0).ConvertToInt32().GetValueOrDefault()));
        }

        public JsValue AddMergeRegion(JsValue[] arguments)
        {
            var mergeRegion = arguments.At(0).Is<MergeRegionInstance>() ? arguments.At(0).As<MergeRegionInstance>() : _interop.MergeRegion.Construct(arguments);
            Sheet.AddMergedRegion(mergeRegion.MergeRegion);
            return mergeRegion;
        }

        public JsValue AutoSizeColumn(JsValue column)
        {
            Sheet.AutoSizeColumn(column.ConvertToInt32().GetValueOrDefault());
            return JsValue.Undefined;
        }
    }

    internal class SheetFactory : InteropFactory<SheetInstance>
    {
        private readonly ExcelInterop _interop;

        public SheetFactory(Engine engine, ExcelInterop interop)
            : base(engine, "Sheet")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "name", self => self.Sheet.SheetName, (self, value) => self.Sheet.Workbook.SetSheetName(self.Sheet.Workbook.GetSheetIndex(self.Sheet), value.ConvertToString()));
            AddInstanceMethod(prototype, "createRow", (self, arguments) => self.CreateRow(arguments), 1);
            AddInstanceMethod(prototype, "addMergeRegion", (self, arguments) => self.AddMergeRegion(arguments), 4);
            AddInstanceProperty(prototype, "defaultRowHeight", self => self.Sheet.DefaultRowHeightInPoints, (self, value) => self.Sheet.DefaultRowHeightInPoints = (float)value.ConvertToDouble().GetValueOrDefault());
            AddInstanceProperty(prototype, "defaultColumnWidth", self => self.Sheet.DefaultColumnWidth, (self, value) => self.Sheet.DefaultColumnWidth = value.ConvertToInt32().GetValueOrDefault());
            AddInstanceMethod(prototype, "autoSizeColumn", (self, arguments) => self.AutoSizeColumn(arguments.At(0)), 1);
        }

        public override SheetInstance NewInstance(JsValue[] arguments)
        {
            return new SheetInstance(Engine, _interop);
        }
    }
}
