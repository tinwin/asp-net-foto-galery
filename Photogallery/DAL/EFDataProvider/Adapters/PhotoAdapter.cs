using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
	class PhotoAdapter : IPhoto, IAdapter<Photo, IPhoto>
    {
        internal readonly Photo _photo;

        public PhotoAdapter()
        {
        }

        public PhotoAdapter(Photo photo)
        {
            _photo = photo;
            _hostAlbum = new AlbumAdapter(photo.Album);
        }

        public int PhotoId 
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public string PhotoTitle
        {
            get { return _photo.Title; }
            set { _photo.Title = value; }
        }

        private Photogallery.IAlbum _hostAlbum;

        public Photogallery.IAlbum HostAlbum
        {
            get { return _hostAlbum; }
            set { _hostAlbum = value; }
        }

		internal List<IComment> LocalComments = new List<IComment>();
		private bool _isCommentsConverted;

        public IEnumerable<IComment> PhotoComments
        {
            get
            {
                if (!_isCommentsConverted)
                {
                    _photo.Comments.Load();
                    foreach(var comment in _photo.Comments)
                        LocalComments.Add(new CommentAdapter(comment));
					_isCommentsConverted = true;
                }
                return LocalComments;
            }
            set { LocalComments = value.ToList(); }
        }

        internal List<ITag> _tags = new List<ITag>();
		private bool _isTagsLoaded;

        public IEnumerable<ITag> PhotoTags
        {
            get
			{
                if (!_isTagsLoaded)
                {
                    _photo.Tags.Load();
                    foreach (var tag in _photo.Tags)
                        _tags.Add(new TagAdapter(tag));
					_isTagsLoaded = true;
                }
                return _tags;
            }
            set { throw new InvalidOperationException("Can't directly set");}
        }

        private IGalleryUser _owningUser;

        public IGalleryUser OwningUser
        {
            get
            {
                if (_owningUser == null)
                    _owningUser = new UserAdapter(_photo.Author);
                return _owningUser;
            }
            set { _owningUser = value; }
        }


        private Image _image;
        public Image PhotoThumbnail
        {
            get
            {
                if (_image == null)
                    _image = new Bitmap(new MemoryStream(_photo.ImageThumbnail));
                return _image;
            }
            set { _image = value; }
        }

        private Image _optimizedPhoto;
        public Image OptimizedPhoto
        {
            get
            {
                if (_optimizedPhoto == null)
                    _optimizedPhoto = new Bitmap(new MemoryStream(_photo.OptimizedImage));
                return _optimizedPhoto;
            }
            set { _optimizedPhoto = value; }
        }

        private Image _originalPhoto;       
        public Image OriginalPhoto
        {
            get
            {
                if (_originalPhoto == null)
                    _originalPhoto = new Bitmap(new MemoryStream(_photo.OriginalImage));
                return _originalPhoto;
            }
            set { _originalPhoto = value; }
        }

        public string PhotoDescription
        {
            get { return _photo.Description; }
            set { _photo.Description = value; }
        }

        public DateTime AdditionDate
        {
            get { return _photo.AdditionDate; }
            set { _photo.AdditionDate = value; }
        }

        public void AddComment(IComment comment)
        {
            LocalComments.Add(comment);
        }

		public void AddTag(ITag tag)
		{
			_tags.Add(tag);
		}

        public void DeleteCommentById(int commentId)
        {
            LocalComments.Remove(LocalComments.Where(c => c.CommentId == commentId).SingleOrDefault());
        }

        public void UpdateComment(IComment comment)
        {


            throw new NotImplementedException();

        }

		public IPhoto CreateAdapter(Photo source)
		{
			return new PhotoAdapter(source);
		}
    }
}
