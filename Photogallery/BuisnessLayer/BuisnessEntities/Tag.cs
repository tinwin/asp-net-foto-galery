﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Photogallery 
{
    public class Tag:ITag 
    {
        public int TagId { get; set; }

        public string TagTitle { get; set; }

    }
}
