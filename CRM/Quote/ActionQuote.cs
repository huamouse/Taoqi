using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Taoqi.TQQuote
{
    public class ActionQuote : WebControl
    {
        public ActionQuote()
        {
            Load += (src, args) =>
            {
                this.Visible = (Security.isBuyer == 1);
                if (!this.Visible)
                    return;
            };
        }

        public string QuoteCheck
        {
            get
            {
                return ViewState["QuoteCheck"].ToString();
            }
            set
            {
                ViewState["QuoteCheck"] = value;
            }
        }
        public string QuoteId
        {
            get
            {
                return ViewState["QuoteId"].ToString();
            }
            set
            {
                ViewState["QuoteId"] = value;
            }
        }
        protected override void RenderContents(HtmlTextWriter writer)
        {
            int nQuoteCheck = 0;
            if ( !int.TryParse(QuoteCheck, out nQuoteCheck))
                return;

            if (nQuoteCheck != 3)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "btnGray1");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, "Quote_btnDelete");
                writer.AddAttribute(HtmlTextWriterAttribute.Value, QuoteId);
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return confirm('提示：确认删除吗？');");
                writer.RenderBeginTag(HtmlTextWriterTag.Button);
                writer.Write("删除");

                writer.RenderEndTag();
            }
        }
    }

    public class QuoteButton : LinkButton
    {
        [DefaultValue(0)]
        public int Status
        {
            get
            {
                return (ViewState["Status"] == null) ? 0 : (int)ViewState["Status"];
            }
            set
            {
                ViewState["Status"] = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            switch (Status)
            {
                case 0:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "txtGray");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("等待审核");
                    writer.RenderEndTag();
                    break;
                case 1:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "txtGray");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("审核失败");
                    writer.RenderEndTag();
                    break;
                case 2:
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "txtGray");
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.Write("等待报价");
                    writer.RenderEndTag();
                    break;
                case 3:
                case 4:
                    base.Render(writer);
                    break;
                default:
                    break;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            switch (Status)
            {
                case 3:
                    writer.Write("<div class=\"btnOrange\">查看报价</div>");
                    base.RenderContents(writer);
                    break;
                case 4:
                    writer.Write("<div class=\"btnOrange\">查看成交</div>");
                    base.RenderContents(writer);
                    break;
                default:
                    break;
            }
        }
    }
}
