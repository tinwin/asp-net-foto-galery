using System;
using BuisnessLayer.AbstractControllers;
using Common;
using Photogallery;

namespace WebUI
{
    public partial class EditAlbum : System.Web.UI.Page
    {
        private IUserController _userController = Windsor.Instance.Resolve<IUserController>();
		private readonly IAlbumController _albumController = Windsor.Instance.Resolve<IAlbumController>();
		private readonly IEnvironment _environment = Windsor.Instance.Resolve<IEnvironment>();

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				int albumId;
				//If user has tried to edit existing photo
				//TODO: Check user rights on photo
				if (int.TryParse(Request.QueryString["id"], out albumId) && albumId > 0)
				{
					IAlbum album = _albumController.GetAlbumById(albumId);

					AlbumId.Value = album.AlbumId.ToString();
					AlbumTitle.Text = album.Title;
					AlbumDescription.Text = album.Description;
				}
			}
		}

        protected void Button1_Click(object sender, EventArgs e)
        {
            int albumId;
            if (int.TryParse(AlbumId.Value, out albumId))
            {
                IAlbum album;
                if (albumId > 0)
                    album = _albumController.GetAlbumById(albumId);
                else
                {
                    album = new Photogallery.Album();
                    album.AlbumId = albumId;
                    album.CreationDate = DateTime.Now;
                }

                //Common properties
                //TODO: implement owner and album initialization
				album.User = _environment.CurrentClient; 
                album.Title = AlbumTitle.Text;
                album.Description = AlbumDescription.Text;

                //Now, update photo and commit changes for tags initialization
                if (albumId > 0)
                {
                    _albumController.Update(album);
                }
                else
                    _albumController.AddNew(album);
				Response.Redirect("/Albums.aspx?user=" + _environment.CurrentClient.UserId);
                    
            }
        }

        protected void ButtonDeleteAlbum_Click(object sender, EventArgs e)
        {
            int albumId;
            if (int.TryParse(AlbumId.Value, out albumId))
            {
                _albumController.DeleteAlbumById(albumId);
            }

            Response.Redirect("/Albums.aspx");
        }
    }
}
