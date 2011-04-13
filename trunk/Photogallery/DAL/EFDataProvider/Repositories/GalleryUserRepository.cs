using System;
using System.Collections.Generic;
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

		public GalleryUserRepository(PhotogalleryEntities context)
		{
			_context = context;
		}

		public IGalleryUser GetUserById(Guid userId)
		{
			return new UserAdapter((
				from u in _context.UserSet
				where u.UserId == userId
				select u).
				Single());
		}

		public IGalleryUser AddUser(IGalleryUser user)
		{
			throw new NotImplementedException();
		}

		public void DeleteUser(Guid UserId)
		{
			throw new NotImplementedException();
		}

		public void UpdateUser(IGalleryUser user)
		{
			throw new NotImplementedException();
		}

		public IGalleryUser SelectUserByGuid(Guid guid)
		{
			return new UserAdapter((from u in _context.UserSet
									where u.UserId == guid
									select u).First());
		}
	}
}
