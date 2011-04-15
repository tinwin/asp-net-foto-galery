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

			switch (context.Request.Params["size"] ?? "optimized")
			{
				case "thumbnail":
					image.PhotoThumbnail.Save(context.Response.OutputStream, ImageFormat.Jpeg);
					break;
				case "optimized":
					image.OptimizedPhoto.Save(context.Response.OutputStream, ImageFormat.Jpeg);
					break;
				case "original":
					image.OriginalPhoto.Save(context.Response.OutputStream, ImageFormat.Jpeg);
					break;
				default:
					throw new ArgumentException("Unsupported image size specificator: \"" + context.Request.Params["size"] + "\"");
			}
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
