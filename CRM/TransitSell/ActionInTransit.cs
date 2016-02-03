using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.TransitSell
{
    public class ActionInTransit : WebControl
    {
        public ActionInTransit()
        {
            Load += (src, args) =>
            {
                this.Visible = (Security.isSeller == 1);
                if (!this.Visible) return;
            };
        }

        public string C_Status
        {
            get
            {
                return ViewState["C_Status"].ToString();
            }
            set
            {
                ViewState["C_Status"] = value;
            }
        }

        public string TransitID
        {
            get
            {
                return ViewState["TransitID"].ToString();
            }
            set
            {
                ViewState["TransitID"] = value;
            }
        }
        
        public string LandingUrl
        {
            get
            {
                return ViewState["LandingUrl"].ToString();
            }
            set
            {
                ViewState["LandingUrl"] = value;
            }
        }

        public string Driver
        {
            get
            {
                return ViewState["Driver"].ToString();
            }
            set
            {
                ViewState["Driver"] = value;
            }
        }

        public string DriverTel
        {
            get
            {
                return ViewState["DriverTel"].ToString();
            }
            set
            {
                ViewState["DriverTel"] = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            int nStatus = 0;
            if (!int.TryParse(C_Status, out nStatus))
                return;

            if (nStatus != 2 && nStatus != 3 && nStatus != 4 && nStatus != 5)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "btnGray1");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, "TransitSell_btnDelete");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, TransitID);
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return layer.confirm('提示：确认删除吗？');");
                //writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "window.swal('提示：确认删除吗？');");
                writer.RenderBeginTag(HtmlTextWriterTag.Button);
                writer.Write("删除");
                writer.RenderEndTag();
            }

            if (nStatus == 2 || nStatus == 3)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "btnOrange");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, "TransitSell_ModelView");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, TransitID);
                writer.RenderBeginTag(HtmlTextWriterTag.Button);
                writer.Write("查看抢购");
                writer.RenderEndTag();
            }

            if (nStatus == 5)
            {
                writer.WriteLine();
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "btnOrange");
                writer.AddAttribute("ng-click", string.Format("viewLanding('{0}', '{1}', '{2}')", LandingUrl, Driver, DriverTel));
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.Write("查看水单");
                writer.RenderEndTag();
            }
        }
    }
}

