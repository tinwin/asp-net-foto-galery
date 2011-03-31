using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.AbstractEntities
{
	public abstract class  AbstractGalleryProvider
	{
         string ConnectionString { get; set; }

	    IAlbumRepository AlbumRepository          
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

	  
        IGalleryUserRepository GalleryUserRepository
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        IPhotoRepository PhotoRepository
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        ITagRepository TagRepository
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }



	}
}
