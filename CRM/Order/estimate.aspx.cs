using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Taoqi.Order
{
    public partial class estimate : SplendidPage
    {
        private DataTable MyOrderEstimate;
        private DataTable TotalOrderEstimate;
        private Guid orderDetailId;
        private string tmp_TQOrderEstimateID;

        protected string MO_SN;
        protected string MO_C_ClientShortName;
        protected string MO_ProductText;
        protected string MO_C_Price;
        protected string MO_DATE_ENTERED;
        protected string MO_StatusName;

        public int AllEstimateCount;

        public string avg_C_EstimateQuality = "0.0";
        public int star_avg_C_EstimateQuality;

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (Security.isBuyer == 0)
            {
                if (Security.isSeller == 1)
                    Response.Redirect("../OrderSell/");
                else if (Security.isAdmin)
                    Response.Redirect("../Client/");
                else if (Security.isCompany == 1)
                    Response.Redirect("../Users/ClientInfo.aspx");
                else
                    Response.Redirect("../Users/PersonalInfo.aspx");
            }

            if (!Guid.TryParse(Request["id"], out orderDetailId))
                Response.Redirect("~/Order");
            OrderInformation();
        }

        //从数据库查数据,并反映到页面
        private void OrderInformation()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                //取个人订单信息
                    cmd.CommandText = "SELECT * FROM vwTQOrderDetail_List " +
                                        "WHERE ID = @orderDetailId";
                    Sql.AddParameter(cmd, "@orderDetailId", orderDetailId);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable Order = new DataTable())
                        {
                            da.Fill(Order);

                            //取数据,并且操作数据，反映到页面
                            if (Order!=null&&Order.Rows.Count > 0)
                            {
                                DataRow row = Order.Rows[0];

                                MO_SN = row["SN"].ToString();
                                MO_C_ClientShortName = Sql.ToString(row["Buyer"]);
                                MO_ProductText = Sql.ToString(row["ProductText"]);
                                MO_C_Price = Sql.ToString(row["C_Price"]);
                                MO_DATE_ENTERED = Sql.ToString(row["DATE_ENTERED"]).Substring(0, 10);
                                MO_StatusName = Sql.ToString(row["StatusName"]);
                            }
                            
                        }
                    }

                //取我的评价信息
                    cmd.CommandText = "SELECT * FROM vwTQOrderEstimate_Edit " +
                                        "WHERE C_OrderDetailID = @orderDetailId AND CREATED_BY = @CREATED_BY " +
                                        "ORDER BY DATE_ENTERED asc";
                    Sql.AddParameter(cmd, "@CREATED_BY", Security.AccountID);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (MyOrderEstimate = new DataTable())
                        {
                            da.Fill(MyOrderEstimate);

                            //取数据,并且操作数据，反映到页面
                            if(MyOrderEstimate.Rows.Count != 0)
                            {
                                Estimate_First.Visible = false;
                                Estimate_Add.Visible = true;

                                //foreach (DataRow row in MyOrderEstimate.Rows)
                                //{
                                //    //转换回UTF8字节数组  
                                //    byte[] bytes = Base64ToBytes( row["C_EstimateContext"].ToString() );  
                                //    //转换回字符串  
                                //    row["C_EstimateContext"] = Encoding.UTF8.GetString(bytes);
                                //}

                                Estimate_Add.MyOrderEstimate = MyOrderEstimate;
                            }
                            else
                            {
                                Estimate_First.Visible = true;
                                Estimate_Add.Visible = false;
                            }
                        }
                    }


                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ////取聚合信息(全部记录)
                        //cmd.CommandText = "SELECT TOP(@TopNum) ID,count(1) as AllEstimateCount FROM vwTQOrderEstimate_List " +
                        //                   "WHERE My_C_OrderID = @ORDER_ID " +
                        //                   "GROUP BY ID,DATE_ENTERED " +
                        //                   "ORDER BY DATE_ENTERED DESC";
                        //Sql.AddParameter(cmd, "@TopNum", 2);

                        //((IDbDataAdapter)da).SelectCommand = cmd;
                        //using (TotalOrderEstimate = new DataTable())
                        //{
                        //    da.Fill(TotalOrderEstimate);

                        //    if (TotalOrderEstimate != null && TotalOrderEstimate.Rows.Count > 0)
                        //    {
                        //        DataRow row = TotalOrderEstimate.Rows[0];

                        //        AllEstimateCount = int.Parse(row["AllEstimateCount"].ToString());
                                
                        //        foreach(DataRow EachRow in TotalOrderEstimate.Rows){
                        //            tmp_TQOrderEstimateID += "'" + EachRow["ID"] + "',";
                        //        }
                        //        tmp_TQOrderEstimateID = tmp_TQOrderEstimateID.TrimEnd(',');
                        //    }


                            //取聚合信息（追评中的气质相符平均值）
                            cmd.CommandText = "SELECT ISNULL(avg(Other_C_EstimateQuality),0) as avg_C_EstimateQuality FROM vwTQOrderEstimate_List " +
                                           "WHERE My_OrderDetailId = @orderDetailId AND Other_C_EstimateQuality != -1 ";
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            using (DataTable Avg_AddAllEstimate = new DataTable())
                            {
                                da.Fill(Avg_AddAllEstimate);

                                if (Avg_AddAllEstimate != null && Avg_AddAllEstimate.Rows.Count > 0)
                                {
                                    DataRow row = Avg_AddAllEstimate.Rows[0];

                                    avg_C_EstimateQuality = string.Format("{0:N1}", row["avg_C_EstimateQuality"]);
                                    star_avg_C_EstimateQuality = (int)Math.Round(double.Parse(row["avg_C_EstimateQuality"].ToString()));
                                }
                            }


                        //    //取全部评价的详细评价内容
                        //    if(!string.IsNullOrEmpty(tmp_TQOrderEstimateID))
                        //    {
                        //        cmd.CommandText = string.Format("SELECT * FROM vwTQOrderEstimate_Edit " +
                        //                                        "WHERE ID in ({0}) " +
                        //                                        "ORDER BY DATE_ENTERED asc", tmp_TQOrderEstimateID);
                        //        ((IDbDataAdapter)da).SelectCommand = cmd;
                        //        using (DataTable AllEstimateDetails = new DataTable())
                        //        {
                        //            da.Fill(AllEstimateDetails);

                        //            RPOtherEsitmate.DataSource = AllEstimateDetails;
                        //            RPOtherEsitmate.DataBind();
                        //        }
                        //    }
                            
                        //}
                    }

                }
            }
        }


        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}