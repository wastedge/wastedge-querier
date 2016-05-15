using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class MergeRegionListInstance : ListInstance<CellRangeAddress, MergeRegionInstance>
    {
        private readonly ExcelInterop _interop;

        public MergeRegionListInstance(Engine engine, ExcelInterop interop)
            : base(engine)
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public SheetInstance Sheet { get; set; }

        public override int Length => Sheet.Sheet.NumMergedRegions;

        protected override MergeRegionInstance CreateWrapper(CellRangeAddress obj)
        {
            var mergeRegion = _interop.MergeRegion.Construct();
            mergeRegion.MergeRegion = obj;
            return mergeRegion;
        }

        public override CellRangeAddress Get(int index)
        {
            return Sheet.Sheet.GetMergedRegion(index);
        }
    }

    internal class MergeRegionListFactory : ListFactory<CellRangeAddress, MergeRegionListInstance, MergeRegionInstance>
    {
        private readonly ExcelInterop _interop;

        public MergeRegionListFactory(Engine engine, ExcelInterop interop)
            : base(engine, "MergeRegionList")
        {
            if (interop == null)
                throw new ArgumentNullException(nameof(interop));

            _interop = interop;
        }

        public override MergeRegionListInstance NewInstance(JsValue[] arguments)
        {
            return new MergeRegionListInstance(Engine, _interop);
        }
    }
}
