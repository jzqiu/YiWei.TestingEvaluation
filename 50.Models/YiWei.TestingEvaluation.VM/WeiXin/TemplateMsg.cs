using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiWei.TestingEvaluation.VM.WeiXin
{
    public class TemplateMsg
    {
        public string touser;
        public string template_id;
        public string url;
        public string topcolor;
        public TemplateNotes data;

        public TemplateMsg()
        {
            topcolor = "#FF0000";
        }
    }

    public class TemplateNotes
    {
        public TemplateNote first { get; set; }
        public TemplateNote keyword1 { get; set; }
        public TemplateNote keyword2 { get; set; }
        public TemplateNote keyword3 { get; set; }
        public TemplateNote keyword4 { get; set; }
        public TemplateNote remark { get; set; }
    }

    public class TemplateNote
    {
        public string value;
        public string color;

        public TemplateNote(string v, string c = "#173177")
        {
            value = v;
            color = c;
        }
    }
}
