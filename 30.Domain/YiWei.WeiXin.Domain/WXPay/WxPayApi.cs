using System;
using System.Web;
using YiWei.Utility;

namespace YiWei.WeiXin.Domain.WXPay
{
    public class WxPayApi
    {
        /**
        *    
        * 查询订单
        * @param WxPayData inputObj 提交给查询订单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回订单查询结果，其他抛异常
        */
        public static WxPayData OrderQuery(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/pay/orderquery";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no") && !inputObj.IsSet("transaction_id"))
            {
                throw new WxPayException("订单查询接口中，out_trade_no、transaction_id至少填一个！");
            }

            inputObj.SetValue("appid", Config.AppId);//公众账号ID
            inputObj.SetValue("mch_id", Config.WxMchId);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            string response = NetHelper.HttpPost(url, xml, SerializationType.Xml);
            //将xml格式的数据转化为对象以返回
            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }

        /**
        * 
        * 统一下单
        * @param WxPaydata inputObj 提交给统一下单API的参数
        * @param int timeOut 超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData UnifiedOrder(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("缺少统一支付接口必填参数out_trade_no！");
            }
            else if (!inputObj.IsSet("body"))
            {
                throw new WxPayException("缺少统一支付接口必填参数body！");
            }
            else if (!inputObj.IsSet("total_fee"))
            {
                throw new WxPayException("缺少统一支付接口必填参数total_fee！");
            }
            else if (!inputObj.IsSet("trade_type"))
            {
                throw new WxPayException("缺少统一支付接口必填参数trade_type！");
            }

            //关联参数
            if (inputObj.GetValue("trade_type").ToString() == "JSAPI" && !inputObj.IsSet("openid"))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数openid！trade_type为JSAPI时，openid为必填参数！");
            }
            if (inputObj.GetValue("trade_type").ToString() == "NATIVE" && !inputObj.IsSet("product_id"))
            {
                throw new WxPayException("统一支付接口中，缺少必填参数product_id！trade_type为JSAPI时，product_id为必填参数！");
            }

            //异步通知url未设置，则使用配置文件中的url
            if (!inputObj.IsSet("notify_url"))
            {
                inputObj.SetValue("notify_url", Config.WxNotifyUrl);//异步通知url
            }

            inputObj.SetValue("appid", Config.AppId);//公众账号ID
            inputObj.SetValue("mch_id", Config.WxMchId);//商户号
            inputObj.SetValue("spbill_create_ip", GetRealClientIP());//终端ip	  	    
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串

            //签名
            inputObj.SetValue("sign", inputObj.MakeSign());
            string xml = inputObj.ToXml();

            string response = NetHelper.HttpPost(url, xml, SerializationType.Xml);
            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }


        /**
        * 
        * 关闭订单
        * @param WxPayData inputObj 提交给关闭订单API的参数
        * @param int timeOut 接口超时时间
        * @throws WxPayException
        * @return 成功时返回，其他抛异常
        */
        public static WxPayData CloseOrder(WxPayData inputObj)
        {
            string url = "https://api.mch.weixin.qq.com/pay/closeorder";
            //检测必填参数
            if (!inputObj.IsSet("out_trade_no"))
            {
                throw new WxPayException("关闭订单接口中，out_trade_no必填！");
            }

            inputObj.SetValue("appid", Config.AppId);//公众账号ID
            inputObj.SetValue("mch_id", Config.WxMchId);//商户号
            inputObj.SetValue("nonce_str", GenerateNonceStr());//随机字符串		
            inputObj.SetValue("sign", inputObj.MakeSign());//签名
            string xml = inputObj.ToXml();

            string response = NetHelper.HttpPost(url, xml, SerializationType.Xml);
            WxPayData result = new WxPayData();
            result.FromXml(response);
            return result;
        }


        /**
        * 根据当前系统时间加随机序列来生成订单号
         * @return 订单号
        */
        public static string GenerateOutTradeNo()
        {
            var ran = new Random();
            return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), ran.Next(999));
        }

        /**
        * 生成时间戳，标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数
         * @return 时间戳
        */
        public static string GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /**
        * 生成随机串，随机串包含字母或数字
        * @return 随机串
        */
        public static string GenerateNonceStr()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 获取客户端的真实IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetRealClientIP()
        {
            //可穿透代理服务器
            string strIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //如果取不到真实地址，那就去代理服务器地址
            if (string.IsNullOrEmpty(strIP))
                strIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //如果取不到代理服务器地址，那就直接去UserHostAddress
            if (string.IsNullOrEmpty(strIP))
                strIP = HttpContext.Current.Request.UserHostAddress;

            return string.IsNullOrEmpty(strIP) ? "127.0.0.1" : strIP;
        }
    }
}
