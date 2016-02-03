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

namespace Taoqi.TQClient
{
    /// <summary>
    ///		Summary description for PopupView.
    /// </summary>
    public class PopupView : SplendidControl
    {
        protected _controls.SearchView ctlSearchView;
        protected _controls.DynamicButtons ctlDynamicButtons;
        protected _controls.CheckAll ctlCheckAll;

        protected UniqueStringCollection arrSelectFields;
        protected DataView vwMain;
        protected SplendidGrid grdMain;
        protected bool bMultiSelect;

        public bool MultiSelect
        {
            get { return bMultiSelect; }
            set { bMultiSelect = value; }
        }

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Search")
                {
                    // 10/13/2005   Make sure to clear the page index prior to applying search. 
                    grdMain.CurrentPageIndex = 0;
                    // 04/27/2008   Sorting has been moved to the database to increase performance. 
                    grdMain.DataBind();
                    SetDashboard();
                }
                // 12/14/2007   We need to capture the sort event from the SearchView. 
                else if (e.CommandName == "SortGrid")
                {
                    grdMain.SetSortFields(e.CommandArgument as string[]);
                    // 04/27/2008   Sorting has been moved to the database to increase performance. 
                    // 03/17/2011   We need to treat a comma-separated list of fields as an array. 
                    arrSelectFields.AddFields(grdMain.SortColumn);
                }
                // 11/17/2010   Populate the hidden Selected field with all IDs. 
                else if (e.CommandName == "SelectAll")
                {
                    // 05/22/2011   When using custom paging, vwMain may not be defined. 
                    if (vwMain == null)
                        grdMain.DataBind();
                    ctlCheckAll.SelectAll(vwMain, "ID");
                    grdMain.DataBind();
                }
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                ctlDynamicButtons.ErrorText = ex.Message;
            }
        }

        //2015.05.05 Panco add
        protected void SetDashboard()
        {
            /*
            //2015.1.11
            DynamicControl ctlSector = new DynamicControl(ctlSearchView, "Sector");
            string sector = ctlSector.Text;
            if (!_code.CommonForCore.IsAdminAccess())
            {
                ctlSector.Text = Security.User_Sector;
                ctlSector.Enabled = false;
                
            }*/
        }

        // 09/08/2009   Add support for custom paging. 
        protected void grdMain_OnSelectMethod(int nCurrentPageIndex, int nPageSize)
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    string sTABLE_NAME = Crm.Modules.TableName(m_sMODULE);
                    // 12/04/2006   Only include active users. 
                    // 02/08/2008   We need to build a list of the fields used by the search clause. 
                    // 04/15/2008   When Team Management is enabled, only show users that are in this users teams. 
                    bool bTeamFilter = !Security.isAdmin && Crm.Config.enable_team_management();
                    if (bTeamFilter)
                    {
                        cmd.CommandText = "  from vwTEAMS_ASSIGNED_TO_List" + ControlChars.CrLf
                                        + " where MEMBERSHIP_USER_ID = @MEMBERSHIP_USER_ID" + ControlChars.CrLf;
                        Sql.AddParameter(cmd, "@MEMBERSHIP_USER_ID", Security.USER_ID);
                    }
                    else
                    {

                        cmd.CommandText = "  from vwTQClient_List" + ControlChars.CrLf
                                        + " where 1 = 1                   " + ControlChars.CrLf;
                    }
                    ctlSearchView.SqlSearchClause(cmd);
                    cmd.CommandText = "select " + Sql.FormatSelectFields(arrSelectFields)
                                    + cmd.CommandText;
                    if (nPageSize > 0)
                    {
                        Sql.PageResults(cmd, sTABLE_NAME, grdMain.OrderByClause(), nCurrentPageIndex, nPageSize);
                    }
                    else
                    {
                        cmd.CommandText += grdMain.OrderByClause();
                    }

                    if (bDebug)
                        RegisterClientScriptBlock("SQLPaged", Sql.ClientScriptBlock(cmd));

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            // 11/06/2012   Apply Business Rules to PopupView. 
                            this.ApplyGridViewRules(m_sMODULE + "." + LayoutListView, dt);

                            vwMain = dt.DefaultView;
                            grdMain.DataSource = vwMain;
                        }
                    }
                }
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle(L10n.Term(m_sMODULE + ".LBL_LIST_FORM_TITLE"));
            // 07/05/2009   We don't use access control on the user list as all users can assign a record to any user. 
            //this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0);
            if (!this.Visible)
                return;

            try
            {
                // 09/08/2009   Add support for custom paging in a DataGrid. Custom paging can be enabled / disabled per module. 
                if (Crm.Config.allow_custom_paging() && Crm.Modules.CustomPaging(m_sMODULE))
                {
                    // 09/18/2012   Disable custom paging if SelectAll was checked. 
                    grdMain.AllowCustomPaging = !ctlCheckAll.SelectAllChecked;
                    grdMain.SelectMethod += new SelectMethodHandler(grdMain_OnSelectMethod);
                    // 11/17/2010   Disable Select All when using custom paging. 
                    //ctlCheckAll.ShowSelectAll = false;
                }

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        grdMain.OrderByClause("C_ClientShortName", "asc");
                        // 12/04/2006   Only include active users. 
                        // 02/08/2008   We need to build a list of the fields used by the search clause. 
                        // 04/15/2008   When Team Management is enabled, only show users that are in this users teams. 

                        bool bTeamFilter = !Security.isAdmin && Crm.Config.enable_team_management();
                        if (bTeamFilter)
                        {
                            cmd.CommandText = "  from vwTEAMS_ASSIGNED_TO_List" + ControlChars.CrLf
                                            + " where MEMBERSHIP_USER_ID = @MEMBERSHIP_USER_ID" + ControlChars.CrLf;
                            Sql.AddParameter(cmd, "@MEMBERSHIP_USER_ID", Security.USER_ID);
                        }
                        else
                        {

                            cmd.CommandText = "  from vwTQClient_List" + ControlChars.CrLf
                                            + " where 1 = 1                   " + ControlChars.CrLf;
                        }
                       // ctlSearchView.SqlSearchClause(cmd);
                        // 03/23/2010   Allow the USER_NAME or the EMAIL1 to be an input filter. 

                       
                        if (grdMain.AllowCustomPaging)
                        {
                            cmd.CommandText = "select count(*)" + ControlChars.CrLf
                                            + cmd.CommandText;

                            if (bDebug)
                                RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

                            grdMain.VirtualItemCount = Sql.ToInteger(cmd.ExecuteScalar());
                        }
                        else
                        {
                            // 04/27/2008   The fields in the search clause need to be prepended after any Saved Search sort has been determined.
                            // 09/08/2009   The order by clause is separate as it can change due to SearchViews. 
                            cmd.CommandText = "select " + Sql.FormatSelectFields(arrSelectFields)
                                            + cmd.CommandText
                                            + grdMain.OrderByClause();

                            if (bDebug)
                                RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable dt = new DataTable())
                                {
                                    da.Fill(dt);
                                    // 11/06/2012   Apply Business Rules to PopupView. 
                                    this.ApplyGridViewRules(m_sMODULE + "." + LayoutListView, dt);

                                    vwMain = dt.DefaultView;
                                    grdMain.DataSource = vwMain;
                                }
                            }
                        }
                    }
                }
                if (!IsPostBack)
                {
                    // 03/11/2008   Move the primary binding to SplendidPage. 
                    //Page DataBind();
                    // 09/08/2009   Let the grid handle the differences between normal and custom paging. 
                    // 09/08/2009   Bind outside of the existing connection so that a second connect would not get created. 
                    grdMain.DataBind();
                    SetDashboard();
                }
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
            ctlSearchView.Command += new CommandEventHandler(Page_Command);
            ctlCheckAll.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "TQClient";
            // 07/26/2007   Use the new PopupView so that the view is customizable. 
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("C_ClientShortName");
            // 08/01/2010   We need a separate view to select the Full Name instead of the User Name. 
            this.AppendGridColumns(grdMain, m_sMODULE + ".PopupView" + (Sql.ToString(Request["C_ClientShortName"]) == "1" ? "Name" : ""), arrSelectFields);
            // 04/29/2008   Make use of dynamic buttons. 
            ctlDynamicButtons.AppendButtons(m_sMODULE + ".Popup" + (bMultiSelect ? "MultiSelect" : "View"), Guid.Empty, Guid.Empty);
            if (!IsPostBack && !bMultiSelect)
                ctlDynamicButtons.ShowButton("Clear", !Sql.ToBoolean(Request["ClearDisabled"]));
        }
        #endregion
    }
}


