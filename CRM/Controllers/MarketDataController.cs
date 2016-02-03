using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Web.Http;
using Taoqi.Common;
using Taoqi.Models;
using System.Linq;

namespace Taoqi.Controllers
{
    public class MarketDataController : ApiController
    {
        private readonly int takeNum = 6;

        [HttpGet]
        public DataSet GetMarketDataInfo()
        {
            var dt = DAL.GetTable("vwTQMarketData_List_ForIndexHTML");

            //增加信息
            dt.Columns.Add("SUBTRACT_Rate", Type.GetType("System.String"));
            //dt.Columns.Add("C_Category_Name", Type.GetType("System.String"));

            foreach (DataRow row in ((DataTable)dt).Rows)
            {
                double int_SUBTRACT = double.Parse(string.IsNullOrEmpty(row["SUBTRACT"].ToString()) ? "0" : row["SUBTRACT"].ToString());
                double int_C_GuidePrice = double.Parse(row["C_GuidePrice"].ToString());
                double SUBTRACT_Rate = int_SUBTRACT / int_C_GuidePrice;
                row["SUBTRACT_Rate"] = string.Format("{0:0.00%}", SUBTRACT_Rate );

                //row["C_Category_Name"] = Str_ClientType(row["C_CategoryID"].ToString());
            }

            DataSet ds = new DataSet();
            int dt_count = dt.Rows.Count;

            for (int i = 0; i < dt_count; i += takeNum)
            {
                var result = (dt.AsEnumerable()).OrderByDescending(e => e["DATE_ENTERED"])
                                .Skip(i).Take(takeNum);

                ds.Tables.Add(result.CopyToDataTable());
            }

            return ds;
        }

        //protected string Str_ClientType(string Description)
        //{
        //    string clientType = "";

        //    if (Description.Length > 0 && Description[0] == '1')
        //        clientType += "液化工厂、";

        //    if (Description.Length > 1 && Description[1] == '1')
        //        clientType += "接收站、";

        //    if (Description.Length > 2 && Description[2] == '1')
        //        clientType += "车用加气站、";

        //    if (Description.Length > 3 && Description[3] == '1')
        //        clientType += "工业气化站、";

        //    if (Description.Length > 4 && Description[4] == '1')
        //        clientType += "城市燃气、";

        //    if (Description.Length > 5 && Description[5] == '1')
        //        clientType += "物流商、";

        //    if (Description.Length > 6 && Description[6] == '1')
        //        clientType += "贸易商、";

        //    if (!string.IsNullOrEmpty(clientType))
        //        clientType = clientType.Substring(0, clientType.Length - 1);

        //    return clientType;
        //}
    }
}