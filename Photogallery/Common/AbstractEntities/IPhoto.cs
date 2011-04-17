using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Drawing;

namespace Photogallery
{
    public interface IPhoto
    {
        int PhotoId { get; set; }

        string PhotoTitle { get; set; }

        IAlbum HostAlbum { get; set; }

        IEnumerable<IComment> PhotoComments { get; set; }

        IEnumerable<ITag> PhotoTags { get; set; }

        IGalleryUser OwningUser { get; set; }


        Image PhotoThumbnail { get; set; }

        Image OptimizedPhoto { get; set; }

        Image OriginalPhoto { get; set; }

        string PhotoDescription { get; set; }

        DateTime AdditionDate { get; set; }

        void AddComment(IComment comment);
		void AddTag(ITag tag);
		void RemoveTag(ITag tag);
        void DeleteCommentById(int commentId);

        void UpdateComment(IComment comment);


    }
}
