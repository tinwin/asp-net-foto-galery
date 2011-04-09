using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.AbstractEntities
{
	interface ICommentRepository
	{
		IComment Add(IComment comment);
	}
}
