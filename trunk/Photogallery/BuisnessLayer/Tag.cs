using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photogallery
{
    public abstract class Tag
    {
        public virtual int TagId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual string TagTitle
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

    }
}
