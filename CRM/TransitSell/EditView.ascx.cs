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
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.TransitSell
{
    /// <summary>
    ///		Summary description for EditView.
    /// </summary>
    public class EditView : SplendidControl
    {
        protected _controls.ModuleHeader ctlModuleHeader;

        protected Guid gID;
        protected HtmlTable tblMain;
        protected HtmlContainerControl gridAttachment;

        protected void Page_Command(Object sender, CommandEventArgs e)
        {
            // 03/15/2014   Enable override of concurrency error. 
            if (e.CommandName == "Save" || e.CommandName == "SaveConcurrency")
            {
                try
                {
                    // 01/16/2006   Enable validator before validating page. 
                    this.ValidateEditViewFields(m_sMODULE + "." + LayoutEditView);
                    if (Page.IsValid)
                    {
                        DbProviderFactory dbf = DbProviderFactories.GetFactory();
                        using (IDbConnection con = dbf.CreateConnection())
                        {
                            con.Open();
                            DataRow rowCurrent = null;
                            DataTable dtCurrent = new DataTable();
                            if (!Sql.IsEmptyGuid(gID))
                            {
                                string sSQL;
                                // 09/08/2010   We need a separate view for the list as the default view filters by MODULE_ENABLED 
                                // and we don't want to filter by that flag in the ListView, DetailView or EditView. 
                                sSQL = "select *             " + ControlChars.CrLf
                                     + "  from vwTQTransit_Edit" + ControlChars.CrLf
                                     + " where 1 = 1         " + ControlChars.CrLf;
                                using (IDbCommand cmd = con.CreateCommand())
                                {
                                    cmd.CommandText = sSQL;
                                    Sql.AppendParameter(cmd, gID, "ID", false);
                                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                                    {
                                        ((IDbDataAdapter)da).SelectCommand = cmd;
                                        da.Fill(dtCurrent);
                                        if (dtCurrent.Rows.Count > 0)
                                        {
                                            rowCurrent = dtCurrent.Rows[0];
                                            DateTime dtLAST_DATE_MODIFIED = Sql.ToDateTime(ViewState["LAST_DATE_MODIFIED"]);
                                            // 03/15/2014   Enable override of concurrency error. 
                                            if (Sql.ToBoolean(Application["CONFIG.enable_concurrency_check"]) && (e.CommandName != "SaveConcurrency") && dtLAST_DATE_MODIFIED != DateTime.MinValue && Sql.ToDateTime(rowCurrent["DATE_MODIFIED"]) > dtLAST_DATE_MODIFIED)
                                            {
                                                throw (new Exception(String.Format(L10n.Term(".ERR_CONCURRENCY_OVERRIDE"), dtLAST_DATE_MODIFIED)));
                                            }
                                        }
                                        else
                                        {
                                            gID = Guid.Empty;
                                        }
                                    }
                                }
                            }

                            // 10/07/2009   We need to create our own global transaction ID to support auditing and workflow on SQL Azure, PostgreSQL, Oracle, DB2 and MySQL. 
                            using (IDbTransaction trn = Sql.BeginTransaction(con))
                            {
                                try
                                {
                                    trn.Commit();

                                    // 11/20/2009   Move module init to a separate function. 
                                    // This will provide a central location to manage the LastModified, which is needed for ModulePopupScripts. 
                                    SplendidInit.InitModules(Context);
                                }
                                catch (Exception ex)
                                {
                                    trn.Rollback();
                                    SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                                    return;
                                }
                                // 10/24/2009   Clear menus just in case a module has been disabled or enabled. 
                                SplendidCache.ClearTabMenu();
                            }
                        }
                            Response.Redirect("view.aspx?ID=" + gID.ToString());
                    }
                }
                catch (Exception ex)
                {
                    SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                }
            }
            else if (e.CommandName == "Cancel")
            {
                if (Sql.IsEmptyGuid(gID))
                    Response.Redirect("default.aspx");
                else
                    Response.Redirect("view.aspx?ID=" + gID.ToString());
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle("发布在途气");
            // 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
            // 03/10/2010   Apply full ACL security rules. 
            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "edit") >= 0) && (Security.isSeller == 1);
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
                    Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
                    if (!Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID))
                    {
                        DbProviderFactory dbf = DbProviderFactories.GetFactory();
                        using (IDbConnection con = dbf.CreateConnection())
                        {
                            string sSQL;
                            // 09/08/2010   We need a separate view for the list as the default view filters by MODULE_ENABLED 
                            // and we don't want to filter by that flag in the ListView, DetailView or EditView. 
                            sSQL = "select *             " + ControlChars.CrLf
                                 + "  from vwTQTransit_Edit" + ControlChars.CrLf
                                 + " where 1 = 1         " + ControlChars.CrLf;
                            using (IDbCommand cmd = con.CreateCommand())
                            {
                                cmd.CommandText = sSQL;
                                Sql.AppendParameter(cmd, gID, "ID", false);
                                con.Open();

                                //if (bDebug)
                                 //   RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

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
                                            ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;

                                            this.AppendEditViewFields(m_sMODULE + "." + LayoutEditView, tblMain, rdr);
                                            ViewState["LAST_DATE_MODIFIED"] = Sql.ToDateTime(rdr["DATE_MODIFIED"]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        this.AppendEditViewFields(m_sMODULE + "." + LayoutEditView, tblMain, null);

                        var C_CustomerManager = new DynamicControl(this, "C_CustomerManager");

                        if (C_CustomerManager!=null)
                        {
                            C_CustomerManager.Text = Security.FULL_NAME;
                        }


                     
                    }
                }
                else
                {
                    ctlModuleHeader.Title = Sql.ToString(ViewState["ctlModuleHeader.Title"]);
                    SetPageTitle(L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
                }
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
            }


            //初始化数据
            //if (!IsPostBack)
            //{
            //    TXTC_GasificationRate = FindControl("TXTC_GasificationRate") as HtmlInputText;


            //    DbProviderFactory dbf = DbProviderFactories.GetFactory();
            //    using (IDbConnection con = dbf.CreateConnection())
            //    {
            //        con.Open();

            //        using (IDbCommand cmd = con.CreateCommand())
            //        {
            //            cmd.CommandText = "select * "
            //                             + "  from vwTQClient"
            //                             + " where ID = @ClientID"; ;
            //            Sql.AddParameter(cmd, "@ClientID", Security.User_ClientId);

            //            using (DbDataAdapter da = dbf.CreateDataAdapter())
            //            {
            //                ((IDbDataAdapter)da).SelectCommand = cmd;
            //                using (DataTable dtCurrent = new DataTable())
            //                {
            //                    da.Fill(dtCurrent);
            //                    if (dtCurrent.Rows.Count > 0)
            //                    {
            //                        DataRow row = dtCurrent.Rows[0];

            //                        TXTC_GasificationRate.Value = row["C_GasificationRate"].ToString();
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

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
            m_sMODULE = "TQTransit";
            SetMenu(m_sMODULE);
            if (IsPostBack)
            {
                this.AppendEditViewFields(m_sMODULE + "." + LayoutEditView, tblMain, null);
            }
        }
        #endregion
    }
}


