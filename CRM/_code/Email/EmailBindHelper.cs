using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taoqi._code;

namespace Taoqi
{
    /// <summary>
    /// 绑定邮箱帮助类
    /// </summary>
    public class EmailBindHelper
    {
        public static bool SendBindEmail(string toEmailaddress, string strValidCode)
        {
            if (!Sql.IsEmptyString(toEmailaddress))
            {
                 bool IsSucesss=GetEmainSmtp(toEmailaddress, strValidCode).Send();
                 return IsSucesss;

            }
            else
            {
                return false;
            }
         
        }
        private static SMTP GetEmainSmtp(string toEmailaddress, string strValidCode)
        {
            //string strEmailBody=string.Format(Utils.AppSettings[""])
           //  toEmailaddress = "519684597@qq.com";
            return new SMTP(new string[] { toEmailaddress }, Utils.AppSettings["EmailSubject"], strValidCode);

        }
    }
}
