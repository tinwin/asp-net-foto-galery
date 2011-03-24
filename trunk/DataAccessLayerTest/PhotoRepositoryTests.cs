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
            repo.Save(new Photo{Title = "title", Description=""});
            var photo = (from item in repo.Items
                         where item.Title=="title"
                        select item).SingleOrDefault();
            Assert.NotNull(photo);
        }
    }
}
