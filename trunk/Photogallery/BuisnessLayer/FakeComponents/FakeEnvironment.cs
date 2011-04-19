using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BuisnessLayer.AbstractControllers;
using Common;
using Photogallery;

namespace BuisnessLayer.FakeComponents
{
	class FakeEnvironment : IEnvironment
	{
		public IGalleryUser CurrentClient
		{
			get
			{
				return Windsor.Instance.Resolve<IUserController>().
					GetUserByGuid(new Guid("29d25edd-7279-4a94-87b7-874c4b34827c"));
			}
		}
	}
}
