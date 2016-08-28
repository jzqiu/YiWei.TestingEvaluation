using System;
using YiWei.TestingEvaluation.VM.WeiXin;

namespace YiWei.WeiXin.Domain
{
    /// <summary>
    /// 微信模板消息
    /// </summary>
    public class TemplateMsgService
    {
        private const string TopColor = "#1caf9a";
        private const string Color="#173177";

        /// <summary>
        /// 订单支付成功通知
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="key1">订单商品</param>
        /// <param name="key2">订单编号</param>
        /// <param name="key3">支付金额</param>
        /// <param name="key4">支付时间</param>
        /// <param name="url">跳转链接</param>
        public static void PaySuccess(string openid, string key1, string key2, string key3, string key4, string url)
        {
            var template = new TemplateMsg
            {
                touser = openid,
                template_id = "BLbaYmzPFrgDMNc-GlTHXQ33Q4OSDn1STBCDvNUNZj0",
                url = url,
                data = new TemplateNotes
                {
                    first = new TemplateNote ("尊敬的用户，您的订单已支付成功"),
                    keyword1 = new TemplateNote(key1),
                    keyword2 = new TemplateNote(key2),
                    keyword3 = new TemplateNote(key3),
                    keyword4 = new TemplateNote(key4),
                    remark = new TemplateNote("感谢您对我们的关注及认可，点击详情可查看。")
                }
            };

            WeiXinService.SendTemplateMsg(template);
        }

        /// <summary>
        /// 乘客支付完成
        /// 支付时间：20160511010101
        /// 支付金额：11.3元
        /// 支付类型：银联支付
        /// 点击查看详细信息
        /// </summary>
        /// <returns></returns>
        public static void PayNotify(string openid, string first, string key1, string key2, string key3)
        {
            var template = new TemplateMsg
            {
                touser = openid,
                template_id = "zLahvh03lGYAgsGsJiGAkV8RxtKp1TNlyxrSO_c4kmE",
                data = new TemplateNotes
                {
                    first = new TemplateNote(first),
                    keyword1 = new TemplateNote(key1),
                    keyword2 = new TemplateNote(key2),
                    keyword3 = new TemplateNote(key3),
                    remark = new TemplateNote("只是提醒，无需操作。")
                }
            };

            WeiXinService.SendTemplateMsg(template);
        }
    }
}
