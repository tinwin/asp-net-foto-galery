﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace BuisnessLayer.AbstractControllers
{
	public interface IPhotoController
	{
		IEnumerable<IPhoto> SelectPhotosPage(int pageNumber, int pageSize);

		IEnumerable<IPhoto> SelectPhotosPage(int albumId, int pageNumber, int pageSize);

		IEnumerable<IPhoto> SelectPhotosPage(Guid userId, int pageNumber, int pageSize);

		int GetPhotosCount();

		int GetPhotosCount(int albumId);

		int GetPhotosCount(Guid userId);

		IPhoto GetPhotoById(int id);
		void AddNew(IPhoto photo);
		void Save(IPhoto photo);

		IPhoto AddOrUpdate(IPhoto photo);

		void DeletePhotoById(int photoId);
	}
}
