using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

namespace Taoqi.Users
{
    public partial class PersonalInfo : SplendidPage
    {
        private Guid C_UserID;

        protected string Phone { get; set; }
        protected string EMail { get; set; }
        protected string RealName { get; set; }
        protected string C_Weixin { get; set; }
        protected string C_QQ { get; set; }
        protected string C_Icon { get; set; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            C_UserID = Security.USER_ID;
            Phone = lblUSER_PHONE.Text = Security.UserMobile;

            AccountInformation();
            
            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.Form["btnLogin"]))
                {
                    EMail = txtEMail.Value.Trim();
                    RealName = txtRealName.Value.Trim();
                    C_QQ = txtQQ.Value.Trim();
                    C_Weixin = txtWeixin.Value.Trim();
                    saveInformation();
                }
            }

            showUserIcon();    
        }

        //从数据库查数据
        protected void AccountInformation()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM vwTQUsers " +
                                        "WHERE ID = @USER_ID";
                    Sql.AddParameter(cmd, "@USER_ID", C_UserID);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            //取数据,并且操作数据，反映到页面
                            DataRow row = dt.Rows[0];

                            RealName = row["LAST_NAME"].ToString();
                            EMail = row["EMAIL1"].ToString();
                            C_QQ = row["C_QQ"].ToString();
                            C_Weixin = row["C_Weixin"].ToString();
                            C_Icon = row["C_Icon"].ToString();

                            //如果是GET请求需要初始化填框为客户信息
                            if (!IsPostBack)
                            {
                                txtRealName.Value = RealName;
                                txtEMail.Value = EMail;
                                txtQQ.Value = C_QQ;
                                txtWeixin.Value = C_Weixin;
                            }
                        }
                    }
                }
            }
        }


        //保存数据
        protected void saveInformation()
        {
            //必填项
            if (string.IsNullOrEmpty(RealName) || string.IsNullOrEmpty(C_QQ))
            {
                lblError.Text = "请将上面带星号的必填项填写完整。";
                return;
            }

            try
            {
                SqlProcs.spUserUpdate(Security.USER_ID,
                    RealName,
                    EMail,
                    C_QQ,
                    C_Weixin,
                    C_Icon);
                lblError.Text = "账户信息已保存！";
                //Response.Write("<script>alert(\"账户信息已保存\")</script>");
            }
            catch (Exception)
            {
                lblError.Text = "账户信息保存失败！";
            }
        }

        protected void showUserIcon()
        {
            if (!string.IsNullOrEmpty(C_Icon))
            {
                ClientImg.Src = WebConfigurationManager.AppSettings["UploadPath_image"] + C_Icon;
            }
        }

        //对上传的文件判断，并且上传，上传成功返回true,反之返回false
        protected void UploadAttachment()
        {
            string btnFileNum = "btnFile8";
            //判断上传功能是否能正常执行
            if (Request.Files.Count != 1)
                throw (new Exception("File Upload Is Fail."));


            //对上传类型的判断
            string FileType = Request.Files[btnFileNum].ContentType;
            string FilePostFix = ".";

            if (FileType != "image/jpeg" && FileType != "image/jpg" && FileType != "image/png")
            {
                return;
            }
            else
            {
                FilePostFix += "PNG";
            }

            //上传判断OK后，保存
            string virtual_UploadURL = string.Format("{0}Attachment{1}{2}{3}", WebConfigurationManager.AppSettings["UploadPath_image"], C_UserID, "UserIcon", FilePostFix);
            string absolute_UploadURL = Request.MapPath(virtual_UploadURL);
            try
            {
                Request.Files[btnFileNum].SaveAs(absolute_UploadURL);

                C_Icon = virtual_UploadURL;


                DbProviderFactory dbf = DbProviderFactories.GetFactory();
                using (IDbConnection con = dbf.CreateConnection())
                {
                    con.Open();
                    using (IDbCommand cmd = con.CreateCommand())
                    {

                        cmd.CommandText = "UPDATE USERS SET C_Icon = @C_Icon WHERE ID = @UserID";

                        Sql.AddParameter(cmd, "@C_Icon", virtual_UploadURL);
                        Sql.AddParameter(cmd, "@UserID", C_UserID);

                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            cmd.ExecuteScalar();
                        }
                    }
                }


                showUserIcon();

            }
            catch
            {
                throw (new Exception("File Upload Is Fail."));
            }
        }
    }
}