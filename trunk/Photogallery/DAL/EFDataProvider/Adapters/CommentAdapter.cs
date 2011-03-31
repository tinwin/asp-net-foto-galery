using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
    class CommentAdapter : IComment
    {
        private readonly Comment _comment;

        public CommentAdapter(Comment comment)
        {
            _comment = comment;
        }

        public int CommentId
        {
            get { return _comment.CommentId; }
            set { _comment.CommentId = value; }
        }

        public string Text
        {
            get { return _comment.Text; }
            set { _comment.Text = value; }
        }

        private IGalleryUser _author;

        public IGalleryUser Author
        {
            get
            {
                if (_author == null)
                    _author = new UserAdapter(_comment.Author);
                return _author;
            }
            set { _author = value; }
        }

        public DateTime AdditionDate
        {
            get { return _comment.AdditionDate; }
            set { _comment.AdditionDate = value; }
        }
    }
}
