using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Photogallery
{
    public abstract class GalleryUser:MembershipUser
    {
        public virtual Album RootAlbum
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IEnumerable<Comment > UserComments 
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string Description
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public virtual void AddComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteCommentById(int commentId)
        {
            throw new NotImplementedException();
        }


        public virtual void UpdateComment(Comment comment)
        {


            throw new NotImplementedException();

        }


    }
}
