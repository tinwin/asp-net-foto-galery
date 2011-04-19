using System;
using System.Collections.Generic;
using Photogallery;

namespace DAL.AbstractEntities
{
    public interface IAlbumRepository
    {
        IAlbum AddAlbum(IAlbum album);

        void DeleteAlbum(int id);

        void UpdateAlbum(IAlbum album);

		IAlbum GetAlbumById(int id);

        IEnumerable<IAlbum> GetAlbumListByUserId(Guid id);
        int GetAlbumsCount();
        IEnumerable<IAlbum> SelectAlbums(int skip, int take);

		int GetAlbumsCount(Guid userId);
		IEnumerable<IAlbum> SelectAlbums(Guid userId, int skip, int take);

    }
}
