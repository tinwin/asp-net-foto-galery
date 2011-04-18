using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                _context.DeleteObject(_context.AlbumSet.Where(albm => albm.AlbumId == id).First());
                _context.SaveChanges();
            }catch(Exception e)
            {
                
            }
		}

		public void UpdateAlbum(IAlbum album)
		{
            var commentRepository = new CommentRepository(_context);
            var tagRepository = new TagRepository(_context);
            var photoRepository = new PhotoRepository(_context);
            var albumRepository = new AlbumRepository(_context);

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

                foreach (var photo in adapter.Photos)
                    if (!(photo is PhotoAdapter))
                    {
                        var savedPhotoEntity = (photoRepository.AddPhoto(photo) as PhotoAdapter)._photo;
                        adapter._album.Photos.Add(savedPhotoEntity);
                    }

                foreach (var childAlbum in adapter.ChildAlbums)
                    if (!(childAlbum is AlbumAdapter))
                    {
                        var savedChildAlbumEntity = (albumRepository.AddAlbum(childAlbum) as AlbumAdapter)._album;
                        adapter._album.ChildAlbums.Add(savedChildAlbumEntity);
                    }
            }

            _context.SaveChanges();
		}

		public IAlbum GetAlbumById(int id)
		{
            try
            {
                var x = (from a in _context.AlbumSet
                         where a.AlbumId == id
                         select a).First();
                return new AlbumAdapter();
            }catch(InvalidOperationException)
            {
                return null;
            }
		}

		public Album Adapte(IAlbum album)
		{
		    Album parentAlbum;
		    try
		    {
		        parentAlbum = _context.AlbumSet.Where(a => a.AlbumId == album.ParentAlbum.AlbumId).First();
		    }catch(NullReferenceException)
            {
                parentAlbum = null;
            }

			return new Album
	       	{
	       		Author = (from User u in _context.UserSet 
						  where u.UserId==album.User.UserId 
						  select u).First(),

				Title = album.Title,
				CreationDate = album.CreationDate,
                AlbumId = album.AlbumId,
                Description = album.Description,

                ParentAlbum = parentAlbum,
	       	};
		}

        public IEnumerable<IAlbum> getAlbumListByUserID(Guid id)
        {
            var entities = ( from album in _context.AlbumSet 
                             where album.Author.UserId == id 
                             select album);
            
            var albums = new List<IAlbum>();
            foreach (var entity in entities)
                albums.Add(new AlbumAdapter(entity));
            return albums;
        }
	}
}
