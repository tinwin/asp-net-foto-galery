﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace BuisnessLayer.AbstractControllers
{
	public interface IUserController
	{
		IGalleryUser GetUserByGuid(Guid guid);
		IGalleryUser GetUserByName(string name);
	}
}
