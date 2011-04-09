using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Photogallery;

namespace DAL.AbstractEntities
{
    public interface ITagRepository
    {
        ITag AddTag(ITag tag);

        void DeleteTag(int TagId);

        void UpdateTag(ITag tag);

        ITag GetTagById(int id);
 
        

    }
}
