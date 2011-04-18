using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.AbstractEntities
{
    public interface IPhotoRepository : IDisposable
    {
        IPhoto AddPhoto(IPhoto photo);

        void DeletePhoto(int PhotoId);

        IPhoto UpdatePhoto(IPhoto photo);

		IPhoto GetPhotoById(int id);

		IEnumerable<IPhoto> SelectPhotos(int startIndex, int count);

		void Commit();

		int GetPhotosCount();
    }
}
