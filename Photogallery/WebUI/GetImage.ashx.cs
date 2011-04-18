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
			try
			{
				var controller = new PhotoController();
				int id;
				if (!int.TryParse(context.Request.Params["id"], out id))
					return;

				var image = controller.GetPhotoById(id);
				//No image - no response
				if (image == null)
					return;

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
			catch (Exception)
			{
				
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
