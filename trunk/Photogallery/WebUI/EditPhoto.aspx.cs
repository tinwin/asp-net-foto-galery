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
		private IPhotoController _photoController = Windsor.Instance.Resolve<IPhotoController>();
		private ITagController _tagController = Windsor.Instance.Resolve<ITagController>();
		private IUserController _userController = Windsor.Instance.Resolve<IUserController>();
		private IAlbumController _albumController = Windsor.Instance.Resolve<IAlbumController>();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				int photoId;
				var allTags = _tagController.GetAllTags();
				//If user has tried to edit existing photo
				//TODO: Check user rights on photo
				if (int.TryParse(Request.QueryString["id"], out photoId) && photoId > 0)
				{
					IPhoto photo = _photoController.GetPhotoById(photoId);

					PhotoId.Value = photo.PhotoId.ToString();
					PhotoTitle.Text = photo.PhotoTitle;
					PhotoDescription.Text = photo.PhotoDescription;
					PhotoImage.Src += photo.PhotoId;

					foreach (var tag in allTags)
						TagsList.Items.Add(new ListItem
						{
							Text = tag.TagTitle,
							Value = tag.TagId.ToString(),
							Selected = photo.PhotoTags.Where(t => t.TagId == tag.TagId).Any()
						});
				}
				else
				{
					foreach (var tag in allTags)
						TagsList.Items.Add(new ListItem
						{
							Text = tag.TagTitle,
							Value = tag.TagId.ToString()
						});
				}
			}
		}

		protected void Button1_Click(object sender, EventArgs e)
		{

			int photoId;
			if (int.TryParse(PhotoId.Value, out photoId))
			{
				IPhoto photo;
				if (photoId > 0)
					photo = _photoController.GetPhotoById(photoId);
				else
				{
					photo = new Photogallery.Photo();
					photo.PhotoId = photoId;
					photo.AdditionDate = DateTime.Now;
				}

				//Common properties
				//TODO: implement owner and album initialization
				photo.OwningUser = _userController.GetUserByGuid(new Guid("29d25edd-7279-4a94-87b7-874c4b34827c"));
				photo.HostAlbum = _albumController.GetAlbumById(1);
				photo.PhotoTitle = PhotoTitle.Text;
				photo.PhotoDescription = PhotoDescription.Text;

				//Images, if photo has been uploaded
				if (PhotoFile.FileContent.Length > 0)
				{
					photo.OriginalPhoto = new Bitmap(PhotoFile.FileContent);
					photo.OptimizedPhoto = new Bitmap(PhotoFile.FileContent);
					photo.PhotoThumbnail = new Bitmap(PhotoFile.FileContent);
				}
				//Now, update photo and commit changes for tags initialization
				photo = _photoController.AddOrUpdate(photo);
				
				//Save changes in tags
				foreach (var item in TagsList.Items)
				{
					var listItem = (ListItem)item;
					int id;
					if (int.TryParse(listItem.Value, out id) && id > 0)
					{
						var tag = _tagController.GetTagById(id);
						//Tag deselected
						if (!listItem.Selected && photo.PhotoTags.Where(t => t.TagId == id).Any())
							photo.RemoveTag(tag);
						//Tag selected
						if (listItem.Selected && !photo.PhotoTags.Where(t => t.TagId == id).Any())
							photo.AddTag(tag);
					}
				}
				_photoController.AddOrUpdate(photo);
			}

		}
	}
}
