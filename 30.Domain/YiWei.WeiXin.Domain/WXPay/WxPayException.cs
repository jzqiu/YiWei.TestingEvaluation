using System;

namespace YiWei.WeiXin.Domain.WXPay
{
    public class WxPayException : Exception
    {
        public WxPayException(string msg)
            : base(msg)
        {

        }
    }
}
