using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;
using System.Web;

namespace DAL.AbstractEntities
{
    public interface IGalleryUserRepository
    {
        void AddUser(IGalleryUser user);

        void DeleteUser(Guid UserId);

        void UpdateUser(IGalleryUser user);


    }
}
