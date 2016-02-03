using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Taoqi._code;
using Taoqi.Common;
using Taoqi.Models;

namespace Taoqi.Controllers
{
    public class ClientAddressController : ApiController
    {

        [HttpGet]
        public DataTable GetList(int total = 0)
        {
            var selectfields = "*";

            Hashtable ht = new Hashtable();
            ht.Add("CREATED_BY",Security.AccountID);

            var dt = DAL.GetTable("vwTQClientAddress_List", ht, total, "DATE_ENTERED desc", selectfields);

            return dt;
        }

        [HttpGet]
        public DataTable GetByID(Guid id)
        {
            var selectfields = "*";

            Hashtable ht = new Hashtable();
            ht.Add("CREATED_BY", Security.AccountID);
            ht.Add("ID", id);

            var dt = DAL.GetTable("vwTQClientAddress_List", ht, 0, "DATE_ENTERED desc", selectfields);

            return dt;
        }

        [HttpPost]
        public string Add([FromBody]ClientAddress clientAddress)
        {
            try
            {
                Guid id = clientAddress.ID;
                Guid C_ClientID;
                Guid.TryParse(Security.UserClientID, out C_ClientID);

                SqlProcs.spTQClientAddress_Update(ref id, C_ClientID, clientAddress.C_StationName, 
                    clientAddress.C_StationShortName, 
                    clientAddress.C_StationCapacity, 
                    clientAddress.C_CountyID, clientAddress.C_Address, 
                    clientAddress.C_ContactName, clientAddress.C_Tel,
                    clientAddress.C_BaiduPosition, clientAddress.C_UserType, 
                    clientAddress.C_DailyConsumption1, clientAddress.C_DailyConsumption2);
               
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}