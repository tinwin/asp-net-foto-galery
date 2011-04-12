using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider;
using DAL.EFDataProvider.Adapters;
using DAL.EFDataProvider.Repositories;
using NUnit.Framework;

namespace DALTests
{
	[TestFixture]
	class AlbumRepositoryTest
	{
		private PhotogalleryEntities _context;
		private IAlbumRepository _albumRepository;

		[TestFixtureSetUp]
		public void Init()
		{
			_context = new PhotogalleryEntities(new EntityConnection(
            @"metadata=res://*/EFDataProvider.EntityDataModel.csdl|res://*/EFDataProvider.EntityDataModel.ssdl|res://*/EFDataProvider.EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string=""Data Source=FEDOR-ПК\SQLEXPRESSWTF;Initial Catalog=photogallery;Integrated Security=True;MultipleActiveResultSets=True"""
				));


			_albumRepository = new AlbumRepository(_context);
		}

		[Test]
		public void AddAlbumTest()
		{
			//Predefined existing user
			var user = (from u in _context.UserSet
						where u.UserId == new Guid("29d25edd-7279-4a94-87b7-874c4b34827c")
						select u).First();

			var date = DateTime.Now;

			var album = new Photogallery.Album
        	{
        		User = new UserAdapter(user),
                Title = "New",
				CreationDate = date
        	};

			var savedAlbum = _albumRepository.AddAlbum(album);
			int savedId = savedAlbum.AlbumId;

			Init();

			savedAlbum = _albumRepository.GetAlbumById(savedId);
			Assert.AreEqual("New", savedAlbum.Title);
		}
	}
}
