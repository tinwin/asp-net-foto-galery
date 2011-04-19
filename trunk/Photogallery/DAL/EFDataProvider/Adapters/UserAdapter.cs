using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using DAL.EFDataProvider.Repositories;
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
                _user.aspnet_Users.LoweredUserName = value.ToLower();
            }
        }

        




        public string UserMail
        {
            get { return _user.Email; }
            set
            {
                _user.LoweredEmail = value.ToLower();
                _user.Email = value;
            }
        }


        
        public IEnumerable<IAlbum> Albums
        {
            get
            {
                _user.Albums.Load();
                List<IAlbum> albums = new List<IAlbum>();
                foreach (Album album in _user.Albums)
                {
                    albums.Add(new AlbumAdapter(album));
                }
                return albums;
            }
            

        }



        public IEnumerable<IComment> UserComments
        {
            get
            {


                _user.Comments.Load();
                List<IComment> comments = new List<IComment>();
                foreach (Comment  comment  in _user.Comments )
                {
                    comments.Add( new CommentAdapter( comment ));
                }
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
                if (string.IsNullOrEmpty(  _role))
                {
                    _user.aspnet_UsersReference.Load();
                    _user.aspnet_Users.aspnet_Roles.Load();
                    _role = _user.aspnet_Users.aspnet_Roles.FirstOrDefault()!=null?
                        _user.aspnet_Users.aspnet_Roles.FirstOrDefault().RoleName :string.Empty;

                }


                return _role;
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
