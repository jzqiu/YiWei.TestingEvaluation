using System;

namespace YiWei.WeiXin.Domain.WXPay
{
    public class JsApiPay
    {
        public WxPayData UnifiedOrder(string orderNo, string productName, string productCode, int totalFee, string openId)
        {
            var data = new WxPayData();
            data.SetValue("body", productName);
            data.SetValue("detail", string.Format("{0} {1} {2}元", DateTime.Now.ToString("yyyyMMddHHmm"), productName, (totalFee / 100)));
            data.SetValue("attach", productCode);
            data.SetValue("out_trade_no", orderNo);
            data.SetValue("total_fee", totalFee);
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            data.SetValue("goods_tag", productCode);
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", openId);

            WxPayData result = WxPayApi.UnifiedOrder(data);
            return result;
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public WxPayData GetJsApiParameters(string prepayId)
        {
            var jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", Config.AppId);
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", string.Format("prepay_id={0}",prepayId));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            return jsApiParam;
        }
    }
}
