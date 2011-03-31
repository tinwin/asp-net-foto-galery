using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web ;
using System.Web.Security;


namespace Photogallery 
{
    public class GalleryUser:MembershipUser,IGalleryUser
    {
        //under construction 
        
        public Guid UserId { get; set; }

        public string Username { get; set; }
        
        public IAlbum RootAlbum { get; set; }

        public IEnumerable<IComment> UserComments
        {
            get
            {
                if (_userComments == null)
                    _userComments = new List<IComment>();
                return _userComments;
            } 

            set
            {
                _userComments = new List<IComment>(value);
            }
        }

        
        
        
        public string Description { get; set; }


        private IList<IComment> _userComments;  


        public void AddComment(IComment comment)
        {
            _userComments.Add(comment); 
        }

        public void DeleteCommentById(int commentId)
        {

            IComment comment = _userComments.Where( p => p.CommentId == commentId).First( );
            _userComments.Remove(comment);
        }


        public void UpdateComment(IComment Comment)
        {
            IComment comment = _userComments.Where(p => p.CommentId == Comment.CommentId ).First();
            comment.Text = Comment.Text; 
        }


       

    }
}
