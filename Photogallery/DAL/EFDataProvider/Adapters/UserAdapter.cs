using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
    class UserAdapter : IGalleryUser
    {
        private aspnet_Membership _user;

        public UserAdapter(aspnet_Membership user)
        {
            _user = user;
        }

        public IAlbum RootAlbum
        {
            get
            {
                throw new NotImplementedException();
            }
            set { throw new NotImplementedException(); }
        }

        public IEnumerable<IComment> UserComments
        {
            get { throw new NotImplementedException(); }
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
