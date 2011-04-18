using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuisnessLayer.AbstractControllers;
using Common;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Repositories;
using Photogallery;

namespace BuisnessLayer.ConcreteControllers
{
	public class PhotoController:IPhotoController
	{
		private IAbstractGalleryProvider _dataProvider;

		public PhotoController()
		{
			_dataProvider = Windsor.Instance.Resolve<IAbstractGalleryProvider>();
		}

		public IEnumerable<IPhoto> SelectPhotosPage(int skip, int take)
		{
			return _dataProvider.PhotoRepository.SelectPhotos(skip, take);
		}

		public int GetPhotosCount()
		{
			return _dataProvider.PhotoRepository.GetPhotosCount();
		}

		public IPhoto GetPhotoById(int id)
		{
			return _dataProvider.PhotoRepository.GetPhotoById(id);
		}

		public void AddNew(IPhoto photo)
		{
			_dataProvider.PhotoRepository.AddPhoto(photo);
		}

		public void Save(IPhoto photo)
		{
			_dataProvider.PhotoRepository.UpdatePhoto(photo);
		}

		public IPhoto AddOrUpdate(IPhoto photo)
		{
			var saved = _dataProvider.PhotoRepository.UpdatePhoto(photo);
			_dataProvider.PhotoRepository.Commit();
			return saved;
		}
	}
}
