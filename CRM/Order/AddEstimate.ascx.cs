using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

namespace Taoqi.Order
{
    public partial class AddEstimate : System.Web.UI.UserControl
    {
        public DataTable MyOrderEstimate;

        private Guid gId = Guid.Empty;
        private string EstimateContext;
        //private string EstimateContext_base64;
        private int EstimateQuality;

        private Guid orderID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Security.isBuyer != 1) return;

            if (!IsPostBack) ViewState["hisURL"] = Request.UrlReferrer.ToString(); 

            if (!Guid.TryParse(Request["id"], out orderID))
                Response.Redirect("~/Order");

        //绑定到Repeater控件
            RPMyHistoryEstimate.DataSource = MyOrderEstimate;
            RPMyHistoryEstimate.DataBind();

            if (IsPostBack && Request.Form["EstimateSubmit"] == "EstimateSubmit_Add")
            {
                EstimateContext = TXT_Estimate.InnerText;
                if (int.TryParse(TXTEstimateQuality.Value, out EstimateQuality) && EstimateQuality > 0 && EstimateQuality < 6 && EstimateContext != null)
                {
                }
                else {
                    //ErrorTxt.InnerText = "请选择星级并且评价内容不能为空。";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AddEstimate", "layer.alert('提示：请选择星级并且评价内容不能为空。',{area: ['385px', '178px'], offset: ['195px', '500px']});", true);
                    return;
                }


                SqlProcs.spTQOrderEstimate_Update(
                    ref gId,
                    orderID,
                    EstimateContext,
                    -1,
                    -1,
                    EstimateQuality,
                    -1
                );
                SqlProcs.spTQOrderDetail_ChangeStatus(orderID, 7);
                Response.Redirect(ViewState["hisURL"].ToString());
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