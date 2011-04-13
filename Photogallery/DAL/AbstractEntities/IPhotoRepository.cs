using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.AbstractEntities
{
    public interface IPhotoRepository
    {
        IPhoto AddPhoto(IPhoto photo);

        void DeletePhoto(int PhotoId);

        void UpdatePhoto(IPhoto photo);

		IPhoto GetPhotoById(int id);

		IEnumerable<IPhoto> SelectPhotos(int startIndex, int count);
    }
}
