﻿using System;
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

		public IEnumerable<IPhoto> SelectPhotosPage(int albumId, int skip, int take)
		{
			return _dataProvider.PhotoRepository.SelectPhotos(albumId, skip, take);
		}

		public int GetPhotosCount()
		{
			return _dataProvider.PhotoRepository.GetPhotosCount();
		}

		public int GetPhotosCount(int albumId)
		{
			return _dataProvider.PhotoRepository.GetPhotosCount(albumId);
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

		/// <summary>
		/// Add new photo or update existing.
		/// Notes:
		/// Method except that photo.OriginalPhoto has seted. Other images are generated by this method
		/// and shoudn't be seted.
		/// </summary>
		/// <param name="photo">Photo to handling</param>
		/// <returns></returns>
		public IPhoto AddOrUpdate(IPhoto photo)
		{
			//TODO: Implement all images generation
			photo.OptimizedPhoto = photo.PhotoThumbnail = photo.OriginalPhoto;           
                      

			var saved = _dataProvider.PhotoRepository.UpdatePhoto(photo);
			_dataProvider.PhotoRepository.Commit();
			return saved;
		}

		public void DeletePhotoById(int photoId)
		{
			_dataProvider.PhotoRepository.DeletePhoto(photoId);
		}
	}
}
