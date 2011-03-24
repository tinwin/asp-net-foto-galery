using System;
using System.Data.EntityClient;
using EFPocoAdapter;

namespace NorthwindEF
{
    public interface INorthwindEntities
    {
        IEntitySet<NorthwindEF.Employee> Employees { get; }
        IEntitySet<NorthwindEF.Territories.Territory> Territories { get; }
        IEntitySet<NorthwindEF.Region> Regions { get; }
        IEntitySet<NorthwindEF.Supplier> Suppliers { get; }
        IEntitySet<NorthwindEF.Product> Products { get; }
        IEntitySet<NorthwindEF.Category> Categories { get; }
        IEntitySet<NorthwindEF.Customer> Customers { get; }
        IEntitySet<NorthwindEF.Order> Orders { get; }
        IEntitySet<NorthwindEF.OrderDetail> OrderDetails { get; }
    }

    public partial class NorthwindEntities : EFPocoContext<NorthwindEF.PocoAdapters.NorthwindEntitiesAdapter>, INorthwindEntities
    {
        public NorthwindEntities() : base(new NorthwindEF.PocoAdapters.NorthwindEntitiesAdapter()) { }
        public NorthwindEntities(string connectionString) : base(new NorthwindEF.PocoAdapters.NorthwindEntitiesAdapter(connectionString)) { }
        public NorthwindEntities(EntityConnection connection) : base(new NorthwindEF.PocoAdapters.NorthwindEntitiesAdapter(connection)) { }

        public IEntitySet<NorthwindEF.Employee> Employees
        {
            get { return GetEntitySet<NorthwindEF.Employee>("Employees"); }
        }

        public IEntitySet<NorthwindEF.Territories.Territory> Territories
        {
            get { return GetEntitySet<NorthwindEF.Territories.Territory>("Territories"); }
        }

        public IEntitySet<NorthwindEF.Region> Regions
        {
            get { return GetEntitySet<NorthwindEF.Region>("Regions"); }
        }

        public IEntitySet<NorthwindEF.Supplier> Suppliers
        {
            get { return GetEntitySet<NorthwindEF.Supplier>("Suppliers"); }
        }

        public IEntitySet<NorthwindEF.Product> Products
        {
            get { return GetEntitySet<NorthwindEF.Product>("Products"); }
        }

        public IEntitySet<NorthwindEF.Category> Categories
        {
            get { return GetEntitySet<NorthwindEF.Category>("Categories"); }
        }

        public IEntitySet<NorthwindEF.Customer> Customers
        {
            get { return GetEntitySet<NorthwindEF.Customer>("Customers"); }
        }

        public IEntitySet<NorthwindEF.Order> Orders
        {
            get { return GetEntitySet<NorthwindEF.Order>("Orders"); }
        }

        public IEntitySet<NorthwindEF.OrderDetail> OrderDetails
        {
            get { return GetEntitySet<NorthwindEF.OrderDetail>("OrderDetails"); }
        }
    }
}
