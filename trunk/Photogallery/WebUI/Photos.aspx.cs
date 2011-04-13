using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLayer.AbstractControllers;
using BuisnessLayer.ConcreteControllers;

namespace WebUI
{
	public partial class Photos : System.Web.UI.Page
	{
		IPhotoController controller = new PhotoController();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindRepeater();
			}
		}

		

		public void pager_Command(object sender, CommandEventArgs e)
		{
			int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
			pager1.CurrentIndex = currnetPageIndx;
			BindRepeater();
		}

		private void BindRepeater()
		{
			int skip = (pager1.CurrentIndex - 1) * pager1.PageSize;
			int take = pager1.PageSize;

			PhotoList.DataSource = controller.SelectPhotosPage(skip, take);
			PhotoList.DataBind();

			pager1.ItemCount = controller.GetPhotosCount();

		}

		protected void btnTest_click(object s, EventArgs e)
		{
			Button btnSender = s as Button;
			if (btnSender.CommandName == "postback")
			{
				Response.Write("postBack occured.");
				BindRepeater();
			}
			else if (btnSender.CommandName == "ps70")
			{
				pager1.PageSize = 70;
				BindRepeater();
			}
			else
			{
				pager1.PageSize = 15;
				BindRepeater();


			}


		}
	}
}
