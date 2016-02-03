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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.TQRewardPoint
{
    /// <summary>
    /// Summary description for DetailView.
    /// </summary>
    public class DetailView : SplendidControl
    {
        protected _controls.ModuleHeader ctlModuleHeader;
        protected _controls.DynamicButtons ctlDynamicButtons;

        protected Guid gID;
        protected HtmlTable tblMain;
        protected Repeater rptShow;
        protected Label lblError;
  
     

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    Response.Redirect("edit.aspx?ID=" + gID.ToString());
                }
                else if (e.CommandName == "Cancel")
                {
                    Response.Redirect("default.aspx");
                }
                else if (e.CommandName == "Delete")
                {
                   
                        SqlProcs.spTQRewardPoint_Delete(gID);
                        Response.Redirect("default.aspx");
                    
                   
                }
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                ctlDynamicButtons.ErrorText = ex.Message;
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle(L10n.Term(".moduleList." + m_sMODULE));
            // 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
            // 03/10/2010   Apply full ACL security rules. 
            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "view") >= 0);
            if (!this.Visible)
            {
                // 03/17/2010   We need to rebind the parent in order to get the error message to display. 
                Parent.DataBind();
                return;
            }

            try
            {
                gID = Sql.ToGuid(Request["ID"]);
                if (!IsPostBack)
                {
                    if (!Sql.IsEmptyGuid(gID))
                    {
                        DbProviderFactory dbf = DbProviderFactories.GetFactory();
                        using (IDbConnection con = dbf.CreateConnection())
                        {
                            string sSQL;
                            // 09/08/2010   We need a separate view for the list as the default view filters by MODULE_ENABLED 
                            // and we don't want to filter by that flag in the ListView, DetailView or EditView. 
                            sSQL = "select *             " + ControlChars.CrLf
                                 + "  from vwTQRewardPoint_Edit" + ControlChars.CrLf
                                 + " where 1 = 1         " + ControlChars.CrLf;
                            using (IDbCommand cmd = con.CreateCommand())
                            {
                                cmd.CommandText = sSQL;
                                Sql.AppendParameter(cmd, gID, "ID", false);
                                con.Open();

                                if (bDebug)
                                    RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

                                // 11/22/2010   Convert data reader to data table for Rules Wizard. 
                                using (DbDataAdapter da = dbf.CreateDataAdapter())
                                {
                                    ((IDbDataAdapter)da).SelectCommand = cmd;
                                    using (DataTable dtCurrent = new DataTable())
                                    {
                                        da.Fill(dtCurrent);
                                        if (dtCurrent.Rows.Count > 0)
                                        {
                                            DataRow rdr = dtCurrent.Rows[0];
                                            //ctlModuleHeader.Title = Sql.ToString(rdr["C_clientname"]);
                                            SetPageTitle(L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);

                                            //this.AppendDetailViewFields(m_sMODULE + "." + LayoutDetailView, tblMain, rdr);
                                            Page.Items["ASSIGNED_USER_ID"] = Guid.Empty;
                                            ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Guid.Empty, rdr);


                                            //2014.12.14
                                            this.rptShow.DataSource = dtCurrent;
                                            this.rptShow.DataBind();

                                          

                                         


                                        }
                                        else
                                        {
                                            // 03/20/2008   Dynamic buttons need to be recreated in order for events to fire. 
                                            ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Guid.Empty, null);
                                            ctlDynamicButtons.DisableAll();
                                            ctlDynamicButtons.ErrorText = L10n.Term("ACL.LBL_NO_ACCESS");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // 03/20/2008   Dynamic buttons need to be recreated in order for events to fire. 
                        ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Guid.Empty, null);
                        ctlDynamicButtons.DisableAll();
                        //ctlDynamicButtons.ErrorText = L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + "ID";
                    }
                }
                // 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
                //Page.DataBind();
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                ctlDynamicButtons.ErrorText = ex.Message;
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
            ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "TQRewardPoint";
            SetMenu(m_sMODULE);
            if (IsPostBack)
            {
                this.AppendDetailViewFields(m_sMODULE + "." + LayoutDetailView, tblMain, null);
                ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutDetailView, Guid.Empty, null);
            }

            
        }

    
        #endregion


    }
}


