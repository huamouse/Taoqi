using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Users.BindingEmail
{
    public partial class BindingUserEmail_New : System.Web.UI.UserControl
    {
        public int Tick { get; set; }   // 倒计时时间（s）
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConfirmBind_Click(object sender, EventArgs e)
        {
            if (Sql.ToString(Session["EmailValidCode"]).Trim() == Sql.ToString(txtEmailCode.Text).Trim().ToUpper() 
                &&!Sql.IsEmptyString(txtEmailCode.Text))
            {
                if (Sql.ToString(Session["EmailFOrValide"]) != Sql.ToString(txtEmail.Text).Trim().ToUpper())
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "jsEnd", "<script>layer.alert('绑定失败,验证邮箱和发送验证码邮箱不一致！！')</script>");
                    return;
                }
                //验证成功,设置邮件
                Security.EMAIL1 = this.txtEmail.Text;
                Security.User_EmailIsActive = true;
                //将绑定成功的记录，保存到数据库中
                SqlProcs.spUSERSEmail_Update(Security.USER_ID, Security.EMAIL1, Security.User_EmailIsActive);
                Response.Redirect("BindingUserEmail.aspx?validSucess=1");  
            }
            else//验证失败
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "jsEnd", "<script>layer.alert('验证码验证失败！')</script>");
           
                return;
            }

        }
        //获取邮件验证码
        protected void btnGetCode1_Click(object sender, EventArgs e)
        {
            Tick = 5;
            if (!Sql.IsEmptyString(this.txtEmail.Text.Trim()))
            {
                string strValidCode = string.Empty;
               //获取邮箱随机验证码
                strValidCode=VerificationCode.RandomString(6);
                Session["EmailValidCode"] = strValidCode;
                Session["EmailFOrValide"] = this.txtEmail.Text.Trim().ToUpper();
                //发送验证码邮件
                if (EmailBindHelper.SendBindEmail(this.txtEmail.Text,strValidCode))
                {
                    this.lbErrorForCode.Text = "邮件已发送";
                }
                else
                {
                    this.lbErrorForCode.Text = "邮件发送失败";
                }
                
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "jsEnd", "<script>layer.alert('邮件地址不能为空！')</script>");
                return;
            }
        }

      
    }
}