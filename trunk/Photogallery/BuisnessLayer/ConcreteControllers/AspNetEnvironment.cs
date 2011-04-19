using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BuisnessLayer.AbstractControllers;
using Common;
using Photogallery;

namespace BuisnessLayer.ConcreteControllers
{
	class AspNetEnvironment : IEnvironment
	{
		private IGalleryUser _currentClient;

		public IGalleryUser CurrentClient
		{
			get
			{
				if (_currentClient == null)
				{
					var userName = HttpContext.Current.User.Identity.Name;
					var userController = Windsor.Instance.Resolve<IUserController>();
					_currentClient = userController.GetUserByName(userName);
				}
				return _currentClient;
			}
		}
	}
}
