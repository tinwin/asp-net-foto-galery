using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLayer.AbstractControllers;
using BuisnessLayer.ConcreteControllers;
using Common;
using Photogallery;

namespace WebUI
{
	public partial class Photos : System.Web.UI.Page
	{
		readonly IPhotoController _photoController = Windsor.Instance.Resolve<IPhotoController>();
		readonly IEnvironment _environment = Windsor.Instance.Resolve<IEnvironment>();

		Guid _currentUserId;

		protected void Page_Load(object sender, EventArgs e)
		{
			_currentUserId = (_environment.CurrentClient == null) ? Guid.Empty : _environment.CurrentClient.UserId;
			PhotoList.ItemDataBound += (s, ee) =>
           	{
				var toolbar = ee.Item.FindControl("PhotoToolbar");
				var dataItem = ee.Item.DataItem as IPhoto;
				if (dataItem.OwningUser.UserId != _currentUserId)
					toolbar.Visible = false;
           	};

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

			int albumId;
			Guid userId=new Guid();
			//Show photos from album
			if (int.TryParse(Request.QueryString["album"], out albumId))
			{
				PhotoList.DataSource = _photoController.SelectPhotosPage(albumId, skip, take);
				pager1.ItemCount = _photoController.GetPhotosCount(albumId);
			}
			//Show photos by user
			if (Utilites.TryParse(Request.QueryString["user"], out userId))
			{
				PhotoList.DataSource = _photoController.SelectPhotosPage(userId, skip, take);
				pager1.ItemCount = _photoController.GetPhotosCount(userId);
			}
			//Show all photos
			else
			{
				PhotoList.DataSource = _photoController.SelectPhotosPage(skip, take);
				pager1.ItemCount = _photoController.GetPhotosCount();
			}
			PhotoList.DataBind();

			

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
