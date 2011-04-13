using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLayer.AbstractControllers;
using Common;
using Photogallery;

namespace WebUI
{
	public partial class Photo : System.Web.UI.Page
	{
		public IPhoto CurrentPhoto;
		IPhotoController _controller = Windsor.Instance.Resolve<IPhotoController>();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.Params["id"] == null)
				throw new HttpException("", 404);
			int photoId = int.Parse(Request.Params["id"]);
			CurrentPhoto = _controller.GetPhotoById(photoId);
		}
	}
}
