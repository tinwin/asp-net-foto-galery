using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuisnessLayer.AbstractControllers;
using Common;
using DAL.AbstractEntities;
using Photogallery;

namespace BuisnessLayer.ConcreteControllers
{
	public class UserController:IUserController
	{
		public IGalleryUser GetUserByGuid(Guid guid)
		{
			var provider = Windsor.Instance.Resolve<IAbstractGalleryProvider>();
			return provider.GalleryUserRepository.GetUserById(guid);
		}

		public IGalleryUser GetUserByName(string name)
		{
			var provider = Windsor.Instance.Resolve<IAbstractGalleryProvider>();
			return provider.GalleryUserRepository.GetUserByName(name);
		}
	}
}
