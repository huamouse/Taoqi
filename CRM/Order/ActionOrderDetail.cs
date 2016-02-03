using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.Order
{
    public class ActionOrderDetail : WebControl
    {
        public ActionOrderDetail()
        {
            Load += (src, args) =>
            {
                this.Visible = (Security.isBuyer == 1);
                if (!this.Visible) return;
            };
        }

        public int Status
        {
            get
            {
                return int.Parse(ViewState["Status"].ToString());
            }
            set
            {
                ViewState["Status"] = value;
            }
        }

        public string OrderDetailID
        {
            get
            {
                return ViewState["OrderDetailID"].ToString();
            }
            set
            {
                ViewState["OrderDetailID"] = value;
            }
        }

        public string CarID
        {
            get
            {
                return ViewState["CarID"].ToString();
            }
            set
            {
                ViewState["CarID"] = value;
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
            switch (Status)
            {
                case 0:
                case 1:
                    if (Status == 1)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                        writer.AddAttribute("Name", "Confirm");
                        writer.AddAttribute("value", OrderDetailID);
                        writer.RenderBeginTag(HtmlTextWriterTag.Button);
                        writer.Write("价格确认");
                        writer.RenderEndTag();
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnSecond btnGray1");
                    }
                    else
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnGray1");

                    writer.AddAttribute("Name", "Delete");
                    writer.AddAttribute("value", OrderDetailID);
                    writer.RenderBeginTag(HtmlTextWriterTag.Button);
                    writer.Write("取消订单");
                    writer.RenderEndTag();
                    break;
                case 4:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/map2.html?id={0}", CarID));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("查看物流");
                    writer.RenderEndTag();
                    break;
                case 5:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                    writer.AddAttribute("ng-click", string.Format("viewLanding('{0}', '{1}', '{2}')", LandingUrl, Driver, DriverTel));
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("查看水单");
                    writer.RenderEndTag();
                    writer.WriteLine();
                    writer.AddAttribute("class", "btn btnSecond btnOrange");
                    writer.AddAttribute("data-ng-click", string.Format("deliverModal('{0}')", OrderDetailID));
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("确认收货");
                    writer.RenderEndTag();
                    break;
                case 6:
                case 7:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                    writer.AddAttribute("ng-click", string.Format("viewLanding('{0}', '{1}', '{2}')", LandingUrl, Driver, DriverTel));
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("查看水单");
                    writer.RenderEndTag();
                    if (Status == 6)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("estimate.aspx?id={0}", OrderDetailID));
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write("评价");
                        writer.RenderEndTag();
                    }
                    else if (Status == 7)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("estimate.aspx?id={0}", OrderDetailID));
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.Write("追加评价");
                        writer.RenderEndTag();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
