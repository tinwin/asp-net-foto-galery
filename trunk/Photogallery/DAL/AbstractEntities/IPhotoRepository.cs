using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.AbstractEntities
{
    public interface IPhotoRepository
    {
        void AddPhoto(IPhoto photo);

        void DeletePhoto(Guid PhotoId);

        void UpdatePhoto(IPhoto photo);

    }
}
