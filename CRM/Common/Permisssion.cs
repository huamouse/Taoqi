using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

namespace Taoqi
{
    public class Permission
    {
        public static void Init()
        {
            Hashtable ht = new Hashtable();
            ht.Add("C_UserID", Security.USER_ID);
            DataTable dt = DAL.GetTable("vwTQAccount_List", ht, 0, "C_Status desc, CompanyStatus desc");
            SaveSecurity(dt);
        }

        public static void Load(Guid accountID)
        {
            Hashtable ht = new Hashtable();
            ht.Add("ID", accountID);
            DataTable dt = DAL.GetTable("vwTQAccount_List", ht);
            SaveSecurity(dt);
        }

        private static void SaveSecurity(DataTable dt)
        {
            if (dt.Rows.Count == 0) return;

            Security.AccountID = Sql.ToGuid(dt.Rows[0]["ID"]);
            Security.UserType = Sql.ToString(dt.Rows[0]["C_Role"]);
            Security.UserCompany = Sql.ToString(dt.Rows[0]["C_ClientName"]);//全称
            Security.UserClientID = Sql.ToString(dt.Rows[0]["C_ClientID"]);
            Security.CompanyStatus = Sql.ToString(dt.Rows[0]["CompanyStatus"]);
            Security.isCompany = Security.AccountID == Guid.Empty ? 0 : 1;

            if (Sql.ToString(dt.Rows[0]["C_Status"]) == "1" && Sql.ToString(dt.Rows[0]["CompanyStatus"]) == "2")
            {
                Security.isCompanyAdmin = int.Parse(Sql.ToString(dt.Rows[0]["isCompanyAdmin"]));
                Dictionary<string, int> roleList = Taoqi.CustomHelper.CustomHelper.GetRoleArry(Security.UserType);
                Security.isBuyer = roleList["isBuyer"];
                Security.isSeller = roleList["isSeller"];
                Security.isEmployee = roleList["isEmployee"];
                Security.isDriver = roleList["isDriver"];
            }
            else
            {
                Security.isCompanyAdmin = 0;
                Security.isBuyer = 0;
                Security.isSeller = 0;
                Security.isEmployee = 0;
                Security.isDriver = 0;
            }
        }
    }
}