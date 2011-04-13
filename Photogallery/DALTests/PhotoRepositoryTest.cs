using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider;
using DAL.EFDataProvider.Adapters;
using DAL.EFDataProvider.Repositories;
using NUnit.Framework;
using DAL.EFDataProvider;
using Photogallery;
using Album = DAL.EFDataProvider.Album;
using Comment = Photogallery.Comment;
using Photo = DAL.EFDataProvider.Photo;
using Tag = DAL.EFDataProvider.Tag;

namespace DALTests
{
	[TestFixture]
	public class PhotoRepositoryTest
	{
		private PhotogalleryEntities _context;
		private IPhotoRepository _photoRepository;

		[TestFixtureSetUp]
		public void Init()
		{
			//TODO: Implement test database attach
			#region Draft
			//string defaultDbDir = @"d:\Projects\Softserve\Fotogallery\Photogallery\DALTests\bin\Debug\Data\DefaultDatabase";
			//string workingDir = @"d:\Projects\Softserve\Fotogallery\Photogallery\DALTests\bin\Debug\Data\WorkingDirectory";
			//string dbFile = "photogallery.mdf";
			//try
			//{
			//    Directory.Delete(workingDir, true);
			//    Directory.CreateDirectory(workingDir);
			//    Process cmd = new Process
			//    {
			//        StartInfo = new ProcessStartInfo
			//        {
			//            FileName = "xcopy",
			//            Arguments = " \"" + defaultDbDir + "\" \"" + workingDir + "\" /E",
			//            UseShellExecute = false
			//        }
			//    };
			//    cmd.Start();
			//    cmd.WaitForExit(10000);
			//}
			//catch(IOException e)
			//{
			//    Assert.Fail("Can't create test environment - can't copy database files from \"" +
			//        defaultDbDir + "\" to \""+workingDir+"\"");
			//}
			
			//_context = new PhotogalleryEntities(new EntityConnection(
			//    @"metadata=res://*/EFDataProvider.EntityDataModel.csdl|res://*/EFDataProvider.EntityDataModel.ssdl|res://*/EFDataProvider.EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string=""Data Source=.\SQLEXPRESS;AttachDbFilename="+workingDir+"\\" + dbFile+@";Database=photogallery;Integrated Security=True;Connect Timeout=30;User Instance=True"""));

			//    //@"metadata=EntityDataModel.csdl|EntityDataModel.ssdl|EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string=""Data Source=.\SQLEXPRESS;AttachDbFilename="+workingDir+"\\" + dbFile+@";Integrated Security=True;Connect Timeout=30;User Instance=True"""));
			//try
			//{
			//    _context.ExecuteSql("EXEC sp_configure filestream_access_level, 2");
			//    _context.ExecuteSql("RECONFIGURE");
			//}
			//catch (Exception e)
			//{


			//}
			#endregion

			_context = new PhotogalleryEntities(new EntityConnection(
            @"metadata=res://*/EFDataProvider.EntityDataModel.csdl|res://*/EFDataProvider.EntityDataModel.ssdl|res://*/EFDataProvider.EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string=""Data Source=FEDOR-ПК\SQLEXPRESSWTF;Initial Catalog=photogallery;Integrated Security=True;MultipleActiveResultSets=True"""
				));

			
			_photoRepository = new PhotoRepository(_context);
		}

		[Test]
		public void AddPhotoTest()
		{
			var photo = CreatePhoto();

			var saved = _photoRepository.AddPhoto(photo);
			_context.SaveChanges();
			//Reselect photo - tests saving
			saved = _photoRepository.GetPhotoById(saved.PhotoId);

			#region Asserts

			Bitmap savedBitmap = new Bitmap(saved.OriginalPhoto);
			Assert.AreEqual(Color.Red.ToArgb(), savedBitmap.GetPixel(0, 0).ToArgb());

			savedBitmap = new Bitmap(saved.OptimizedPhoto);
			Assert.AreEqual(Color.Red.ToArgb(), savedBitmap.GetPixel(0, 0).ToArgb());

			savedBitmap = new Bitmap(saved.PhotoThumbnail);
			Assert.AreEqual( Color.Red.ToArgb(), savedBitmap.GetPixel(0, 0).ToArgb());

			Assert.AreEqual(photo.PhotoTitle, saved.PhotoTitle);
			Assert.AreEqual(photo.PhotoDescription, saved.PhotoDescription);
			Assert.AreEqual(photo.HostAlbum.AlbumId, saved.HostAlbum.AlbumId);
			Assert.AreEqual(photo.OwningUser.UserId, saved.OwningUser.UserId);

			#endregion

		}

		[Test]
		public void DeletePhotoTest()
		{
			_photoRepository.AddPhoto(CreatePhoto());
			var photo = _context.PhotoSet.First();
			_photoRepository.DeletePhoto(photo.PhotoId);
			var deletedPhoto = _photoRepository.GetPhotoById(photo.PhotoId);
			Assert.Null(deletedPhoto);
		}

		[Test]
		public void UpdatePhotoTest()
		{
			var photo = _photoRepository.AddPhoto(CreatePhoto());
			photo.PhotoTitle = "===";
			photo.AddComment(new Comment
         	{
         		AdditionDate = DateTime.Now,
                Author = photo.OwningUser,
				Text = "111"
         	});
			photo.AddTag(new Photogallery.Tag { TagTitle = "NewTag" });
			_photoRepository.UpdatePhoto(photo);
			Init();
			var updatedPhoto = _photoRepository.GetPhotoById(photo.PhotoId);
			Assert.AreEqual("===",updatedPhoto.PhotoTitle);
			
			Assert.AreEqual("111", updatedPhoto.PhotoComments.First().Text);
			Assert.AreEqual(1, updatedPhoto.PhotoComments.Count());
			
			Assert.AreEqual("NewTag",updatedPhoto.PhotoTags.First().TagTitle);
			Assert.AreEqual(1, updatedPhoto.PhotoTags.Count());
		}

		[Test]
		public void SelectPageTest()
		{
			for (int i = 0; i < 5; i++)
				_photoRepository.AddPhoto(CreatePhoto());

			var photos = _photoRepository.SelectPhotos(2, 5);
			foreach(var photo in photos)
				Assert.AreEqual("1", photo.PhotoTitle);

		}

		private Photogallery.Photo CreatePhoto()
		{
			//Predefined existing user
			var user = (from u in _context.UserSet
						where u.UserId == new Guid("29d25edd-7279-4a94-87b7-874c4b34827c")
						select u).First();
			//Predefined existing album
			var album = (from a in _context.AlbumSet
						 where a.AlbumId == 1
						 select a).First();
			//Photo to add
			var photo = new Photogallery.Photo();

			#region Photo initialization

			photo.HostAlbum = new AlbumAdapter(album);
			photo.PhotoDescription = "1";
			photo.PhotoTitle = "1";
			photo.OwningUser = new UserAdapter(user);
			photo.AdditionDate = DateTime.Now;

			var bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Color.Red);
			photo.OriginalPhoto = bitmap;

			bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Color.Red);
			photo.OptimizedPhoto = bitmap;

			bitmap = new Bitmap(1, 1);
			bitmap.SetPixel(0, 0, Color.Red);
			photo.PhotoThumbnail = bitmap;

			#endregion

			return photo;
		}
	}
}
