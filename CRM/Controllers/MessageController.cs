using System;
using System.Data;
using System.Web.Http;
using System.Collections;
using Taoqi.Common;

namespace Taoqi.Controllers
{
    public class MessageController : ApiController
    {
        [HttpGet]
        public DataTable Unread()
        {
            DataTable dt = new DataTable();

            Hashtable ht = new Hashtable();
            if (Security.isAdmin)
                ht.Add("C_TO", Security.USER_ID);
            else
                ht.Add("C_TO", Security.AccountID);

            var selectfields = "*";
            string strOrderBy = "DATE_ENTERED desc";
            dt = DAL.GetTable("vwTQMessageUnread", ht, 0, strOrderBy, selectfields);

            return dt;
        }

        [HttpGet]
        public DataTable All()
        {
            DataTable dt = new DataTable();

            Hashtable ht = new Hashtable();
            if (Security.isAdmin)
                ht.Add("C_TO", Security.USER_ID);
            else
                ht.Add("C_TO", Security.AccountID);

            var selectfields = "*";
            string strOrderBy = "C_Flag asc, DATE_ENTERED desc";
            dt = DAL.GetTable("vwTQMessage_List", ht, 0, strOrderBy, selectfields);

            return dt;
        }

        [HttpGet]
        public int Read(Guid id)
        {
            try
            {
                SqlProcs.spTQMessageRead(id);
                return 1;
            }
            catch (Exception)
            {
            }

            return -1;
        }
    }
}