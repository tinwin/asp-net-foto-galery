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
    class PhotoAdapter : Photogallery.IPhoto
    {
        private readonly Photo _photo;

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

        private List<IComment> _comments;

        public IEnumerable<IComment> PhotoComments
        {
            get
            {
                if (_comments == null)
                {
                    _photo.Comments.Load();
					_comments = new List<IComment>();
                    foreach(var comment in _photo.Comments)
                        _comments.Add(new CommentAdapter(comment));
                }
                return _comments;
            }
            set { _comments = value.ToList(); }
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
            _comments.Add(comment);
        }

        public void DeleteCommentById(int commentId)
        {
            _comments.Remove(_comments.Where(c => c.CommentId == commentId).SingleOrDefault());
        }

        public void UpdateComment(IComment comment)
        {


            throw new NotImplementedException();

        }
    }
}
