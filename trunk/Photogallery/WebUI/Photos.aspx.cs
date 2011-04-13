using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLayer.ConcreteControllers;

namespace WebUI
{
	public partial class Photos : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var controller = new PhotoController();
			PhotoList.DataSource = controller.SelectPhotosPage(0, 10);
			PhotoList.DataBind();
		}
	}
}
