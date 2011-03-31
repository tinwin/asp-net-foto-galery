using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photogallery
{
    public interface ITag
    {
        int TagId { get; set; }

        string TagTitle { get; set; }

    }
}
