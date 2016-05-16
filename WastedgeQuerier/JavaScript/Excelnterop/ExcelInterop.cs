using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Jint.Runtime;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class ExcelInterop
    {
        public Engine Engine { get; }
        public WorkbookFactory Workbook { get; }
        public SheetFactory Sheet { get; }
        public SheetListFactory SheetList { get; }
        public RowFactory Row { get; }
        public RowListFactory RowList { get; }
        public CellFactory Cell { get; }
        public CellListFactory CellList { get; }
        public CommentFactory Comment { get; }
        public ColorFactory Color { get; }
        public FontFactory Font { get; }
        public MergeRegionListFactory MergeRegionList { get; }
        public MergeRegionFactory MergeRegion { get; }
        public CellStyleListFactory CellStyleList { get; }
        public CellStyleFactory CellStyle { get; }

        public ExcelInterop(Engine engine)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));

            Engine = engine;

            Workbook = new WorkbookFactory(engine, this);
            Sheet = new SheetFactory(engine, this);
            SheetList = new SheetListFactory(engine, this);
            Row = new RowFactory(engine, this);
            RowList = new RowListFactory(engine, this);
            Cell = new CellFactory(engine, this);
            CellList = new CellListFactory(engine, this);
            Comment = new CommentFactory(engine);
            Color = new ColorFactory(engine);
            Font = new FontFactory(engine, this);
            MergeRegionList = new MergeRegionListFactory(engine, this);
            MergeRegion = new MergeRegionFactory(engine);
            CellStyleList = new CellStyleListFactory(engine, this);
            CellStyle = new CellStyleFactory(engine, this);
        }

        public void Setup()
        {
            var target = Engine.Object.Construct(Arguments.Empty);
            Engine.Global.FastAddProperty("Excel", target, true, false, true);

            Workbook.Setup(target);
            Sheet.Setup(target);
            SheetList.Setup(target);
            Row.Setup(target);
            RowList.Setup(target);
            Cell.Setup(target);
            CellList.Setup(target);
            Comment.Setup(target);
            Color.Setup(target);
            Font.Setup(target);
            MergeRegionList.Setup(target);
            MergeRegion.Setup(target);
            CellStyleList.Setup(target);
            CellStyle.Setup(target);
        }

        public JsValue CreateColor(XSSFColor color)
        {
            var result = Color.Construct();
            result.FromColor(color);
            return result;
        }

        public XSSFColor UnwrapColor(JsValue value)
        {
            if (value.Is<ColorInstance>())
                return value.As<ColorInstance>().ToColor();
            return UnwrapColor(Color.Construct(new[] { value }));
        }
    }
}
