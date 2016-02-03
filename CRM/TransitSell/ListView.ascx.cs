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

namespace Taoqi.TransitSell
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
        protected Repeater rp_model;
        protected UpdatePanel updatePanel_model;
        protected HtmlInputHidden HIHMyTransitID;
        protected HtmlInputHidden HIHC_TransitID;
        
        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle("出售中的在途气");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0) && (Security.isSeller == 1);
            if (!this.Visible)
                return;
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

                        cmd.CommandText = "  from vwTQTransit_List" + ControlChars.CrLf
                                        + " where 1 = 1           " + ControlChars.CrLf;

                        if (!Security.isAdmin)
                        {
                            cmd.CommandText += " and CREATED_BY = @AccountID " + ControlChars.CrLf;
                            Sql.AddParameter(cmd, "@AccountID", Security.AccountID);
                        }

                        if (scope == "1")
                            cmd.CommandText += " and ((C_Status = 2) or (C_Status = 3))" + ControlChars.CrLf;
                        else if (scope == "2")
                            cmd.CommandText += " and (C_Status = 0)" + ControlChars.CrLf;
                        else if (scope == "3")
                            cmd.CommandText += " and (C_Status = 1)" + ControlChars.CrLf;

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

            rp_model = FindControl("rptModal") as Repeater;
            updatePanel_model = FindControl("updatePanel") as UpdatePanel;
            HIHC_TransitID = FindControl("C_TransitID") as HtmlInputHidden;

            //执行删除以及收货的操作
            try
            {
                if (IsPostBack)
                {
                    if (Request.Form["TransitSell_btnDelete"] != null)
                    {
                        Guid id = Guid.Parse(Convert.ToString(Request.Form["TransitSell_btnDelete"]));
                        SqlProcs.spTQTransit_Delete(id);

                        Response.Redirect(Request.RawUrl);
                    }
                    else if (Request.Form["TransitSell_ModelView"] != null)
                    {
                        Guid id = Guid.Parse(Convert.ToString(Request.Form["TransitSell_ModelView"]));
                        HIHC_TransitID.Value = Request.Form["TransitSell_ModelView"];
                        
                        Hashtable ht1 = new Hashtable();
                        ht1.Add("C_TransitID", id);

                        DataTable tb1 = DAL.GetTable("vwTransitForSale_allBuyer", ht1);

                        rp_model.DataSource = tb1;
                        rp_model.DataBind();


                        int i = 0;

                        foreach (DataRow row in tb1.Rows)
                        {
                            HIHMyTransitID = rp_model.Items[i].FindControl("MyTransitID") as HtmlInputHidden;

                            HIHMyTransitID.Value = row["ID"].ToString();

                            i++;
                        }

                        ScriptManager.RegisterClientScriptBlock(updatePanel_model, this.GetType(), "modal", "$('#TransitSellModal').modal({keyboard:false})", true);
                    }
                    else if (Request.Form["Command"] == "btnConfirm")
                    {
                        Guid C_TransitID = Guid.Empty;

                        bool hasOrder = false;
                        for (int i = 0; i < rp_model.Items.Count; i++)
                        {
                            HtmlInputCheckBox cbx = rp_model.Items[i].FindControl("ckPrice") as HtmlInputCheckBox;
                            if (cbx.Checked)
                            {
                                hasOrder = true;

                                HIHMyTransitID = rp_model.Items[i].FindControl("MyTransitID") as HtmlInputHidden;
                                Guid gid = Guid.Parse(HIHMyTransitID.Value);
                                SqlProcs.spTQTransitMy_Change(gid, 2);
                                Msg.AcceptTransit(gid);
                            }
                        }

                        if (hasOrder)
                        {
                            C_TransitID = Guid.Parse( HIHC_TransitID.Value );
                            SqlProcs.spTQTransitChange(C_TransitID, 4); 
                            ScriptManager.RegisterClientScriptBlock(updatePanel_model, this.GetType(), "modal_ProductInTransit", "modal_ProductInTransit('TransitSellModal', true)", true);
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(updatePanel_model, this.GetType(), "modal_ProductInTransit", "modal_ProductInTransit('TransitSellModal')", true);
                    }
                }
                
            }
            catch
            {
                Response.Redirect("~/TransitSell");
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
            m_sMODULE = "TQTransit";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("C_Status");
            arrSelectFields.Add("C_LandingUrl");
            arrSelectFields.Add("C_Driver");
            arrSelectFields.Add("C_Tel");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
            grdMain.ItemCommand += grdMain_ItemCommand;
        }
        #endregion

        void grdMain_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            if (e.CommandName == "Delete")
            {
                Guid id = Guid.Parse(Convert.ToString(e.CommandArgument));
                SqlProcs.spTQTransit_Delete(id);
                Response.Redirect("default.aspx");
            }
        }
    }
}


