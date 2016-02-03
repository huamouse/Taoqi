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
    public class ClientController : ApiController
    {
        private int PageSize = 2;

        [HttpGet]
        public DataTable GetClient(Guid id)
        {
             string cache_key = CacheSericeEx.MD5(string.Format("Client_GetClient/{0}",id));
             DataTable dt = CacheSericeEx.GetItem(cache_key) as DataTable;
            if(dt == null)
            {
                Hashtable ht = new Hashtable();
                ht.Add("ID", id);
                string fields = "ID, C_CategoryID, C_ClientName, C_ProvinceID, C_ProvinceName, C_CityID, C_CityName, C_CountyID, C_CountyName, C_Address, FullAddress, "
                    + "C_imgIcon, C_imgClient, C_Ranking, C_Status, "
                    + "C_Attachment1, C_Attachment2, C_Attachment3, C_Attachment4, C_Attachment5, C_Attachment6, C_Attachment7, C_Attachment8, C_Attachment9, C_Attachment10";
                dt = DAL.GetTable("vwTQClient_List", ht, 0, null, fields);
                if (!dt.Columns.Contains("TypeSell")) dt.Columns.Add("TypeSell", System.Type.GetType("System.Boolean"));
                if (!dt.Columns.Contains("TypeBuy")) dt.Columns.Add("TypeBuy", System.Type.GetType("System.Boolean"));

                if (!dt.Columns.Contains("C_EstimateLogisticsAvg")) dt.Columns.Add("C_EstimateLogisticsAvg", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateServiceAvg")) dt.Columns.Add("C_EstimateServiceAvg", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateQualityAvg")) dt.Columns.Add("C_EstimateQualityAvg", System.Type.GetType("System.String"));

                if (!dt.Columns.Contains("More_C_EstimateLogisticsAvg")) dt.Columns.Add("More_C_EstimateLogisticsAvg", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("More_C_EstimateServiceAvg")) dt.Columns.Add("More_C_EstimateServiceAvg", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("More_C_EstimateQualityAvg")) dt.Columns.Add("More_C_EstimateQualityAvg", System.Type.GetType("System.String"));

                if (!dt.Columns.Contains("C_EstimateLogisticsCount")) dt.Columns.Add("C_EstimateLogisticsCount", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateServiceCount")) dt.Columns.Add("C_EstimateServiceCount", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateQualityCount")) dt.Columns.Add("C_EstimateQualityCount", System.Type.GetType("System.String"));

                if (!dt.Columns.Contains("C_EstimateLogisticsAvg_1")) dt.Columns.Add("C_EstimateLogisticsAvg_1", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateLogisticsAvg_2")) dt.Columns.Add("C_EstimateLogisticsAvg_2", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateLogisticsAvg_3")) dt.Columns.Add("C_EstimateLogisticsAvg_3", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateLogisticsAvg_4")) dt.Columns.Add("C_EstimateLogisticsAvg_4", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateLogisticsAvg_5")) dt.Columns.Add("C_EstimateLogisticsAvg_5", System.Type.GetType("System.String"));

                if (!dt.Columns.Contains("C_EstimateServiceAvg_1")) dt.Columns.Add("C_EstimateServiceAvg_1", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateServiceAvg_2")) dt.Columns.Add("C_EstimateServiceAvg_2", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateServiceAvg_3")) dt.Columns.Add("C_EstimateServiceAvg_3", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateServiceAvg_4")) dt.Columns.Add("C_EstimateServiceAvg_4", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateServiceAvg_5")) dt.Columns.Add("C_EstimateServiceAvg_5", System.Type.GetType("System.String"));

                if (!dt.Columns.Contains("C_EstimateQualityAvg_1")) dt.Columns.Add("C_EstimateQualityAvg_1", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateQualityAvg_2")) dt.Columns.Add("C_EstimateQualityAvg_2", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateQualityAvg_3")) dt.Columns.Add("C_EstimateQualityAvg_3", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateQualityAvg_4")) dt.Columns.Add("C_EstimateQualityAvg_4", System.Type.GetType("System.String"));
                if (!dt.Columns.Contains("C_EstimateQualityAvg_5")) dt.Columns.Add("C_EstimateQualityAvg_5", System.Type.GetType("System.String"));

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    string category = row["C_CategoryID"].ToString();
                    row["TypeSell"] = false;
                    row["TypeBuy"] = false;
                    if (category.Length > 0 && category[0] == '1') row["TypeSell"] = true;
                    if (category.Length > 1 && category[1] == '1') row["TypeBuy"] = true;

                    //string uploadURL = WebConfigurationManager.AppSettings["UploadPath_image"].Replace("~", HttpContext.Current.Request.ApplicationPath);
                    //string[] columns = new string[] { "C_Attachment1", "C_Attachment2", "C_Attachment3", "C_Attachment4", "C_Attachment5", "C_Attachment6", 
                    //                                  "C_Attachment7", "C_Attachment8", "C_Attachment9", "C_Attachment10", "C_imgIcon", "C_imgClient" };
                    //foreach (string imgColumn in columns)
                    //{
                    //    if (!string.IsNullOrEmpty(row[imgColumn].ToString()))
                    //        row[imgColumn] = uploadURL + row[imgColumn].ToString();
                    //    else
                    //        row[imgColumn] = "NoImage.png";
                    //}

                    int total_C_EstimateLogisticsAvg = 0;
                    int total_C_EstimateServiceAvg = 0;
                    int total_C_EstimateQualityAvg = 0;
                    int total_C_DriverServiceAvg = 0;

                    int C_EstimateLogisticsAvg = 0;
                    int C_EstimateServiceAvg = 0;
                    int C_EstimateQualityAvg = 0;
                    int C_DriverServiceAvg = 0;

                    int C_EstimateLogisticsCount = 0;
                    int C_EstimateServiceCount = 0;
                    int C_EstimateQualityCount = 0;
                    int C_DriverServiceCount = 0;


                    //各个星级的数据统计
                    int C_EstimateLogisticsCount_1 = 0;
                    int C_EstimateLogisticsCount_2 = 0;
                    int C_EstimateLogisticsCount_3 = 0;
                    int C_EstimateLogisticsCount_4 = 0;
                    int C_EstimateLogisticsCount_5 = 0;

                    int C_EstimateServiceCount_1 = 0;
                    int C_EstimateServiceCount_2 = 0;
                    int C_EstimateServiceCount_3 = 0;
                    int C_EstimateServiceCount_4 = 0;
                    int C_EstimateServiceCount_5 = 0;

                    int C_EstimateQualityCount_1 = 0;
                    int C_EstimateQualityCount_2 = 0;
                    int C_EstimateQualityCount_3 = 0;
                    int C_EstimateQualityCount_4 = 0;
                    int C_EstimateQualityCount_5 = 0;



                    DbProviderFactory dbf = DbProviderFactories.GetFactory();
                    using (IDbConnection con = dbf.CreateConnection())
                    {
                        con.Open();
                        using (IDbCommand cmd = con.CreateCommand())
                        {
                            //取个人物流到岸及时率
                            cmd.CommandText = "SELECT ISNULL(avg(C_EstimateLogistics),0) as avgNum ,count(1) as CountNum " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                               "WHERE a.SellerClientID = @Client_ID AND C_EstimateLogistics > 0 ";
                            Sql.AddParameter(cmd, "@Client_ID", id);

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    if (OrderEstimateDetail.Rows.Count > 0)
                                    {
                                        C_EstimateLogisticsAvg = int.Parse(OrderEstimateDetail.Rows[0]["avgNum"].ToString());
                                        C_EstimateLogisticsCount = int.Parse(OrderEstimateDetail.Rows[0]["CountNum"].ToString());
                                    }
                                }
                            }


                            //取个人买家服务态度
                            cmd.CommandText = "SELECT ISNULL(avg(C_EstimateService),0) as avgNum ,count(1) as CountNum " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                               "WHERE a.SellerClientID = @Client_ID AND C_EstimateService > 0 ";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    if (OrderEstimateDetail.Rows.Count > 0)
                                    {
                                        C_EstimateServiceAvg = int.Parse(OrderEstimateDetail.Rows[0]["avgNum"].ToString());
                                        C_EstimateServiceCount = int.Parse(OrderEstimateDetail.Rows[0]["CountNum"].ToString());
                                    }
                                }
                            }


                            //取个人气质与描述相符
                            cmd.CommandText = "SELECT ISNULL(avg(C_EstimateQuality),0) as avgNum ,count(1) as CountNum " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                               "WHERE a.SellerClientID = @Client_ID AND C_EstimateQuality > 0 ";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    if (OrderEstimateDetail.Rows.Count > 0)
                                    {
                                        C_EstimateQualityAvg = int.Parse(OrderEstimateDetail.Rows[0]["avgNum"].ToString());
                                        C_EstimateQualityCount = int.Parse(OrderEstimateDetail.Rows[0]["CountNum"].ToString());
                                    }
                                }
                            }



                            //取全部物流到岸及时率
                            cmd.CommandText = "SELECT ISNULL(avg(C_EstimateLogistics),0) as countNum " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                               "WHERE C_EstimateLogistics > 0 ";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    if (OrderEstimateDetail.Rows.Count > 0)
                                    {
                                        total_C_EstimateLogisticsAvg = int.Parse(OrderEstimateDetail.Rows[0]["countNum"].ToString());
                                    }
                                }
                            }


                            //取全部买家服务态度
                            cmd.CommandText = "SELECT ISNULL(avg(C_EstimateService),0) as countNum " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                               "WHERE C_EstimateService > 0 ";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    if (OrderEstimateDetail.Rows.Count > 0)
                                    {
                                        total_C_EstimateServiceAvg = int.Parse(OrderEstimateDetail.Rows[0]["countNum"].ToString());
                                    }
                                }
                            }


                            //取全部气质与描述相符
                            cmd.CommandText = "SELECT ISNULL(avg(C_EstimateQuality),0) as countNum " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                               "WHERE C_EstimateQuality > 0 ";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    if (OrderEstimateDetail.Rows.Count > 0)
                                    {
                                        total_C_EstimateQualityAvg = int.Parse(OrderEstimateDetail.Rows[0]["countNum"].ToString());
                                    }
                                }
                            }








                            //取个人物流到岸及时率各星级评定统计
                            cmd.CommandText = "SELECT count(ID) as countNum, C_EstimateLogistics " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                                "WHERE a.SellerClientID = @Client_ID " +
                                               "GROUP BY C_EstimateLogistics";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    foreach (DataRow tempRow in OrderEstimateDetail.Rows)
                                    {
                                        switch (tempRow["C_EstimateLogistics"].ToString())
                                        {
                                            case "1": C_EstimateLogisticsCount_1 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "2": C_EstimateLogisticsCount_2 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "3": C_EstimateLogisticsCount_3 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "4": C_EstimateLogisticsCount_4 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "5": C_EstimateLogisticsCount_5 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                        }
                                    }
                                }
                            }


                            //取个人买家服务态度各星级评定统计
                            cmd.CommandText = "SELECT count(ID) as countNum, C_EstimateService " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                                "WHERE a.SellerClientID = @Client_ID " +
                                               "GROUP BY C_EstimateService";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    foreach (DataRow tempRow in OrderEstimateDetail.Rows)
                                    {
                                        switch (tempRow["C_EstimateService"].ToString())
                                        {
                                            case "1": C_EstimateLogisticsCount_1 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "2": C_EstimateLogisticsCount_2 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "3": C_EstimateLogisticsCount_3 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "4": C_EstimateLogisticsCount_4 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "5": C_EstimateLogisticsCount_5 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                        }
                                    }
                                }
                            }


                            //取个人全部气质与描述相符各星级评定统计
                            cmd.CommandText = "SELECT count(ID) as countNum, C_EstimateQuality " +
                                                "FROM vwTQOrderEstimate_Edit as a " +
                                                "WHERE a.SellerClientID = @Client_ID " +
                                               "GROUP BY C_EstimateQuality";

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable OrderEstimateDetail = new DataTable())
                                {
                                    da.Fill(OrderEstimateDetail);

                                    foreach (DataRow tempRow in OrderEstimateDetail.Rows)
                                    {
                                        switch (tempRow["C_EstimateQuality"].ToString())
                                        {
                                            case "1": C_EstimateLogisticsCount_1 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "2": C_EstimateLogisticsCount_2 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "3": C_EstimateLogisticsCount_3 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "4": C_EstimateLogisticsCount_4 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                            case "5": C_EstimateLogisticsCount_5 = int.Parse(tempRow["countNum"].ToString());
                                                break;
                                        }
                                    }
                                }
                            }

                        }
                    }

                    row["C_EstimateLogisticsAvg"] = C_EstimateLogisticsAvg.ToString();
                    row["C_EstimateServiceAvg"] = C_EstimateServiceAvg.ToString();
                    row["C_EstimateQualityAvg"] = C_EstimateQualityAvg.ToString();


                    row["C_EstimateLogisticsCount"] = C_EstimateLogisticsCount.ToString();
                    row["C_EstimateServiceCount"] = C_EstimateServiceCount.ToString();
                    row["C_EstimateQualityCount"] = C_EstimateQualityCount.ToString();


                    row["More_C_EstimateLogisticsAvg"] = C_EstimateLogisticsAvg == 0 ? "0.00%" : string.Format("{0:P}", (C_EstimateLogisticsAvg - total_C_EstimateLogisticsAvg) / total_C_EstimateLogisticsAvg);
                    row["More_C_EstimateServiceAvg"] = C_EstimateServiceAvg == 0 ? "0.00%" : string.Format("{0:P}", (C_EstimateServiceAvg - total_C_EstimateServiceAvg) / total_C_EstimateServiceAvg);
                    row["More_C_EstimateQualityAvg"] = C_EstimateQualityAvg == 0 ? "0.00%" : string.Format("{0:P}", (C_EstimateQualityAvg - total_C_EstimateQualityAvg) / total_C_EstimateQualityAvg);


                    row["C_EstimateLogisticsAvg_1"] = C_EstimateLogisticsCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateLogisticsCount_1 / C_EstimateLogisticsCount);
                    row["C_EstimateLogisticsAvg_2"] = C_EstimateLogisticsCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateLogisticsCount_2 / C_EstimateLogisticsCount);
                    row["C_EstimateLogisticsAvg_3"] = C_EstimateLogisticsCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateLogisticsCount_3 / C_EstimateLogisticsCount);
                    row["C_EstimateLogisticsAvg_4"] = C_EstimateLogisticsCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateLogisticsCount_4 / C_EstimateLogisticsCount);
                    row["C_EstimateLogisticsAvg_5"] = C_EstimateLogisticsCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateLogisticsCount_5 / C_EstimateLogisticsCount);

                    row["C_EstimateServiceAvg_1"] = C_EstimateServiceCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateServiceCount_1 / C_EstimateServiceCount);
                    row["C_EstimateServiceAvg_2"] = C_EstimateServiceCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateServiceCount_2 / C_EstimateServiceCount);
                    row["C_EstimateServiceAvg_3"] = C_EstimateServiceCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateServiceCount_3 / C_EstimateServiceCount);
                    row["C_EstimateServiceAvg_4"] = C_EstimateServiceCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateServiceCount_4 / C_EstimateServiceCount);
                    row["C_EstimateServiceAvg_5"] = C_EstimateServiceCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateServiceCount_5 / C_EstimateServiceCount);

                    row["C_EstimateQualityAvg_1"] = C_EstimateQualityCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateQualityCount_1 / C_EstimateQualityCount);
                    row["C_EstimateQualityAvg_2"] = C_EstimateQualityCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateQualityCount_2 / C_EstimateQualityCount);
                    row["C_EstimateQualityAvg_3"] = C_EstimateQualityCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateQualityCount_3 / C_EstimateQualityCount);
                    row["C_EstimateQualityAvg_4"] = C_EstimateQualityCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateQualityCount_4 / C_EstimateQualityCount);
                    row["C_EstimateQualityAvg_5"] = C_EstimateQualityCount == 0 ? "0.00%" : string.Format("{0:P}", C_EstimateQualityCount_5 / C_EstimateQualityCount);


                    CacheSericeEx.SetItem(cache_key, dt, 86400);//缓存一天
                }
            }
            

            return dt;
        }

        [HttpGet]
        public Hashtable Client_GetAllEstimateDetail(
            string clientId = "",
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
                        cmd.CommandText = "SELECT * FROM (SELECT *,ROW_NUMBER() OVER(ORDER BY a.C_OrderDetailID asc,a.BuyerClientID asc,a.DATE_ENTERED asc) AS Number " +
                                            "FROM vwTQOrderEstimate_Edit as a " +
                                           "WHERE a.SellerClientID = @Client_ID " +
                                           ") as TmpTable WHERE Number between @PreNum and @NextNum";
                        Sql.AddParameter(cmd, "@Client_ID", clientId);
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
                        cmd.CommandText = "SELECT count(1) as AllEstimateCount FROM vwTQOrderEstimate_Edit as a " +
                                          "WHERE a.SellerClientID = @Client_ID ";

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