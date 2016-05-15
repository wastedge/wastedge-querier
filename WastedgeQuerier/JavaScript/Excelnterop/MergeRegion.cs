using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Runtime;
using NPOI.SS.Util;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class MergeRegionInstance : InteropInstance
    {
        public CellRangeAddress MergeRegion { get; set; }

        public MergeRegionInstance(Engine engine, JsValue firstRow, JsValue lastRow, JsValue firstColumn, JsValue lastColumn)
            : base(engine)
        {
            MergeRegion = new CellRangeAddress(
                firstRow.ConvertToInt32().GetValueOrDefault(),
                lastRow.ConvertToInt32().GetValueOrDefault(),
                firstColumn.ConvertToInt32().GetValueOrDefault(),
                lastColumn.ConvertToInt32().GetValueOrDefault()
            );
        }
    }

    internal class MergeRegionFactory : InteropFactory<MergeRegionInstance>
    {
        public MergeRegionFactory(Engine engine)
            : base(engine, "MergeRegion")
        {
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "firstRow", self => self.MergeRegion.FirstRow, (self, value) => self.MergeRegion.FirstRow = value.ConvertToInt32().GetValueOrDefault());
            AddInstanceProperty(prototype, "lastRow", self => self.MergeRegion.LastRow, (self, value) => self.MergeRegion.LastRow = value.ConvertToInt32().GetValueOrDefault());
            AddInstanceProperty(prototype, "firstColumn", self => self.MergeRegion.FirstColumn, (self, value) => self.MergeRegion.FirstColumn = value.ConvertToInt32().GetValueOrDefault());
            AddInstanceProperty(prototype, "lastColumn", self => self.MergeRegion.LastColumn, (self, value) => self.MergeRegion.LastColumn = value.ConvertToInt32().GetValueOrDefault());
        }

        public override MergeRegionInstance NewInstance(JsValue[] arguments)
        {
            return new MergeRegionInstance(Engine, arguments.At(0), arguments.At(1), arguments.At(2), arguments.At(3));
        }
    }
}
