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
    public partial class FirstEstimate : System.Web.UI.UserControl
    {
        private Guid gId = Guid.Empty;
        private string EstimateContext;
        //private string EstimateContext_base64;
        protected int EstimateLogistics;
        protected int EstimateService;
        protected int DriverService;
        protected string MostEstimate;

        private Guid orderID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Security.isBuyer != 1) return;

            if (!IsPostBack) ViewState["hisURL"] = Request.UrlReferrer.ToString();

            if (!Guid.TryParse(Request["id"], out orderID))
                Response.Redirect("~/Order");

            if (IsPostBack && Request.Form["EstimateSubmit"] == "EstimateSubmit_First")
            {
                EstimateContext = TXT_Estimate.InnerText;
                if (int.TryParse(TXTEstimateLogistics.Value, out EstimateLogistics) && EstimateLogistics > 0 && EstimateLogistics < 6 &&
                    int.TryParse(TXTEstimateService.Value, out EstimateService) && EstimateService > 0 && EstimateService < 6 &&
                    int.TryParse(TXTDriverService.Value, out DriverService) && DriverService > 0 && DriverService < 6 && 
                    EstimateContext != null)
                {
                }
                else
                {
                    //ErrorTxt.InnerText = "请选择星级并且评价内容不能为空。";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "FirstEstimate", "layer.alert('提示：请选择星级并且评价内容不能为空。',{area: ['385px', '178px'], offset: ['195px', '500px']});", true);
                    return;
                }

                SqlProcs.spTQOrderEstimate_Update(
                    ref gId,
                    orderID,
                    EstimateContext,
                    EstimateLogistics,
                    EstimateService,
                    -1,
                    DriverService
                );
                SqlProcs.spTQOrderDetail_ChangeStatus(orderID, 6);
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