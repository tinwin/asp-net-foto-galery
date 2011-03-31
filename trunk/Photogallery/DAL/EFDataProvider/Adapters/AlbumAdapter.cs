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

        public override int AlbumId
        {
            get { return _album.AlbumId; }
            set { _album.AlbumId = value; }
        }

        public override IGalleryUser User
        {
            get { return _album.User; }
            set { _album.User = value; }
        }

        public override string Title
        {
            get { return _album.Title; }
            set { _album.Title = value; }
        }

        public override Album ParentAlbum
        {
            get { return _album.ParentAlbum; }
            set { _album.ParentAlbum = value; }
        }

        public override IEnumerable<Album> ChildAlbums
        {
            get { return _album.ChildAlbums; }
            set { _album.ChildAlbums = value; }
        }

        public override IEnumerable<Photo> Photos
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override string Description
        {
            get { return _album.Description; }
            set { _album.Description = value; }
        }


        public override DateTime CreationDate
        {
            get { return _album.CreationDate; }
            set { _album.CreationDate = value; }
        }

        public override bool IsRootAlbum
        {
            get { return _album.IsRootAlbum; }

        }

        public override IEnumerable<Tag> AlbumTags
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


        public override void AddComment(Photogallery.IComment comment)
        {
            throw new NotImplementedException();
        }

        public override void DeleteCommentById(int commentId)
        {
            throw new NotImplementedException();
        }


        public override void UpdateComment(Photogallery.IComment comment)
        {
            throw new NotImplementedException();
        }
    }
}
