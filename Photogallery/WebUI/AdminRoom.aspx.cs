using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using DAL.AbstractEntities;
using Photogallery;

namespace WebUI.Admin
{
    public partial class AdminRoom : System.Web.UI.Page
    {
        public IGalleryUserRepository repository;
        
        public class AdminRoomProvider
        {

            
            private IGalleryUserRepository _userRepository;

            public AdminRoomProvider ()
            {
                IAbstractGalleryProvider provider = Windsor.Instance.Resolve<IAbstractGalleryProvider>();
                _userRepository = provider.GalleryUserRepository;
            
            }

            public void DeleteUser(Guid UserId)
            {
                _userRepository.DeleteUser(UserId); 
            }

            public void DeleteUser(IGalleryUser user)
            {
                _userRepository.DeleteUser(user.UserId);
            }

            public void UpdateUser(Guid UserId, string Username, string UserMail, string UserRole)
            {
                _userRepository.UpdateUser(UserId, Username, UserMail, UserRole);
            }

            public void UpdateUser(IGalleryUser user )
            {
                _userRepository.UpdateUser(user);
            }

            public  string[] AvailibleRoles()
            {
                return _userRepository.GalleryRoles;
            }

            public  IEnumerable<IGalleryUser> GetAllUsers()
            {
                return _userRepository.GetAllUsers();
            }
        }




        protected void Page_Load(object sender, EventArgs e)
        {

            repository = Windsor.Instance.Resolve<IAbstractGalleryProvider>().GalleryUserRepository;
            if (!HttpContext.Current.User.IsInRole("admin"))
            {
                Response.StatusCode = 403;
                Response.SuppressContent = true;
            } 
        
        }

        public void AdminRoomBind(object sender ,GridViewRowEventArgs eventArgs  )
        {
            
        }



        public void GridViewRowUpdating(object sender,GridViewUpdateEventArgs args)
        {
            args.Cancel = false;
            if (args.OldValues["Username"].ToString()  != args.NewValues["Username"].ToString()  
                && repository.UserExists((string)args.NewValues["Username"]))
            {
                args.Cancel = true;
                userExistsLabel.Visible = true;
            }
            if(args.OldValues["UserMail"].ToString() !=args.NewValues["UserMail"].ToString() &&
                    repository.UserMailExists((string)args.NewValues["UserMail"]  ))
            {
                args.Cancel = true;
                emailExistsLabel.Visible = true;
            }
   
        } 

    }
}
