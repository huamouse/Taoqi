using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;

namespace Taoqi.About
{
    public partial class management : SplendidPage
    {

        protected string C_qyjj;
        protected string C_qywh;
        protected string C_lxwm;
        protected string C_yqlj;
        protected string C_flsm;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Security.isAdmin)//权限检查
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

            DB_Process();
            if (IsPostBack)
            {
                switch (Request.Form["submit"])
                {
                    case "qyjj":
                        C_qyjj = Request.Form["UE_qyjj"];
                        break;
                    case "qywh":
                        C_qywh = Request.Form["UE_qywh"];
                        break;
                    case "lxwm":
                        C_lxwm = Request.Form["UE_lxwm"];
                        break;
                    case "yqlj":
                        C_yqlj = Request.Form["UE_yqlj"];
                        break;
                    case "flsm":
                        C_flsm = Request.Form["UE_flsm"];
                        break;
                        
                }

                SqlProcs.spTQAbout(
                    C_qyjj
                    , C_qywh
                    , C_lxwm
                    , C_yqlj
                    , C_flsm
                    );
            }
        }


        protected void DB_Process()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                con.Open();
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM TQAbout ";

                    using (DbDataAdapter da = dbf.CreateDataAdapter())
                    {
                        ((IDbDataAdapter)da).SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);

                            if(dt.Rows.Count > 0)
                            {
                                DataRow row = dt.Rows[0];

                                C_qyjj = row["C_qyjj"].ToString();
                                C_qywh = row["C_qywh"].ToString();
                                C_lxwm = row["C_lxwm"].ToString();
                                C_yqlj = row["C_yqlj"].ToString();
                                C_flsm = row["C_flsm"].ToString();
                            }
                        }
                    }
                }
            }

        }
    }
}