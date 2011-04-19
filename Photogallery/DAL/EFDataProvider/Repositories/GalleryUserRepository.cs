using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Web.Security;
using DAL.AbstractEntities;
using DAL.EFDataProvider.Adapters;
using Photogallery;

namespace DAL.EFDataProvider.Repositories
{
    public class GalleryUserRepository : IGalleryUserRepository
    {
        private PhotogalleryEntities _context;
        




    	public string[] GalleryRoles
        {
            get
            {
                return (from role in _context.aspnet_Roles
                        select role.RoleName).ToArray();
            }
        }



        
        
        public GalleryUserRepository(PhotogalleryEntities context)
        {
            _context = context;

        }

   



        public IGalleryUser GetUserById(Guid userId)
        {
            return new UserAdapter((
                from u in _context.UserSet
                where u.UserId == userId
                select u).FirstOrDefault());

        }
       

        public void DeleteUser(Guid UserId)
        {

            var objToDelete = _context.UserSet.Where(p => p.UserId == UserId).SingleOrDefault();
            if (objToDelete != null)
            {
                _context.DeleteObject(objToDelete);
                _context.SaveChanges();
            }
        }

        public void DeleteUserByName(string name)
        {
            string loweredName = name.ToLower();
            User userToDelete = _context.UserSet.Where(p => p.aspnet_Users.LoweredUserName ==loweredName ).FirstOrDefault();
            if(userToDelete!=null)
            {
                _context.DeleteObject(userToDelete);
                _context.SaveChanges();
            }
        
        }


        public void UpdateUser(Guid userId, string userName, string userMail ,string role)
        {
          
         

            IGalleryUser user = GetUserById(userId);
            user.Username  = userName;
            user.UserMail = userMail;
            user.UserRole = role;
            UpdateUser(user);
        }

       



        public void UpdateUser(IGalleryUser user)
        {
            UserAdapter userAdapter = user as UserAdapter;
            if (userAdapter != null)
            {
                aspnet_Users updatedEntity = _context.aspnet_Users.Where(p => p.UserId == user.UserId).FirstOrDefault();
              

                aspnet_Roles newRole =
                    _context.aspnet_Roles.Where(p => p.RoleName == user.UserRole).FirstOrDefault();
                if (newRole != null)
                {
                    updatedEntity.aspnet_Roles.Load();
                    if (GalleryRoles.Contains(newRole.RoleName))
                    {
                        updatedEntity.aspnet_Roles.Clear();
                        updatedEntity.aspnet_Roles.Add(newRole);
                    }

                }
                _context.SaveChanges();

            }
           
        }
    




    	public IGalleryUser GetUserByName(string name)
        {
            return new UserAdapter(_context.UserSet.Where(p => p.aspnet_Users.LoweredUserName  == name.ToLower( )).FirstOrDefault());
        }

     

        public bool UserExists(string name)
        {
            aspnet_Users user= _context.aspnet_Users.Where(p => p.UserName == name).FirstOrDefault();
            if(user!=null )
            {
                return true;
            }
            return false;
        }


        public bool UserMailExists(string mail)
        {
            if (_context.UserSet.Where(p => p.LoweredEmail == mail.ToLower()).FirstOrDefault() != null)
                return true;
            return false;

        }

        public IEnumerable<IGalleryUser> GetAllUsers()
        {
            foreach (User user in _context.UserSet)
                yield return new UserAdapter(user);
        }  
    }
}
