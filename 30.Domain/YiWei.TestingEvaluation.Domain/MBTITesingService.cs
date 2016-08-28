using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiWei.TestingEvaluation.Data;
using YiWei.TestingEvaluation.Entity.TT;

namespace YiWei.TestingEvaluation.Domain
{
    public class MBTITesingService
    {
        private readonly MBTITesingData _data;

        public MBTITesingService(MBTITesingData data)
        {
            this._data = data;
        }

        public IEnumerable<MBTIQuestion> GetQuestion(int tesingId)
        {
            IEnumerable<MBTIQuestion> list = _data.QueryQuestion(tesingId);
            return list.OrderBy(q => q.Id);
        }

        public bool AddReport(MBTIReport report)
        {
            report.TypeCode = report.E > report.I ? "E" : "I";
            report.TypeCode += report.S > report.N ? "S" : "N";
            report.TypeCode += report.T > report.F ? "T" : "F";
            report.TypeCode += report.J > report.P ? "J" : "P";

            report.CreateDate = DateTime.Now;
            return _data.InsertReport(report);
        }

        public MBTIReport HadTest(string openId, int testingType)
        {
            IEnumerable<MBTIReport> list = _data.QueryReports(openId, testingType);
            if (list != null && list.Any())
            {
                return list.OrderByDescending(r => r.Id).First();
            }

            return new MBTIReport();
        }

        public MBTIReportText GetReportText(string typeCode)
        {
            IEnumerable<MBTIReportText> list = _data.QueryReportText(typeCode);
            if (list != null && list.Any())
            {
                return list.First();
            }

            return new MBTIReportText();
        }
    }
}
