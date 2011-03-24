using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Photogallery.DataAccessLayer.Abstract
{
    public interface IRepository<T>
    {
        IEnumerable<T> Items { get; }
    }
}
