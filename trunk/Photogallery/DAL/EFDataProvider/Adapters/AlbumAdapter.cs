using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
	public class AlbumAdapter : IAlbum
	{
		internal Album _album;
        private IGalleryUser _user;
        private IAlbum _parentAlbum;
        private List<IAlbum> _childAlbums;
        private List<IPhoto> _photos;
        private List<ITag> _albumTags;
        private List<IComment> _albumComments;

		public AlbumAdapter()
		{
		}

		public AlbumAdapter(Album album)
		{
			_album = album;
		}

		public int AlbumId
		{
			get { return _album.AlbumId; }
			set { _album.AlbumId = value; }
		}
		
		public IGalleryUser User
		{
			get
			{
				if (_user == null)
					_user = new UserAdapter(_album.Author);
				return _user;
			}
			set { _user = value; }
		}

		public string Title
		{
			get { return _album.Title; }
			set { _album.Title = value; }
		}

		public IAlbum ParentAlbum
		{
			get
			{
				if (_parentAlbum == null)
					_parentAlbum = new AlbumAdapter(_album.ParentAlbum);
				return _parentAlbum;
			}
			set { _parentAlbum = value; }
		}

		public IEnumerable<IAlbum> ChildAlbums
		{
			get
			{
				if (_childAlbums == null)
				{
					_childAlbums = new List<IAlbum>();
					_album.ChildAlbums.Load();
					foreach (var album in _album.ChildAlbums)
						_childAlbums.Add(new AlbumAdapter(album));
				}
				return _childAlbums;
			}
			set { _childAlbums = value.ToList(); }
		}

		public IEnumerable<IPhoto> Photos
		{
			get
			{
				if (_photos == null)
				{
					_photos = new List<IPhoto>();
					_album.Photos.Load();
					foreach (var photo in _album.Photos)
						_photos.Add(new PhotoAdapter(photo));
				}
				return _photos;
			}
			set { _photos = value.ToList(); }
		}

		public string Description
		{
			get { return _album.Description; }
			set { _album.Description = value; }
		}

		public DateTime CreationDate
		{
			get { return _album.CreationDate; }
			set { _album.CreationDate = value; }
		}

		public bool IsRootAlbum
		{
			get { return _album.Author.aspnet_Users.RootAlbum == _album; }
		}

		public IEnumerable<ITag> AlbumTags
		{
			get
			{
				if (_albumTags == null)
				{
					_albumTags = new List<ITag>();
					_album.Tags.Load();
					foreach (var tag in _album.Tags)
						_albumTags.Add(new TagAdapter(tag));
				}
				return _albumTags;
			}
			set { throw new NotImplementedException(); }
		}

		public IEnumerable<IComment> AlbumComments
		{
			get
			{
				if(_albumComments==null)
				{
					_albumComments = new List<IComment>();
					_album.Comments.Load();
					foreach (var comment in _album.Comments)
						_albumComments.Add(new CommentAdapter(comment));
				}
				return _albumComments;
			}
			set
			{
				_albumComments = value.ToList();
			}
		}

		public void AddComment(IComment comment)
		{
            _albumComments.Add(comment);           

		}

		public void DeleteCommentById(int commentId)
		{
		    _albumComments.Remove(_albumComments.Where(cmnts => cmnts.CommentId == commentId).SingleOrDefault());
		}

		public void UpdateComment(IComment comment)
		{
			throw new NotImplementedException();
		}
	}
}
