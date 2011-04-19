using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Common;
using DAL.AbstractEntities;
using DAL.EFDataProvider;
using Photogallery;

namespace WebUI
{
    public partial class Registration : System.Web.UI.Page
    {
        private IGalleryUserRepository _userRepository;
        
        protected void Page_Load(object sender, EventArgs e)
        {

            IAbstractGalleryProvider provider = Windsor.Instance.Resolve<IAbstractGalleryProvider >();

            _userRepository = provider.GalleryUserRepository;
            if (Request.IsAuthenticated)
                Response.Redirect("Default.aspx");
        

        }

        public void NameCheck(object sender ,ServerValidateEventArgs args)
        {
           if( _userRepository.UserExists(args.Value  ))
           {
               args.IsValid = false;
           }
           else
           {
               args.IsValid = true; 
           }
           

        }

        public void CheckEmail(object sender, ServerValidateEventArgs args)
        {
            if (_userRepository.UserMailExists( args.Value))
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }


        }

        public void CheckPassword(object sender, ServerValidateEventArgs args)
        {
            if(password.Text.Length<10)
                args.IsValid = password.Text == confirmPassword.Text ? true : false;
            else
            {
                args.IsValid = false;
                CustomValidator1.ErrorMessage = "password should be no longer then 10 symbols";
            }
            

        }

        public void CheckDescription(object sender, ServerValidateEventArgs args)
        {
            
                args.IsValid = args.Value.Length <800 ? true : false;
           

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            MembershipCreateStatus status;
            Membership.CreateUser(name.Text, password.Text, email.Text, "111", "111", true, out status);
            if (status == MembershipCreateStatus.Success)
            {
                IGalleryUser user= _userRepository.GetUserByName(name.Text);
                user.Description = description.Text;
                user.UserRole = "user";
                _userRepository.UpdateUser(user); 
                Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
                FormsAuthentication.SetAuthCookie(name.Text, false);
                Response.Redirect("Default.aspx");
            }
            else
            {
                Label1.Visible = true;
            }

            
           
        }
    }
}
