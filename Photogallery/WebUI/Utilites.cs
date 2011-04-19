using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI
{
	public static class Utilites
	{
		/// <summary>
		/// Try parse GUID from string
		/// </summary>
		/// <param name="str">String, contained GUID</param>
		/// <param name="guid">GUID</param>
		/// <returns>True, if GUID has been parsed successfully, False otherwise.</returns>
		public static bool TryParse(string str, out Guid guid)
		{
			try
			{
				guid = new Guid(str);
				return true;
			}
			catch (FormatException e)
			{
				guid = Guid.Empty;
			}
			catch (ArgumentNullException e)
			{
				guid = Guid.Empty;
			}
			return false;
		}
	}
}
