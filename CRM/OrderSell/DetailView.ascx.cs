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

namespace Taoqi.OrderSell
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

        protected Guid C_OrderID;
        protected string C_PlateNumber;
        protected string C_Driver;
        protected string C_Driver2;
        protected string C_Tel;
        protected string C_Tel2;
        protected float C_Tonnage;

        protected UpdatePanel updatePanel;

        protected Guid C_OrderDetailID;

        protected string str_OrderDetailID { 
            get{
                return string.IsNullOrEmpty(ViewState["str_OrderDetailID"].ToString()) ? "" : ViewState["str_OrderDetailID"].ToString();
            }
            set {
                ViewState["str_OrderDetailID"] = value;
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            if (!Guid.TryParse(Request.QueryString["id"], out C_OrderID))
                Response.Redirect("~/OrderSell");

            SetPageTitle("我的订单");

            this.Visible = (Taoqi.Security.GetUserAccess(m_sMODULE, "view") >= 0) && (Security.isSeller == 1);
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
                
                if(!IsPostBack)
                    grdMain.DataBind();
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                lblError.Text = ex.Message;
            }
            
            if (IsPostBack)
            {
                Guid gid = Guid.Empty;

                if (Request.Form["btnEdit"] != null)
                {
                    str_OrderDetailID = Request.Form["btnEdit"];
                }
                else if (Request.Form["AddCar"] == "AddCar")
                {
                    C_PlateNumber = Request.Form["plateNumber"];
                    if (!float.TryParse(Request.Form["tonnage"], out C_Tonnage) || C_Tonnage < 45 || C_Tonnage > 60)
                    {
                        ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "modalError",
                            "$(function(){$('#fahuoModal').modal();$('#fahuoaddModal').modal();alert('车辆罐容必须为数字，且在45-60的范围内！');});", true);
                        return;
                    }

                    C_Driver = Request.Form["driver"];
                    C_Driver2 = Request.Form["driver2"];
                    C_Tel = Request.Form["phone"];
                    C_Tel2 = Request.Form["phone2"];
                    if (C_PlateNumber == "" || C_Driver == "" || C_Tel == "")
                    {
                        ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "modalError",
                            "$(function(){$('#fahuoModal').modal();$('#fahuoaddModal').modal();alert('车牌号、驾驶员、驾驶员手机为必填项！');});", true);
                        return;
                    }

                    SqlProcs.spTQCar_Update(ref gid, C_PlateNumber, C_Tonnage, C_Driver, C_Driver2, C_Tel, C_Tel2, null);

                    updatePanel = FindControl("updatePanel") as UpdatePanel;
                    ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "modal_fahuoModal", "$(function(){$('#fahuoModal').modal();});", true);
                }
                else if (Request.Form["SendCars"] == "SendCars" && Guid.TryParse(str_OrderDetailID, out C_OrderDetailID))
                {
                    Guid CarID = Guid.Empty;
                    Guid.TryParse(Request.Form["RDCar"], out CarID);
                    if (CarID != Guid.Empty)
                    {
                        SqlProcs.spTQOrderDetailCar(C_OrderDetailID, CarID);
                        string msgContent = "";
                        Msg.DispatchCar(C_OrderDetailID, ref msgContent);
                        ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "modalError",
                                string.Format("layer.alert('系统已向车辆司机发送一条提醒短信：{0} ');", msgContent), true);

                        //SqlProcs.spTQOrderDetail_ChangeStatus(C_OrderDetailID, 4);
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(updatePanel, this.GetType(), "alertCar", "layer.alert('提示：请先选择车辆。',{area: ['385px', '178px'],offset: ['195px', '500px'],});", true);
                    }
                }
            }

            CarInformation();
        }

        //从数据库查数据
        private void CarInformation()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    //取客户车辆信息
                    cmd.CommandText = "SELECT * FROM vwTQCar " +
                                        "WHERE CREATED_BY = @AccountID " +
                                        "ORDER BY DATE_ENTERED desc";
                    Sql.AddParameter(cmd, "@AccountID", Security.AccountID);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                (FindControl("RPSelectCar") as Repeater).DataSource = dt;
                                (FindControl("RPSelectCar") as Repeater).DataBind();
                            }
                        }
                    }

                    //取订单信息
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
                                Response.Redirect("~/OrderSell");
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
            // 11/26/2005   Add fields early so that sort events will get called. 
            m_sMODULE = "TQOrderDetailSell";
            SetMenu(m_sMODULE);
            // 02/08/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("ID");
            arrSelectFields.Add("C_Status");
            arrSelectFields.Add("C_CarID");
            arrSelectFields.Add("C_ShippingUrl");
            arrSelectFields.Add("C_Driver");
            arrSelectFields.Add("C_Tel");
            this.AppendGridColumns(grdMain, m_sMODULE + "." + LayoutListView, arrSelectFields);
        }

        void rptShow_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                Guid ID = Guid.Parse(dataRow["ID"].ToString());


                Repeater rptAddress = e.Item.FindControl("rptAddress") as Repeater;
                rptAddress.ItemDataBound += rptAddress_ItemDataBound;

                Hashtable ht1 = new Hashtable();
                ht1.Add("C_OrderID", ID);

                var addressList = DAL.GetTable("vwTQOrderDetail_List", ht1);

                rptAddress.DataSource = addressList;
                rptAddress.DataBind();
            }
        }

        void rptAddress_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView dataRow = e.Item.DataItem as DataRowView;
                Guid ID = Guid.Parse(dataRow["ID"].ToString());

                Repeater rptCar = e.Item.FindControl("rptCar") as Repeater;

                Hashtable ht1 = new Hashtable();
                ht1.Add("C_OrderDetailID", ID);
                var orderDetail_List = DAL.GetTable("vwTQOrderDetail_List", ht1);

                rptCar.DataSource = orderDetail_List;
                rptCar.DataBind();
            }
        }

        #endregion
    }
}


