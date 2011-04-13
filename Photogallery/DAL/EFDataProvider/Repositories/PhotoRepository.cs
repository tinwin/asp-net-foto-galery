using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Adapters;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
	public class PhotoRepository : IPhotoRepository
	{
		private PhotogalleryEntities _context;

		public PhotoRepository(PhotogalleryEntities context)
		{
			_context = context;
		}

		public IPhoto AddPhoto(IPhoto photo)
		{
			var saved = Adapte(photo);
			_context.AddToPhotoSet(saved);
			_context.SaveChanges();
			return new PhotoAdapter(saved);
		}

		public void DeletePhoto(int photoId)
		{
			_context.DeleteObject(_context.PhotoSet.Where(p => p.PhotoId == photoId).First());
			_context.SaveChanges();
		}

		public void UpdatePhoto(IPhoto photo)
		{
			var commentRepository = new CommentRepository(_context);
			var tagRepository = new TagRepository(_context);

			var adapter = photo as PhotoAdapter;
			if (adapter!=null)
			{
				foreach (var comment in adapter.LocalComments)
					if (!(comment is CommentAdapter))
					{
						var savedCommentEntity = (commentRepository.Add(comment) as CommentAdapter)._comment;
						adapter._photo.Comments.Add(savedCommentEntity);
					}

				foreach (var tag in adapter._tags)
					if (!(tag is TagAdapter))
					{
						var savedTagEntity = (tagRepository.AddTag(tag) as TagAdapter)._tag;
						adapter._photo.Tags.Add(savedTagEntity);
					}
			}

			_context.SaveChanges();
		}

		public IPhoto GetPhotoById(int id)
		{
			var selected = (from photo in _context.PhotoSet
							where photo.PhotoId == id
							select photo).FirstOrDefault();
			return (selected == null) ? null : new PhotoAdapter(selected);
		}

		public IEnumerable<IPhoto> SelectPhotos(int startIndex, int count)
		{
			var entities = (from photo in _context.PhotoSet
							orderby photo.AdditionDate descending 
							select photo).
							Skip(startIndex).
							Take(count);
			List<IPhoto> photos = new List<IPhoto>();
			foreach (var entity in entities)
				photos.Add(new PhotoAdapter(entity));
			return photos;
		}

		private Photo Adapte(IPhoto photo)
		{
			Photo entity = new Photo
			{
				AdditionDate = photo.AdditionDate,
				Album = _context.AlbumSet.
					Where(a => a.AlbumId == photo.HostAlbum.AlbumId).
					FirstOrDefault(),
				Author = _context.UserSet.
					Where(m => m.UserId == photo.OwningUser.UserId).
					FirstOrDefault(),
				Description = photo.PhotoDescription,
				Title = photo.PhotoTitle,
                OriginalImage = photo.OriginalPhoto.ToByteArray(),
                OptimizedImage = photo.OptimizedPhoto.ToByteArray(),
				ImageThumbnail = photo.PhotoThumbnail.ToByteArray()                
			};
			return entity;
		}
	}
}
