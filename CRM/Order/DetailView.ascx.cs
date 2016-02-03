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
using System.Collections;
using System.Linq;

namespace Taoqi.Order
{
    /// <summary>
    ///		Summary description for ListView.
    /// </summary>
    public class DetailView : SplendidControl
    {

        protected UniqueStringCollection arrSelectFields;
        protected DataView vwMain;
        protected SplendidGrid grdMain;
        protected Label lblError;

        protected Guid C_ClientID;
        protected string Client_ShortName;
        protected string Client_Address;

        protected Guid C_OrderID;
        protected string C_PlateNumber;
        protected string C_Driver;
        protected string C_Tel;
        protected int C_Tonnage;

        protected UpdatePanel updatePanel;

        protected Guid C_OrderDetailID;

        protected string OrderDetailID { 
            get{
                return ViewState["OrderDetailID"].ToString() ?? "";
            }
            set {
                ViewState["OrderDetailID"] = value;
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Guid.TryParse(Request.QueryString["id"], out C_OrderID))
                Response.Redirect("~/Order");

            SetPageTitle("��ϸ����");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "view") >= 0) && (Security.isBuyer == 1);
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

                        cmd.CommandText = "  from vwTQOrderDetail_List" + ControlChars.CrLf
                                        + " where 1 = 1  AND  C_OrderID =  @C_OrderID      " + ControlChars.CrLf;

                        Sql.AddParameter(cmd, "@C_OrderID", C_OrderID);
                        
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
            
            if (IsPostBack)
            {
                if (Request.Form["Delete"] != null)
                {
                    Guid id = Guid.Parse(Convert.ToString(Request.Form["Delete"]));
                    SqlProcs.spTQOrderDetail_Delete(id);

                    Response.Redirect(Request.RawUrl);
                }
                else if (Request.Form["Confirm"] != null)
                {
                    Guid id = Guid.Parse(Convert.ToString(Request.Form["Confirm"]));
                    SqlProcs.spTQOrderDetail_ChangeStatus(id, 2);
                    Msg.AcceptPrice(id);

                    Response.Redirect(Request.RawUrl);
                }
            }

            OrderInfo();
        }

        private void OrderInfo()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    //ȡ������Ϣ
                    cmd.CommandText = "SELECT * FROM vwTQOrder_List " +
                                        "WHERE ID = @C_OrderID ";

                    Sql.AddParameter(cmd, "@C_OrderID", C_OrderID);
                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            if (dt.Rows.Count != 1)
                                Response.Redirect("~/Order");
                            else
                            {
                                (FindControl("RPOrderInformation") as Repeater).DataSource = dt;
                                (FindControl("RPOrderInformation") as Repeater).DataBind();
                            }
                        }
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
            //ctlSearchView.Command += new CommandEventHandler(Page_Command);
            //ctlExportHeader.Command += new CommandEventHandler(Page_Command);
            // 11/26/2005   Add fields early so that sort events will get called. 
            m_sMODULE = "TQOrderDetailSell";
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


