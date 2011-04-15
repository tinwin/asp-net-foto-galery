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
using DAL.AbstractEntities;
using Photogallery;

namespace WebUI
{
	public partial class EditPhoto : System.Web.UI.Page
	{
		private IPhotoController controller = Windsor.Instance.Resolve<IPhotoController>();
		private IAbstractGalleryProvider _provider = Windsor.Instance.Resolve<IAbstractGalleryProvider>();

		protected void Page_Load(object sender, EventArgs e)
		{
			int photoId;
			//If user has tried to edit existing photo
			//TODO: Check user rights on photo
			if (int.TryParse(Request.QueryString["id"], out photoId) && photoId > 0)
			{
				
				FormViewPhoto.DataSource = new List<IPhoto> { _provider.PhotoRepository.GetPhotoById(photoId) };
			}
			//If user has tried to create new photo
			//TODO: implement owner and album initialization
			else
			{
				var albumController = Windsor.Instance.Resolve<IAlbumController>();
				var userController = Windsor.Instance.Resolve<IUserController>();

				FormViewPhoto.DataSource = new Photogallery.Photo
               	{
               		OwningUser = userController.GetUserByGuid(new Guid("29d25edd-7279-4a94-87b7-874c4b34827c")),
					HostAlbum = albumController.GetAlbumById(1)
               	};
			}
			var tagsList = (CheckBoxList)FormViewPhoto.FindControl("TagsList");

			tagsList.DataSource = _provider.TagRepository.GetAllTags();

			FormViewPhoto.DataBind();
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			//Most of fields are filled by binding
			IPhoto photo = (IPhoto)FormViewPhoto.DataItem;
			//But not uploaded photo
			var photoFile = (FileUpload)FormViewPhoto.FindControl("PhotoFile");
			//Photo hasn't image and user hasn't uploaded image
			if (photoFile.FileContent.Length<=0 && photo.OriginalPhoto==null)
			{
				return;
			}

			if (photoFile.FileContent.Length > 0)
			{
				photo.OriginalPhoto = new Bitmap(photoFile.FileContent);
				photo.OptimizedPhoto = new Bitmap(photoFile.FileContent);
				photo.PhotoThumbnail = new Bitmap(photoFile.FileContent);
			}
			
			//Creation date shoud be set manualy
			photo.AdditionDate = DateTime.Now;

			
			_provider.PhotoRepository.UpdatePhoto(photo);
		}
	}
}
