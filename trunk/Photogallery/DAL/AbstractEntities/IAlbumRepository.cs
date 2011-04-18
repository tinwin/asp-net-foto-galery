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

        IEnumerable<IAlbum> getAlbumListByUserID(Guid id);

    }
}
