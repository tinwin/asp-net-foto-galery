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
		readonly IPhotoController _controller = Windsor.Instance.Resolve<IPhotoController>();

		protected void Page_Load(object sender, EventArgs e)
		{
			try
			{
				int photoId = int.Parse(Request.Params["id"]);
				CurrentPhoto = _controller.GetPhotoById(photoId);
			}
			catch (NullReferenceException ee)
			{
				throw new InvalidOperationException("Page has got wrong url GET parameter(s)", ee);
			}
		}
	}
}
