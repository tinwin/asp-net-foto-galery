[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "EmployeesTerritories", "Employees", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.Employee), "Territories", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.Territory))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Products_Category", "Category", global::System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(.Category), "Products", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.Product))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "CustomerOrders", "Customer", global::System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(.Customer), "Orders", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.Order))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Order_Details_Order", "Order", global::System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(.Order), "OrderDetails", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.OrderDetail))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Order_Details_Product", "Product", global::System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(.Product), "OrderDetails", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.OrderDetail))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "Products_Supplier", "Supplier", global::System.Data.Metadata.Edm.RelationshipMultiplicity.ZeroOrOne, typeof(.Supplier), "Products", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.Product))]
[assembly: global::System.Data.Objects.DataClasses.EdmRelationshipAttribute("NorthwindEFModel", "TerritoriesRegion", "Region", global::System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(.Region), "Territories", global::System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(.Territory))]
[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

