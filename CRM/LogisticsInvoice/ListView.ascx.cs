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

namespace Taoqi.TQLogisticsInvoice
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
                }
                // 12/14/2007   We need to capture the sort event from the SearchView. 
                else if (e.CommandName == "SortGrid")
                {
                    grdMain.SetSortFields(e.CommandArgument as string[]);
                    // 04/27/2008   Sorting has been moved to the database to increase performance. 
                    // 03/17/2011   We need to treat a comma-separated list of fields as an array. 
                    arrSelectFields.AddFields(grdMain.SortColumn);
                }
                else if (e.CommandName == "Export")
                {
                    // 11/03/2006   Apply ACL rules to Export. 
                    int nACLACCESS = Taoqi.Security.GetUserAccess(m_sMODULE, "export");
                    if (nACLACCESS >= 0)
                    {
                        // 10/05/2009   When exporting, we may need to manually bind.  Custom paging should be disabled when exporting all. 
                        if (vwMain == null)
                            grdMain.DataBind();
                        // 05/10/2007   ACL_ACCESS.OWNER is not defined for the EMPLOYEES module. 
                        //if ( nACLACCESS == ACL_ACCESS.OWNER )
                        //	vwMain.RowFilter = "ASSIGNED_USER_ID = '" + Security.USER_ID.ToString() + "'";
                        //string[] arrID = null;  // 11/27/2010   Checkbox selection is not supported for this module.

                        //设置要导出的字段

                        //var dtExport = vwMain.ToTable(false, "C_clientname", "C_Industrytype", "C_ContactName", "C_appellationNAME", "C_ContactTitle", "C_contact", "C_plane", "C_Address", "C_Fax", "C_email", "C_CustomerManager");
                        //var dvExport = new DataView(dtExport);
                        //SplendidExport.Export(dvExport, m_sMODULE, ctlExportHeader.ExportFormat, ctlExportHeader.ExportRange, grdMain.CurrentPageIndex, grdMain.PageSize, arrID, grdMain.AllowCustomPaging);
                        //SplendidExport.Export(vwMain, m_sMODULE, ctlExportHeader.ExportFormat, ctlExportHeader.ExportRange, grdMain.CurrentPageIndex, grdMain.PageSize, arrID, grdMain.AllowCustomPaging);
                    }
                }
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                lblError.Text = ex.Message;
            }
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
                    string sTABLE_NAME = "TQLogisticsInvoice";
                    cmd.CommandText = "  from vw" + sTABLE_NAME + "_List" + ControlChars.CrLf
                                    + " where 1 = 1" + ControlChars.CrLf;
                    //Security.Filter(cmd, m_sMODULE, "list");
                    //ctlSearchView.SqlSearchClause(cmd);
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
                        //RegisterClientScriptBlock("SQLPaged", Sql.ClientScriptBlock(cmd));

                        // 01/13/2010   Allow default search to be disabled. 
                        if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
                        {
                            using (DbDataAdapter da = dbf.CreateDataAdapter())
                            {
                                ((IDbDataAdapter)da).SelectCommand = cmd;
                                using (DataTable dt = new DataTable())
                                {
                                    da.Fill(dt);
                                    // 11/22/2010   Apply Business Rules. 
                                    this.ApplyGridViewRules(m_sMODULE + "." + LayoutListView, dt);

                                    vwMain = dt.DefaultView;
                                    grdMain.DataSource = vwMain;
                                }
                            }
                            //ctlExportHeader.Visible = true;
                        }
                        else
                        {
                            //ctlExportHeader.Visible = false;
                        }
                }
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {


            SetPageTitle("我的发票");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0);
            if (!this.Visible)
                return;

            try
            {
                // 09/08/2009   Add support for custom paging in a DataGrid. Custom paging can be enabled / disabled per module. 
                if (Crm.Config.allow_custom_paging() && Crm.Modules.CustomPaging(m_sMODULE))
                {
                    // 10/05/2009   We need to make sure to disable paging when exporting all. 
                    //grdMain.AllowCustomPaging = (Request.Form[ctlExportHeader.ExportUniqueID] == null || ctlExportHeader.ExportRange != "All");
                    grdMain.SelectMethod += new SelectMethodHandler(grdMain_OnSelectMethod);
                }

                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {
                        //grdMain.OrderByClause("C_clientname", "desc");

                        // 02/19/2010   We cannot lookup the table.  We must hard-code the table to be EMPLOYEES 
                        // The reason is that the lookup will return USERS, and that is the wrong view to use here. 

                        cmd.CommandText = "  from vwTQLogisticsInvoice_List" + ControlChars.CrLf
                                        + " where 1 = 1           " + ControlChars.CrLf;

                        if (!Security.isAdmin)
                        {
                            cmd.CommandText += " and ( C_SendName = @AccountID or C_ReceiveName = @AccountID ) " + ControlChars.CrLf;
                            Sql.AddParameter(cmd, "@AccountID", Security.AccountID);
                        }
                        // 05/10/2007   ACL_ACCESS.OWNER is not defined for the EMPLOYEES module. 
                        //Security.Filter(cmd, m_sMODULE, "list");


                        //ctlSearchView.SqlSearchClause(cmd);
                        // 09/08/2009   Custom paging will always require two queries, the first is to get the total number of rows. 


                        if (grdMain.AllowCustomPaging)
                        {
                            cmd.CommandText = "select count(*)" + ControlChars.CrLf
                                            + cmd.CommandText;

                            if (bDebug)
                                //RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

                                // 01/13/2010   Allow default search to be disabled. 
                                if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
                                {
                                    grdMain.VirtualItemCount = Sql.ToInteger(cmd.ExecuteScalar());
                                }
                        }
                        else
                        {
                            // 04/27/2008   The fields in the search clause need to be prepended after any Saved Search sort has been determined.
                            // 09/08/2009   The order by clause is separate as it can change due to SearchViews. 
                            // 04/20/2011   If there are no fields in the GridView.Export, then return all fields (*). 
                            cmd.CommandText = "select " + Sql.FormatSelectFields(arrSelectFields)
                                            + cmd.CommandText
                                            + grdMain.OrderByClause();

                            if (bDebug)
                                //RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));

                                // 01/13/2010   Allow default search to be disabled. 
                                if (PrintView || IsPostBack || Taoqi.Crm.Modules.DefaultSearch(m_sMODULE))
                                {
                                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                                    {
                                        ((IDbDataAdapter)da).SelectCommand = cmd;
                                        using (DataTable dt = new DataTable())
                                        {
                                            da.Fill(dt);
                                            // 11/22/2010   Apply Business Rules. 
                                            this.ApplyGridViewRules(m_sMODULE + "." + LayoutListView, dt);

                                            vwMain = dt.DefaultView;

                                            /*
                                            if (Request.Path.ToLower().Contains("my.aspx"))
                                            {
                                                vwMain.RowFilter = string.Format("C_CustomerManager = '{0}'", Taoqi.Security.FULL_NAME);
                                            }
                                            */

                                            grdMain.DataSource = vwMain;
                                        }
                                    }
                                    //ctlExportHeader.Visible = true;
                                }
                                else
                                {
                                    // ctlExportHeader.Visible = false;
                                }
                        }
                    }
                }




                //string c_company = null;
                //DL_Company.SelectedItem.Text = Security.User_Sector;

                if (!IsPostBack)
                {
                    grdMain.DataBind();


                    // 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
                    //Page.DataBind();
                    // 09/08/2009   Let the grid handle the differences between normal and custom paging. 
                    // 09/08/2009   Bind outside of the existing connection so that a second connect would not get created. 

                    /*
                    DL_Company.SelectedIndex = DL_Company.Items.IndexOf(DL_Company.Items.FindByValue(Security.User_Sector));
                    c_company = Security.User_Sector;
                    if (!_code.CommonForCore.IsMiddleLeadership()&&!Security.isAdmin)
                    {
                        //var ctlSector2 = new DynamicControl(this.ctlSearchView, Security.User_Sector);
                        DL_Company.Enabled = false;
                    }
                   
                    c_company_btn(c_company);
                    */
                    //DataTable dtHonourList = _code.CommonForCore.GetProcedureDataSet("spTQLogisticsInvoices_RedTop10_Report", "@sector", c_company);
                    //DataView dvHonourList = new DataView(dtHonourList);
                    //dvHonourList.RowFilter = "序号 <= 10";

                    //this.rptHonourList.DataSource = dvHonourList;
                    //this.rptHonourList.DataBind();

                    //DataTable dtBlackList = _code.CommonForCore.GetProcedureDataSet("spTQLogisticsInvoices_BlackTop10_Report", "@sector", c_company);
                    //DataView dvBlackList = new DataView(dtBlackList);
                    //dvBlackList.RowFilter = "序号 <= 10";
                    //this.rptBlackList.DataSource = dvBlackList;
                    //this.rptBlackList.DataBind();

                }
                else
                {
                    /*
                    c_company = DL_Company.SelectedItem.Text;
                    c_company_btn(c_company);
                    */
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
            //ctlSearchView.Command += new CommandEventHandler(Page_Command);
            //ctlExportHeader.Command += new CommandEventHandler(Page_Command);
            // 11/26/2005   Add fields early so that sort events will get called. 
            m_sMODULE = "TQLogisticsInvoice";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
        }
        #endregion
        public void c_company_btn(string c_company)
        {
            /*
            grdMain.DataBind();
            DataTable dtHonourList = _code.CommonForCore.GetProcedureDataSet("spTQLogisticsInvoices_RedTop10_Report", "@sector", c_company);
            DataView dvHonourList = new DataView(dtHonourList);
            dvHonourList.RowFilter = "序号 <= 10";

            this.rptHonourList.DataSource = dvHonourList;
            this.rptHonourList.DataBind();

            DataTable dtBlackList = _code.CommonForCore.GetProcedureDataSet("spTQLogisticsInvoices_BlackTop10_Report", "@sector", c_company);
            DataView dvBlackList = new DataView(dtBlackList);
            dvBlackList.RowFilter = "序号 <= 10";
            this.rptBlackList.DataSource = dvBlackList;
            this.rptBlackList.DataBind();

            */

        }

        protected void DL_Company_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            string C_company = this.DL_Company.SelectedItem.Text;
            
           
            c_company_btn(C_company);
            */
        }
    }
}


