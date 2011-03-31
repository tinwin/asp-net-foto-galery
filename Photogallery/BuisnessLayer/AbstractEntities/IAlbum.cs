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

        IGalleryUser User { get; set; }

        string Title { get; set; }

        IAlbum ParentAlbum { get; set; }

        IEnumerable<IAlbum> ChildAlbums { get; set; }

        IEnumerable<IPhoto> Photos { get; set; }

        string Description { get; set; }


        DateTime CreationDate { get; set; }

        bool IsRootAlbum { get;}

        IEnumerable<ITag> AlbumTags { get; set; }

        IEnumerable<IComment> AlbumComments { get; set; }



        void AddComment(IComment comment);

        void DeleteCommentById(int commentId);


        void UpdateComment(IComment comment);



    }
}
