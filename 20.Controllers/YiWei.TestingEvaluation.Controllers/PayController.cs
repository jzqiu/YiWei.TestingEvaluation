using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using YiWei.WeiXin.Domain.WXPay;

namespace YiWei.TestingEvaluation.Controllers
{
    public class PayController : BaseController
    {
        public ActionResult Index()
        {
            string orderNo = GenerateOutTradeNo();
            WxPayData wxOrder = new JsApiPay().UnifiedOrder(orderNo, "测试支付", "test", 10, "oIdbxw8WdYPauF_m1qR-tkSJk644");
            if (wxOrder.GetValue("return_code").ToString().ToUpper() == "SUCCESS"
                && wxOrder.GetValue("result_code").ToString().ToUpper() == "SUCCESS")
            {
                string para = new JsApiPay().GetJsApiParameters(wxOrder.GetValue("prepay_id").ToString());
                ViewBag.Para = para;
            }
            
            return View();
        }

        public string WxNotify()
        {
            WxPayData notify = YiWei.WeiXin.Domain.WXPay.WxNotify.GetNotify();

            //检查支付结果中transaction_id是否存在
            if (!notify.IsSet("transaction_id"))
            {
                var res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                return res.ToXml();
            }
            //再次跟微信查询订单
            WxPayData check = WxPayApi.OrderQuery(notify.GetValue("transaction_id").ToString());
            if (check.GetValue("return_code").ToString().ToUpper() != "SUCCESS" 
                || check.GetValue("result_code").ToString().ToUpper() != "SUCCESS")
            {
                var res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                return res.ToXml();
            }

            decimal totalFee = decimal.Parse(notify.GetValue("total_fee").ToString());
            string openId = notify.GetValue("openid").ToString();
            string orderNo = notify.GetValue("out_trade_no").ToString();
            string code = notify.GetValue("attach").ToString();
            //TODO 更新状态
            //TODO 发送消息

            WxPayData result = new WxPayData();
            result.SetValue("return_code", "SUCCESS");
            result.SetValue("return_msg", "OK");
            return result.ToXml();
        }

        private string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ran.Next(999));
        }
    }
}
