using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photogallery
{
    public interface IComment
    {
        int CommentId { get; set; }

        string Text { get; set; }

        IGalleryUser Author { get; set; }


        DateTime AdditionDate { get; set; }


    }
}
