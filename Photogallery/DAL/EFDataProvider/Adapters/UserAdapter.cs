using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
    public class UserAdapter : IGalleryUser
    {
        private User _user;

		public UserAdapter()
		{
			
		}

        public string UserPassword
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string UserMail
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

    	public IEnumerable<IAlbum> Albums
    	{
    		get { throw new NotImplementedException(); }
    		set { throw new NotImplementedException(); }
    	}


    	public UserAdapter(User user)
        {
            _user = user;

        }

        private Guid _userId;
        public Guid UserId
        {
            get
            {
                if (_userId == Guid.Empty)
                    _userId = _user.UserId;
                return _userId;
            }
            set { _userId = value; }
        }


        public string Username
        {
            get
            {
                if (!_user.aspnet_UsersReference.IsLoaded)
                    _user.aspnet_UsersReference.Load();
                return _user.aspnet_Users.UserName;
            }
            set
            {
                if (!_user.aspnet_UsersReference.IsLoaded)
                    _user.aspnet_UsersReference.Load();
                _user.aspnet_Users.UserName = value;
            }
        }

        


        private AlbumAdapter _rootAlbum;

        public IAlbum RootAlbum
        {
            get
            {
                return _rootAlbum ?? (_rootAlbum = new AlbumAdapter(_user.aspnet_Users.RootAlbum));
            }
            set { _rootAlbum = value as AlbumAdapter; }
        }

        IEnumerable<IComment> IGalleryUser.UserComments
        {
        	get { throw new NotImplementedException(); }
        }

    	List<IComment> _userComments;

        public IEnumerable<IComment> UserComments
        {
            get
            {


                _user.AboutUserComments.Load();
                List<IComment> comments = new List<IComment>();
                foreach (Comment com in _user.AboutUserComments.ToArray())
                    comments.Add(new CommentAdapter(com));
                return comments;
            }

        }

        public string Description
        {
            get
            {
                if (!_user.aspnet_UsersReference.IsLoaded)
                    _user.aspnet_UsersReference.Load();
                return _user.aspnet_Users.Description;

            }
            set
            {
                if (!_user.aspnet_UsersReference.IsLoaded)
                    _user.aspnet_UsersReference.Load();
                _user.aspnet_Users.Description = value;
            }
        }

        private string _role;
        public string UserRole
        {
            get
            {
                if (_role == string.Empty)
                {
                    _user.aspnet_UsersReference.Load();
                    _user.aspnet_Users.aspnet_Roles.Load();

                }
                return _role = _user.aspnet_Users.aspnet_Roles.Single().RoleName;
            }
            set
            {
                _role = value;
            }
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
