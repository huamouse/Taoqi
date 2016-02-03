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
    public class EstimateController : ApiController
    {
        private int PageSize = 2;


        [HttpGet]
        public Hashtable GetAllEstimateDetail(
            string orderDetailId = "",
            int pageIndex = 1)
        {
            //string cache_key = CacheSericeEx.MD5(string.Format("GetFreeQuotationList/{0}",
            //    C_OrderID
            //    ));

            //Hashtable htResult = CacheSericeEx.GetItem(cache_key) as Hashtable;
            Hashtable htResult = null;

            if (htResult == null)
            {
                htResult = new Hashtable();

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        //取我的评价信息
                        cmd.CommandText = "SELECT * FROM (SELECT e.*,ROW_NUMBER() OVER(ORDER BY e.DATE_ENTERED DESC) AS Number FROM vwTQOrderEstimate_List as a " +
                                            "LEFT OUTER JOIN vwTQOrderEstimate_Edit as e ON e.ID = a.ID " +
                                           "WHERE a.My_OrderDetailId = @orderDetailId " +
                                           ") as TmpTable WHERE Number between @PreNum and @NextNum";
                        Sql.AddParameter(cmd, "@orderDetailId", orderDetailId);
                        Sql.AddParameter(cmd, "@PreNum", (pageIndex - 1) * PageSize + 1);
                        Sql.AddParameter(cmd, "@NextNum", pageIndex * PageSize);

                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            using (DataTable OrderEstimateDetail = new DataTable())
                            {
                                da.Fill(OrderEstimateDetail);
                                htResult.Add("items", OrderEstimateDetail);
                            }
                        }


                        //取聚合信息
                        cmd.CommandText = "SELECT count(1) as AllEstimateCount FROM vwTQOrderEstimate_List as a " +
                                          "WHERE a.My_OrderDetailId = @orderDetailId ";

                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            using (DataTable OrderEstimateDetail = new DataTable())
                            {
                                da.Fill(OrderEstimateDetail);

                                if (OrderEstimateDetail.Rows.Count > 0)
                                {
                                    htResult.Add("total", OrderEstimateDetail.Rows[0]["AllEstimateCount"]);
                                }
                            }
                        }
                    }
                }
                
                //CacheSericeEx.SetItem(cache_key, htResult);
            }

            return htResult;
        }

    }
}