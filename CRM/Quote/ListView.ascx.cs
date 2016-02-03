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
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.TQQuote
{
    /// <summary>
    ///		Summary description for ListView.
    /// </summary>
    public class ListView : SplendidControl
    {
        public string script;

        protected UniqueStringCollection arrSelectFields;
        protected UpdatePanel updatePanel;
        protected DataView vwMain;
        protected SplendidGrid grdMain;
        protected Repeater rptModal;
        protected Repeater rptPrice;
        protected LinkButton btnConfirm;
        protected Label lblError;

        private Guid quoteDetailID
        {
            get
            {
                return (ViewState["QuoteDetailID"] != null) ? Guid.Parse(ViewState["QuoteDetailID"].ToString()) : Guid.Empty;
            }
            set
            {
                ViewState["QuoteDetailID"] = value;
            }
        }

        /// <summary>
        /// 是否是查看报价模式
        /// </summary>
        protected bool IsView  
        {
            get
            {
                return (ViewState["IsView"] != null) ? bool.Parse(ViewState["IsView"].ToString()) : true;
            }
            set
            {
                ViewState["IsView"] = value;
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle("我的求购");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "list") >= 0) && (Security.isBuyer == 1);
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

                        cmd.CommandText = "  from vwTQQuoteDetail_List" + ControlChars.CrLf
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

            //执行删除以及收货的操作
            try
            {
                if (Request.Form["Quote_btnDelete"] != null)
                {
                    Guid id = Guid.Parse(Convert.ToString(Request.Form["Quote_btnDelete"]));
                    SqlProcs.spTQQuoteDetail_Delete(id);

                    Response.Redirect(Request.RawUrl);
                }
            }
            catch
            {
                Response.Redirect("~/Quote");
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
            // 11/26/2005   Add fields early so that sort events will get called. 
            m_sMODULE = "TQQuoteDetail";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("C_Status");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
            grdMain.ItemCommand += grdMain_ItemCommand;
            rptModal.ItemDataBound += rptModal_ItemDataBound;
            btnConfirm.Click += btnConfirm_Click;
        }
        #endregion

        void grdMain_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "QuoteView")
            {
                int nStatus = ((QuoteButton)e.CommandSource).Status;
                switch (nStatus)
                {
                    case 3: // 查看报价
                    case 4: // 查看成交
                        if (nStatus == 4) IsView = false;
                        else IsView = true;

                        quoteDetailID = Guid.Parse(Convert.ToString(e.CommandArgument));
                        Hashtable ht = new Hashtable();
                        ht.Add("C_QuoteDetailID", quoteDetailID);

                        var tbTQQuotePrice = DAL.GetTable("vwTQQuotePrice", ht, 0, null, "distinct SellerID, Seller, SellerName, SellerPhone, SellerQQ, C_QuoteDetailID");
                        rptModal.DataSource = tbTQQuotePrice;
                        rptModal.DataBind();

                        ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "modal", "$(function(){$('#quoteModal').modal()})", true);
                        break;
                    default:
                        break;
                }
            }
        }

        void rptModal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Repeater rptPrice = e.Item.FindControl("rptPrice") as Repeater;
                DataRowView dataRow = e.Item.DataItem as DataRowView;

                Guid sellerID = Sql.ToGuid(dataRow["SellerID"]);
                Guid quoteDetailID = Sql.ToGuid(dataRow["C_QuoteDetailID"]); 
                Hashtable ht = new Hashtable();
                ht.Add("SellerID", sellerID);
                ht.Add("C_QuoteDetailID", quoteDetailID);

                var quotePrice = DAL.GetTable("vwTQQuotePrice", ht);

                rptPrice.DataSource = quotePrice;
                rptPrice.DataBind();
            }
        }

        void btnConfirm_Click(object sender, EventArgs e)
        {
            bool hasOrder = false;
            try
            {
                for (int i = 0; i < rptModal.Items.Count; i++)
                {
                    Repeater rptPrice = (Repeater)rptModal.Items[i].FindControl("rptPrice");
                    for (int j = 0; j < rptPrice.Items.Count; j++)
                    {
                        CheckBox cbx = (CheckBox)rptPrice.Items[j].FindControl("ckPrice");
                        if (cbx.Checked)
                        {
                            Guid gid = Guid.Empty;
                            TextBox txt = (TextBox)cbx.FindControl("quotePriceID");
                            Guid id = Guid.Parse(txt.Text);
                            SqlProcs.spTQQuotePrice_NewOrder(ref gid, id);
                            Msg.AcceptQuotePrice(id);
                            hasOrder = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            if (hasOrder)
            {
                SqlProcs.spTQQuoteDetailChange(quoteDetailID, 4); //求购结束
                ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "modalQuit", "modalQuit('quoteModal', true)", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "modalQuit", "modalQuit('quoteModal')", true);
        }
    }
}


