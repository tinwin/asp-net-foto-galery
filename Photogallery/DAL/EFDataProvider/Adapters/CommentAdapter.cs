using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;

namespace DAL.EFDataProvider.Adapters
{
    class CommentAdapter : IComment
    {
        internal readonly Comment _comment;

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
		private bool _isAuthorLoaded;

        public IGalleryUser Author
        {
            get
            {
				if (!_isAuthorLoaded)
				{
					_comment.AuthorReference.Load();
					_author = new UserAdapter(_comment.Author);
					_isAuthorLoaded = true;
				}
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
