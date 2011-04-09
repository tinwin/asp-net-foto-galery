using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace DAL.EFDataProvider
{
	public static class ImageExtensions
	{
		public static byte[] ToByteArray(this Image imageIn)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
				return ms.ToArray();
			}
		}
	}
}
