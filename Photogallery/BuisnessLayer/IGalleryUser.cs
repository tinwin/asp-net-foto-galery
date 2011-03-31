using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Photogallery
{
    public interface IGalleryUser
    {
        IAlbum RootAlbum { get; set; }

        IEnumerable<IComment> UserComments { get; set; }

        string Description { get; set; }


        void AddComment(IComment comment);

        void DeleteCommentById(int commentId);


        void UpdateComment(IComment comment);

    }
}
