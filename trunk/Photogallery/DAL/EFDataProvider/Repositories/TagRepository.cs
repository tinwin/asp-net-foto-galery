using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Adapters;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
	class TagRepository:ITagRepository
	{
		private PhotogalleryEntities _context;

		public TagRepository(PhotogalleryEntities context)
		{
			_context = context;
		}

		public ITag AddTag(ITag tag)
		{
			var entity = Adapte(tag);
			_context.AddToTagSet(entity);
			_context.SaveChanges();
			return new TagAdapter(entity);
		}

		public void DeleteTag(int TagId)
		{
			throw new NotImplementedException();
		}

		public void UpdateTag(ITag tag)
		{
			throw new NotImplementedException();
		}

		public ITag GetTagById(int id)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<ITag> GetAllTags()
		{
			var tags = new List<ITag>();
			foreach (var tag in _context.TagSet)
				tags.Add(new TagAdapter(tag));
			return tags;
		}

		public Tag Adapte(ITag tag)
		{
			return new Tag
	       	{
	       		Title = tag.TagTitle
	       	};
		}
	}
}
