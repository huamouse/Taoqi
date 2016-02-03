using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Taoqi
{
    public class SMS
    {
        private CCPRestSDK.CCPRestSDK api = new CCPRestSDK.CCPRestSDK();
        public bool IsInit { get; set; }    // 初始化状态
        public string ErrorMessage { get; set; }    // 错误信息

        public SMS()
        {
            IsInit = false;

            //ip格式如下，不带https://
            IsInit = api.init("app.cloopen.com", "8883");
            api.setAccount("aaf98f894fa5766f014fa6d8b7a4023a", "8da7aad37dbb47819b9b3633a5f65d3b");
            api.setAppId("8a48b5514fa577af014fa6e367cb064f");
        }

        /// <summary>
        /// 发送模板短信
        /// </summary>
        /// <param name="to">短信接收端手机号码集合，用英文逗号分开，每批发送的手机号数量不得超过100个</param>
        /// <param name="templateId">模板Id</param>
        /// <param name="data">可选字段 内容数据，用于替换模板中{序号}</param>
        /// <returns></returns>
        public bool SendTemplate(string to, string templateId, string[] data)
        {
            if (IsInit)  // 检查REST（Web Api）
            {
                // 校验手机号码
                Regex moblieReg = new Regex("1[3|5|7|8|][0-9]{9}");
                if (moblieReg.IsMatch(to))
                {
                    try
                    {
                        // 发送短信验证码
                        object statusMsg;
                        Dictionary<string, object> retData = api.SendTemplateSMS(to, templateId, data);

                        retData.TryGetValue("statusMsg", out statusMsg);
                        ErrorMessage = statusMsg.ToString();

                        if (statusMsg.ToString().Equals("成功")) return true;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
                else
                    ErrorMessage = "手机号码非法";
            }
            else
                ErrorMessage = "初始化失败"; 

            return false;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="to">短信接收端手机号码集合，用英文逗号分开，每批发送的手机号数量不得超过100个</param>
        /// <param name="body">短信内容</param>
        /// <returns></returns>
        public bool Send(string to, string body)
        {
            if (IsInit)  // 检查REST（Web Api）
            {
                // 校验手机号码
                Regex moblieReg = new Regex("1[3|5|7|8|][0-9]{9}");
                if (moblieReg.IsMatch(to))
                {
                    try
                    {
                        // 发送短信验证码
                        object statusMsg;
                        Dictionary<string, object> retData = api.SendSMS(to, body);

                        retData.TryGetValue("statusMsg", out statusMsg);
                        ErrorMessage = statusMsg.ToString();

                        if (statusMsg.ToString().Equals("成功")) return true;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                }
                else
                    ErrorMessage = "手机号码非法";
            }
            else
                ErrorMessage = "初始化失败";

            return false;
        }

        // 短信验证码
        public bool SendCode(string to)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            string smsCode = random.Next(100000, 1000000).ToString();
            HttpContext.Current.Session["smsCode"] = smsCode;

            return SendTemplate(to, "49467", new[] { smsCode, "10" });
        }

        // 账户审核提醒
        public bool RegisterVerify(string to, string body)
        {
            return SendTemplate(to, "50674", new[] { body });
        }
    }
}
