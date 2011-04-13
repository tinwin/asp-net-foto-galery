using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.Services;
using BuisnessLayer.ConcreteControllers;

namespace WebUI
{
	/// <summary>
	/// Summary description for $codebehindclassname$
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class GetImage : IHttpHandler
	{

		public void ProcessRequest(HttpContext context)
		{
			var controller = new PhotoController();
			int id = int.Parse(context.Request.Params["id"]);
			var image = controller.GetPhotoById(id);
			context.Response.ContentType = "image/jpeg";
			image.PhotoThumbnail.Save(context.Response.OutputStream, ImageFormat.Jpeg);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
