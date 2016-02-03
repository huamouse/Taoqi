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
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Web.UI;
using System.Web.Configuration;
using System.Collections;

namespace Taoqi.TQClient
{
    /// <summary>
    ///		Summary description for EditView.
    /// </summary>
    public class DetailView : SplendidControl
    {
        protected _controls.ModuleHeader ctlModuleHeader;
        protected _controls.DynamicButtons ctlDynamicButtons;
        // 01/13/2010   Add footer buttons. 
        protected _controls.DynamicButtons ctlFooterButtons;

        protected Guid gID;
        protected HtmlTable tblMain;
        protected HtmlContainerControl gridAttachment;

        private Guid gUserID;
        private Guid gClientID;
        protected string C_Category;
        protected int C_Status;
        protected string C_ClientName;
        protected string C_ClientShortName;
        protected Guid NewBossID;
        protected string NewBossName;

        protected string C_BossRole;  //第一位采购  第二位 销售  第三位 场站员工  第四位 驾驶员

        protected HtmlInputCheckBox cbxSeller;
        protected HtmlInputCheckBox cbxBuyer;

        //protected HtmlInputRadioButton C_Status_WSH;
        protected HtmlInputRadioButton C_Status_TG;
        protected HtmlInputRadioButton C_Status_WTG;
        protected HtmlInputRadioButton C_Status_TGBQJRRM;

        protected HtmlInputText txtClientName;
        protected HtmlInputText txtClientShortName;

        protected TextBox txtUserName;
        protected HiddenField hiddenUserID;

        protected Image Image1;
        protected Image Image2;
        protected Image Image3;
        protected Image Image4;
        protected Image Image5;
        protected Image Image6;
        protected Image Image7;
        protected Image Image8;
        protected Image Image9;
        protected Image Image10;
        
        private void Page_Load(object sender, System.EventArgs e)
        {
            SetPageTitle("新建/编辑企业信息");
            // 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
            // 03/10/2010   Apply full ACL security rules. 
            this.Visible = (Taoqi.Security.AdminUserAccess(m_sMODULE, "list") >= 0);
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
                            sSQL = "select * " + ControlChars.CrLf
                                 + " from vwTQClient" + ControlChars.CrLf
                                 + " where 1 = 1 " + ControlChars.CrLf;
                            using (IDbCommand cmd = con.CreateCommand())
                            {
                                cmd.CommandText = sSQL;
                                Sql.AppendParameter(cmd, gID, "ID", false);
                                con.Open();
                            }
                        }
                    }
                    else
                    {
                        this.AppendEditViewFields(m_sMODULE + "." + LayoutEditView, tblMain, null);
                        ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
                        ctlFooterButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);

                        var C_CustomerManager = new DynamicControl(this, "C_CustomerManager");

                        if (C_CustomerManager != null)
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
                //ctlDynamicButtons.ErrorText = ex.Message;
            }

            if (Request.QueryString["isDlg"] != null)
            {

                //2014.12.14           
                Button btnCancel = this.ctlDynamicButtons.FindButton("Cancel") as Button;

                if (btnCancel != null)
                {
                    btnCancel.Visible = false;
                }

                Button btnCancel_footer = this.ctlFooterButtons.FindButton("Cancel") as Button;

                if (btnCancel_footer != null)
                {
                    btnCancel_footer.Visible = false;
                }
            }

            gUserID = Security.USER_ID;
            if (!Guid.TryParse(Request.QueryString["id"], out gClientID))
                Response.Redirect("~/Client");

            txtClientName = FindControl("txtClientName") as HtmlInputText;
            txtClientShortName = FindControl("txtClientShortName") as HtmlInputText;
            hiddenUserID = FindControl("hiddenUserID") as HiddenField;
            txtUserName = FindControl("txtUserName") as TextBox;

            cbxSeller = FindControl("cbxSeller") as HtmlInputCheckBox;
            cbxBuyer = FindControl("cbxBuyer") as HtmlInputCheckBox;

            //C_Status_WSH = FindControl("C_Status_WSH") as HtmlInputRadioButton;
            C_Status_TG = FindControl("C_Status_TG") as HtmlInputRadioButton;
            C_Status_WTG = FindControl("C_Status_WTG") as HtmlInputRadioButton;
            C_Status_TGBQJRRM = FindControl("C_Status_TGBQJRRM") as HtmlInputRadioButton;

            if(!IsPostBack)
                AccountInformation();

            if (!IsPostBack)
                showInformation();
        }

        //从数据库查数据
        private void AccountInformation()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    //cmd.CommandText = "SELECT C_CategoryID,C_Status,C_ClientShortName *FROM vwTQClient " +
                    //                    "WHERE ID = @ClientId";
                    cmd.CommandText = "SELECT  * FROM vwTQClient_List " +
                                        "WHERE ID = @ClientID";
                    Sql.AddParameter(cmd, "@ClientID", gClientID);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            //取数据,并且操作数据，反映到页面
                            DataRow row = dt.Rows[0];

                            C_Category = row["C_CategoryID"].ToString();
                            C_Status = int.Parse(row["C_Status"].ToString());
                            C_ClientName = row["C_ClientName"].ToString();
                            C_ClientShortName = row["C_ClientShortName"].ToString();

                            Guid.TryParse(row["C_BossID"].ToString(), out NewBossID);

                            NewBossName = string.IsNullOrEmpty(row["LAST_NAME"].ToString()) ? row["USER_NAME"].ToString() : row["LAST_NAME"].ToString();

                            InitImage(row);
                        }
                    }

                }
            }
        }

        protected void showInformation()
        {
            showVerifyStatus(C_Status);
            ShowUserType(C_Category.ToString());
            txtClientName.Value = C_ClientName;
            txtClientShortName.Value = C_ClientShortName;
            txtUserName.Text = NewBossName;

        }

        private void showVerifyStatus(int C_Status)
        {
            switch (C_Status)
            {
                case 2:
                    C_Status_TG.Checked = true;
                    break;
                case 3:
                    C_Status_WTG.Checked = true;
                    break;
            }
        }

        private void ShowUserType(string Description)
        {
            if (Description.Length > 0 && Description[0] == '1')
                cbxSeller.Checked = true;
            else
                cbxSeller.Checked = false;

            if (Description.Length > 1 && Description[1] == '1')
                cbxBuyer.Checked = true;
            else
                cbxBuyer.Checked = false;
        }

        private string GetUserType()
        {
            C_BossRole = "0000";
            char[] BossRole_Array = new char[4];//第一位采购  第二位 销售  第三位 场站员工  第四位 驾驶员
            BossRole_Array = C_BossRole.ToCharArray();

            string userType = "";

            if (cbxSeller.Checked)
            {
                userType += "1";
                BossRole_Array[1] = '1';
                BossRole_Array[3] = '1';
            }
            else
            {
                userType += "0";
            }

            if (cbxBuyer.Checked)
            {
                userType += "1";
                BossRole_Array[0] = '1';
                BossRole_Array[2] = '1';
            }
            else
            {
                userType += "0";
            }

            C_BossRole = new String(BossRole_Array);

            return userType;
        }


        private void InitImage(DataRow rdr)
        {
            //加载图片
            InitOneImage(Sql.ToString(rdr["C_Attachment1"]), this.Image1);
            InitOneImage(Sql.ToString(rdr["C_Attachment2"]), this.Image2);
            InitOneImage(Sql.ToString(rdr["C_Attachment3"]), this.Image3);
            InitOneImage(Sql.ToString(rdr["C_Attachment4"]), this.Image4);
            InitOneImage(Sql.ToString(rdr["C_Attachment5"]), this.Image5);
            InitOneImage(Sql.ToString(rdr["C_Attachment6"]), this.Image6);
            InitOneImage(Sql.ToString(rdr["C_Attachment7"]), this.Image7);
            InitOneImage(Sql.ToString(rdr["C_Attachment8"]), this.Image8);
            InitOneImage(Sql.ToString(rdr["C_Attachment9"]), this.Image9);
            InitOneImage(Sql.ToString(rdr["C_Attachment10"]), this.Image10);
        }
        private void InitOneImage(string ImagURL, Image image)
        {
            if (!string.IsNullOrEmpty(ImagURL))
            {
                image.ImageUrl = WebConfigurationManager.AppSettings["UploadPath_image"] + ImagURL;
                //image.Attributes["layer-src"] = ImagURL;
            }
            //else
            //{
            //    image.ImageUrl = "/images/Information/pic_03.png";
            //}
            //image.ImageUrl = "/images/Information/pic_03.png";
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
            //ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
            //ctlFooterButtons.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "TQClient";
            SetMenu(m_sMODULE);
            if (IsPostBack)
            {
                this.AppendEditViewFields(m_sMODULE + "." + LayoutEditView, tblMain, null);
                //ctlDynamicButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
                //ctlFooterButtons.AppendButtons(m_sMODULE + "." + LayoutEditView, Guid.Empty, null);
            }
        }
        #endregion
    }
}


