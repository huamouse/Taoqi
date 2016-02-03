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

namespace Taoqi.TQTransitMy
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
            SetPageTitle("我的在途气");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0) && (Security.isBuyer == 1);
            if (!this.Visible)
                return;

            try
            {
                // 执行操作
                if (Request.Form["Delete"] != null)
                {
                    Guid id = Guid.Parse(Request.Form["Delete"].ToString());
                    SqlProcs.spTQTransitMy_Delete(id);
                    Response.Redirect("default.aspx");
                }
                else if (Request.Form["Finish"] != null)
                {
                    Guid id = Guid.Parse(Request.Form["Finish"].ToString());
                    SqlProcs.spTQTransitMy_Change(id, 4);
                    Response.Redirect("default.aspx");
                }

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        grdMain.OrderByClause("DATE_ENTERED", "desc");

                        cmd.CommandText = "  from vwTQTransitMy_List" + ControlChars.CrLf
                                        + " where 1 = 1" + ControlChars.CrLf;

                        if (!Security.isAdmin)
                        {
                            cmd.CommandText += " and CREATED_BY = @AccountID" + ControlChars.CrLf;

                            Sql.AddParameter(cmd, "@AccountID", Security.AccountID);
                        }

                        if (grdMain.AllowCustomPaging)
                        {
                            cmd.CommandText = "select count(*)" + ControlChars.CrLf
                                            + cmd.CommandText;

                            if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
                                grdMain.VirtualItemCount = Sql.ToInteger(cmd.ExecuteScalar());
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

                if (!IsPostBack) grdMain.DataBind();
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                lblError.Text = ex.Message;
            }
        }

        protected string ActionFor(object status, object ID, object carID, object landingUrl, object driver, object driverTel)
        {
            string id = ID.ToString();

            switch ((int)status)
            {
                case 0:
                    return "<div class=\"txtGray\">等待确认</div>"
                        + "<button class=\"btnGray1\" name=\"Delete\" value=\"" + id + "\">取消抢购</button>";
                case 1: // 已关闭
                    return "<div class=\"txtGray\">抢购失败</div>"
                        + "<button class=\"btnGray1\" name=\"Delete\" value=\"" + id + "\">删除</button>";
                case 2:
                    return "<a href=\"/map2.html?id=" + carID + "\" class=\"btnOrange\">查看物流</a>";
                case 3:
                    return string.Format("<div class=\"btnOrange\" ng-click=\"viewLanding('{0}', '{1}', '{2}')\">查看水单</div>", landingUrl, driver, driverTel)
                        + "<button class=\"btnOrange\" name=\"Finish\" value=\"" + id + "\">确认收货</button>";
                default:
                    return "";
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
            //ctlSearchView.Command += new CommandEventHandler(Page_Command);
            //ctlExportHeader.Command += new CommandEventHandler(Page_Command);
            // 11/26/2005   Add fields early so that sort events will get called. 
            m_sMODULE = "TQTransitMy";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("C_Status");
            arrSelectFields.Add("C_CarID");
            arrSelectFields.Add("C_LandingUrl");
            arrSelectFields.Add("C_Driver");
            arrSelectFields.Add("C_Tel");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
        }
        #endregion
    }
}


