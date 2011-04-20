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
		readonly IEnvironment _environment = Windsor.Instance.Resolve<IEnvironment>();
		public Control AddCommentContainer;

		protected void Page_Load(object sender, EventArgs e)
		{
			int photoId;

			try
			{
				photoId = int.Parse(Request.Params["id"]);

			}
			catch (NullReferenceException ee)
			{
				throw new InvalidOperationException("Page has got wrong url GET parameter(s)", ee);
			}
			catch (ArgumentNullException ee)
			{
				throw new InvalidOperationException("Page has got wrong url GET parameter(s)", ee);
			}

			CurrentPhoto = _controller.GetPhotoById(photoId);
			LoadComments();

		}

		protected void ButtonAdd_Click(object sender, EventArgs e)
		{
			IComment comment = new Comment
           	{
           		Text = TextBoxComment.Text,
				AdditionDate = DateTime.Now,
				Author = _environment.CurrentClient
           	};
			CurrentPhoto.AddComment(comment);
			_controller.AddOrUpdate(CurrentPhoto);
			TextBoxComment.Text = "";
			LoadComments();
		}

		private void LoadComments()
		{
			int skip = (pager1.CurrentIndex - 1) * pager1.PageSize;
			int take = pager1.PageSize;

			PhotoComments.DataSource = CurrentPhoto.GetPhotoCommentsPage(skip, take);
			PhotoComments.DataBind();
			if (_environment.CurrentClient == null)
				AddCommentContainer.Visible = false;
			
			pager1.ItemCount = CurrentPhoto.CommentsCount;
		}

		public void pager_Command(object sender, CommandEventArgs e)
		{
			int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
			pager1.CurrentIndex = currnetPageIndx;
			LoadComments();
		}
	}
}
