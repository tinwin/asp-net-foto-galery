using System;
using System.Collections.Generic;
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

        

        private MembershipProvider _membershipProvider;



        public string[] GalleryRoles
        {
            get
            {
                return (from role in _context.aspnet_Roles
                        select role.RoleName).ToArray();
            }

        }


        public GalleryUserRepository(PhotogalleryEntities context,

                                      MembershipProvider membershipProvider)
        {
            _context = context;
            _membershipProvider = membershipProvider;
            if (_membershipProvider.PasswordFormat != MembershipPasswordFormat.Clear)
                throw new Exception("Membership provider must implement Encrypted passwords");



        }




        public IGalleryUser GetUserById(Guid userId)
        {
            return new UserAdapter((
                from u in _context.UserSet
                where u.UserId == userId
                select u).
                Single());
        }

        public IGalleryUser AddUser(IGalleryUser user, string userPassword)
        {
            MembershipCreateStatus userStatus;
            UserAdapter userAdapter;
            MembershipUser membershipUser = _membershipProvider.CreateUser(user.Username,
                                           userPassword,
                                           user.UserMail,
                                            "111",
                                           "111",
                                           true,
                                           null,
                                           out userStatus);

            if (userStatus == MembershipCreateStatus.Success)
            {
                if (user.UserComments == null || user.UserComments.Count() != 0)
                {
                    throw new Exception(" newly created user cannot contain comments ");
                }
                if (GalleryRoles.Contains(user.UserRole))
                {
                    Guid userId = (Guid)membershipUser.ProviderUserKey;
                    User contextObject =
                        _context.UserSet.Where(p => p.UserId == userId).First();
                    contextObject.aspnet_UsersReference.Load();
                    contextObject.aspnet_Users.aspnet_Roles.Load();
                    contextObject.aspnet_Users.Description = user.Description;
                    contextObject.aspnet_Users.aspnet_Roles.Add(
                        _context.aspnet_Roles.Where(p => p.RoleName == user.UserRole).First());


                    userAdapter =
                        new UserAdapter(contextObject);

                    AlbumRepository albumRepository = new AlbumRepository(_context);
                    /*int rootAlbumId = albumRepository.AddAlbum(new  Photogallery.   Album()
                    {
                        CreationDate = DateTime.Now,
                        Title = "RootAlbum",
                        User = userAdapter
                    }).AlbumId;
                    contextObject.aspnet_Users.RootAlbum =
                        _context.AlbumSet.Where(p => p.AlbumId == rootAlbumId).First();
                    */
                     _context.SaveChanges();

                    return userAdapter;

                }
                else
                {
                    throw new Exception(string.Format("Role {0} is not enabled on this server", user.UserRole));
                }
            }
            else
            {
                throw new MembershipCreateUserException(userStatus);
            }
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

        public void UpdateUser(IGalleryUser user)
        {

            if (user != null)
            {
                AlbumRepository albumRepository = new AlbumRepository(_context);

                User updatedEntity = _context.UserSet.Where(p => p.UserId == user.UserId).FirstOrDefault();
                if (updatedEntity != null)
                {
                    updatedEntity.aspnet_UsersReference.Load();
                    updatedEntity.aspnet_Users.UserName = user.Username;
                    updatedEntity.aspnet_Users.LoweredUserName = user.Username.ToLower();
                    updatedEntity.Email = user.UserMail;
                    updatedEntity.LoweredEmail = user.UserMail.ToLower();
                    aspnet_Roles newRole =
                        _context.aspnet_Roles.Where(p => p.RoleName == user.UserRole).FirstOrDefault();
                    if (newRole != null)
                    {
                        updatedEntity.aspnet_Users.aspnet_Roles.Load();
                        if (!GalleryRoles.Contains(newRole.RoleName))
                        {
                            updatedEntity.aspnet_Users.aspnet_Roles.Clear();
                            updatedEntity.aspnet_Users.aspnet_Roles.Add(newRole);
                        }

                    }

                    if (user.RootAlbum != null)
                    {
                        Album newAlbum =
                            _context.AlbumSet.Where(p => p.AlbumId == user.RootAlbum.AlbumId).FirstOrDefault();

                        updatedEntity.aspnet_Users.RootAlbumReference.Load();
                        int? oldAlbumId = updatedEntity.aspnet_Users.RootAlbum != null
                                              ? (int?)updatedEntity.aspnet_Users.RootAlbum.AlbumId
                                              : null;


                        if (newAlbum != null)
                        {
                            updatedEntity.aspnet_Users.RootAlbum = newAlbum;
                            _context.SaveChanges();

                            if (oldAlbumId != null)
                                albumRepository.DeleteAlbum((int)oldAlbumId);

                        }
                        else
                        {
                            _context.SaveChanges();
                        }

                    }
                    else
                    {
                        _context.SaveChanges();
                    }

                }
                else
                {
                    throw new Exception(string.Format("There is no user in db with id:{0}  ", user.UserId));
                }

            }
        }


        public IGalleryUser GetUserByName(string name)
        {
            return new UserAdapter(_context.UserSet.Where(p => p.aspnet_Users.UserName == name).First());
        }


        public void ChangeUserPassword(Guid userId, string password)
        {
            MembershipUser user = _membershipProvider.GetUser(userId, false);
            user.ChangePassword(user.GetPassword(), password);
        }
    }
}
