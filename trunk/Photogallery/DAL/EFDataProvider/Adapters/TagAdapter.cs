using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.EFDataProvider.Adapters
{
    class TagAdapter : Photogallery.ITag
    {
        internal readonly Tag _tag;

        public TagAdapter(Tag tag)
        {
            _tag = tag;
        }

        public int TagId
        {
            get { return _tag.TagId; }
            set { _tag.TagId = value; }
        }

        public string TagTitle
        {
            get { return _tag.Title; }
            set { _tag.Title = value; }
        }
    }
}
