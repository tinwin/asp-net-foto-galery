using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Adapters;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
	public class AlbumRepository:IAlbumRepository
	{
		private PhotogalleryEntities _context;

		public AlbumRepository(PhotogalleryEntities context)
		{
			_context = context;
		}

		public IAlbum AddAlbum(IAlbum album)
		{
			var entity = Adapte(album);
			_context.AddToAlbumSet(entity);
			_context.SaveChanges();
			return new AlbumAdapter(entity);
		}

		public void DeleteAlbum(int id)
		{
            _context.DeleteObject(_context.AlbumSet.Where(albm => albm.AlbumId == id).First());
            _context.SaveChanges();
		}

		public void UpdateAlbum(IAlbum album)
		{
            var commentRepository = new CommentRepository(_context);
            var tagRepository = new TagRepository(_context);

            var adapter = album as AlbumAdapter;
            if (adapter != null)
            {
                foreach (var comment in adapter.AlbumComments)
                    if (!(comment is CommentAdapter))
                    {
                        var savedCommentEntity = (commentRepository.Add(comment) as CommentAdapter)._comment;
                        adapter._album.Comments.Add(savedCommentEntity);
                    }

                foreach (var tag in adapter.AlbumTags)
                    if (!(tag is TagAdapter))
                    {
                        var savedTagEntity = (tagRepository.AddTag(tag) as TagAdapter)._tag;
                        adapter._album.Tags.Add(savedTagEntity);
                    }
            }

            _context.SaveChanges();
		}

		public IAlbum GetAlbumById(int id)
		{
			return new AlbumAdapter((from a in _context.AlbumSet
									 where a.AlbumId==id
									 select a).First());
		}

		public Album Adapte(IAlbum album)
		{
			return new Album
	       	{
	       		Author = (from User u in _context.UserSet 
						  where u.UserId==album.User.UserId 
						  select u).First(),
				Title = album.Title,
				CreationDate = album.CreationDate
	       	};
		}
	}
}
