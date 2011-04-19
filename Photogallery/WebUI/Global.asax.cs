using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WebUI
{
	public class Global : System.Web.HttpApplication
	{

		protected void Application_Start(object sender, EventArgs e)
		{

		}

		protected void Session_Start(object sender, EventArgs e)
		{

		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(object sender, EventArgs e)
		{

		}

		protected void Application_Error(object sender, EventArgs e)
		{
			Exception objErr = Server.GetLastError().GetBaseException();
			string err = "Error Caught in Application_Error event\n" +
					"Error in: " + Request.Url +
					"\nError Message:" + objErr.Message +
					"\nStack Trace:" + objErr.StackTrace;
			EventLog.WriteEntry("Sample_WebApp", err, EventLogEntryType.Error);
			//TODO: Remove output to stdout on deployment
			Response.Write(err);
			Server.ClearError();
		}

		protected void Session_End(object sender, EventArgs e)
		{

		}

		protected void Application_End(object sender, EventArgs e)
		{

		}
	}
}