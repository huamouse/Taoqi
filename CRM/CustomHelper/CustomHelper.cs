using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Taoqi.CustomHelper
{
    public class CustomHelper
    {

        //将表示用户类型的二进制字符串转换为表示权限的字符串
        public static Dictionary<string, int> GetRoleArry(string userType)
        {
            //第一位采购  第二位 销售  第三位 场站员工  第四位 驾驶员

            Dictionary<string, int> roleList = new Dictionary<string, int>();

            if (userType.Length > 0 && userType[0] == '1')
                roleList.Add("isBuyer", 1);
            else
                roleList.Add("isBuyer", 0);

            if (userType.Length > 1 && userType[1] == '1')
                roleList.Add("isSeller", 1);
            else
                roleList.Add("isSeller", 0);

            if (userType.Length > 2 && userType[2] == '1')
                roleList.Add("isEmployee", 1);
            else
                roleList.Add("isEmployee", 0);

            if (userType.Length > 3 && userType[3] == '1')
                roleList.Add("isDriver", 1);
            else
                roleList.Add("isDriver", 0);

            return roleList;
        }


        //把8位字节数组转换成Base64字符串  
        public static String StringToBase64str(string str)
        {
            try
            {
                //生成UTF8字节数组  
                Byte[] bytes = Encoding.UTF8.GetBytes(str);
                //转换成Base64字符串  
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return null;
            }
        }

        //把Base64字符串转换成8位字节数组  
        public static String Base64strToString(String base64)
        {
            try
            {
                //转换回UTF8字节数组  
                Byte[] bytes = Convert.FromBase64String(base64);  
                //转换回字符串  
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return null;
            }
        }
    }
}