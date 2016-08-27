using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YiWei.Utility;
using YiWei.WeiXin.Domain;

namespace YiWei.TestingEvaluation.Controllers
{
    public class WeiXinController : Controller
    {
        private WeiXinService _service;
        private const string CookieKeyOpenId = "Cookie.OpenId";

        public WeiXinController(WeiXinService service)
        {
            _service = service;
        }

        public ActionResult Oauth2Base(string code = "", int state = 0, string reurl = "")
        {
            if (string.IsNullOrEmpty(code))
            {
                //授权失败
                return Content("授权失败");
            }

            string openId = _service.GetOpenId(code);
            if (!string.IsNullOrWhiteSpace(openId))
            {
                var cookie = new HttpCookie(CookieKeyOpenId, openId);
                cookie.Expires = DateTime.Now.AddMonths(1);
                CookieHelper.Save(cookie);
            }

            return Redirect(reurl);
        }

        public ActionResult Oauth2(string code = "", int state = 0, string reurl = "")
        {
            if (string.IsNullOrEmpty(code))
            {
                //授权失败
                return Content("授权失败");
            }

            var user = _service.GetUserInfo(code);
            if (user != null)
            {
                var cookie = new HttpCookie(CookieKeyOpenId, user.openid);
                cookie.Expires = DateTime.Now.AddMonths(1);
                CookieHelper.Save(cookie);

                //TODO 新用户信息入库
            }

            return Redirect(reurl);
        }
    }
}
