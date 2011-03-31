using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photogallery
{
    public abstract class Comment
    {
        public virtual int CommentId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string Text
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual GalleryUser Author
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public virtual DateTime AdditionDate
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }




    }
}
