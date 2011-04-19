using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLayer.AbstractControllers;
using BuisnessLayer.ConcreteControllers;
using Common;
using Photogallery;

namespace WebUI
{
    public partial class Albums : System.Web.UI.Page
    {
        IAlbumController controller = new AlbumController();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRepeater();
            }
        }



        public void pager_Command(object sender, CommandEventArgs e)
        {
            int currnetPageIndx = Convert.ToInt32(e.CommandArgument);
            pager1.CurrentIndex = currnetPageIndx;
            pager2.CurrentIndex = pager1.CurrentIndex;
            BindRepeater();
        }

        private void BindRepeater()
        {
            int skip = (pager1.CurrentIndex - 1) * pager1.PageSize;
            int take = pager1.PageSize;

			Guid userId;
			//Show photos by user
			if (Utilites.TryParse(Request.QueryString["user"], out userId))
			{
				AlbumList.DataSource = controller.SelectAlbumsPage(userId, skip, take);
				pager1.ItemCount = controller.GetAlbumsCount(userId);
			}
			else
			{
				AlbumList.DataSource = controller.SelectAllAlbumsPage(skip, take);
				pager1.ItemCount = controller.GetAlbumsCount();
			}
			AlbumList.DataBind();
			pager2.ItemCount = pager1.ItemCount;
        }

        protected void btnTest_click(object s, EventArgs e)
        {
            Button btnSender = s as Button;
            if (btnSender.CommandName == "postback")
            {
                Response.Write("postBack occured.");
                BindRepeater();
            }
            else if (btnSender.CommandName == "ps70")
            {
                pager1.PageSize = 70;
                pager2.PageSize = 70;
                BindRepeater();
            }
            else
            {
                pager1.PageSize = 15;
                pager2.PageSize = 15;
                BindRepeater();
            }
        }
    }
}
