using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Drawing;

namespace Photogallery
{
    public abstract class Photo
    {
        public virtual Guid PhotoId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string PhotoTitle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual Album HostAlbum
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IEnumerable<Comment > PhotoComments
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IEnumerable <Tag> PhotoTags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual GalleryUser OwningUser
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public virtual Image PhotoThumbnail
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual Image OptimizedPhoto
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual Image OriginalPhoto
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string PhotoDescription 
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual DateTime AdditionDate
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
