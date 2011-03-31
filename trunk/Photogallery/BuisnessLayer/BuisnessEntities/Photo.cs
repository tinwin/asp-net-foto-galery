using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Photogallery 
{
    public class Photo:IPhoto
    {

        public Guid PhotoId { get; set; }

        public string PhotoTitle { get; set; }

        public IAlbum HostAlbum { get; set; }

        private IList<IComment> _photoComments;
        
        public IEnumerable<IComment> PhotoComments
        {
            get
            {
                if (_photoComments == null)
                    _photoComments = new List<IComment>();
                return _photoComments;
            }

            set
            {
                _photoComments = new List<IComment>(value);
            }
        }


        public IEnumerable<ITag> PhotoTags 
        { 
            get
            {
                if (_photoTags == null)
                    _photoTags = new List<ITag>();
                return _photoTags;    
            } 
            set
            {
                _photoTags = new List<ITag>(value);
            } 
        }

        private IList<ITag> _photoTags;

        public IGalleryUser OwningUser { get; set; }


        public Image PhotoThumbnail { get; set; }

        public Image OptimizedPhoto { get; set; }

        public Image OriginalPhoto { get; set; }

        public string PhotoDescription { get; set; }

        public DateTime AdditionDate { get; set; }

        public void AddComment(IComment comment)
        {
            _photoComments.Add(comment);
        }

        public void DeleteCommentById(int commentId)
        {

            IComment comment = _photoComments.Where(p => p.CommentId == commentId).First();
            _photoComments.Remove(comment);
        }


        public void UpdateComment(IComment Comment)
        {
            IComment comment = _photoComments.Where(p => p.CommentId == Comment.CommentId).First();
            comment.Text = Comment.Text;
        }

    }
}
