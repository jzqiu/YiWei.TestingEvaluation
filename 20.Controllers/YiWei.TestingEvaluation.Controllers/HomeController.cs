using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using YiWei.TestingEvaluation.Domain;

namespace YiWei.TestingEvaluation.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("OK");
        }
    }
}
