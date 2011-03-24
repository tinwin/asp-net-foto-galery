using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Photogallery.DataAccessLayer.Concrete;

namespace DataAccessLayerTest
{
    [TestFixture]
    public class PhotoRepositoryTests
    {
        [Test]
        public void ItemsTest()
        {
            PhotoRepository repo = new PhotoRepository();
            var photo = (from item in repo.Items
                        select item).SingleOrDefault();
        }
    }
}
