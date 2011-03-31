﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;
using System.Web;

namespace DAL.AbstractEntities
{
    public interface IGalleryUserRepository
    {
        void AddUser(GalleryUser user);

        void DeleteUser(Guid UserId);

        void UpdateUser(GalleryUser user);


    }
}