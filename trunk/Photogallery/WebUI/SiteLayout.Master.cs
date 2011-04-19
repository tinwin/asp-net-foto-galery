using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLayer.AbstractControllers;
using Common;
using DAL.EFDataProvider;
using DAL.EFDataProvider.Repositories;

namespace WebUI
{
	public partial class SiteLayout : System.Web.UI.MasterPage
	{
		public Guid userGuid;

		protected void Page_Load(object sender, EventArgs e)
		{
			var currentUser = Windsor.Instance.Resolve<IEnvironment>().CurrentClient;

			userGuid = (currentUser == null)?Guid.Empty: currentUser.UserId;           
		    
		}
	}
}
