using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Taoqi.TQQuote
{
    /// <summary>
    /// QuoteForm 的摘要说明
    /// </summary>
    /// 
    public class QuoteForm : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (Security.isBuyer != 1)
                return;

            //检测用户是否登录，并取用户ID
            Guid UserId = Security.USER_ID;
            if (UserId == Guid.Empty)
                context.Response.Redirect("~/Users/Login.aspx");

            //求购ID
            Guid Quote_Id;
            if(Guid.TryParse(context.Request.Form["btn_quote"], out Quote_Id))
            {
                //初始化检索器
                string Init_QuoteFactory_HtmlName = Quote_Id.ToString() + "Quote_Factory1";
                string Init_QuoteQuote_HtmlName = Quote_Id.ToString() + "Quote_Quote1";
                for (; true; Init_QuoteFactory_HtmlName += "1", Init_QuoteQuote_HtmlName += "1")
                {
                    Guid guid = Guid.Empty;
                    //报价
                    decimal QuoteQuote;
                    bool QuoteQuote_transfer = decimal.TryParse(context.Request[Init_QuoteQuote_HtmlName], out QuoteQuote);
                    //气源地
                    Guid QuoteFactory;
                    bool QuoteFactory_transfer = Guid.TryParse(context.Request[Init_QuoteFactory_HtmlName], out QuoteFactory);

                    //判断跳过或终止
                    if (!QuoteFactory_transfer && !QuoteQuote_transfer)
                        break;
                    else if (!QuoteFactory_transfer || !QuoteQuote_transfer)
                        continue;

                    //SqlProcs.spTQQuotePrice_Update(
                    //    ref guid,
                    //    Quote_Id,
                    //    QuoteQuote,
                    //    false,
                    //    QuoteFactory
                    //    );
                }
            }

            context.Response.Redirect("/index.html");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}