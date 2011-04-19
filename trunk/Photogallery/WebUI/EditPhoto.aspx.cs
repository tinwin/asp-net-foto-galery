using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BuisnessLayer.AbstractControllers;
using Common;
using Photogallery;

namespace WebUI
{
	public partial class EditPhoto : System.Web.UI.Page
	{
		private readonly IPhotoController _photoController = Windsor.Instance.Resolve<IPhotoController>();
		private readonly ITagController _tagController = Windsor.Instance.Resolve<ITagController>();
		private readonly IUserController _userController = Windsor.Instance.Resolve<IUserController>();
		private readonly IAlbumController _albumController = Windsor.Instance.Resolve<IAlbumController>();
		private readonly IEnvironment _environment = Windsor.Instance.Resolve<IEnvironment>();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				int photoId;
				var allTags = _tagController.GetAllTags();
				var user = _environment.CurrentClient;
				if (user==null)
					throw new InvalidOperationException("Photo can't be edited if user is unidentified");
				var allAlbums = _albumController.SelectAlbumsByUserId(user.UserId);

				AlbumsList.DataSource = allAlbums;
				AlbumsList.DataBind();

				//If user has tried to edit existing photo
				//TODO: Check user rights on photo
				if (int.TryParse(Request.QueryString["id"], out photoId) && photoId > 0)
				{
					IPhoto photo = _photoController.GetPhotoById(photoId);

					if (photo == null)
						throw new HttpException(404, "Photo with specified ID hasn't been found");

					PhotoId.Value = photo.PhotoId.ToString();
					PhotoTitle.Text = photo.PhotoTitle;
					PhotoDescription.Text = photo.PhotoDescription;
					//Add pseudo-random number to url for prevent cache using
					PhotoImage.Src += photo.PhotoId + "&amp;rand=" + DateTime.Now.ToFileTime();

					int i = 0;
					foreach (var item in AlbumsList.DataSource as IEnumerable<IAlbum>)
						if (item.AlbumId == photo.HostAlbum.AlbumId)
						{
							AlbumsList.SelectedIndex = i;
							break;
						}
						else
							++i;                   

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
					Button2.Visible = false;
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
				photo.OwningUser = _userController.GetUserByGuid(_environment.CurrentClient.UserId);
				photo.HostAlbum = _albumController.GetAlbumById(int.Parse(AlbumsList.SelectedValue));
				photo.PhotoTitle = PhotoTitle.Text;
				photo.PhotoDescription = PhotoDescription.Text;

				//Images, if photo has been uploaded
				if (PhotoFile.FileContent.Length > 0)
					photo.OriginalPhoto = new Bitmap(PhotoFile.FileContent);
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

		/// <summary>
		/// Remove photo
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Button2_Click(object sender, EventArgs e)
		{
			int photoId = int.Parse(PhotoId.Value);
			_photoController.DeletePhotoById(photoId);
			Response.Redirect("/Photos.aspx");
		}
	}
}
