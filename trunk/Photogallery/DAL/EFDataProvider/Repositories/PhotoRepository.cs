using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
    class PhotoRepository : IPhotoRepository
    {
        private PhotogalleryEntities _context;

        public PhotoRepository(PhotogalleryEntities context)
        {
            _context = context;
        }

        public void AddPhoto(IPhoto photo)
        {
            
            var savedPhoto = _context.
            _context.AddToPhotoSet();
        }

        public void DeletePhoto(Guid PhotoId)
        {
            throw new NotImplementedException();
        }

        public void UpdatePhoto(IPhoto photo)
        {
            throw new NotImplementedException();
        }

        private Photo Adapte(IPhoto photo)
        {
            Photo entity = new Photo
                               {
                                   AdditionDate = photo.AdditionDate,
                                   Album = _context.AlbumSet.Where(a => a.AlbumId == photo.HostAlbum.AlbumId).First(),
                                   Author =
                                       _context.aspnet_Membership.Where(m => m.UserId == photo.OwningUser.UserId).First()
               };
        }
    }
}
