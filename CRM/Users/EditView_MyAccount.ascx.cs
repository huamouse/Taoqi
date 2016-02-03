using System;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace Taoqi.Users
{
    public partial class EditView_MyAccount : System.Web.UI.UserControl
    {
        private readonly string TXTERROR1 = "上传格式不正确，请重新上传。";
        private readonly string TXTERROR2 = "上传内容为空，请重新上传。";
        private readonly string SUCCESS1 = "已上传，查看大图";

        public string Phone { get; set; }
        public string UserType { get; set; }
        public string EMail { get; set; }
        public string RealName { get; set; }
        public string DetailAddress { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAbbreviation { get; set; }
                
        private Guid UserId;
        private Guid User_ClientID;
        private string C_ContactName;
        private string C_QQOpenId;
        public int ProvinceID;
        public int CityID;
        
        private string C_Attachment1;
        private string C_Attachment2;
        private string C_Attachment3;
        private string C_Attachment4;
        private string C_Attachment5;
        private string C_Attachment6;
        private string C_Attachment7;
        private string C_ClientImg;   //企业标识

        protected void Page_Load(object sender, EventArgs e)
        {
            Phone = lblUSER_PHONE.Text = Security.UserMobile;
            UserId = Security.USER_ID;
            User_ClientID = Guid.Parse(Security.UserClientID);

            AccountInformation();

            if(IsPostBack)
            {
                if ( !string.IsNullOrEmpty(Request.Form["btn_UploadClientImg"]) )
                {
                    UploadAttachment(8);
                }
            }
        }

        #region 上传按钮事件

        public void btn_Upload1_Click(object sender, EventArgs e)
        {
            UploadAttachment(1);
        }

        public void btn_Upload2_Click(object sender, EventArgs e)
        {
            UploadAttachment(2);
        }

        public void btn_Upload3_Click(object sender, EventArgs e)
        {
            UploadAttachment(3);
        }

        public void btn_Upload4_Click(object sender, EventArgs e)
        {
            UploadAttachment(4);
        }

        public void btn_Upload5_Click(object sender, EventArgs e)
        {
            UploadAttachment(5);
        }

        public void btn_Upload6_Click(object sender, EventArgs e)
        {
            UploadAttachment(6);
        }

        public void btn_Upload7_Click(object sender, EventArgs e)
        {
            UploadAttachment(7);
        }

        #endregion 上传按钮事件

        public void btnLogin_Click(object sender, EventArgs e)
        {
            UserType = GetUserType();
            EMail = txtEMail.Text.Trim();
            RealName = txtRealName.Text.Trim();
            C_ContactName = txtLinkman.Text.Trim();
            C_QQOpenId = txtQQ.Text.Trim();
            CompanyName = txtCompanyName.Text.Trim();
            CompanyAbbreviation = txtCompanyAbbreviation.Text.Trim();

            var tca = txtCompanyAbbreviation.Text.Trim();
            if(tca.Length>7){
                this.lblError.Text = "公司简称请勿超过七个字";
                return;
            }
            if (string.IsNullOrEmpty(Request["SLC_ProvinceName"]) ||
                string.IsNullOrEmpty(Request["SLC_CityName"]) 
               )
            {
                this.lblError.Text = "提示：请选择省、市。";
                 return;

            }
           

            //在客户部分暂时不精确到省和区以及详细信息，先留着，以后再说
            ProvinceID = int.Parse(Request["SLC_ProvinceName"]?? "0");
            /*
            RegionID = int.Parse(Request["SLC_CityName"]?? "0");
            CityID = int.Parse(Request["SLC_CountyName"]?? "0");
            */
            CityID = int.Parse(Request["SLC_CityName"] ?? "0");

            DetailAddress = detailAddress.Text.Trim();

            //必填项
            if (UserType == "0000000" || RealName == "" || C_ContactName == "" || CompanyName == "" || CompanyAbbreviation == "" ||
                //CityID == 0 || ProvinceID == 0 || RegionID == 0 || Text_DetailAddress == "")
                CityID == 0 || DetailAddress == "")
            {
                lblError.Text = "请将上面带星号的必填项填写完整。";
                return;
            }

            //SqlProcs.spTQUsers_Update_AccountInformation
            //   (ref UserId
            //   , UserType
            //   , EMail
            //   , RealName
            //   , C_ContactName
            //   , C_QQOpenId
            //   , CompanyName
            //   , CompanyAbbreviation
            //   , CityID
            //   , DetailAddress
            //   , C_Attachment1
            //   , C_Attachment2
            //   , C_Attachment3
            //   , C_Attachment4
            //   , C_Attachment5
            //   , C_Attachment6
            //   , C_Attachment7
            //   );

            //做出显示
            //showInformation();

            //更新Security中的数据，以便用户继续在CRM中操作不受影响，否则要做强制注销
            Security.EMAIL1 = EMail;
            Security.UserCompany = CompanyAbbreviation;
            Security.FULL_NAME = RealName;

            lblError.Text = "账户信息保存成功。";
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
                    cmd.CommandText = "SELECT * FROM vwTQUsers_List " +
                                        "WHERE ID = @USER_ID";
                    Sql.AddParameter(cmd, "@USER_ID", UserId);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            //取数据,并且操作数据，反映到页面
                            DataRow row = dt.Rows[0];

                            CompanyName = row["C_ClientName"].ToString();
                            CompanyAbbreviation = row["C_ClientShortName"].ToString();
                            RealName = row["LAST_NAME"].ToString();
                            EMail = row["EMAIL1"].ToString();
                            DetailAddress = row["C_Address"].ToString();
                            C_ContactName = row["C_ContactName"].ToString();
                            C_QQOpenId = row["C_QQOpenId"].ToString();

                            UserType = row["DESCRIPTION"].ToString();
                            CityID = (row["C_CityID"] as int?) ?? 0;

                            ProvinceID = 0;
                            int.TryParse(Convert.ToString(row["C_ProvinceID"]), out ProvinceID);


                            C_Attachment1 = row["C_Attachment1"].ToString();
                           // C_Attachment2 = row["C_Attachment2"].ToString();
                            C_Attachment3 = row["C_Attachment3"].ToString();
                            C_Attachment4 = row["C_Attachment4"].ToString();
                            C_Attachment5 = row["C_Attachment5"].ToString();
                            C_Attachment6 = row["C_Attachment6"].ToString();
                            C_Attachment7 = row["C_Attachment7"].ToString();

                            C_ClientImg = row["C_ImgUrl"].ToString();

                          

                            //如果是GET请求需要初始化填框为客户信息
                            if(!IsPostBack)
                            {
                                showInformation();

                         
                            }
                            

                        }
                    }

                }
            }
        }

        protected void showInformation()
        {
            txtCompanyName.Text = CompanyName;
            txtCompanyAbbreviation.Text = CompanyAbbreviation;
            txtRealName.Text = RealName;
            txtEMail.Text = EMail;
            detailAddress.Text = DetailAddress;
            txtLinkman.Text = C_ContactName;
            txtQQ.Text = C_QQOpenId;

            if ( !string.IsNullOrEmpty(UserType) )
                ShowUserType(UserType);

            showAttachment(C_Attachment1, 1);
            showAttachment(C_Attachment2, 2);
            showAttachment(C_Attachment3, 3);
            showAttachment(C_Attachment4, 4);
            showAttachment(C_Attachment5, 5);
            showAttachment(C_Attachment6, 6);
            showAttachment(C_Attachment7, 7);
            showClientImg();
        }

        private void ShowUserType(string Description)
        {
            if (Description.Length > 0 && Description[0] == '1')
                YHGC.Checked = true;
            else
                YHGC.Checked = false;

            if (Description.Length > 1 && Description[1] == '1')
                JSZ.Checked = true;
            else
                JSZ.Checked = false;

            if (Description.Length > 2 && Description[2] == '1')
                CYJQZ.Checked = true;
            else
                CYJQZ.Checked = false;

            if (Description.Length > 3 && Description[3] == '1')
                GYQHZ.Checked = true;
            else
                GYQHZ.Checked = false;

            if (Description.Length > 4 && Description[4] == '1')
                CSRQ.Checked = true;
            else
                CSRQ.Checked = false;

            if (Description.Length > 5 && Description[5] == '1')
                WLS.Checked = true;
            else
                WLS.Checked = false;

            if (Description.Length > 6 && Description[6] == '1')
                MYS.Checked = true;
            else
                MYS.Checked = false;
        }

        private void showAttachment(string AttachmentPath, int num)
        {
            switch (num)
            {
                case 1:
                    if ( !string.IsNullOrEmpty(AttachmentPath) )
                    {
                        img_Upload1.Src = AttachmentPath;
                        error1.InnerText = SUCCESS1;
                        error1.HRef = AttachmentPath;
                    }
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        img_Upload2.Src = AttachmentPath;
                        error2.InnerText = SUCCESS1;
                        error2.HRef = AttachmentPath;
                    }
                    break;
                case 3:
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        img_Upload3.Src = AttachmentPath;
                        error3.InnerText = SUCCESS1;
                        error3.HRef = AttachmentPath;
                    }
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        img_Upload4.Src = AttachmentPath;
                        error4.InnerText = SUCCESS1;
                        error4.HRef = AttachmentPath;
                    }
                    break;
                case 5:

                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        img_Upload5.Src = AttachmentPath;
                        error5.InnerText = SUCCESS1;
                        error5.HRef = AttachmentPath;
                    }
                    break;
                case 6:
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        img_Upload6.Src = AttachmentPath;
                        error6.InnerText = SUCCESS1;
                        error6.HRef = AttachmentPath;
                    }
                    break;
                case 7:
                    if (!string.IsNullOrEmpty(AttachmentPath))
                    {
                        img_Upload7.Src = AttachmentPath;
                        error7.InnerText = SUCCESS1;
                        error7.HRef = AttachmentPath;
                    }
                    break;
            }
        }

        private void showClientImg()
        {
            if (!string.IsNullOrEmpty(C_ClientImg))
            {
                ClientImg.Src = C_ClientImg;
                A_uploadClientImg.InnerText = "重新上传企业标识";
            }
        }

        //对上传的文件判断，并且上传，上传成功返回true,反之返回false
        private void UploadAttachment(int num)
        {
            string btnFileNum = "btnFile" + num;
            //判断上传功能是否能正常执行
            if (Request.Files.Count != 8)
                throw (new Exception("File Upload Is Fail."));

            //上传内容不能为空
            if (Request.Files[btnFileNum].ContentLength <= 0)
            {
                switch (num)
                {
                    case 1: error1.InnerText = TXTERROR2; break;
                    case 2: error2.InnerText = TXTERROR2; break;
                    case 3: error3.InnerText = TXTERROR2; break;
                    case 4: error4.InnerText = TXTERROR2; break;
                    case 5: error5.InnerText = TXTERROR2; break;
                    case 6: error6.InnerText = TXTERROR2; break;
                    case 7: error7.InnerText = TXTERROR2; break;
                    case 8: error8.Text = TXTERROR2; break;
                }

                return;
            }

            //对上传类型的判断
            string FileType = Request.Files[btnFileNum].ContentType;
            string FilePostFix = ".";

            if (FileType != "image/jpeg" && FileType != "image/jpg" && FileType != "image/png")
            {
                switch (num)
                {
                    case 1: error1.InnerText = TXTERROR1; break;
                    case 2: error2.InnerText = TXTERROR1; break;
                    case 3: error3.InnerText = TXTERROR1; break;
                    case 4: error4.InnerText = TXTERROR1; break;
                    case 5: error5.InnerText = TXTERROR1; break;
                    case 6: error6.InnerText = TXTERROR1; break;
                    case 7: error7.InnerText = TXTERROR1; break;
                    case 8: error8.Text = TXTERROR1; break;
                }

                return;
            }
            else
            {
                if (FileType == "image/jpeg")
                    FilePostFix += "JPEG";
                else if (FileType == "image/jpg")
                    FilePostFix += "JPG";
                else if (FileType == "image/png")
                    FilePostFix += "PNG";
            }

            //上传判断OK后，保存
            string virtual_UploadURL = string.Format("{0}/Attachment{1}{2}{3}", WebConfigurationManager.AppSettings["UploadPath_image"], UserId, num.ToString(), FilePostFix);
            string absolute_UploadURL = Request.MapPath(virtual_UploadURL);
            try
            {
                Request.Files[btnFileNum].SaveAs(absolute_UploadURL);

                switch (num)
                {
                    case 1: C_Attachment1 = virtual_UploadURL; break;
                    case 2: C_Attachment2 = virtual_UploadURL; break;
                    case 3: C_Attachment3 = virtual_UploadURL; break;
                    case 4: C_Attachment4 = virtual_UploadURL; break;
                    case 5: C_Attachment5 = virtual_UploadURL; break;
                    case 6: C_Attachment6 = virtual_UploadURL; break;
                    case 7: C_Attachment7 = virtual_UploadURL; break;
                    case 8: C_ClientImg = virtual_UploadURL; break;
                    default: return;
                }
                if (num == 8)
                {
                    SqlProcs.spTQClient_Update_ForClientImg(
                        ref User_ClientID
                        ,C_ClientImg
                        );

                    showClientImg();
                }
                else
                {
                    //SqlProcs.spTQUsers_Update_AccountInformation
                    //(ref UserId
                    //, UserType
                    //, EMail
                    //, RealName
                    //, C_ContactName
                    //, C_QQOpenId
                    //, CompanyName
                    //, CompanyAbbreviation
                    //, CityID
                    //, DetailAddress
                    //, C_Attachment1
                    //, C_Attachment2
                    //, C_Attachment3
                    //, C_Attachment4
                    //, C_Attachment5
                    //, C_Attachment6
                    //, C_Attachment7
                    //);

                    //做出显示
                    showInformation();
                }
                
            }
            catch
            {
                throw (new Exception("File Upload Is Fail."));
            }
        }

        //对用户类型判断，二进制表示法
        private string GetUserType()
        {
            string userType = "";

            if (YHGC.Checked)
                userType += "1";
            else
                userType += "0";

            if (JSZ.Checked)
                userType += "1";
            else
                userType += "0";

            if (CYJQZ.Checked)
                userType += "1";
            else
                userType += "0";

            if (GYQHZ.Checked)
                userType += "1";
            else
                userType += "0";

            if (CSRQ.Checked)
                userType += "1";
            else
                userType += "0";

            if (WLS.Checked)
                userType += "1";
            else
                userType += "0";

            if (MYS.Checked)
                userType += "1";
            else
                userType += "0";

            return userType;
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
        }

        #endregion Web Form Designer generated code
    }
}