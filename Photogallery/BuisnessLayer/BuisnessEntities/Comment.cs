using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Photogallery 
{
    public class Comment:IComment
    {
        public int CommentId { get; set; }

        public string Text { get; set; }

        public IGalleryUser Author { get; set; }

        public DateTime AdditionDate { get; set; }

    }
}
