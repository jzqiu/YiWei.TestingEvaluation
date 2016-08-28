using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using YiWei.Utility;
using YiWei.WeiXin.Domain;

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

        protected string GetOpenIdUrl()
        {
            return AuthorizeUrl.GetOauth2BaseUrl(Request.Url.ToString());
        }

        protected string GetUserInfoUrl()
        {
            return AuthorizeUrl.GetOauth2UserInfoUrl(Request.Url.ToString());
        }

        protected string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ran.Next(999));
        }
    }
}
