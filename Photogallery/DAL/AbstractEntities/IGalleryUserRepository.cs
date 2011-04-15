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
        IGalleryUser AddUser(IGalleryUser user, string userPassword);

        string[] GalleryRoles { get; }

        void DeleteUser(Guid UserId);

        string ResetUserPassword(Guid userId);

        IGalleryUser GetUserById(Guid Id);

        IGalleryUser GetUserByName(string name); 

        void UpdateUser(IGalleryUser user);


    }
}
