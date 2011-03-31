using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
    class AlbumAdapter : Photogallery.IAlbum
    {
        internal Album _album;

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


        private IGalleryUser _user;

        public IGalleryUser User
        {
            get
            {
                if (_user == null)
                    _user = new UserAdapter(_album.aspnet_Membership);
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
            get { return _album.ParentAlbum; }
            set { _album.ParentAlbum = value; }
        }

        IEnumerable<IAlbum> IAlbum.ChildAlbums
        {
            get { return _album.ChildAlbums; }
            set { _album.ChildAlbums = value; }
        }

        public bool IsRootAlbum
        {
            get { return _album.IsRootAlbum; }
            set { throw new NotImplementedException(); }
        }


        private IEnumerable<IPhoto> _photos;
        public IEnumerable<IPhoto> Photos
        {
            get
            {
                if (_photos == null)
                {
                    _album.Photos.Load();
                    var list = new List<IPhoto>();
                    foreach (var photo in _album.Photos)
                        list.Add(new PhotoAdapter(photo));
                }
                return _photos;
            }
            set { _photos = value; }
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

        private IEnumerable<ITag> _tags;
        public IEnumerable<ITag> AlbumTags
        {
            get
            {
                if (_tags == null)
                {
                    _album.Tags.Load();
                    var list = new List<ITag>();
                    foreach (var tag in _album.Tags)
                        list.Add(new TagAdapter(tag));
                }
                return _tags;
            }
            set { _tags = value; }
        }


        public void AddComment(IComment comment)
        {
            throw new NotImplementedException();
        }

        public void DeleteCommentById(int commentId)
        {
            throw new NotImplementedException();
        }


        public void UpdateComment(Photogallery.IComment comment)
        {
            throw new NotImplementedException();
        }
    }
}
