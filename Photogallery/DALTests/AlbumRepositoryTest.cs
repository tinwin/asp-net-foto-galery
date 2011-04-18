using System;
using System.Data.EntityClient;
using System.Linq;
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
		    var album = CreateAlbum();

			var savedAlbum = _albumRepository.AddAlbum(album);
			int savedId = savedAlbum.AlbumId;

			Init();

			savedAlbum = _albumRepository.GetAlbumById(savedId);
            Assert.AreEqual("albumTitle%%", savedAlbum.Title);
		}

    [Test]
        public void DeleteAlbumTest()
        {
            var newAlbum = _albumRepository.AddAlbum(CreateAlbum());
            var album = _context.AlbumSet.Where(a => a.AlbumId == newAlbum.AlbumId).First();
            _albumRepository.DeleteAlbum(album.AlbumId);
            var deletedAlbum = _albumRepository.GetAlbumById(album.AlbumId);
            Assert.Null(deletedAlbum);
        }

    [Test]
    public void UpdateAlbumTest()
    {
        var album = _albumRepository.AddAlbum(CreateAlbum());
        album.Title = "TESTALBUM";
        album.Description = "TESTDescription";
        _albumRepository.UpdateAlbum(album);
        Init();
        var updatedAlbum = _albumRepository.GetAlbumById(album.AlbumId);
        Assert.AreEqual("TESTALBUM", updatedAlbum.Title);
        Assert.AreEqual("TESTDescription", updatedAlbum.Description);
    }

    [Test]
    public void SelectPageTest()
    {

        var albums = _albumRepository.GetAlbumListByUserId(new Guid("29d25edd-7279-4a94-87b7-874c4b34827c"));
        foreach (var album in albums)
            Console.WriteLine(album.Title);

    }

        private Photogallery.Album CreateAlbum()
        {
            var user = _context.UserSet.Where(u => u.UserId == new Guid("29d25edd-7279-4a94-87b7-874c4b34827c")).
                                        First();
            var album = new Photogallery.Album();

            #region Album initialization

            album.Description = "albumDescription%%";
            album.Title = "albumTitle%%";
            album.User = new UserAdapter(user);
            album.CreationDate = DateTime.Now;

            #endregion

            return album;
        }
	}
}
