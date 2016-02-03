using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;

namespace Taoqi.Advertisement
{
    public partial class SetAdvertisement : SplendidPage
    {
        private readonly string TXTERROR1 = "上传格式不正确，请重新上传。";
        //private readonly string TXTERROR2 = "上传内容为空，请重新上传。";
        private readonly string SUCCESS1 = "上传成功";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Security.isAdmin)
            {
                if (Security.isBuyer == 1)
                    Response.Redirect("../Order/");
                else if (Security.isSeller == 1)
                    Response.Redirect("../OrderSell/");
                else if (Security.isCompany == 1)
                    Response.Redirect("../Users/ClientInfo.aspx");
                else
                    Response.Redirect("../Users/PersonalInfo.aspx");
            }

            if (IsPostBack)
            {
                if (Request.Files.Count > 0)
                {
                    UploadAttachment(1);
                    UploadAttachment(2);
                    UploadAttachment(3);
                    UploadAttachment(4);
                    UploadAttachment(5);
                    UploadAttachment(6);
                }
            }
        }

        private void UploadAttachment(int num)
        {
            string btnFileNum = "btnFile" + num;

            //上传内容不能为空
            if (Request.Files[btnFileNum].ContentLength <= 0)
            {
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
                }

                return;
            }
            else
            {
                    FilePostFix += "png";
            }

            //上传判断OK后，保存
            string virtual_UploadURL = string.Format("{0}/banner{1}{2}", WebConfigurationManager.AppSettings["UploadPath_Banner"], num.ToString(), FilePostFix);
            string absolute_UploadURL = Request.MapPath(virtual_UploadURL);
            try
            {
                Request.Files[btnFileNum].SaveAs(absolute_UploadURL);
                switch (num)
                {
                    case 1: error1.InnerText = SUCCESS1; break;
                    case 2: error2.InnerText = SUCCESS1; break;
                    case 3: error3.InnerText = SUCCESS1; break;
                }
            }
            catch
            {
                throw (new Exception("File Upload Is Fail."));
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
        }
        #endregion
    }
}