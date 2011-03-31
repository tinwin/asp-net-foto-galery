using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Photogallery;

namespace DAL.AbstractEntities
{
    public interface ITagRepository
    {
        void AddTag(Tag tag);

        void DeleteTag(int TagId);

        void UpdateTag(Tag tag);

        Tag GetTagById(int id);
 
        

    }
}
