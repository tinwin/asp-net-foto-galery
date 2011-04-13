using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuisnessLayer.AbstractControllers;
using DAL.AbstractEntities;
using Common;
using Photogallery;

namespace BuisnessLayer.ConcreteControllers
{
	public class AlbumController:IAlbumController
	{
		private IAbstractGalleryProvider _provider = Windsor.Instance.Resolve<IAbstractGalleryProvider>();

		public IAlbum GetAlbumById(int id)
		{
			return _provider.AlbumRepository.GetAlbumById(id);
		}
	}
}
