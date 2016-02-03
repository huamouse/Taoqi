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
    public partial class BossView_EditUser : SplendidPage
    {
        private Guid AccountID;
        private Guid C_UserID;
        private Guid C_ClientID;

        private string new_Password;
        private string new_ReapeterPassword;

        protected string Mobile { get; set; }
        protected string RealName { get; set; }
        protected string Password{get;set;}
        protected string EMail { get; set; }
        protected string C_Weixin { get; set; }
        protected string C_QQ { get; set; }
        protected string C_Icon { get; set; }
        protected string C_Role { get; set; }
        protected string BossType { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid.TryParse(Request.QueryString["id"], out AccountID);
            BossType = Security.UserType;

            AccountInformation();

            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.Form["btnLogin"]))
                {
                    RealName = txtRealName.Value.Trim();
                    EMail = txtEMail.Value.Trim();
                    C_QQ = txtQQ.Value.Trim();
                    C_Weixin = txtWeixin.Value.Trim();
                    Mobile = txtUSER_PHONE.Value.Trim();
                    C_Role = GetUserType(BossType);
                    new_Password = NewPassword.Value;
                    new_ReapeterPassword = RepeatNewPassword.Value;
                    saveInformation();
                }
            }
                
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
                    cmd.CommandText = "SELECT * FROM vwTQAccount_List " +
                                        "WHERE ID = @AccountID";
                    Sql.AddParameter(cmd, "@AccountID", AccountID);

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            //取数据,并且操作数据，反映到页面
                            DataRow row = dt.Rows[0];

                            Guid.TryParse(row["C_UserID"].ToString(), out C_UserID);
                            Guid.TryParse(row["C_ClientID"].ToString(), out C_ClientID);
                            Mobile = row["USER_NAME"].ToString();
                            RealName = row["LAST_NAME"].ToString();
                            EMail = row["EMAIL1"].ToString();
                            C_QQ = row["C_QQ"].ToString();
                            C_Weixin = row["C_Weixin"].ToString();
                            C_Icon = row["C_Icon"].ToString();
                            C_Role = row["C_Role"].ToString();

                            //如果是GET请求需要初始化填框为客户信息
                            if (!IsPostBack)
                                showInformation();
                        }
                    }

                }
            }
        }


        //保存数据
        protected void saveInformation()
        {
            //必填项
            if (string.IsNullOrEmpty(RealName) || string.IsNullOrEmpty(C_QQ) || string.IsNullOrEmpty(Mobile))
            {
                lblError.Text = "请将上面带星号的必填项填写完整。";
                return;
            }


            //当输入密码的时候允许改密码
            if (new_Password.Length != 0 && new_ReapeterPassword.Length != 0)
            {
                if (new_Password != new_ReapeterPassword)
                {
                    this.lblError.Text = "提示：两次输入密码不一致。";
                    return;
                }

                string hashPassword = Security.HashPassword(new_Password);

                Guid id = Guid.Empty;
                SqlProcs.spUserAdd(ref id, Mobile, hashPassword, RealName);
                SqlProcs.spUserUpdate(id, RealName, EMail, C_QQ, C_Weixin, C_Icon);
            }

            SqlProcs.spTQAccountRole(AccountID, C_Role);

            lblError.Text = "账户信息已保存！";
        }

        protected void showInformation()
        {
            txtRealName.Value = RealName;
            txtEMail.Value = EMail;
            txtQQ.Value = C_QQ;
            txtWeixin.Value = C_Weixin;
            txtUSER_PHONE.Value = Mobile;

            ShowUserType(BossType,C_Role);
        }


        private void ShowUserType(string bossType,string userType)
        {
            if (bossType == "0000" || bossType.Length < 4)
                div_UserType.Visible = false;
            else
            {
                //先用bossType判断显示哪些可选的权限
                //if (!(bossType.Length > 0 && bossType[0] == '1'))
                //    div_CGY.Visible = false;

                //if (!(bossType.Length > 1 && bossType[1] == '1'))
                //    div_XSY.Visible = false;

                //if (!(bossType.Length > 2 && bossType[2] == '1'))
                //    div_CZYG.Visible = false;

                //if (!(bossType.Length > 3 && bossType[3] == '1'))
                //    div_JSY.Visible = false;

                if (bossType[0] == '0' && bossType[2] == '0')
                {
                    div_CGY.Visible = false;
                    div_CZYG.Visible = false;
                }

                if (bossType[1] == '0' && bossType[3] == '0')
                {
                    div_XSY.Visible = false;
                    div_JSY.Visible = false;
                }

                //再用userType判断勾选哪些权限
                if (userType.Length > 0 && userType[0] == '1')
                    CGY.Checked = true;
                else
                    CGY.Checked = false;

                if (userType.Length > 1 && userType[1] == '1')
                    XSY.Checked = true;
                else
                    XSY.Checked = false;

                if (userType.Length > 2 && userType[2] == '1')
                    CZYG.Checked = true;
                else
                    CZYG.Checked = false;

                if (userType.Length > 3 && userType[3] == '1')
                    JSY.Checked = true;
                else
                    JSY.Checked = false;
            }
            
        }

        //对用户类型判断，二进制表示法
        private string GetUserType(string bossType)
        {
            string userType = "";
            char[] charArry_userType = new char[4];

            //先用userType判断勾选了哪些权限
            if (CGY.Visible && CGY.Checked)
                userType += "1";
            else
                userType += "0";

            if (XSY.Visible && XSY.Checked)
                userType += "1";
            else
                userType += "0";

            if (CZYG.Visible && CZYG.Checked)
                userType += "1";
            else
                userType += "0";

            if (JSY.Visible && JSY.Checked)
                userType += "1";
            else
                userType += "0";

            //charArry_userType = userType.ToCharArray();

            //再用bossType覆盖没有的权限
            //if (!(bossType.Length > 0 && bossType[0] == '1'))
            //    charArry_userType[0] = '0';

            //if (!(bossType.Length > 1 && bossType[1] == '1'))
            //    charArry_userType[1] = '0';

            //if (!(bossType.Length > 2 && bossType[2] == '1'))
            //    charArry_userType[2] = '0';

            //if (!(bossType.Length > 3 && bossType[3] == '1'))
            //    charArry_userType[3] = '0';


            //userType = new String(charArry_userType);

            return userType;
        }

    }
}