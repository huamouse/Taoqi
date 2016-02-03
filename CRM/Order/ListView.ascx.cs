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
using Taoqi._controls;

namespace Taoqi.TQOrder
{
    /// <summary>
    ///		Summary description for ListView.
    /// </summary>
    public class ListView : SplendidControl
    {
        protected _controls.SearchView ctlSearchView;
        protected UniqueStringCollection arrSelectFields;
        protected DataView vwMain;
        protected SplendidGrid grdMain;
        protected TextBox txtSN;
        protected TextBox txtCompanyName;
        protected DatePicker dateFrom;
        protected DatePicker dateTo;
        protected Label lblError;
        
        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle("我的订单");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0) && (Security.isBuyer == 1);
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
                                        + " where BuyerID = @AccountID" + ControlChars.CrLf;
                        Sql.AddParameter(cmd, "@AccountID", Security.AccountID);

                        if (!Sql.IsEmptyString(txtSN.Text))
                        {
                            cmd.CommandText += " and SN like @SN" + ControlChars.CrLf;
                            Sql.AddParameter(cmd, "@SN", "%" + txtSN.Text.Trim() + "%");
                        }

                        if (!Sql.IsEmptyString(txtCompanyName.Text))
                        {
                            cmd.CommandText += " and Seller like @Seller" + ControlChars.CrLf;
                            Sql.AddParameter(cmd, "@Seller", "%" + txtCompanyName.Text.Trim() + "%");
                        }

                        if (!Sql.IsEmptyString(dateFrom.DateText))
                        {
                            cmd.CommandText += " and DATE_ENTERED > @dateFrom" + ControlChars.CrLf;
                            Sql.AddParameter(cmd, "@dateFrom", dateFrom.Value);
                        }

                        if (!Sql.IsEmptyString(dateTo.DateText))
                        {
                            cmd.CommandText += " and DATE_ENTERED < @dateTo" + ControlChars.CrLf;
                            Sql.AddParameter(cmd, "@dateTo", dateTo.Value);
                        }

                        if (scope == "1")
                            Sql.AppendParameter(cmd, 0, "C_Status", false);
                        else if (scope == "2")
                            cmd.CommandText += " and (C_Status = 1 or C_Status = 2 or C_Status =3)" + ControlChars.CrLf;
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

                grdMain.DataBind();
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
                    Msg.OrderCancel(id);
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
            string resultHtml = string.Format("<a href=\"view.aspx?id={0}\" class=\"btn btnOrange\">订单详情</a>", orderId);
            int nStatus = int.Parse(TradingStatus);
            switch (nStatus)
            {
                case 0:
                    resultHtml += string.Format("<Button class=\"btn btnSecond btnGray1\" name=\"Delete\" value=\"{0}\">取消订单</Button>", orderId);
                    break;
                case 1:
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
            
            return resultHtml;
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
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
            m_sMODULE = "TQOrder";
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


