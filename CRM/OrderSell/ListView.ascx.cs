/**********************************************************************************************************************
 * Taoqi is a Customer Relationship Management program created by Taoqi Software, Inc. 
 * Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved.
 * 
 * This program is free software: you can redistribute it and/or modify it under the terms of the 
 * GNU Affero General Public License as published by the Free Software Foundation, either version 3 
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * See the GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>. 
 * 
 * You can contact Taoqi Software, Inc. at email address support@Taoqi.com. 
 * 
 * In accordance with Section 7(b) of the GNU Affero General Public License version 3, 
 * the Appropriate Legal Notices must display the following words on all interactive user interfaces: 
 * "Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved."
 *********************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.OrderSell
{
    /// <summary>
    ///		Summary description for ListView.
    /// </summary>
    public class ListView : SplendidControl
    {

        protected UniqueStringCollection arrSelectFields;
        protected DataView vwMain;
        protected SplendidGrid grdMain;
        protected Label lblError;
 
        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle("我的订单");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0) && (Security.isSeller == 1);
            if (!this.Visible) return;

            string scope = Request.QueryString["scope"];

            try
            {
                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        grdMain.OrderByClause("DATE_ENTERED", "desc");
                        
                        cmd.CommandText = " from vwTQOrder_List" + ControlChars.CrLf
                                        + " where SellerID = @AccountID" + ControlChars.CrLf;
                        Sql.AddParameter(cmd, "@AccountID", Security.AccountID);

                        if (scope == "1")
                            Sql.AppendParameter(cmd, 0, "C_Status",false);
                        else if (scope == "2")
                            cmd.CommandText += " and (C_Status = 1 or C_Status =2 or C_Status =3)" + ControlChars.CrLf;
                        else if (scope == "3")
                            cmd.CommandText += " and (C_Status > 3)" + ControlChars.CrLf;

                        if (grdMain.AllowCustomPaging)
                        {
                            cmd.CommandText = "select count(*)" + ControlChars.CrLf
                                            + cmd.CommandText;

                            if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
                            {
                                grdMain.VirtualItemCount = Sql.ToInteger(cmd.ExecuteScalar());
                            }
                        }
                        else
                        {
                            cmd.CommandText = "select " + Sql.FormatSelectFields(arrSelectFields)
                                            + cmd.CommandText
                                            + grdMain.OrderByClause();

                            if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
                            {
                                using (DbDataAdapter da = dbf.CreateDataAdapter())
                                {
                                    ((IDbDataAdapter)da).SelectCommand = cmd;
                                    using (DataTable dt = new DataTable())
                                    {
                                        da.Fill(dt);
                                        this.ApplyGridViewRules(m_sMODULE + "." + LayoutListView, dt);

                                        vwMain = dt.DefaultView;
                                        grdMain.DataSource = vwMain;
                                    }
                                }
                            }
                        }
                    }
                }

                if (!IsPostBack)
                {
                    grdMain.DataBind();
                }
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                lblError.Text = ex.Message;
            }

            //执行删除以及收货的操作
            try
            {
                if (Request.Form["Delete"] != null)
                {
                    Guid id = Guid.Parse(Convert.ToString(Request.Form["Delete"]));
                    SqlProcs.spTQOrder_Delete(id);

                    Response.Redirect("default.aspx");
                }
                else if (Request.Form["Received"] != null)
                {
                    Guid id = Guid.Parse(Convert.ToString(Request.Form["Received"]));
                    SqlProcs.spTQOrder_ModifyStatus(id, 4);

                    Response.Redirect("default.aspx");
                }
            }
            catch
            {
                Response.Redirect("~/Order");
            }

        }

        protected string ActionForStatus(string TradingStatus,string orderId)
        {
            string resultHtml = "";
            switch (TradingStatus)
            {
                case "0":
                case "1":
                case "2":
                    resultHtml = string.Format("<a href=\"view.aspx?id={0}\" class=\"btn btnOrange\">订单详情</a>", orderId);
                    break;
                //case "3":
                //    resultHtml = string.Format("<a href=\"estimate.aspx?id={0}\" class=\"btn btnOrange\">评价</a>", orderId);
                //    break;
            }
            
            return resultHtml;
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
            // 11/26/2005   Add fields early so that sort events will get called. 
            m_sMODULE = "TQOrderSell";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("C_Status");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
        }

        #endregion
    }
}


