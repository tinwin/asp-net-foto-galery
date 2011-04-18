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

        public IEnumerable<IAlbum> SelectAllAlbumsPage(int skip, int take)
        {
            return _provider.AlbumRepository.SelectAlbums(skip, take);
        }

	    public IEnumerable<IAlbum> SelectAlbumsByUserId(Guid userId)
	    {
            return _provider.AlbumRepository.GetAlbumListByUserId(userId);
	    }

	    public int GetAlbumsCount()
        {
            return _provider.AlbumRepository.GetAlbumsCount();
        }

        public void AddNew(IAlbum album)
        {
            _provider.AlbumRepository.AddAlbum(album);
        }

	    public void Update(IAlbum album)
	    {
            _provider.AlbumRepository.UpdateAlbum(album);
	    }
	}
}
