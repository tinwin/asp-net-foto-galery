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
	class TagController : ITagController
	{
		private readonly IAbstractGalleryProvider _dataProvider = Windsor.Instance.Resolve<IAbstractGalleryProvider>();

		public IEnumerable<ITag> GetAllTags()
		{
			return _dataProvider.TagRepository.GetAllTags();
		}

		public ITag GetTagById(int id)
		{
			return _dataProvider.TagRepository.GetTagById(id);
		}
	}
}
