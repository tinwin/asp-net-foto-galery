using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace BuisnessLayer.AbstractControllers
{
	public interface IAlbumController
	{
		IAlbum GetAlbumById(int id);
	}
}
