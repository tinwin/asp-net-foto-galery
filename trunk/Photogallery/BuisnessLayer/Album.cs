using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photogallery
{
    public abstract class Album
    {
        
        public virtual int AlbumId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual GalleryUser User
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string Title
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual Album ParentAlbum
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IEnumerable<Album> ChildAlbums 
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IEnumerable<Photo> Photos
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string Description
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        
        
        public virtual DateTime CreationDate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual bool IsRootAlbum
        {
            get { throw new NotImplementedException(); }
            
        }

        public virtual IEnumerable <Tag> AlbumTags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }



        public virtual void AddComment(Comment comment )
        {
            throw new NotImplementedException();
        }

        public virtual void DeleteCommentById(int commentId)
        {
            throw new NotImplementedException();
        }


        public virtual void UpdateComment(Comment comment )
        {
             
        
            throw new NotImplementedException();
        
        }

    

    }
}
