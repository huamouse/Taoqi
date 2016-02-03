using System;
using System.IO;
using System.Web;
using System.Threading;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Web.SessionState;

namespace Taoqi 
{

	public class Global : System.Web.HttpApplication
	{
	
		private System.ComponentModel.IContainer components = null;

		public Global()
		{
			InitializeComponent();
		}

		protected void Application_OnError(Object sender, EventArgs e)
		{
	
		}
		
		protected void Application_Start(Object sender, EventArgs e)
		{
            //web apiµÄÂ·ÓÉÅäÖÃ
            var config = GlobalConfiguration.Configuration;

            config.Routes.MapHttpRoute(
                name: "API",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

		}
 
		protected void Session_Start(Object sender, EventArgs e)
		{
			SplendidInit.InitSession(this.Context);
		}

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
            if (Application.Count == 0)
            {
                SplendidInit.InitApp(this.Context);
            }
		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AcquireRequestState(Object sender, EventArgs e)
		{
			
			if ( HttpContext.Current.Session != null )
			{
				
				if ( !Sql.IsEmptyString(HttpContext.Current.Session["USER_NAME"]) )
				{
					
					if ( !HttpContext.Current.User.Identity.IsAuthenticated )
						HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(Security.USER_NAME, "Forms"), null);
				}
			}
		}

		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{
			
			try
			{
				Guid gUSER_LOGIN_ID = Security.USER_LOGIN_ID;
				if ( !Sql.IsEmptyGuid(gUSER_LOGIN_ID) )
					SqlProcs.spUSERS_LOGINS_Logout(gUSER_LOGIN_ID);
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
			}

			
			foreach ( string sKey in Session.Keys )
			{
				if ( sKey.StartsWith("TempFile.") )
				{
					string sTempFileName = Sql.ToString(Session[sKey]);
					string sTempPathName = Path.Combine(Path.GetTempPath(), sTempFileName);
					if ( File.Exists(sTempPathName) )
					{
						try
						{
							File.Delete(sTempPathName);
						}
						catch(Exception ex)
						{
							SplendidError.SystemError(new StackTrace(true).GetFrame(0), "Could not delete temp file: " + sTempPathName + ControlChars.CrLf + ex.Message);
						}
					}
				}
			}
		}

		protected void Application_End(Object sender, EventArgs e)
		{
           
             SplendidInit.StopApp(this.Context);
		}

        public override void Init()
        {
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }
			
		#region Web Form Designer generated code

		private void InitializeComponent()
		{    
			this.components = new System.ComponentModel.Container();
		}
		#endregion
	}
}


