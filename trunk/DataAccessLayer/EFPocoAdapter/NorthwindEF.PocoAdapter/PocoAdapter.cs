// Global Attributes

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

// Association Types

[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "TerritoriesRegion", "Region", global::System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(NorthwindEF.PocoAdapters.RegionAdapter), "Territories", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.Territories.PocoAdapters.TerritoryAdapter))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "EmployeesTerritories", "Employees", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.PocoAdapters.EmployeeAdapter), "Territories", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.Territories.PocoAdapters.TerritoryAdapter))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Products_Supplier", "Supplier", global::System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(NorthwindEF.PocoAdapters.SupplierAdapter), "Products", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.PocoAdapters.ProductAdapter))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Products_Category", "Category", global::System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(NorthwindEF.PocoAdapters.CategoryAdapter), "Products", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.PocoAdapters.ProductAdapter))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "CustomerOrders", "Customer", global::System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(NorthwindEF.PocoAdapters.CustomerAdapter), "Orders", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.PocoAdapters.OrderAdapter))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Order_Details_Order", "Order", global::System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(NorthwindEF.PocoAdapters.OrderAdapter), "OrderDetails", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.PocoAdapters.OrderDetailAdapter))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Order_Details_Product", "Product", global::System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(NorthwindEF.PocoAdapters.ProductAdapter), "OrderDetails", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(NorthwindEF.PocoAdapters.OrderDetailAdapter))]

namespace NorthwindEF.PocoAdapters
{
    using System;
    using System.Data;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ComponentModel;
    using EFPocoAdapter;
    using EFPocoAdapter.DataClasses;

    // Entity Containers

    public partial class NorthwindEntitiesAdapter : ObjectContext, IPocoAdapterObjectContext
    {
        private static QueryTranslationCache _qtc = new QueryTranslationCache();
        static NorthwindEntitiesAdapter()
        {
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.CommonAddress), typeof(global::NorthwindEF.PocoAdapters.CommonAddressAdapter), null);
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Employee), typeof(global::NorthwindEF.PocoAdapters.EmployeeAdapter), null);
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Territories.Territory), typeof(global::NorthwindEF.Territories.PocoAdapters.TerritoryAdapter), (arg) => new global::NorthwindEF.Territories.PocoAdapters.TerritoryAdapter((global::NorthwindEF.Territories.Territory)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Region), typeof(global::NorthwindEF.PocoAdapters.RegionAdapter), (arg) => new global::NorthwindEF.PocoAdapters.RegionAdapter((global::NorthwindEF.Region)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Supplier), typeof(global::NorthwindEF.PocoAdapters.SupplierAdapter), (arg) => new global::NorthwindEF.PocoAdapters.SupplierAdapter((global::NorthwindEF.Supplier)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Product), typeof(global::NorthwindEF.PocoAdapters.ProductAdapter), (arg) => new global::NorthwindEF.PocoAdapters.ProductAdapter((global::NorthwindEF.Product)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Category), typeof(global::NorthwindEF.PocoAdapters.CategoryAdapter), (arg) => new global::NorthwindEF.PocoAdapters.CategoryAdapter((global::NorthwindEF.Category)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.ContactInfo), typeof(global::NorthwindEF.PocoAdapters.ContactInfoAdapter), null);
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Customer), typeof(global::NorthwindEF.PocoAdapters.CustomerAdapter), (arg) => new global::NorthwindEF.PocoAdapters.CustomerAdapter((global::NorthwindEF.Customer)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.Order), typeof(global::NorthwindEF.PocoAdapters.OrderAdapter), (arg) => new global::NorthwindEF.PocoAdapters.OrderAdapter((global::NorthwindEF.Order)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.OrderDetail), typeof(global::NorthwindEF.PocoAdapters.OrderDetailAdapter), (arg) => new global::NorthwindEF.PocoAdapters.OrderDetailAdapter((global::NorthwindEF.OrderDetail)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.InternationalOrder), typeof(global::NorthwindEF.PocoAdapters.InternationalOrderAdapter), (arg) => new global::NorthwindEF.PocoAdapters.InternationalOrderAdapter((global::NorthwindEF.InternationalOrder)arg));
            _qtc.AddTypeMapping(typeof(global::CurrentEmployee), typeof(global::PocoAdapters.CurrentEmployeeAdapter), (arg) => new global::PocoAdapters.CurrentEmployeeAdapter((global::CurrentEmployee)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.PreviousEmployee), typeof(global::NorthwindEF.PocoAdapters.PreviousEmployeeAdapter), (arg) => new global::NorthwindEF.PocoAdapters.PreviousEmployeeAdapter((global::NorthwindEF.PreviousEmployee)arg));
            _qtc.AddTypeMapping(typeof(global::NorthwindEF.DiscontinuedProduct), typeof(global::NorthwindEF.PocoAdapters.DiscontinuedProductAdapter), (arg) => new global::NorthwindEF.PocoAdapters.DiscontinuedProductAdapter((global::NorthwindEF.DiscontinuedProduct)arg));
        }
        public NorthwindEntitiesAdapter() : base("name=NorthwindEntities", "NorthwindEntities") { OnContextCreated(); }
        public NorthwindEntitiesAdapter(string connectionString) : base(connectionString, "NorthwindEntities") { OnContextCreated(); }
        public NorthwindEntitiesAdapter(System.Data.EntityClient.EntityConnection connection) : base(connection, "NorthwindEntities") { OnContextCreated(); }
        partial void OnContextCreated();
        public QueryTranslationCache QueryTranslationCache
        {
            get { return _qtc; }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.EmployeeAdapter> _Employees;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.EmployeeAdapter> Employees
        {
            get
            {
                if (_Employees == null)
                    _Employees = base.CreateQuery<NorthwindEF.PocoAdapters.EmployeeAdapter>("[Employees]");
                return _Employees;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.Territories.PocoAdapters.TerritoryAdapter> _Territories;
        public System.Data.Objects.ObjectQuery<NorthwindEF.Territories.PocoAdapters.TerritoryAdapter> Territories
        {
            get
            {
                if (_Territories == null)
                    _Territories = base.CreateQuery<NorthwindEF.Territories.PocoAdapters.TerritoryAdapter>("[Territories]");
                return _Territories;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.RegionAdapter> _Regions;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.RegionAdapter> Regions
        {
            get
            {
                if (_Regions == null)
                    _Regions = base.CreateQuery<NorthwindEF.PocoAdapters.RegionAdapter>("[Regions]");
                return _Regions;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.SupplierAdapter> _Suppliers;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.SupplierAdapter> Suppliers
        {
            get
            {
                if (_Suppliers == null)
                    _Suppliers = base.CreateQuery<NorthwindEF.PocoAdapters.SupplierAdapter>("[Suppliers]");
                return _Suppliers;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.ProductAdapter> _Products;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.ProductAdapter> Products
        {
            get
            {
                if (_Products == null)
                    _Products = base.CreateQuery<NorthwindEF.PocoAdapters.ProductAdapter>("[Products]");
                return _Products;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.CategoryAdapter> _Categories;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.CategoryAdapter> Categories
        {
            get
            {
                if (_Categories == null)
                    _Categories = base.CreateQuery<NorthwindEF.PocoAdapters.CategoryAdapter>("[Categories]");
                return _Categories;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.CustomerAdapter> _Customers;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.CustomerAdapter> Customers
        {
            get
            {
                if (_Customers == null)
                    _Customers = base.CreateQuery<NorthwindEF.PocoAdapters.CustomerAdapter>("[Customers]");
                return _Customers;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.OrderAdapter> _Orders;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.OrderAdapter> Orders
        {
            get
            {
                if (_Orders == null)
                    _Orders = base.CreateQuery<NorthwindEF.PocoAdapters.OrderAdapter>("[Orders]");
                return _Orders;
            }
        }

        private System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.OrderDetailAdapter> _OrderDetails;
        public System.Data.Objects.ObjectQuery<NorthwindEF.PocoAdapters.OrderDetailAdapter> OrderDetails
        {
            get
            {
                if (_OrderDetails == null)
                    _OrderDetails = base.CreateQuery<NorthwindEF.PocoAdapters.OrderDetailAdapter>("[OrderDetails]");
                return _OrderDetails;
            }
        }
    }

    // POCO Adapters for Complex Types

    [EdmComplexType(NamespaceName="NorthwindEFModel", Name="CommonAddress")]
    public partial class CommonAddressAdapter : ComplexObject, IComplexTypeAdapter<NorthwindEF.CommonAddress>
    {
        private String _Address;
        private String _City;
        private String _Region;
        private String _PostalCode;
        private String _Country;

        public NorthwindEF.CommonAddress CreatePocoStructure()
        {
            return new NorthwindEF.CommonAddress(this.Address, this.City, this.Region, this.PostalCode, this.Country) {
            };
        }
        public void DetectChangesFrom(NorthwindEF.CommonAddress pocoObject, IPocoAdapter parentObject, string propertyName)
        {
            bool different = false;
            if (this.Address != pocoObject.Address) different = true;
            if (this.City != pocoObject.City) different = true;
            if (this.Region != pocoObject.Region) different = true;
            if (this.PostalCode != pocoObject.PostalCode) different = true;
            if (this.Country != pocoObject.Country) different = true;
            if (!different) return;
            NorthwindEF.CommonAddress oldValue = CreatePocoStructure();
            this.Address = pocoObject.Address;
            this.City = pocoObject.City;
            this.Region = pocoObject.Region;
            this.PostalCode = pocoObject.PostalCode;
            this.Country = pocoObject.Country;
            NorthwindEF.CommonAddress newValue = CreatePocoStructure();
            if (!Object.Equals(oldValue, newValue))
            {
                parentObject.RaiseChangeDetected(propertyName, oldValue, newValue);
            }
        }

        [EdmScalarProperty(IsNullable=true)]
        public String Address
        {
            get { return _Address; }
            set
            {
                ReportPropertyChanging("Address");
                _Address = value;
                ReportPropertyChanged("Address");
            }
        }
        [EdmScalarProperty(IsNullable=true)]
        public String City
        {
            get { return _City; }
            set
            {
                ReportPropertyChanging("City");
                _City = value;
                ReportPropertyChanged("City");
            }
        }
        [EdmScalarProperty(IsNullable=true)]
        public String Region
        {
            get { return _Region; }
            set
            {
                ReportPropertyChanging("Region");
                _Region = value;
                ReportPropertyChanged("Region");
            }
        }
        [EdmScalarProperty(IsNullable=true)]
        public String PostalCode
        {
            get { return _PostalCode; }
            set
            {
                ReportPropertyChanging("PostalCode");
                _PostalCode = value;
                ReportPropertyChanged("PostalCode");
            }
        }
        [EdmScalarProperty(IsNullable=true)]
        public String Country
        {
            get { return _Country; }
            set
            {
                ReportPropertyChanging("Country");
                _Country = value;
                ReportPropertyChanged("Country");
            }
        }
    }

    [EdmComplexType(NamespaceName="NorthwindEFModel", Name="ContactInfo")]
    public partial class ContactInfoAdapter : ComplexObject, IComplexTypeAdapter<NorthwindEF.ContactInfo>
    {
        private String _Title;
        private String _Name;

        public NorthwindEF.ContactInfo CreatePocoStructure()
        {
            return new NorthwindEF.ContactInfo() {
                Title = this.Title,
                Name = this.Name,
            };
        }
        public void DetectChangesFrom(NorthwindEF.ContactInfo pocoObject, IPocoAdapter parentObject, string propertyName)
        {
            bool different = false;
            if (this.Title != pocoObject.Title) different = true;
            if (this.Name != pocoObject.Name) different = true;
            if (!different) return;
            NorthwindEF.ContactInfo oldValue = CreatePocoStructure();
            this.Title = pocoObject.Title;
            this.Name = pocoObject.Name;
            NorthwindEF.ContactInfo newValue = CreatePocoStructure();
            if (!Object.Equals(oldValue, newValue))
            {
                parentObject.RaiseChangeDetected(propertyName, oldValue, newValue);
            }
        }

        [EdmScalarProperty(IsNullable=true)]
        public String Title
        {
            get { return _Title; }
            set
            {
                ReportPropertyChanging("Title");
                _Title = value;
                ReportPropertyChanged("Title");
            }
        }
        [EdmScalarProperty(IsNullable=true)]
        public String Name
        {
            get { return _Name; }
            set
            {
                ReportPropertyChanging("Name");
                _Name = value;
                ReportPropertyChanged("Name");
            }
        }
    }

    // POCO Adapters for Entity Types

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Employee")]
    public abstract partial class EmployeeAdapter : PocoAdapterBase<NorthwindEF.Employee>, IPocoAdapter<NorthwindEF.Employee>
    {
        private String _LastName;
        private String _FirstName;
        private String _Title;
        private String _TitleOfCourtesy;
        private DateTime? _BirthDate;
        private DateTime? _HireDate;
        private NorthwindEF.PocoAdapters.CommonAddressAdapter _Address;
        private bool _Address_Initialized = false;
        private String _HomePhone;
        private String _Extension;
        private Byte[] _Photo;
        private String _Notes;
        private String _PhotoPath;
        public EmployeeAdapter() { OnAdapterCreated(); }
        public EmployeeAdapter(NorthwindEF.Employee pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override void Init()
        {
            base.Init();
            this.Territories.AssociationChanged += Territories_Changed;
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
            if (PocoEntity.Territories == null)
            {
                PocoEntity.Territories = new HashSet<NorthwindEF.Territories.Territory>();
            }
        }
        void Territories_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    PocoEntity.Territories.Add(((NorthwindEF.Territories.PocoAdapters.TerritoryAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Remove:
                    PocoEntity.Territories.Remove(((NorthwindEF.Territories.PocoAdapters.TerritoryAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Refresh:
                default:
                    UpdateCollection(PocoEntity.Territories, this.Territories);
                    break;
            }
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.EmployeeID, ref _EmployeeID, "EmployeeID");
            DetectChanges(PocoEntity.LastName, ref _LastName, "LastName");
            DetectChanges(PocoEntity.FirstName, ref _FirstName, "FirstName");
            DetectChanges(PocoEntity.Title, ref _Title, "Title");
            DetectChanges(PocoEntity.TitleOfCourtesy, ref _TitleOfCourtesy, "TitleOfCourtesy");
            DetectChanges(PocoEntity.HireDate, ref _HireDate, "HireDate");
            this.Address.DetectChangesFrom(PocoEntity.Address, this, "Address");
            DetectChanges(PocoEntity.HomePhone, ref _HomePhone, "HomePhone");
            DetectChanges(PocoEntity.Extension, ref _Extension, "Extension");
            DetectChanges(PocoEntity.Photo, ref _Photo, "Photo");
            DetectChanges(PocoEntity.Notes, ref _Notes, "Notes");
            DetectChanges(PocoEntity.PhotoPath, ref _PhotoPath, "PhotoPath");
            DetectChanges(PocoEntity.Territories, this.Territories, "Territories");
            if (!(PocoEntity is IEntityProxy))
            {
                DetectChanges(PocoEntity.BirthDate, ref _BirthDate, "BirthDate");
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.EmployeeID = _EmployeeID;
            PocoEntity.LastName = _LastName;
            PocoEntity.FirstName = _FirstName;
            PocoEntity.Title = _Title;
            PocoEntity.TitleOfCourtesy = _TitleOfCourtesy;
            PocoEntity.HireDate = _HireDate;
            PocoEntity.Address = _Address.CreatePocoStructure();
            PocoEntity.HomePhone = _HomePhone;
            PocoEntity.Extension = _Extension;
            PocoEntity.Photo = _Photo;
            PocoEntity.Notes = _Notes;
            PocoEntity.PhotoPath = _PhotoPath;
            UpdateCollection(PocoEntity.Territories, this.Territories);
            if (!(PocoEntity is IEntityProxy))
            {
                PocoEntity.BirthDate = _BirthDate;
            }
        }
        private Int32 _EmployeeID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 EmployeeID
        {
            get
            {
                return _EmployeeID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.EmployeeID = value;
                }
                ReportPropertyChanging("EmployeeID");
                _EmployeeID = value;
                ReportPropertyChanged("EmployeeID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.LastName = value;
                }
                ReportPropertyChanging("LastName");
                _LastName = value;
                ReportPropertyChanged("LastName");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.FirstName = value;
                }
                ReportPropertyChanging("FirstName");
                _FirstName = value;
                ReportPropertyChanged("FirstName");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Title = value;
                }
                ReportPropertyChanging("Title");
                _Title = value;
                ReportPropertyChanged("Title");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String TitleOfCourtesy
        {
            get
            {
                return _TitleOfCourtesy;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.TitleOfCourtesy = value;
                }
                ReportPropertyChanging("TitleOfCourtesy");
                _TitleOfCourtesy = value;
                ReportPropertyChanged("TitleOfCourtesy");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public DateTime? BirthDate
        {
            get
            {
                return _BirthDate;
            }
            set
            {
                if (_pocoEntity != null && !(_pocoEntity is IEntityProxy))
                {
                    PocoEntity.BirthDate = value;
                }
                ReportPropertyChanging("BirthDate");
                _BirthDate = value;
                ReportPropertyChanged("BirthDate");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public DateTime? HireDate
        {
            get
            {
                return _HireDate;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.HireDate = value;
                }
                ReportPropertyChanging("HireDate");
                _HireDate = value;
                ReportPropertyChanged("HireDate");
            }
        }

        [EdmComplexProperty]
        public NorthwindEF.PocoAdapters.CommonAddressAdapter Address
        {
            get
            {
                this._Address = this.GetValidValue(this._Address, "Address", false, this._Address_Initialized);
                this._Address_Initialized = true;
                this._Address.PropertyChanged += new PropertyChangedEventHandler(_Address_PropertyChanged);
                return _Address;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Address = value.CreatePocoStructure();
                }
                ReportPropertyChanging("Address");
                this._Address = this.SetValidValue(this._Address, value, "Address");
                this._Address_Initialized = true;
                this._Address.PropertyChanged += new PropertyChangedEventHandler(_Address_PropertyChanged);
                ReportPropertyChanged("Address");
            }
        }
        void _Address_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pocoEntity != null)
            {
                PocoEntity.Address = _Address.CreatePocoStructure();
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String HomePhone
        {
            get
            {
                return _HomePhone;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.HomePhone = value;
                }
                ReportPropertyChanging("HomePhone");
                _HomePhone = value;
                ReportPropertyChanged("HomePhone");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Extension
        {
            get
            {
                return _Extension;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Extension = value;
                }
                ReportPropertyChanging("Extension");
                _Extension = value;
                ReportPropertyChanged("Extension");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public Byte[] Photo
        {
            get
            {
                return _Photo;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Photo = value;
                }
                ReportPropertyChanging("Photo");
                _Photo = value;
                ReportPropertyChanged("Photo");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Notes
        {
            get
            {
                return _Notes;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Notes = value;
                }
                ReportPropertyChanging("Notes");
                _Notes = value;
                ReportPropertyChanged("Notes");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String PhotoPath
        {
            get
            {
                return _PhotoPath;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.PhotoPath = value;
                }
                ReportPropertyChanging("PhotoPath");
                _PhotoPath = value;
                ReportPropertyChanged("PhotoPath");
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "EmployeesTerritories", "Territories")]
        public EntityCollection<NorthwindEF.Territories.PocoAdapters.TerritoryAdapter> Territories
        {
            get
            {
                if (_TerritoriesCollectionCache == null)
                    _TerritoriesCollectionCache = GetRelatedCollection<NorthwindEF.Territories.PocoAdapters.TerritoryAdapter>("EmployeesTerritories", "Territories");
                 return _TerritoriesCollectionCache;
            }
        }
        private EntityCollection<NorthwindEF.Territories.PocoAdapters.TerritoryAdapter> _TerritoriesCollectionCache = null;
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Region")]
    public partial class RegionAdapter : PocoAdapterBase<NorthwindEF.Region>, IPocoAdapter<NorthwindEF.Region>
    {
        private String _RegionDescription;
        public RegionAdapter() { OnAdapterCreated(); }
        public RegionAdapter(NorthwindEF.Region pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.Region(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.PocoProxies.RegionProxy(this); }
        public override void Init()
        {
            base.Init();
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.RegionID, ref _RegionID, "RegionID");
            DetectChanges(PocoEntity.RegionDescription, ref _RegionDescription, "RegionDescription");
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.Region(); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.RegionProxy(this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.RegionID = _RegionID;
            PocoEntity.RegionDescription = _RegionDescription;
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        private Int32 _RegionID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 RegionID
        {
            get
            {
                return _RegionID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.RegionID = value;
                }
                ReportPropertyChanging("RegionID");
                _RegionID = value;
                ReportPropertyChanged("RegionID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String RegionDescription
        {
            get
            {
                return _RegionDescription;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.RegionDescription = value;
                }
                ReportPropertyChanging("RegionDescription");
                _RegionDescription = value;
                ReportPropertyChanged("RegionDescription");
            }
        }
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Supplier")]
    public partial class SupplierAdapter : PocoAdapterBase<NorthwindEF.Supplier>, IPocoAdapter<NorthwindEF.Supplier>
    {
        private String _CompanyName;
        private String _ContactName;
        private String _ContactTitle;
        private NorthwindEF.PocoAdapters.CommonAddressAdapter _Address;
        private bool _Address_Initialized = false;
        private String _Phone;
        private String _Fax;
        private String _HomePage;
        public SupplierAdapter() { OnAdapterCreated(); }
        public SupplierAdapter(NorthwindEF.Supplier pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.Supplier(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.PocoProxies.SupplierProxy(this); }
        public override void Init()
        {
            base.Init();
            this.Products.AssociationChanged += Products_Changed;
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
            if (PocoEntity.Products == null)
            {
                if (enableProxies)
                {
                    PocoEntity.Products = new IListEntityCollectionAdapter<NorthwindEF.Product,NorthwindEF.PocoAdapters.ProductAdapter>(this, "Products", this.Products, this.Context);
                }
                else
                {
                    PocoEntity.Products = new List<NorthwindEF.Product>();
                }
            }
        }
        void Products_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            if (PocoEntity.Products is IListEntityCollectionAdapter<NorthwindEF.Product,NorthwindEF.PocoAdapters.ProductAdapter>) return;
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    PocoEntity.Products.Add(((NorthwindEF.PocoAdapters.ProductAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Remove:
                    PocoEntity.Products.Remove(((NorthwindEF.PocoAdapters.ProductAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Refresh:
                default:
                    UpdateCollection(PocoEntity.Products, this.Products);
                    break;
            }
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.SupplierID, ref _SupplierID, "SupplierID");
            DetectChanges(PocoEntity.CompanyName, ref _CompanyName, "CompanyName");
            DetectChanges(PocoEntity.ContactTitle, ref _ContactTitle, "ContactTitle");
            this.Address.DetectChangesFrom(PocoEntity.Address, this, "Address");
            DetectChanges(PocoEntity.Phone, ref _Phone, "Phone");
            DetectChanges(PocoEntity.Fax, ref _Fax, "Fax");
            DetectChanges(PocoEntity.HomePage, ref _HomePage, "HomePage");
            if (!(PocoEntity is IEntityProxy))
            {
                DetectChanges(PocoEntity.Products, this.Products, "Products");
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.Supplier(); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.SupplierProxy(this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.SupplierID = _SupplierID;
            PocoEntity.CompanyName = _CompanyName;
            PocoEntity.ContactTitle = _ContactTitle;
            PocoEntity.Address = _Address.CreatePocoStructure();
            PocoEntity.Phone = _Phone;
            PocoEntity.Fax = _Fax;
            PocoEntity.HomePage = _HomePage;
            if (!(PocoEntity is IEntityProxy))
            {
                UpdateCollection(PocoEntity.Products, this.Products);
            }
        }
        private Int32 _SupplierID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 SupplierID
        {
            get
            {
                return _SupplierID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.SupplierID = value;
                }
                ReportPropertyChanging("SupplierID");
                _SupplierID = value;
                ReportPropertyChanged("SupplierID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String CompanyName
        {
            get
            {
                return _CompanyName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.CompanyName = value;
                }
                ReportPropertyChanging("CompanyName");
                _CompanyName = value;
                ReportPropertyChanged("CompanyName");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ContactName
        {
            get
            {
                return _ContactName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                }
                ReportPropertyChanging("ContactName");
                _ContactName = value;
                ReportPropertyChanged("ContactName");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ContactTitle
        {
            get
            {
                return _ContactTitle;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ContactTitle = value;
                }
                ReportPropertyChanging("ContactTitle");
                _ContactTitle = value;
                ReportPropertyChanged("ContactTitle");
            }
        }

        [EdmComplexProperty]
        public NorthwindEF.PocoAdapters.CommonAddressAdapter Address
        {
            get
            {
                this._Address = this.GetValidValue(this._Address, "Address", false, this._Address_Initialized);
                this._Address_Initialized = true;
                this._Address.PropertyChanged += new PropertyChangedEventHandler(_Address_PropertyChanged);
                return _Address;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Address = value.CreatePocoStructure();
                }
                ReportPropertyChanging("Address");
                this._Address = this.SetValidValue(this._Address, value, "Address");
                this._Address_Initialized = true;
                this._Address.PropertyChanged += new PropertyChangedEventHandler(_Address_PropertyChanged);
                ReportPropertyChanged("Address");
            }
        }
        void _Address_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pocoEntity != null)
            {
                PocoEntity.Address = _Address.CreatePocoStructure();
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Phone = value;
                }
                ReportPropertyChanging("Phone");
                _Phone = value;
                ReportPropertyChanged("Phone");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Fax
        {
            get
            {
                return _Fax;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Fax = value;
                }
                ReportPropertyChanging("Fax");
                _Fax = value;
                ReportPropertyChanged("Fax");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String HomePage
        {
            get
            {
                return _HomePage;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.HomePage = value;
                }
                ReportPropertyChanging("HomePage");
                _HomePage = value;
                ReportPropertyChanged("HomePage");
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Products_Supplier", "Products")]
        public EntityCollection<NorthwindEF.PocoAdapters.ProductAdapter> Products
        {
            get
            {
                if (_ProductsCollectionCache == null)
                    _ProductsCollectionCache = GetRelatedCollection<NorthwindEF.PocoAdapters.ProductAdapter>("Products_Supplier", "Products");
                 return _ProductsCollectionCache;
            }
        }
        private EntityCollection<NorthwindEF.PocoAdapters.ProductAdapter> _ProductsCollectionCache = null;
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Product")]
    public partial class ProductAdapter : PocoAdapterBase<NorthwindEF.Product>, IPocoAdapter<NorthwindEF.Product>
    {
        private String _ProductName;
        private String _QuantityPerUnit;
        private Decimal _UnitPrice;
        private Int16? _UnitsInStock;
        private Int16? _UnitsOnOrder;
        private Int16? _ReorderLevel;
        public ProductAdapter() { OnAdapterCreated(); }
        public ProductAdapter(NorthwindEF.Product pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return null; }
        public override object CreatePocoEntityProxy() { return null; }
        public override void Init()
        {
            base.Init();
            this.CategoryReference.AssociationChanged += Category_Changed;
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
        }
        void Category_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            PocoEntity.Category = this.Category.GetPocoEntityOrNull();
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.ProductID, ref _ProductID, "ProductID");
            DetectChanges(PocoEntity.ProductName, ref _ProductName, "ProductName");
            DetectChanges(PocoEntity.QuantityPerUnit, ref _QuantityPerUnit, "QuantityPerUnit");
            DetectChanges(PocoEntity.UnitPrice, ref _UnitPrice, "UnitPrice");
            DetectChanges(PocoEntity.UnitsInStock, ref _UnitsInStock, "UnitsInStock");
            DetectChanges(PocoEntity.UnitsOnOrder, ref _UnitsOnOrder, "UnitsOnOrder");
            DetectChanges(PocoEntity.ReorderLevel, ref _ReorderLevel, "ReorderLevel");
            DetectChanges(PocoEntity.Category, this.CategoryReference, "Category");
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.Product(Supplier.GetPocoEntityOrNull()); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.ProductProxy(Supplier.GetPocoEntityOrNull(), this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.ProductID = _ProductID;
            PocoEntity.ProductName = _ProductName;
            PocoEntity.QuantityPerUnit = _QuantityPerUnit;
            PocoEntity.UnitPrice = _UnitPrice;
            PocoEntity.UnitsInStock = _UnitsInStock;
            PocoEntity.UnitsOnOrder = _UnitsOnOrder;
            PocoEntity.ReorderLevel = _ReorderLevel;
            PocoEntity.Category = this.CategoryReference.Value.GetPocoEntityOrNull();
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        private Int32 _ProductID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 ProductID
        {
            get
            {
                return _ProductID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ProductID = value;
                }
                ReportPropertyChanging("ProductID");
                _ProductID = value;
                ReportPropertyChanged("ProductID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String ProductName
        {
            get
            {
                return _ProductName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ProductName = value;
                }
                ReportPropertyChanging("ProductName");
                _ProductName = value;
                ReportPropertyChanged("ProductName");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String QuantityPerUnit
        {
            get
            {
                return _QuantityPerUnit;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.QuantityPerUnit = value;
                }
                ReportPropertyChanging("QuantityPerUnit");
                _QuantityPerUnit = value;
                ReportPropertyChanged("QuantityPerUnit");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public Decimal UnitPrice
        {
            get
            {
                return _UnitPrice;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.UnitPrice = value;
                }
                ReportPropertyChanging("UnitPrice");
                _UnitPrice = value;
                ReportPropertyChanged("UnitPrice");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public Int16? UnitsInStock
        {
            get
            {
                return _UnitsInStock;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.UnitsInStock = value;
                }
                ReportPropertyChanging("UnitsInStock");
                _UnitsInStock = value;
                ReportPropertyChanged("UnitsInStock");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public Int16? UnitsOnOrder
        {
            get
            {
                return _UnitsOnOrder;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.UnitsOnOrder = value;
                }
                ReportPropertyChanging("UnitsOnOrder");
                _UnitsOnOrder = value;
                ReportPropertyChanged("UnitsOnOrder");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public Int16? ReorderLevel
        {
            get
            {
                return _ReorderLevel;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ReorderLevel = value;
                }
                ReportPropertyChanging("ReorderLevel");
                _ReorderLevel = value;
                ReportPropertyChanged("ReorderLevel");
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Products_Category", "Category")]
        public NorthwindEF.PocoAdapters.CategoryAdapter Category
        {
            get { return this.CategoryReference.Value; }
            set { this.CategoryReference.Value = value; }
        }

        private EntityReference<NorthwindEF.PocoAdapters.CategoryAdapter> _CategoryReferenceCache = null;
        public EntityReference<NorthwindEF.PocoAdapters.CategoryAdapter> CategoryReference
        {
            get
            {
                if (_CategoryReferenceCache == null)
                    _CategoryReferenceCache = GetRelatedReference<NorthwindEF.PocoAdapters.CategoryAdapter>("Products_Category", "Category");
                return _CategoryReferenceCache;
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Order_Details_Product", "OrderDetails")]
        public EntityCollection<NorthwindEF.PocoAdapters.OrderDetailAdapter> OrderDetails
        {
            get
            {
                if (_OrderDetailsCollectionCache == null)
                    _OrderDetailsCollectionCache = GetRelatedCollection<NorthwindEF.PocoAdapters.OrderDetailAdapter>("Order_Details_Product", "OrderDetails");
                 return _OrderDetailsCollectionCache;
            }
        }
        private EntityCollection<NorthwindEF.PocoAdapters.OrderDetailAdapter> _OrderDetailsCollectionCache = null;

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Products_Supplier", "Supplier")]
        public NorthwindEF.PocoAdapters.SupplierAdapter Supplier
        {
            get { return this.SupplierReference.Value; }
            set { this.SupplierReference.Value = value; }
        }

        private EntityReference<NorthwindEF.PocoAdapters.SupplierAdapter> _SupplierReferenceCache = null;
        public EntityReference<NorthwindEF.PocoAdapters.SupplierAdapter> SupplierReference
        {
            get
            {
                if (_SupplierReferenceCache == null)
                    _SupplierReferenceCache = GetRelatedReference<NorthwindEF.PocoAdapters.SupplierAdapter>("Products_Supplier", "Supplier");
                return _SupplierReferenceCache;
            }
        }
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Category")]
    public partial class CategoryAdapter : PocoAdapterBase<NorthwindEF.Category>, IPocoAdapter<NorthwindEF.Category>
    {
        private String _CategoryName;
        private String _Description;
        private Byte[] _Picture;
        public CategoryAdapter() { OnAdapterCreated(); }
        public CategoryAdapter(NorthwindEF.Category pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return null; }
        public override object CreatePocoEntityProxy() { return null; }
        public override void Init()
        {
            base.Init();
            this.Products.AssociationChanged += Products_Changed;
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
        }
        void Products_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    PocoEntity.Products.Add(((NorthwindEF.PocoAdapters.ProductAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Remove:
                    PocoEntity.Products.Remove(((NorthwindEF.PocoAdapters.ProductAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Refresh:
                default:
                    UpdateCollection(PocoEntity.Products, this.Products);
                    break;
            }
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.CategoryID, ref _CategoryID, "CategoryID");
            DetectChanges(PocoEntity.CategoryName, ref _CategoryName, "CategoryName");
            DetectChanges(PocoEntity.Description, ref _Description, "Description");
            DetectChanges(PocoEntity.Picture, ref _Picture, "Picture");
            DetectChanges(PocoEntity.Products, this.Products, "Products");
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.Category(CategoryID, CategoryName, Description, ConvertTo<List<NorthwindEF.Product>,NorthwindEF.Product>(Products)); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.CategoryProxy(CategoryID, CategoryName, Description, ConvertTo<List<NorthwindEF.Product>,NorthwindEF.Product>(Products), this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.Picture = _Picture;
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        private Int32 _CategoryID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 CategoryID
        {
            get
            {
                return _CategoryID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                }
                ReportPropertyChanging("CategoryID");
                _CategoryID = value;
                ReportPropertyChanged("CategoryID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String CategoryName
        {
            get
            {
                return _CategoryName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                }
                ReportPropertyChanging("CategoryName");
                _CategoryName = value;
                ReportPropertyChanged("CategoryName");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (_pocoEntity != null)
                {
                }
                ReportPropertyChanging("Description");
                _Description = value;
                ReportPropertyChanged("Description");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public Byte[] Picture
        {
            get
            {
                return _Picture;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Picture = value;
                }
                ReportPropertyChanging("Picture");
                _Picture = value;
                ReportPropertyChanged("Picture");
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Products_Category", "Products")]
        public EntityCollection<NorthwindEF.PocoAdapters.ProductAdapter> Products
        {
            get
            {
                if (_ProductsCollectionCache == null)
                    _ProductsCollectionCache = GetRelatedCollection<NorthwindEF.PocoAdapters.ProductAdapter>("Products_Category", "Products");
                 return _ProductsCollectionCache;
            }
        }
        private EntityCollection<NorthwindEF.PocoAdapters.ProductAdapter> _ProductsCollectionCache = null;
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Customer")]
    public partial class CustomerAdapter : PocoAdapterBase<NorthwindEF.Customer>, IPocoAdapter<NorthwindEF.Customer>
    {
        private String _CompanyName;
        private NorthwindEF.PocoAdapters.ContactInfoAdapter _ContactPerson;
        private bool _ContactPerson_Initialized = false;
        private NorthwindEF.PocoAdapters.CommonAddressAdapter _Address;
        private bool _Address_Initialized = false;
        private String _Phone;
        private String _Fax;
        public CustomerAdapter() { OnAdapterCreated(); }
        public CustomerAdapter(NorthwindEF.Customer pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.Customer(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.PocoProxies.CustomerProxy(this); }
        public override void Init()
        {
            base.Init();
            this.Orders.AssociationChanged += Orders_Changed;
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
            if (PocoEntity.Orders == null)
            {
                if (enableProxies)
                {
                    PocoEntity.Orders = new IListEntityCollectionAdapter<NorthwindEF.Order,NorthwindEF.PocoAdapters.OrderAdapter>(this, "Orders", this.Orders, this.Context);
                }
                else
                {
                    PocoEntity.Orders = new List<NorthwindEF.Order>();
                }
            }
        }
        void Orders_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            if (PocoEntity.Orders is IListEntityCollectionAdapter<NorthwindEF.Order,NorthwindEF.PocoAdapters.OrderAdapter>) return;
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    PocoEntity.Orders.Add(((NorthwindEF.PocoAdapters.OrderAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Remove:
                    PocoEntity.Orders.Remove(((NorthwindEF.PocoAdapters.OrderAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Refresh:
                default:
                    UpdateCollection(PocoEntity.Orders, this.Orders);
                    break;
            }
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.CustomerID, ref _CustomerID, "CustomerID");
            DetectChanges(PocoEntity.CompanyName, ref _CompanyName, "CompanyName");
            this.ContactPerson.DetectChangesFrom(PocoEntity.ContactPerson, this, "ContactPerson");
            this.Address.DetectChangesFrom(PocoEntity.Address, this, "Address");
            DetectChanges(PocoEntity.Phone, ref _Phone, "Phone");
            DetectChanges(PocoEntity.Fax, ref _Fax, "Fax");
            if (!(PocoEntity is IEntityProxy))
            {
                DetectChanges(PocoEntity.Orders, this.Orders, "Orders");
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.Customer(); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.CustomerProxy(this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.CustomerID = _CustomerID;
            PocoEntity.CompanyName = _CompanyName;
            PocoEntity.ContactPerson = _ContactPerson.CreatePocoStructure();
            PocoEntity.Address = _Address.CreatePocoStructure();
            PocoEntity.Phone = _Phone;
            PocoEntity.Fax = _Fax;
            if (!(PocoEntity is IEntityProxy))
            {
                UpdateCollection(PocoEntity.Orders, this.Orders);
            }
        }
        private String _CustomerID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public String CustomerID
        {
            get
            {
                return _CustomerID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.CustomerID = value;
                }
                ReportPropertyChanging("CustomerID");
                _CustomerID = value;
                ReportPropertyChanged("CustomerID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String CompanyName
        {
            get
            {
                return _CompanyName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.CompanyName = value;
                }
                ReportPropertyChanging("CompanyName");
                _CompanyName = value;
                ReportPropertyChanged("CompanyName");
            }
        }

        [EdmComplexProperty]
        public NorthwindEF.PocoAdapters.ContactInfoAdapter ContactPerson
        {
            get
            {
                this._ContactPerson = this.GetValidValue(this._ContactPerson, "ContactPerson", false, this._ContactPerson_Initialized);
                this._ContactPerson_Initialized = true;
                this._ContactPerson.PropertyChanged += new PropertyChangedEventHandler(_ContactPerson_PropertyChanged);
                return _ContactPerson;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ContactPerson = value.CreatePocoStructure();
                }
                ReportPropertyChanging("ContactPerson");
                this._ContactPerson = this.SetValidValue(this._ContactPerson, value, "ContactPerson");
                this._ContactPerson_Initialized = true;
                this._ContactPerson.PropertyChanged += new PropertyChangedEventHandler(_ContactPerson_PropertyChanged);
                ReportPropertyChanged("ContactPerson");
            }
        }
        void _ContactPerson_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pocoEntity != null)
            {
                PocoEntity.ContactPerson = _ContactPerson.CreatePocoStructure();
            }
        }

        [EdmComplexProperty]
        public NorthwindEF.PocoAdapters.CommonAddressAdapter Address
        {
            get
            {
                this._Address = this.GetValidValue(this._Address, "Address", false, this._Address_Initialized);
                this._Address_Initialized = true;
                this._Address.PropertyChanged += new PropertyChangedEventHandler(_Address_PropertyChanged);
                return _Address;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Address = value.CreatePocoStructure();
                }
                ReportPropertyChanging("Address");
                this._Address = this.SetValidValue(this._Address, value, "Address");
                this._Address_Initialized = true;
                this._Address.PropertyChanged += new PropertyChangedEventHandler(_Address_PropertyChanged);
                ReportPropertyChanged("Address");
            }
        }
        void _Address_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_pocoEntity != null)
            {
                PocoEntity.Address = _Address.CreatePocoStructure();
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Phone
        {
            get
            {
                return _Phone;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Phone = value;
                }
                ReportPropertyChanging("Phone");
                _Phone = value;
                ReportPropertyChanged("Phone");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String Fax
        {
            get
            {
                return _Fax;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Fax = value;
                }
                ReportPropertyChanging("Fax");
                _Fax = value;
                ReportPropertyChanged("Fax");
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "CustomerOrders", "Orders")]
        public EntityCollection<NorthwindEF.PocoAdapters.OrderAdapter> Orders
        {
            get
            {
                if (_OrdersCollectionCache == null)
                    _OrdersCollectionCache = GetRelatedCollection<NorthwindEF.PocoAdapters.OrderAdapter>("CustomerOrders", "Orders");
                 return _OrdersCollectionCache;
            }
        }
        private EntityCollection<NorthwindEF.PocoAdapters.OrderAdapter> _OrdersCollectionCache = null;
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Order")]
    public partial class OrderAdapter : PocoAdapterBase<NorthwindEF.Order>, IPocoAdapter<NorthwindEF.Order>
    {
        private Int32 _EmployeeID;
        private DateTime? _OrderDate;
        private DateTime? _RequiredDate;
        private DateTime? _ShippedDate;
        private Decimal? _Freight;
        private String _ShipName;
        private String _ShipAddress;
        private String _ShipCity;
        private String _ShipRegion;
        private String _ShipPostalCode;
        private String _ShipCountry;
        public OrderAdapter() { OnAdapterCreated(); }
        public OrderAdapter(NorthwindEF.Order pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.Order(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.PocoProxies.OrderProxy(this); }
        public override void Init()
        {
            base.Init();
            this.CustomerReference.AssociationChanged += Customer_Changed;
            this.OrderDetails.AssociationChanged += OrderDetails_Changed;
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
            if (PocoEntity.OrderDetails == null)
            {
                if (enableProxies)
                {
                    PocoEntity.OrderDetails = new IListEntityCollectionAdapter<NorthwindEF.OrderDetail,NorthwindEF.PocoAdapters.OrderDetailAdapter>(this, "OrderDetails", this.OrderDetails, this.Context);
                }
                else
                {
                    PocoEntity.OrderDetails = new List<NorthwindEF.OrderDetail>();
                }
            }
        }
        void Customer_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            if (PocoEntity is IEntityProxy) return;
            PocoEntity.Customer = this.Customer.GetPocoEntityOrNull();
        }
        void OrderDetails_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            if (PocoEntity.OrderDetails is IListEntityCollectionAdapter<NorthwindEF.OrderDetail,NorthwindEF.PocoAdapters.OrderDetailAdapter>) return;
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    PocoEntity.OrderDetails.Add(((NorthwindEF.PocoAdapters.OrderDetailAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Remove:
                    PocoEntity.OrderDetails.Remove(((NorthwindEF.PocoAdapters.OrderDetailAdapter)e.Element).PocoEntity);
                    break;
                case CollectionChangeAction.Refresh:
                default:
                    UpdateCollection(PocoEntity.OrderDetails, this.OrderDetails);
                    break;
            }
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.OrderID, ref _OrderID, "OrderID");
            DetectChanges(PocoEntity.EmployeeID, ref _EmployeeID, "EmployeeID");
            DetectChanges(PocoEntity.OrderDate, ref _OrderDate, "OrderDate");
            DetectChanges(PocoEntity.ShippedDate, ref _ShippedDate, "ShippedDate");
            DetectChanges(PocoEntity.Freight, ref _Freight, "Freight");
            DetectChanges(PocoEntity.ShipName, ref _ShipName, "ShipName");
            DetectChanges(PocoEntity.ShipAddress, ref _ShipAddress, "ShipAddress");
            DetectChanges(PocoEntity.ShipCity, ref _ShipCity, "ShipCity");
            DetectChanges(PocoEntity.ShipRegion, ref _ShipRegion, "ShipRegion");
            DetectChanges(PocoEntity.ShipPostalCode, ref _ShipPostalCode, "ShipPostalCode");
            DetectChanges(PocoEntity.ShipCountry, ref _ShipCountry, "ShipCountry");
            if (!(PocoEntity is IEntityProxy))
            {
                DetectChanges(PocoEntity.Customer, this.CustomerReference, "Customer");
                DetectChanges(PocoEntity.OrderDetails, this.OrderDetails, "OrderDetails");
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.Order(); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.OrderProxy(this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.OrderID = _OrderID;
            PocoEntity.EmployeeID = _EmployeeID;
            PocoEntity.OrderDate = _OrderDate;
            PocoEntity.ShippedDate = _ShippedDate;
            PocoEntity.Freight = _Freight;
            PocoEntity.ShipName = _ShipName;
            PocoEntity.ShipAddress = _ShipAddress;
            PocoEntity.ShipCity = _ShipCity;
            PocoEntity.ShipRegion = _ShipRegion;
            PocoEntity.ShipPostalCode = _ShipPostalCode;
            PocoEntity.ShipCountry = _ShipCountry;
            if (!(PocoEntity is IEntityProxy))
            {
                PocoEntity.Customer = this.CustomerReference.Value.GetPocoEntityOrNull();
                UpdateCollection(PocoEntity.OrderDetails, this.OrderDetails);
            }
        }
        private Int32 _OrderID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 OrderID
        {
            get
            {
                return _OrderID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.OrderID = value;
                }
                ReportPropertyChanging("OrderID");
                _OrderID = value;
                ReportPropertyChanged("OrderID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public Int32 EmployeeID
        {
            get
            {
                return _EmployeeID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.EmployeeID = value;
                }
                ReportPropertyChanging("EmployeeID");
                _EmployeeID = value;
                ReportPropertyChanged("EmployeeID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public DateTime? OrderDate
        {
            get
            {
                return _OrderDate;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.OrderDate = value;
                }
                ReportPropertyChanging("OrderDate");
                _OrderDate = value;
                ReportPropertyChanged("OrderDate");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public DateTime? RequiredDate
        {
            get
            {
                return _RequiredDate;
            }
            set
            {
                if (_pocoEntity != null)
                {
                }
                ReportPropertyChanging("RequiredDate");
                _RequiredDate = value;
                ReportPropertyChanged("RequiredDate");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public DateTime? ShippedDate
        {
            get
            {
                return _ShippedDate;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ShippedDate = value;
                }
                ReportPropertyChanging("ShippedDate");
                _ShippedDate = value;
                ReportPropertyChanged("ShippedDate");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public Decimal? Freight
        {
            get
            {
                return _Freight;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.Freight = value;
                }
                ReportPropertyChanging("Freight");
                _Freight = value;
                ReportPropertyChanged("Freight");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ShipName
        {
            get
            {
                return _ShipName;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ShipName = value;
                }
                ReportPropertyChanging("ShipName");
                _ShipName = value;
                ReportPropertyChanged("ShipName");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ShipAddress
        {
            get
            {
                return _ShipAddress;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ShipAddress = value;
                }
                ReportPropertyChanging("ShipAddress");
                _ShipAddress = value;
                ReportPropertyChanged("ShipAddress");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ShipCity
        {
            get
            {
                return _ShipCity;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ShipCity = value;
                }
                ReportPropertyChanging("ShipCity");
                _ShipCity = value;
                ReportPropertyChanged("ShipCity");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ShipRegion
        {
            get
            {
                return _ShipRegion;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ShipRegion = value;
                }
                ReportPropertyChanging("ShipRegion");
                _ShipRegion = value;
                ReportPropertyChanged("ShipRegion");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ShipPostalCode
        {
            get
            {
                return _ShipPostalCode;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ShipPostalCode = value;
                }
                ReportPropertyChanging("ShipPostalCode");
                _ShipPostalCode = value;
                ReportPropertyChanged("ShipPostalCode");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public String ShipCountry
        {
            get
            {
                return _ShipCountry;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ShipCountry = value;
                }
                ReportPropertyChanging("ShipCountry");
                _ShipCountry = value;
                ReportPropertyChanged("ShipCountry");
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "CustomerOrders", "Customer")]
        public NorthwindEF.PocoAdapters.CustomerAdapter Customer
        {
            get { return this.CustomerReference.Value; }
            set { this.CustomerReference.Value = value; }
        }

        private EntityReference<NorthwindEF.PocoAdapters.CustomerAdapter> _CustomerReferenceCache = null;
        public EntityReference<NorthwindEF.PocoAdapters.CustomerAdapter> CustomerReference
        {
            get
            {
                if (_CustomerReferenceCache == null)
                    _CustomerReferenceCache = GetRelatedReference<NorthwindEF.PocoAdapters.CustomerAdapter>("CustomerOrders", "Customer");
                return _CustomerReferenceCache;
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Order_Details_Order", "OrderDetails")]
        public EntityCollection<NorthwindEF.PocoAdapters.OrderDetailAdapter> OrderDetails
        {
            get
            {
                if (_OrderDetailsCollectionCache == null)
                    _OrderDetailsCollectionCache = GetRelatedCollection<NorthwindEF.PocoAdapters.OrderDetailAdapter>("Order_Details_Order", "OrderDetails");
                 return _OrderDetailsCollectionCache;
            }
        }
        private EntityCollection<NorthwindEF.PocoAdapters.OrderDetailAdapter> _OrderDetailsCollectionCache = null;
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="OrderDetail")]
    public partial class OrderDetailAdapter : PocoAdapterBase<NorthwindEF.OrderDetail>, IPocoAdapter<NorthwindEF.OrderDetail>
    {
        private Decimal _UnitPrice;
        private Int16 _Quantity;
        private Single _Discount;
        public OrderDetailAdapter() { OnAdapterCreated(); }
        public OrderDetailAdapter(NorthwindEF.OrderDetail pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.OrderDetail(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.PocoProxies.OrderDetailProxy(this); }
        public override void Init()
        {
            base.Init();
            this.OrderReference.AssociationChanged += Order_Changed;
            this.ProductReference.AssociationChanged += Product_Changed;
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
        }
        void Order_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            if (PocoEntity is IEntityProxy) return;
            PocoEntity.Order = this.Order.GetPocoEntityOrNull();
        }
        void Product_Changed(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            if (IsDetectingChanges) return;
            if (_pocoEntity == null) return;
            if (PocoEntity is IEntityProxy) return;
            PocoEntity.Product = this.Product.GetPocoEntityOrNull();
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.OrderID, ref _OrderID, "OrderID");
            DetectChanges(PocoEntity.ProductID, ref _ProductID, "ProductID");
            if (!(PocoEntity is IEntityProxy))
            {
                DetectChanges(PocoEntity.UnitPrice, ref _UnitPrice, "UnitPrice");
                DetectChanges(PocoEntity.Quantity, ref _Quantity, "Quantity");
                DetectChanges(PocoEntity.Discount, ref _Discount, "Discount");
                DetectChanges(PocoEntity.Order, this.OrderReference, "Order");
                DetectChanges(PocoEntity.Product, this.ProductReference, "Product");
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.OrderDetail(); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.OrderDetailProxy(this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.OrderID = _OrderID;
            PocoEntity.ProductID = _ProductID;
            if (!(PocoEntity is IEntityProxy))
            {
                PocoEntity.UnitPrice = _UnitPrice;
                PocoEntity.Quantity = _Quantity;
                PocoEntity.Discount = _Discount;
                PocoEntity.Order = this.OrderReference.Value.GetPocoEntityOrNull();
                PocoEntity.Product = this.ProductReference.Value.GetPocoEntityOrNull();
            }
        }
        private Int32 _OrderID;
        private Int32 _ProductID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 OrderID
        {
            get
            {
                return _OrderID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.OrderID = value;
                }
                ReportPropertyChanging("OrderID");
                _OrderID = value;
                ReportPropertyChanged("OrderID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 ProductID
        {
            get
            {
                return _ProductID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ProductID = value;
                }
                ReportPropertyChanging("ProductID");
                _ProductID = value;
                ReportPropertyChanged("ProductID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public Decimal UnitPrice
        {
            get
            {
                return _UnitPrice;
            }
            set
            {
                if (_pocoEntity != null && !(_pocoEntity is IEntityProxy))
                {
                    PocoEntity.UnitPrice = value;
                }
                ReportPropertyChanging("UnitPrice");
                _UnitPrice = value;
                ReportPropertyChanged("UnitPrice");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public Int16 Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                if (_pocoEntity != null && !(_pocoEntity is IEntityProxy))
                {
                    PocoEntity.Quantity = value;
                }
                ReportPropertyChanging("Quantity");
                _Quantity = value;
                ReportPropertyChanged("Quantity");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public Single Discount
        {
            get
            {
                return _Discount;
            }
            set
            {
                if (_pocoEntity != null && !(_pocoEntity is IEntityProxy))
                {
                    PocoEntity.Discount = value;
                }
                ReportPropertyChanging("Discount");
                _Discount = value;
                ReportPropertyChanged("Discount");
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Order_Details_Order", "Order")]
        public NorthwindEF.PocoAdapters.OrderAdapter Order
        {
            get { return this.OrderReference.Value; }
            set { this.OrderReference.Value = value; }
        }

        private EntityReference<NorthwindEF.PocoAdapters.OrderAdapter> _OrderReferenceCache = null;
        public EntityReference<NorthwindEF.PocoAdapters.OrderAdapter> OrderReference
        {
            get
            {
                if (_OrderReferenceCache == null)
                    _OrderReferenceCache = GetRelatedReference<NorthwindEF.PocoAdapters.OrderAdapter>("Order_Details_Order", "Order");
                return _OrderReferenceCache;
            }
        }

        [EdmRelationshipNavigationProperty("NorthwindEFModel", "Order_Details_Product", "Product")]
        public NorthwindEF.PocoAdapters.ProductAdapter Product
        {
            get { return this.ProductReference.Value; }
            set { this.ProductReference.Value = value; }
        }

        private EntityReference<NorthwindEF.PocoAdapters.ProductAdapter> _ProductReferenceCache = null;
        public EntityReference<NorthwindEF.PocoAdapters.ProductAdapter> ProductReference
        {
            get
            {
                if (_ProductReferenceCache == null)
                    _ProductReferenceCache = GetRelatedReference<NorthwindEF.PocoAdapters.ProductAdapter>("Order_Details_Product", "Product");
                return _ProductReferenceCache;
            }
        }
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="InternationalOrder")]
    public partial class InternationalOrderAdapter : NorthwindEF.PocoAdapters.OrderAdapter, IPocoAdapter<NorthwindEF.InternationalOrder>
    {
        private String _CustomsDescription;
        private Decimal _ExciseTax;
        public InternationalOrderAdapter() { OnAdapterCreated(); }
        public InternationalOrderAdapter(NorthwindEF.InternationalOrder pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.InternationalOrder(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.PocoProxies.InternationalOrderProxy(this); }
        public new NorthwindEF.InternationalOrder PocoEntity { get { return (NorthwindEF.InternationalOrder)base.PocoEntity; } }
        public override void Init()
        {
            base.Init();
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.CustomsDescription, ref _CustomsDescription, "CustomsDescription");
            DetectChanges(PocoEntity.ExciseTax, ref _ExciseTax, "ExciseTax");
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.InternationalOrder(); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.InternationalOrderProxy(this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.CustomsDescription = _CustomsDescription;
            PocoEntity.ExciseTax = _ExciseTax;
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String CustomsDescription
        {
            get
            {
                return _CustomsDescription;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.CustomsDescription = value;
                }
                ReportPropertyChanging("CustomsDescription");
                _CustomsDescription = value;
                ReportPropertyChanged("CustomsDescription");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public Decimal ExciseTax
        {
            get
            {
                return _ExciseTax;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.ExciseTax = value;
                }
                ReportPropertyChanging("ExciseTax");
                _ExciseTax = value;
                ReportPropertyChanged("ExciseTax");
            }
        }
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="PreviousEmployee")]
    public partial class PreviousEmployeeAdapter : NorthwindEF.PocoAdapters.EmployeeAdapter, IPocoAdapter<NorthwindEF.PreviousEmployee>
    {
        public PreviousEmployeeAdapter() { OnAdapterCreated(); }
        public PreviousEmployeeAdapter(NorthwindEF.PreviousEmployee pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.PreviousEmployee(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.PocoProxies.PreviousEmployeeProxy(this); }
        public new NorthwindEF.PreviousEmployee PocoEntity { get { return (NorthwindEF.PreviousEmployee)base.PocoEntity; } }
    }

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="DiscontinuedProduct")]
    public partial class DiscontinuedProductAdapter : NorthwindEF.PocoAdapters.ProductAdapter, IPocoAdapter<NorthwindEF.DiscontinuedProduct>
    {
        private DateTime? _DiscontinuedDate;
        public DiscontinuedProductAdapter() { OnAdapterCreated(); }
        public DiscontinuedProductAdapter(NorthwindEF.DiscontinuedProduct pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return null; }
        public override object CreatePocoEntityProxy() { return null; }
        public new NorthwindEF.DiscontinuedProduct PocoEntity { get { return (NorthwindEF.DiscontinuedProduct)base.PocoEntity; } }
        public override void Init()
        {
            base.Init();
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            DetectChanges(PocoEntity.DiscontinuedDate, ref _DiscontinuedDate, "DiscontinuedDate");
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.DiscontinuedProduct(Supplier.GetPocoEntityOrNull()); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.PocoProxies.DiscontinuedProductProxy(Supplier.GetPocoEntityOrNull(), this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            PocoEntity.DiscontinuedDate = _DiscontinuedDate;
            if (!(PocoEntity is IEntityProxy))
            {
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=true)]
        public DateTime? DiscontinuedDate
        {
            get
            {
                return _DiscontinuedDate;
            }
            set
            {
                if (_pocoEntity != null)
                {
                    PocoEntity.DiscontinuedDate = value;
                }
                ReportPropertyChanging("DiscontinuedDate");
                _DiscontinuedDate = value;
                ReportPropertyChanged("DiscontinuedDate");
            }
        }
    }
}
namespace NorthwindEF.Territories.PocoAdapters
{
    using System;
    using System.Data;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ComponentModel;
    using EFPocoAdapter;
    using EFPocoAdapter.DataClasses;

    // POCO Adapters for Entity Types

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="Territory")]
    public partial class TerritoryAdapter : PocoAdapterBase<NorthwindEF.Territories.Territory>, IPocoAdapter<NorthwindEF.Territories.Territory>
    {
        private String _TerritoryDescription;
        public TerritoryAdapter() { OnAdapterCreated(); }
        public TerritoryAdapter(NorthwindEF.Territories.Territory pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new NorthwindEF.Territories.Territory(); }
        public override object CreatePocoEntityProxy() { return new NorthwindEF.Territories.PocoProxies.TerritoryProxy(this); }
        public override void Init()
        {
            base.Init();
        }
        public override void InitCollections(bool enableProxies)
        {
            if (_pocoEntity == null) return;
            base.InitCollections(enableProxies);
        }
        public override void DetectChanges()
        {
            base.DetectChanges();
            if (!(PocoEntity is IEntityProxy))
            {
                DetectChanges(PocoEntity.TerritoryDescription, ref _TerritoryDescription, "TerritoryDescription");
            }
        }
        public override void PopulatePocoEntity(bool enableProxies)
        {
            if (_pocoEntity == null)
            {
                if (!enableProxies)
                {
                    _pocoEntity = new NorthwindEF.Territories.Territory(); // poco
                }
                else
                {
                    _pocoEntity = new NorthwindEF.Territories.PocoProxies.TerritoryProxy(this); // proxy
                }
                RegisterAdapterInContext();
                InitCollections(enableProxies);
            }
            base.PopulatePocoEntity(enableProxies);
            if (!(PocoEntity is IEntityProxy))
            {
                PocoEntity.TerritoryDescription = _TerritoryDescription;
            }
        }
        private Int32 _TerritoryID;

        [EdmScalarProperty(EntityKeyProperty=true, IsNullable=false)]
        public Int32 TerritoryID
        {
            get
            {
                return _TerritoryID;
            }
            set
            {
                if (_pocoEntity != null)
                {
                }
                ReportPropertyChanging("TerritoryID");
                _TerritoryID = value;
                ReportPropertyChanged("TerritoryID");
            }
        }

        [EdmScalarProperty(EntityKeyProperty=false, IsNullable=false)]
        public String TerritoryDescription
        {
            get
            {
                return _TerritoryDescription;
            }
            set
            {
                if (_pocoEntity != null && !(_pocoEntity is IEntityProxy))
                {
                    PocoEntity.TerritoryDescription = value;
                }
                ReportPropertyChanging("TerritoryDescription");
                _TerritoryDescription = value;
                ReportPropertyChanged("TerritoryDescription");
            }
        }
    }
}
namespace PocoAdapters
{
    using System;
    using System.Data;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ComponentModel;
    using EFPocoAdapter;
    using EFPocoAdapter.DataClasses;

    // POCO Adapters for Entity Types

    [EdmEntityType(NamespaceName="NorthwindEFModel", Name="CurrentEmployee")]
    public partial class CurrentEmployeeAdapter : NorthwindEF.PocoAdapters.EmployeeAdapter, IPocoAdapter<CurrentEmployee>
    {
        public CurrentEmployeeAdapter() { OnAdapterCreated(); }
        public CurrentEmployeeAdapter(CurrentEmployee pocoObject) : base(pocoObject) { OnAdapterCreated(); }

        partial void OnAdapterCreated();
        public override object CreatePocoEntity() { return new CurrentEmployee(); }
        public override object CreatePocoEntityProxy() { return new PocoProxies.CurrentEmployeeProxy(this); }
        public new CurrentEmployee PocoEntity { get { return (CurrentEmployee)base.PocoEntity; } }
    }
}
namespace NorthwindEF.PocoProxies
{
    using System;
    using System.Data;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ComponentModel;
    using EFPocoAdapter;
    using EFPocoAdapter.DataClasses;

    public partial class RegionProxy : NorthwindEF.Region, IEntityProxy
    {
        NorthwindEF.PocoAdapters.RegionAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.RegionAdapter)value; }
        }
        public RegionProxy(NorthwindEF.PocoAdapters.RegionAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
    }

    public partial class SupplierProxy : NorthwindEF.Supplier, IEntityProxy
    {
        NorthwindEF.PocoAdapters.SupplierAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.SupplierAdapter)value; }
        }
        public SupplierProxy(NorthwindEF.PocoAdapters.SupplierAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
    }

    public partial class ProductProxy : NorthwindEF.Product, IEntityProxy
    {
        NorthwindEF.PocoAdapters.ProductAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.ProductAdapter)value; }
        }
        public ProductProxy(NorthwindEF.Supplier pSupplier, NorthwindEF.PocoAdapters.ProductAdapter adapter) : base(pSupplier) { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
    }

    public partial class CategoryProxy : NorthwindEF.Category, IEntityProxy
    {
        NorthwindEF.PocoAdapters.CategoryAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.CategoryAdapter)value; }
        }
        public CategoryProxy(Int32 pCategoryID, String pCategoryName, String pDescription, List<NorthwindEF.Product> pProducts, NorthwindEF.PocoAdapters.CategoryAdapter adapter) : base(pCategoryID, pCategoryName, pDescription, pProducts) { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
    }

    public partial class CustomerProxy : NorthwindEF.Customer, IEntityProxy
    {
        NorthwindEF.PocoAdapters.CustomerAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.CustomerAdapter)value; }
        }
        public CustomerProxy(NorthwindEF.PocoAdapters.CustomerAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
    }

    public partial class OrderProxy : NorthwindEF.Order, IEntityProxy
    {
        NorthwindEF.PocoAdapters.OrderAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.OrderAdapter)value; }
        }
        public OrderProxy(NorthwindEF.PocoAdapters.OrderAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
        public override NorthwindEF.Customer Customer
        {
            get
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.CustomerAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.CustomerAdapter>("CustomerOrders","Customer");
                if (!reference.IsLoaded && _adapter.CanLoadProperty("Customer"))
                {
                    using (ThreadLocalContext.Set(_adapter.Context)) reference.Load();
                }
                return this._adapter.Customer.GetPocoEntityOrNull();
            }
            set
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.CustomerAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.CustomerAdapter>("CustomerOrders","Customer");
                var newValue = _adapter.Context.GetAdapterObject<NorthwindEF.PocoAdapters.CustomerAdapter>(value);
                if (!Object.ReferenceEquals(reference.Value, newValue))
                {
                    _adapter.Context.RaiseChangeDetected(this, "Customer", reference.Value.GetPocoEntityOrNull(), newValue.GetPocoEntityOrNull());
                    reference.Value = newValue;
                }
            }
        }
    }

    public partial class OrderDetailProxy : NorthwindEF.OrderDetail, IEntityProxy
    {
        NorthwindEF.PocoAdapters.OrderDetailAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.OrderDetailAdapter)value; }
        }
        public OrderDetailProxy(NorthwindEF.PocoAdapters.OrderDetailAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
        public override Decimal UnitPrice
        {
            get { return this._adapter.UnitPrice; }
            set
            {
                base.UnitPrice = value;
                if (this._adapter.Context != null && value != this._adapter.UnitPrice)
                {
                    _adapter.Context.RaiseChangeDetected(this, "UnitPrice", this._adapter.UnitPrice, value);
                }
                this._adapter.UnitPrice = value;
            }
        }
        public override Int16 Quantity
        {
            get { return this._adapter.Quantity; }
            set
            {
                base.Quantity = value;
                if (this._adapter.Context != null && value != this._adapter.Quantity)
                {
                    _adapter.Context.RaiseChangeDetected(this, "Quantity", this._adapter.Quantity, value);
                }
                this._adapter.Quantity = value;
            }
        }
        public override Single Discount
        {
            get { return this._adapter.Discount; }
            set
            {
                base.Discount = value;
                if (this._adapter.Context != null && value != this._adapter.Discount)
                {
                    _adapter.Context.RaiseChangeDetected(this, "Discount", this._adapter.Discount, value);
                }
                this._adapter.Discount = value;
            }
        }
        public override NorthwindEF.Order Order
        {
            get
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.OrderAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.OrderAdapter>("Order_Details_Order","Order");
                if (!reference.IsLoaded && _adapter.CanLoadProperty("Order"))
                {
                    using (ThreadLocalContext.Set(_adapter.Context)) reference.Load();
                }
                return this._adapter.Order.GetPocoEntityOrNull();
            }
            set
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.OrderAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.OrderAdapter>("Order_Details_Order","Order");
                var newValue = _adapter.Context.GetAdapterObject<NorthwindEF.PocoAdapters.OrderAdapter>(value);
                if (!Object.ReferenceEquals(reference.Value, newValue))
                {
                    _adapter.Context.RaiseChangeDetected(this, "Order", reference.Value.GetPocoEntityOrNull(), newValue.GetPocoEntityOrNull());
                    reference.Value = newValue;
                }
            }
        }
        public override NorthwindEF.Product Product
        {
            get
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.ProductAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.ProductAdapter>("Order_Details_Product","Product");
                if (!reference.IsLoaded && _adapter.CanLoadProperty("Product"))
                {
                    using (ThreadLocalContext.Set(_adapter.Context)) reference.Load();
                }
                return this._adapter.Product.GetPocoEntityOrNull();
            }
            set
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.ProductAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.ProductAdapter>("Order_Details_Product","Product");
                var newValue = _adapter.Context.GetAdapterObject<NorthwindEF.PocoAdapters.ProductAdapter>(value);
                if (!Object.ReferenceEquals(reference.Value, newValue))
                {
                    _adapter.Context.RaiseChangeDetected(this, "Product", reference.Value.GetPocoEntityOrNull(), newValue.GetPocoEntityOrNull());
                    reference.Value = newValue;
                }
            }
        }
    }

    public partial class InternationalOrderProxy : NorthwindEF.InternationalOrder, IEntityProxy
    {
        NorthwindEF.PocoAdapters.InternationalOrderAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.InternationalOrderAdapter)value; }
        }
        public InternationalOrderProxy(NorthwindEF.PocoAdapters.InternationalOrderAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
        public override NorthwindEF.Customer Customer
        {
            get
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.CustomerAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.CustomerAdapter>("CustomerOrders","Customer");
                if (!reference.IsLoaded && _adapter.CanLoadProperty("Customer"))
                {
                    using (ThreadLocalContext.Set(_adapter.Context)) reference.Load();
                }
                return this._adapter.Customer.GetPocoEntityOrNull();
            }
            set
            {
                IEntityWithRelationships iewr = _adapter as IEntityWithRelationships;
                EntityReference<NorthwindEF.PocoAdapters.CustomerAdapter> reference = iewr.RelationshipManager.GetRelatedReference<NorthwindEF.PocoAdapters.CustomerAdapter>("CustomerOrders","Customer");
                var newValue = _adapter.Context.GetAdapterObject<NorthwindEF.PocoAdapters.CustomerAdapter>(value);
                if (!Object.ReferenceEquals(reference.Value, newValue))
                {
                    _adapter.Context.RaiseChangeDetected(this, "Customer", reference.Value.GetPocoEntityOrNull(), newValue.GetPocoEntityOrNull());
                    reference.Value = newValue;
                }
            }
        }
    }

    public partial class PreviousEmployeeProxy : NorthwindEF.PreviousEmployee, IEntityProxy
    {
        NorthwindEF.PocoAdapters.PreviousEmployeeAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.PreviousEmployeeAdapter)value; }
        }
        public PreviousEmployeeProxy(NorthwindEF.PocoAdapters.PreviousEmployeeAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
        public override DateTime? BirthDate
        {
            get { return this._adapter.BirthDate; }
            set
            {
                base.BirthDate = value;
                if (this._adapter.Context != null && value != this._adapter.BirthDate)
                {
                    _adapter.Context.RaiseChangeDetected(this, "BirthDate", this._adapter.BirthDate, value);
                }
                this._adapter.BirthDate = value;
            }
        }
    }

    public partial class DiscontinuedProductProxy : NorthwindEF.DiscontinuedProduct, IEntityProxy
    {
        NorthwindEF.PocoAdapters.DiscontinuedProductAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.PocoAdapters.DiscontinuedProductAdapter)value; }
        }
        public DiscontinuedProductProxy(NorthwindEF.Supplier pSupplier, NorthwindEF.PocoAdapters.DiscontinuedProductAdapter adapter) : base(pSupplier) { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
    }
}
namespace NorthwindEF.Territories.PocoProxies
{
    using System;
    using System.Data;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ComponentModel;
    using EFPocoAdapter;
    using EFPocoAdapter.DataClasses;

    public partial class TerritoryProxy : NorthwindEF.Territories.Territory, IEntityProxy
    {
        NorthwindEF.Territories.PocoAdapters.TerritoryAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (NorthwindEF.Territories.PocoAdapters.TerritoryAdapter)value; }
        }
        public TerritoryProxy(NorthwindEF.Territories.PocoAdapters.TerritoryAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
        public override String TerritoryDescription
        {
            get { return this._adapter.TerritoryDescription; }
            set
            {
                base.TerritoryDescription = value;
                if (this._adapter.Context != null && value != this._adapter.TerritoryDescription)
                {
                    _adapter.Context.RaiseChangeDetected(this, "TerritoryDescription", this._adapter.TerritoryDescription, value);
                }
                this._adapter.TerritoryDescription = value;
            }
        }
    }
}
namespace PocoProxies
{
    using System;
    using System.Data;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ComponentModel;
    using EFPocoAdapter;
    using EFPocoAdapter.DataClasses;

    public partial class CurrentEmployeeProxy : CurrentEmployee, IEntityProxy
    {
        PocoAdapters.CurrentEmployeeAdapter _adapter;
        IPocoAdapter IEntityProxy.Adapter
        {
            get { return _adapter; }
            set { _adapter = (PocoAdapters.CurrentEmployeeAdapter)value; }
        }
        public CurrentEmployeeProxy(PocoAdapters.CurrentEmployeeAdapter adapter) : base() { _adapter = adapter; OnProxyCreated(); }
        partial void OnProxyCreated();
        public override DateTime? BirthDate
        {
            get { return this._adapter.BirthDate; }
            set
            {
                base.BirthDate = value;
                if (this._adapter.Context != null && value != this._adapter.BirthDate)
                {
                    _adapter.Context.RaiseChangeDetected(this, "BirthDate", this._adapter.BirthDate, value);
                }
                this._adapter.BirthDate = value;
            }
        }
    }
}
