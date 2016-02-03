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

namespace Taoqi.TQCar
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
            SetPageTitle("�����б�");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0) && (Security.isSeller == 1);
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
                        cmd.CommandText = "  from vwTQCar_List" + ControlChars.CrLf
                                        + " where 1 = 1           " + ControlChars.CrLf;
                        
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
            m_sMODULE = "TQCar";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
            grdMain.ItemCommand += grdMain_ItemCommand;
        }
        #endregion

        void grdMain_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                Guid id;
                Guid.TryParse(e.CommandArgument.ToString(), out id);
                SqlProcs.spTQCar_Delete(id);
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}


