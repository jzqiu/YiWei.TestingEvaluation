using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace YiWei.TestingEvaluation.Controllers
{
    public class MyController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BaseInfo()
        {
            return Content(OpenId);
            //return View();
        }

        //[HttpPost]
        //public ActionResult BaseInfo()
        //{
            
        //}
    }
}
