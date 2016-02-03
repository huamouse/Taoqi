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
using System.Linq;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.InTransitCheck
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
        protected Literal lblListTitle;
        private DataTable dt;
        
        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle("在途气审核");

            this.Visible = (Taoqi.Security.AdminUserAccess(m_sMODULE, "list") >= 0);
            if (!this.Visible)
                return;

            try
            {
                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        grdMain.OrderByClause("DATE_ENTERED", "desc");

                        cmd.CommandText = "  from vwTQTransit_List" + ControlChars.CrLf
                                        + " where 1 = 1           " + ControlChars.CrLf
                                        + " order by C_Status asc, DATE_ENTERED desc";

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
                                            + cmd.CommandText;

                            if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
                            {
                                using (DbDataAdapter da = dbf.CreateDataAdapter())
                                {
                                    ((IDbDataAdapter)da).SelectCommand = cmd;
                                    using (dt = new DataTable())
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

            m_sMODULE = "TQTransitCheck";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("CREATED_BY");
            arrSelectFields.Add("C_Status");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
            grdMain.ItemCommand += grdMain_ItemCommand;
        }
        #endregion


        void grdMain_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            Guid gID = Guid.Empty;
            Guid.TryParse(e.CommandArgument.ToString(), out gID);
            if (gID == Guid.Empty) return;

            if (e.CommandName == "Pass")
            {
                SqlProcs.spTQTransitCheck(gID, Security.USER_ID, 2);
                Msg.TransitAudit(gID);
            }
            else if (e.CommandName == "NoPass")
            {
                SqlProcs.spTQTransitCheck(gID, Security.USER_ID, 1);
                Msg.TransitAudit(gID, false);
            }

            // 检索消息接收者
            //Guid gTO = dt.AsEnumerable().First(r => Guid.Parse(r["ID"].ToString()) == gID).Field<Guid>("CREATED_BY");

            Response.Redirect("default.aspx");
        }
    }
}


