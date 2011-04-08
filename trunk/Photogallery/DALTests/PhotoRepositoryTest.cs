using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider;
using DAL.EFDataProvider.Adapters;
using DAL.EFDataProvider.Repositories;
using NUnit.Framework;
using Photogallery;
using Album = DAL.EFDataProvider.Album;
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
			string defaultDbDir = @"d:\Projects\Softserve\Fotogallery\Photogallery\DALTests\bin\Debug\Data\DefaultDatabase";
			string workingDir = @"d:\Projects\Softserve\Fotogallery\Photogallery\DALTests\bin\Debug\Data\WorkingDirectory";
			Process cmd = new Process();
			cmd.StartInfo = new ProcessStartInfo
        	{
				FileName = "xcopy",
				Arguments = " \"" + defaultDbDir + "\" \"" + workingDir + "\" /E",
				UseShellExecute = false
        	};
			
			
			cmd.Start();
			
			cmd.WaitForExit();
			



			_context = new PhotogalleryEntities(new EntityConnection(
				@"metadata=EntityDataModel.csdl|EntityDataModel.ssdl|EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string=""Data Source=PRIZRAK\SQLEXPRESS;Initial Catalog=photogallery;Integrated Security=True;MultipleActiveResultSets=True"""));
			_photoRepository = new PhotoRepository(_context);
		}

		[Test]
		public void AddPhotoTest()
		{

			var user = (from u in _context.UserSet
				where u.UserId == new Guid("29d25edd-7279-4a94-87b7-874c4b34827c")
				select u).
				First();

			var album = (from a in _context.AlbumSet
						 where a.AlbumId == 1
						 select a).
				First();
			
			
			var photo = new Photogallery.Photo();
			photo.HostAlbum = new AlbumAdapter(album);
			photo.PhotoDescription = "1";
			photo.PhotoTitle = "1";

			photo.OwningUser = new UserAdapter(user);
			photo.AdditionDate = DateTime.Now;
			var saved = _photoRepository.AddPhoto(photo);
			_context.SaveChanges();

			 _photoRepository.GetPhotoById(saved.PhotoId);
		}
	}
}
