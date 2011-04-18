using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace BuisnessLayer.AbstractControllers
{
	public interface ITagController
	{
		IEnumerable<ITag> GetAllTags();
		ITag GetTagById(int id);
	}
}
