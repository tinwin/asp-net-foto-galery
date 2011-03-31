using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photogallery
{
    public interface IAlbum
    {

        int AlbumId { get; set; }

        GalleryUser User { get; set; }

        string Title { get; set; }

        IAlbum ParentAlbum { get; set; }

        IEnumerable<IAlbum> ChildAlbums { get; set; }

        IEnumerable<Photo> Photos { get; set; }

        string Description { get; set; }


        DateTime CreationDate { get; set; }

        bool IsRootAlbum { get; set; }

        IEnumerable<Tag> AlbumTags { get; set; }



        void AddComment(Comment comment);

        void DeleteCommentById(int commentId);


        void UpdateComment(Comment comment);



    }
}
