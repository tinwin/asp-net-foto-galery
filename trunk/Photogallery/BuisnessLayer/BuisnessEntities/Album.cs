using System;
using System.Collections.Generic;
using System.Linq;

namespace Photogallery 
{
    
    
    public class Album:IAlbum 
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }

        public IGalleryUser User { get; set; }
        public IAlbum ParentAlbum { get; set; }
        public IEnumerable<IAlbum> ChildAlbums { get; set; }
        public IEnumerable<IPhoto> Photos { get; set; }
        public IEnumerable<ITag> AlbumTags { get; set; }
        private IList<IComment> _albumComments;


        public IEnumerable<IComment> AlbumComments
        {
            get
            {
                if (_albumComments == null)
                    _albumComments = new List<IComment>();
                return _albumComments;
            }

            set
            {
                _albumComments = new List<IComment>(value);
            }
        }

        public bool IsRootAlbum 
        { 
            get
            {
                if(ParentAlbum ==null)
                    return true;
                return false;
            }
        }

        public void AddComment(IComment comment)
        {
            _albumComments.Add(comment);
        }

        public void DeleteCommentById(int commentId)
        {

            IComment comment = _albumComments.Where(p => p.CommentId == commentId).First();
            _albumComments.Remove(comment);
        }

        public void UpdateComment(IComment Comment)
        {
            IComment comment = _albumComments.Where(p => p.CommentId == Comment.CommentId).First();
            comment.Text = Comment.Text;
        }
    }
}
  