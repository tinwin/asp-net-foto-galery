using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.AbstractEntities
{
	public interface IAbstractGalleryProvider
	{
		IAlbumRepository AlbumRepository { get; }

		IGalleryUserRepository GalleryUserRepository { get; }

		IPhotoRepository PhotoRepository { get; }

		ITagRepository TagRepository { get; }
	}
}
