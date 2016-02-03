using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using Taoqi.Models;

namespace Taoqi.Common
{
    public class CacheSericeEx
    {
        public static DataTable GetTerminologyList(string listName)
        {

            HttpApplicationState application = HttpContext.Current.Application;

            if (!application.AllKeys.Contains(listName))
            {

                Hashtable ht = new Hashtable();
                ht.Add("LIST_NAME", listName);

                DataTable dt = DAL.GetTable("vwTERMINOLOGY", ht, 30, "LIST_ORDER asc", "NAME, DISPLAY_NAME");

                application[listName] = dt;
            }


            return application[listName] as DataTable;

        }

        public static object GetItem(string key)
        {
           return HttpRuntime.Cache.Get(key);
        }

        public static void SetItem(string key, object obj, int expire = 30)
        {
            //缓存30s
            HttpRuntime.Cache.Insert(key, obj, null, DateTime.Now.AddSeconds(expire), System.Web.Caching.Cache.NoSlidingExpiration);
        } 

        public static string MD5(string input)
        {
            return Security.HashPassword(input);
            //return FormsAuthentication.HashPasswordForStoringInConfigFile(input,"MD5");
        }
    
    }
}