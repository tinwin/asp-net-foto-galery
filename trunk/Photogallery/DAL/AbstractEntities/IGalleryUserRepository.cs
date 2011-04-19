using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery;
using System.Web;
using System.Web.Security;

namespace DAL.AbstractEntities
{
    public interface IGalleryUserRepository
    {
        
    
        
        string[] GalleryRoles { get; }

        void DeleteUser(Guid UserId);

        IGalleryUser GetUserById(Guid Id);

        IGalleryUser GetUserByName(string name); 

        void UpdateUser(IGalleryUser user);
        void UpdateUser(Guid userId, string name, string mail, string role);

        bool UserExists(string name);
        bool UserMailExists(string name);
        void DeleteUserByName(string name);
        
        IEnumerable<IGalleryUser> GetAllUsers();
        

    }
}
