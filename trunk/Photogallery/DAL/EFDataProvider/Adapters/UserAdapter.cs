using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
    public class UserAdapter : IGalleryUser
    {
        private User _user;

        public UserAdapter(User user)
        {
            _user = user;
        }

    	public Guid UserId
    	{
			get { return _user.UserId; }
			set { throw new NotImplementedException(); }
    	}

    	public string Username
    	{
			get { return _user.aspnet_Users.UserName; }
    		set { throw new NotImplementedException(); }
    	}

    	public string UserPassword
    	{
    		get { return _user.Password;}
    		set { throw new NotImplementedException(); }
    	}

    	public string UserMail
    	{
			get { return _user.Email; }
    		set { throw new NotImplementedException(); }
    	}

		private AlbumAdapter _rootAlbum;

    	public IAlbum RootAlbum
        {
            get
            {
				return _rootAlbum ?? (_rootAlbum = new AlbumAdapter(_user.aspnet_Users.RootAlbum));
            }
            set { throw new NotImplementedException(); }
        }

		List<IComment> _userComments;

        public IEnumerable<IComment> UserComments
        {
            get
            {
				if (_userComments == null)
				{
					_user.Comments.Load();
					_userComments = new List<IComment>();
					foreach (var comment in _user.Comments)
						_userComments.Add(new CommentAdapter(comment));
				}
				return _userComments;
            }
            set { throw new NotImplementedException(); }
        }

        public string Description
        {
			get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void AddComment(IComment comment)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommentById(int commentId)
        {
            throw new NotImplementedException();
        }

        public void UpdateComment(IComment comment)
        {
            throw new NotImplementedException();
        }
    }
}
