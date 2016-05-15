using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Jint.Runtime;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WastedgeQuerier.JavaScript.Excelnterop
{
    internal class CommentInstance : InteropInstance
    {
        public XSSFComment Comment { get; set; }

        public CommentInstance(Engine engine)
            : base(engine)
        {
        }

        public void SetValue(JsValue value)
        {
            string str = value.ConvertToString();
            Comment.String = str == null ? null : new XSSFRichTextString(str);
        }
    }

    internal class CommentFactory : InteropFactory<CommentInstance>
    {
        public CommentFactory(Engine engine)
            : base(engine, "Comment")
        {
        }

        public override void ConfigurePrototype(InteropPrototype prototype)
        {
            AddInstanceProperty(prototype, "author", self => self.Comment.Author, (self, value) => self.Comment.Author = value.ConvertToString());
            AddInstanceProperty(prototype, "value", self => (JsValue)self.Comment.String?.String, (self, value) => self.SetValue(value));
            AddInstanceProperty(prototype, "visible", self => self.Comment.Visible, (self, value) => self.Comment.Visible = value.ConvertToBoolean().GetValueOrDefault());
        }

        public override CommentInstance NewInstance(JsValue[] arguments)
        {
            return new CommentInstance(Engine);
        }
    }
}
