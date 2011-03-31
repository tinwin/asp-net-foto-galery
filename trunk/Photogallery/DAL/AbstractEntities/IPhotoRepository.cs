using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.AbstractEntities
{
    public interface IPhotoRepository
    {
        void AddPhoto(Photo photo);

        void DeletePhoto(Guid PhotoId);

        void UpdatePhoto(Photo photo);

    }
}
