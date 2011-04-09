using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Adapters;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
	public class AlbumRepository:IAlbumRepository
	{
		private PhotogalleryEntities _context;

		public AlbumRepository(PhotogalleryEntities context)
		{
			_context = context;
		}

		public IAlbum AddAlbum(IAlbum album)
		{
			var entity = Adapte(album);
			_context.AddToAlbumSet(entity);
			_context.SaveChanges();
			return new AlbumAdapter(entity);
		}

		public void DeleteAlbum(int id)
		{
			throw new NotImplementedException();
		}

		public void UpdateAlbum(IAlbum album)
		{
			throw new NotImplementedException();
		}

		public IAlbum GetAlbumById(int id)
		{
			return new AlbumAdapter((from a in _context.AlbumSet
									 where a.AlbumId==id
									 select a).First());
		}

		public Album Adapte(IAlbum album)
		{
			return new Album
	       	{
	       		Author = (from User u in _context.UserSet 
						  where u.UserId==album.User.UserId 
						  select u).First(),
				Title = album.Title,
				CreationDate = album.CreationDate
	       	};
		}
	}
}
