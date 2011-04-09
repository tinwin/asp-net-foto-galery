using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Adapters;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
	class CommentRepository:ICommentRepository
	{
		private PhotogalleryEntities _context;

		public CommentRepository(PhotogalleryEntities context)
		{
			_context = context;
		}

		public IComment Add(IComment comment)
		{

			var entity = Adapte(comment);
			_context.AddToCommentSet(entity);
			_context.SaveChanges();

			return new CommentAdapter((from c in _context.CommentSet
									   where c.CommentId == entity.CommentId
									   select c).First());
		}

		private Comment Adapte(IComment comment)
		{
			var entity = new Comment
			{
				AdditionDate = comment.AdditionDate,
				Author = (from u in _context.UserSet 
						  where u.UserId == comment.Author.UserId 
						  select u).First(),
				Text = comment.Text             
			};
			return entity;
		}
	}
}
