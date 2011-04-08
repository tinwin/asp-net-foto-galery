using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Adapters;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
	public class PhotoRepository : IPhotoRepository
	{
		private PhotogalleryEntities _context;

		public PhotoRepository(PhotogalleryEntities context)
		{
			_context = context;
		}

		public IPhoto AddPhoto(IPhoto photo)
		{
			var saved = Adapte(photo);
			_context.AddToPhotoSet(saved);
			return new PhotoAdapter(saved);
		}

		public void DeletePhoto(int photoId)
		{
			_context.DeleteObject(_context.PhotoSet.Where(p=>p.PhotoId==photoId).First());
		}

		public void UpdatePhoto(IPhoto photo)
		{
			_context.SaveChanges();    
		}

		public IPhoto GetPhotoById(int id)
		{
			return new PhotoAdapter((
				from photo in _context.PhotoSet
				where photo.PhotoId == id 
				select photo).
				First());
		}

		private Photo Adapte(IPhoto photo)
		{
			Photo entity = new Photo
			{
				AdditionDate = photo.AdditionDate,
				Album = _context.AlbumSet.
					Where(a => a.AlbumId == photo.HostAlbum.AlbumId).
					FirstOrDefault(),
				Author = _context.UserSet.
					Where(m => m.UserId == photo.OwningUser.UserId).
					FirstOrDefault(),
				Description = photo.PhotoDescription,
				Title = photo.PhotoTitle
			};
			return entity;
		}
	}
}
