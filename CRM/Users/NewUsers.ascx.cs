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
    public partial class NewUsers : System.Web.UI.UserControl
    {
        private Guid C_UserID;

        protected string Phone { get; set; }
        protected string EMail { get; set; }
        protected string RealName { get; set; }
        protected string C_Weixin { get; set; }
        protected string C_QQ { get; set; }
        protected string Password { get; set; }
        protected string Password2 { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            C_UserID = Sql.ToGuid(Request["id"]);
            NewInformation();

            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.Form["Save"]))
                {
                    RealName = textrealname.Value.Trim();
                    Phone = textphone.Value.Trim();
                    EMail = textemail.Value.Trim();
                    C_QQ = textqq.Value.Trim();
                    C_Weixin = textwechat.Value.Trim();
                    Password = textpassword.Value.Trim();
                    Password2 = textpassword2.Value.Trim();
                    SaveInformation();
                }
            }

        }

        //从数据库查数据
        protected void NewInformation()
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
                            Phone = row["PHONE_MOBILE"].ToString();

                            //如果是GET请求需要初始化填框为客户信息
                            if (!IsPostBack)
                            {
                                textrealname.Value = RealName;
                                textemail.Value = EMail;
                                textqq.Value = C_QQ;
                                textwechat.Value = C_Weixin;
                                textphone.Value = Phone;
                            }
                        }
                    }

                }
            }
        }


        //保存数据
        protected void SaveInformation()
        {
            //必填项
            if (string.IsNullOrEmpty(RealName) || string.IsNullOrEmpty(Phone) || string.IsNullOrEmpty(C_QQ))
            {
                lblError.Text = "请将上面带星号的必填项填写完整。";
                return;
            }

            if(Password!=Password2){
                lblError.Text = "两次输入密码请一致。";
                return;
            }

            try
            {
                if (Password.Length == 0)
                {
                    SqlProcs.spUserAdminUpdate(C_UserID, RealName, EMail, C_QQ, C_Weixin, Phone, "");
                }
                else
                {
                    SqlProcs.spUserAdminUpdate(C_UserID, RealName, EMail, C_QQ, C_Weixin, Phone, Security.HashPassword(Password));
                }

                lblError.Text = "账户信息已保存！";
                //Response.Write("<script>alert(\"账户信息已保存\")</script>");
            }
            catch (Exception)
            {
                lblError.Text = "账户信息保存失败！";
            }
        }
    }
}