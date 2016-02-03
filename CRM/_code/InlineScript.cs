using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Taoqi
{
	// http://weblogs.asp.net/infinitiesloop/archive/2007/09/17/inline-script-inside-an-asp-net-ajax-updatepanel.aspx
	// 02/05/2008   An AJAX UpdatePanel does not handle inline scripts.  The workaround is to use a user control 
	// to wrap the script so that it can be registered with the ScriptManager. 
	public class InlineScript : Control
	{
		public bool PageClientScript { get; set; }

		protected override void Render(HtmlTextWriter writer)
		{
			ScriptManager sm = ScriptManager.GetCurrent(Page);
			// 05/02/2008   Make sure ScriptManager exists.
			if ( sm != null && sm.IsInAsyncPostBack )
			{
				StringBuilder sb = new StringBuilder();
				base.Render(new HtmlTextWriter(new StringWriter(sb)));
				string script = sb.ToString();
				// 12/27/2012   To register a script block every time that an asynchronous postback occurs, use the RegisterClientScriptBlock(Page, Type, String, String, Boolean) overload of this method. 
				// http://msdn.microsoft.com/en-us/library/bb338357.aspx
				if ( PageClientScript )
					ScriptManager.RegisterClientScriptBlock(Page, typeof(InlineScript), UniqueID, script, true);
				else
					ScriptManager.RegisterStartupScript(this, typeof(InlineScript), UniqueID, script, false);
			}
			else
			{
				base.Render(writer);
			}
		}
	}
}

