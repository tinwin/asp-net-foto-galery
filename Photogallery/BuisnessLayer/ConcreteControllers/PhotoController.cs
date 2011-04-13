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

		public IEnumerable<IPhoto> SelectPhotosPage(int pageNumber, int pageSize)
		{
			return _dataProvider.PhotoRepository.SelectPhotos(pageNumber * pageSize, pageSize);
		}

		public IPhoto GetPhotoById(int id)
		{
			return _dataProvider.PhotoRepository.GetPhotoById(id);
		}

		public void AddNew(IPhoto photo)
		{
			_dataProvider.PhotoRepository.AddPhoto(photo);
		}
	}
}
