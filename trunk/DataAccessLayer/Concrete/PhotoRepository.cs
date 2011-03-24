using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Photogallery.DataAccessLayer.Abstract;

namespace Photogallery.DataAccessLayer.Concrete
{
    public class PhotoRepository : IRepository<Photo>
    {
        private EntityContainer _context = new EntityContainer(@"metadata=res://*/Concrete.EntityDataModel.csdl|res://*/Concrete.EntityDataModel.ssdl|res://*/Concrete.EntityDataModel.msl;provider=System.Data.SqlClient;provider connection string=""Data Source=PRIZRAK\SQLEXPRESS;Initial Catalog=photogallery;Integrated Security=True;MultipleActiveResultSets=True""");

        public IEnumerable<Photo> Items
        {
            get { return from photo in _context.PhotoSet select photo; }
        }

        public void Save(Photo item)
        {
            _context.AddToPhotoSet(item);
            _context.SaveChanges();
        }
    }
}
