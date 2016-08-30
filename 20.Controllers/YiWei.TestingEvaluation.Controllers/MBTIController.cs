using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using YiWei.TestingEvaluation.Domain;
using YiWei.TestingEvaluation.Entity.TT;

namespace YiWei.TestingEvaluation.Controllers
{
    public class MBTIController : BaseController
    {
        private MBTITesingService _service;

        public MBTIController(MBTITesingService service)
        {
            this._service = service;
        }

        public ActionResult Index(int tid)
        {
            ViewBag.TId = tid;
            return View();
        }

        //[HttpPost]
        //public JsonResult HadTest(int tid)
        //{
        //    MBTIReport report = _service.HadTest(OpenId, tid);
        //    //if (report.Id > 0)
        //    //{
        //    //    report.ProReportRemark = report.CreateDate.ToString("yyyy年MM月dd日 HH时mm分");
        //    //}

        //    return Json(report);
        //}

        public ActionResult Testing(int tid)
        {
            IEnumerable<MBTIQuestion> list = _service.GetQuestion(tid);
            return View(list);
        }

        [HttpPost]
        public ActionResult Testing(string answers)
        {
            var report=new MBTIReport();
            report.OpenId = this.OpenId;
            report.E = answers.Length - answers.Replace("E", "").Length;
            report.I = answers.Length - answers.Replace("I", "").Length;
            report.F = answers.Length - answers.Replace("F", "").Length;
            report.J = answers.Length - answers.Replace("J", "").Length;
            report.N = answers.Length - answers.Replace("N", "").Length;
            report.P = answers.Length - answers.Replace("P", "").Length;
            report.S = answers.Length - answers.Replace("S", "").Length;
            report.T = answers.Length - answers.Replace("T", "").Length;
            bool result = _service.AddReport(report);

            return Content("ok");
        }
    }
}
