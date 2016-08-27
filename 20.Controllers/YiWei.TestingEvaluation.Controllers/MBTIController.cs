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
            return View();
        }

        public ActionResult Testing(int tid)
        {
            IEnumerable<MBTIQuestion> list = _service.GetQuestion(tid);
            return Json(list, JsonRequestBehavior.AllowGet);
            //return View();
        }

        [HttpPost]
        public ActionResult Testing(MBTIReport report)
        {
            report.OpenId = this.OpenId;
            bool result = _service.AddReport(report);

            return Content("ok");
        }
    }
}
