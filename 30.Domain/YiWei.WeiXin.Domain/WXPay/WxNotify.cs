using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiWei.WeiXin.Domain.WXPay
{
    public class WxNotify
    {
        public static WxPayData GetNotify()
        {
            using (System.IO.Stream s = System.Web.HttpContext.Current.Request.InputStream)
            {
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();

                //转换数据格式并验证签名
                var data = new WxPayData();
                try
                {
                    data.FromXml(builder.ToString());
                }
                catch (WxPayException ex)
                {
                    //若签名错误，则立即返回结果给微信支付后台
                    data.SetValue("return_code", "FAIL");
                    data.SetValue("return_msg", ex.Message);
                }

                return data;
            }
        }
    }
}
