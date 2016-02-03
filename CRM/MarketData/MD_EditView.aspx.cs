using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.SessionState;
using Taoqi.Controllers;
using System.Data;

namespace Taoqi.Users
{
    public partial class MD_EditView : SplendidPage
    {
        protected Guid TQMarketDataID;

        protected Guid ProductID;

        protected string C_GasSourceName;
        protected int C_GuidePrice;

        protected void Page_Load(object sender, EventArgs e)
        {
            TQMarketDataID = string.IsNullOrEmpty(Request.QueryString["id"]) ? Guid.Empty : Guid.Parse(Request.QueryString["id"]);

            if (IsPostBack)
            {
                if(!string.IsNullOrEmpty(Request.Form["btn_save"]))
                {
                    int breakReason = 0;

                    if (int.TryParse(TXT_C_GuidePrice.Value, out C_GuidePrice) && Guid.TryParse(HDNC_ProductID.Value, out ProductID))
                    {
                        SqlProcs.spTQMarketData_Update(
                            ref TQMarketDataID,
                            ProductID,
                            C_GuidePrice,
                            ref breakReason
                            );

                        if (breakReason == 1)
                        {
                            lbl_error.InnerText = "气源地已存在，请检查。";
                        }
                        else if (breakReason == 0)
                        {
                            Response.Redirect(string.Format("~/MarketData/view.aspx?id={0}", TQMarketDataID));
                        }
                    }
                    else
                        lbl_error.InnerText = "您提交的值不正确，请检查。";
                }
                else if(!string.IsNullOrEmpty(Request.Form["btn_cancel"]))
                {
                    if (TQMarketDataID != Guid.Empty)
                        Response.Redirect(string.Format("~/MarketData/view.aspx?id={0}", TQMarketDataID));
                    else
                        Response.Redirect("~/MarketData/");
                }
            }
            else
            {
                if (TQMarketDataID != Guid.Empty)
                {
                    AccountInformation();
                }
            }
        }


        //从数据库查数据
        private void AccountInformation()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM vwTQMarketData_List " +
                                        "WHERE ID = @ID";
                    Sql.AddParameter(cmd, "@ID", TQMarketDataID);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            //取数据,并且操作数据，反映到页面
                            DataRow row = dt.Rows[0];

                            C_GuidePrice = int.Parse(row["C_GuidePrice"].ToString());
                            C_GasSourceName = row["C_GasSourceName"].ToString();
                            ProductID = Guid.Parse(row["C_ProductID"].ToString());

                            //如果是GET请求需要初始化填框为客户信息
                            showInformation();

                        }
                    }

                }
            }
        }


        protected void showInformation()
        {
            HDNC_ProductID.Value = ProductID.ToString();
            TXT_C_GuidePrice.Value = C_GuidePrice.ToString();
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

        #endregion Web Form Designer generated code
    }
}