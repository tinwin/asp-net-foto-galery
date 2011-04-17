using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Web.Security;

namespace Photogallery
{
    
    
    
    public interface IGalleryUser
    {
        Guid UserId { get; set; }

        string Username { get; set; }

      

       string UserRole { get; set; }

        string UserMail { get; set; } 
        
        IAlbum RootAlbum { get; set; }

        IEnumerable<IComment> UserComments { get; }

        string Description { get; set; }
 

        void AddComment(IComment comment);

        void DeleteCommentById(int commentId);


        void UpdateComment(IComment comment);

    }
}
