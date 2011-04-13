using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BuisnessLayer.AbstractControllers;
using BuisnessLayer.ConcreteControllers;
using Common;
using Photogallery;

namespace WebUI
{
	public partial class AddPhoto : System.Web.UI.Page
	{

		protected void Page_Load(object sender, EventArgs e)
		{
			
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			var albumController = Windsor.Instance.Resolve<IAlbumController>();
			var userController = Windsor.Instance.Resolve<IUserController>();

			IPhoto photo = new Photo
           	{
           		OriginalPhoto = new Bitmap(PhotoFile.FileContent),
				AdditionDate = DateTime.Now,
                HostAlbum = albumController.GetAlbumById(1),
				OwningUser = userController.GetUserByGuid(new Guid("29d25edd-7279-4a94-87b7-874c4b34827c")),
				PhotoTitle = "",
                PhotoDescription = "",
				OptimizedPhoto = new Bitmap(PhotoFile.FileContent),
				PhotoThumbnail = new Bitmap(PhotoFile.FileContent)
           	};
			var controller = new PhotoController();
			controller.AddNew(photo);
		}
	}
}
