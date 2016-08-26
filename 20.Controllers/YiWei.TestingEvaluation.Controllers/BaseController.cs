using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using YiWei.Utility;

namespace YiWei.TestingEvaluation.Controllers
{
    public class BaseController : Controller
    {
        public string OpenId;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            OpenId = CookieHelper.GetValue("Cookie.OpenId");
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            var ex = filterContext.Exception;
            Log4Helper.Write(ex.ToString(), LogMessageType.Error);
        }
    }
}
