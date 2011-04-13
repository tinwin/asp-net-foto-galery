using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace BuisnessLayer.AbstractControllers
{
	public interface IPhotoController
	{
		IEnumerable<IPhoto> SelectPhotosPage(int pageNumber, int pageSize);
	}
}
