using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
    class PhotoAdapter : Photogallery.IPhoto
    {
        private Photo _photo;

        public PhotoAdapter()
        {
        }

        public PhotoAdapter(Photo photo)
        {
            _photo = photo;
            HostAlbum = new AlbumAdapter(photo.Album);
        }

        public Guid PhotoId 
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public string PhotoTitle
        {
            get { return _photo.Title; }
            set { _photo.Title = value; }
        }

        public IAlbum HostAlbum { get; set; }

        private IEnumerable<IComment> _comments;

        public IEnumerable<IComment> PhotoComments
        {
            get
            {
                if (_comments == null)
                {
                    _photo.Comments.Load();
                    var list = new List<IComment>();
                    foreach(var comment in _photo.Comments)
                        list.Add(new CommentAdapter(comment));
                }
                return _comments;
            }
            set { _comments = value; }
        }

        private IEnumerable<ITag> _tags;

        public IEnumerable<ITag> PhotoTags
        {
            get
            {
                if (_tags == null)
                {
                    _photo.Tags.Load();
                    var list = new List<ITag>();
                    foreach (var tag in _photo.Tags)
                        list.Add(new TagAdapter(tag));
                }
                return _tags;
            }
            set { _tags = value; }
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
            get { return _image; }
            set
            {
                if (_image == null)
                    _image = new Bitmap(new MemoryStream(_photo.ImageThumbnail));
                _photo.PhotoId = value;
            }
        }

        public Image OptimizedPhoto
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public Image OriginalPhoto
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public string PhotoDescription
        {
            get { return _photo.Description; }
            set { _photo.Description = value; }
        }

        public DateTime AdditionDate
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public void AddComment(IComment comment)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommentById(int commentId)
        {
            throw new NotImplementedException();
        }

        public void UpdateComment(IComment comment)
        {
            throw new NotImplementedException();
        }
    }
}
