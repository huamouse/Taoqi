using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.OrderSell
{
    public class ActionOrderDetail : WebControl
    {
        public ActionOrderDetail()
        {
            Load += (src, args) =>
            {
                if (Security.isSeller != 1) return;
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

        public string ShippingUrl
        {
            get
            {
                return ViewState["ShippingUrl"].ToString();
            }
            set
            {
                ViewState["ShippingUrl"] = value;
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
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                    writer.AddAttribute("ng-click", "modifyModal('" + OrderDetailID + "')");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("修改价格");
                    writer.RenderEndTag();
                    writer.WriteLine();
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnSecond btnOrange");
                    writer.AddAttribute("Name", "btnEdit");
                    writer.AddAttribute("data-toggle", "modal");
                    writer.AddAttribute("data-target", "#fahuoModal");
                    writer.AddAttribute("value", OrderDetailID);
                    writer.RenderBeginTag(HtmlTextWriterTag.Button);
                    writer.Write("选择车辆");
                    writer.RenderEndTag();
                    break;
                case 2:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                    writer.AddAttribute("Name", "btnEdit");
                    writer.AddAttribute("data-toggle", "modal");
                    writer.AddAttribute("data-target", "#fahuoModal");
                    writer.AddAttribute("value", OrderDetailID);
                    writer.RenderBeginTag(HtmlTextWriterTag.Button);
                    writer.Write("选择车辆");
                    writer.RenderEndTag();
                    break;
                //case 3:
                //    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                //    writer.AddAttribute("Name", "btnEdit");
                //    writer.AddAttribute("value", OrderDetailID);
                //    writer.RenderBeginTag(HtmlTextWriterTag.Button);
                //    writer.Write("发车");
                //    writer.RenderEndTag();
                //    break;
                case 4:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnOrange");
                    writer.AddAttribute("ng-click", string.Format("viewManifest('{0}', '{1}', '{2}')", ShippingUrl, Driver, DriverTel));
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("查看货单");
                    writer.RenderEndTag();
                    writer.WriteLine();
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "btn btnSecond btnOrange");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, string.Format("/map2.html?id={0}", CarID));
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                    writer.Write("查看物流");
                    writer.RenderEndTag();
                    break;
                default:
                    break;
            }
        }

    }
}
