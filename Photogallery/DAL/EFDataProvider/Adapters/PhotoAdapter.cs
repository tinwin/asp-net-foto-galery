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
            _hostAlbum = new AlbumAdapter(photo.Album);
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

        private Photogallery.IAlbum _hostAlbum;

        public Photogallery.IAlbum HostAlbum
        {
            get { return _hostAlbum; }
            set { _hostAlbum = value; }
        }

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

        public override Image OptimizedPhoto
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public override Image OriginalPhoto
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public override string PhotoDescription
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }

        public override DateTime AdditionDate
        {
            get { return _photo.PhotoId; }
            set { _photo.PhotoId = value; }
        }




        public override void AddComment(Comment comment)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCommentById(int commentId)
        {
            throw new NotImplementedException();
        }


        public override void UpdateComment(Comment comment)
        {


            throw new NotImplementedException();

        }
    }
}
