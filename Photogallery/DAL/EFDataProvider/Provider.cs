using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Repositories;

namespace DAL.EFDataProvider
{
	public class Provider : IAbstractGalleryProvider
	{
		private readonly PhotogalleryEntities _context;
		private IPhotoRepository _photoRepository;
		private IAlbumRepository _albumRepository;
		private IGalleryUserRepository _userRepository;
		private ITagRepository _tagRepository;

		public Provider(string sqlConnectionString)
		{
			_context = new PhotogalleryEntities(new EntityConnection(
			@"metadata=res://*/EFDataProvider.EntityDataModel.csdl|res://*/EFDataProvider.EntityDataModel.ssdl|res://*/EFDataProvider.EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string="""+sqlConnectionString+"\""
				));
		}

		public IAlbumRepository AlbumRepository
		{
			get { return _albumRepository ?? (_albumRepository = new AlbumRepository(_context)); }
		}

		public IGalleryUserRepository GalleryUserRepository
		{
			get
			{
			    return _userRepository ?? (_userRepository = new GalleryUserRepository(_context));
			   
			}
		}

		public IPhotoRepository PhotoRepository
		{
			get { return _photoRepository ?? (_photoRepository = new PhotoRepository(_context)); }
		}

		public ITagRepository TagRepository
		{
			get { return _tagRepository ?? (_tagRepository = new TagRepository(_context)); }
		}
	}
}
