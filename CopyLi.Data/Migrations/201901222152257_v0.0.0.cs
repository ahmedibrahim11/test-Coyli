namespace CopyLi.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v000 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        PasswordSalt = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        RoleId = c.Long(),
                        Deleted = c.DateTime(),
                        CreatedById = c.Long(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedById = c.Long(),
                        UpdatedOn = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ApplicationRefreshTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefreshToken = c.String(),
                        ApplicationId = c.Int(nullable: false),
                        MembershipId = c.Long(nullable: false),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresOnUtc = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applications", t => t.ApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.Memberships", t => t.MembershipId, cascadeDelete: true)
                .Index(t => t.ApplicationId)
                .Index(t => t.MembershipId);
            
            CreateTable(
                "dbo.Applications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AllowedOrigin = c.String(),
                        DisplayName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Secret = c.String(),
                        SecretSalt = c.String(),
                        TokenLifeTime = c.Int(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedById = c.Long(),
                        UpdatedOn = c.DateTime(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Mobile = c.String(),
                        MembershipId = c.Long(nullable: false),
                        Location = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Memberships", t => t.MembershipId, cascadeDelete: true)
                .Index(t => t.MembershipId);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerId = c.Long(nullable: false),
                        VendorId = c.Long(nullable: false),
                        RequestStatus = c.Int(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Deleted = c.DateTime(),
                        CreatedById = c.Long(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedById = c.Long(),
                        UpdatedOn = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.CustomerId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Mobile = c.String(),
                        MembershipId = c.Long(nullable: false),
                        Latitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LocationName = c.String(),
                        Longitude = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Token = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Memberships", t => t.MembershipId, cascadeDelete: true)
                .Index(t => t.MembershipId);
            
            CreateTable(
                "dbo.RequestHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerId = c.Long(nullable: false),
                        CreatedById = c.Long(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedById = c.Long(),
                        UpdatedOn = c.DateTime(),
                        RequestId = c.Long(nullable: false),
                        vendorId = c.Long(),
                        RequestStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Descrption = c.String(),
                        Data = c.String(),
                        ServiceTypeId = c.Long(nullable: false),
                        RequestId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.RequestId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceTypes", t => t.ServiceTypeId, cascadeDelete: true)
                .Index(t => t.ServiceTypeId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.ServiceTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        TitleAr = c.String(),
                        Properties = c.String(),
                        BidProperties = c.String(),
                        Services_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.Services_Id)
                .Index(t => t.Services_Id);
            
            CreateTable(
                "dbo.RequestBids",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Data = c.String(),
                        RequestId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Requests", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerId = c.Long(nullable: false),
                        BidData = c.String(),
                        CoverLetter = c.Boolean(nullable: false),
                        HoldingPlasticFile = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        ReviewData = c.String(),
                        VendorId = c.Long(nullable: false),
                        RequestId = c.Long(nullable: false),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Deleted = c.DateTime(),
                        CreatedById = c.Long(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedById = c.Long(),
                        UpdatedOn = c.DateTime(),
                        LastLoginDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.VendorId)
                .Index(t => t.CustomerId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.ItemOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Descrption = c.String(),
                        Data = c.String(),
                        ServiceTypeId = c.Long(nullable: false),
                        orderId = c.Long(nullable: false),
                        Order_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.orderId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceTypes", t => t.ServiceTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .Index(t => t.ServiceTypeId)
                .Index(t => t.orderId)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.OrderHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreatedById = c.Long(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        UpdatedById = c.Long(),
                        UpdatedOn = c.DateTime(),
                        OrderId = c.Long(nullable: false),
                        vendorId = c.Long(),
                        OrderStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        MembershipId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Memberships", t => t.MembershipId, cascadeDelete: true)
                .Index(t => t.MembershipId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        TitleAr = c.String(),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceTypes", "Services_Id", "dbo.Services");
            DropForeignKey("dbo.Admins", "MembershipId", "dbo.Memberships");
            DropForeignKey("dbo.Orders", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.ItemOrders", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.ItemOrders", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.ItemOrders", "orderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Requests", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.RequestBids", "Id", "dbo.Requests");
            DropForeignKey("dbo.Items", "ServiceTypeId", "dbo.ServiceTypes");
            DropForeignKey("dbo.Items", "RequestId", "dbo.Requests");
            DropForeignKey("dbo.Requests", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.RequestHistories", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "MembershipId", "dbo.Memberships");
            DropForeignKey("dbo.Vendors", "MembershipId", "dbo.Memberships");
            DropForeignKey("dbo.Memberships", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.ApplicationRefreshTokens", "MembershipId", "dbo.Memberships");
            DropForeignKey("dbo.ApplicationRefreshTokens", "ApplicationId", "dbo.Applications");
            DropIndex("dbo.Admins", new[] { "MembershipId" });
            DropIndex("dbo.ItemOrders", new[] { "Order_Id" });
            DropIndex("dbo.ItemOrders", new[] { "orderId" });
            DropIndex("dbo.ItemOrders", new[] { "ServiceTypeId" });
            DropIndex("dbo.Orders", new[] { "VendorId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.RequestBids", new[] { "Id" });
            DropIndex("dbo.ServiceTypes", new[] { "Services_Id" });
            DropIndex("dbo.Items", new[] { "RequestId" });
            DropIndex("dbo.Items", new[] { "ServiceTypeId" });
            DropIndex("dbo.RequestHistories", new[] { "CustomerId" });
            DropIndex("dbo.Customers", new[] { "MembershipId" });
            DropIndex("dbo.Requests", new[] { "VendorId" });
            DropIndex("dbo.Requests", new[] { "CustomerId" });
            DropIndex("dbo.Vendors", new[] { "MembershipId" });
            DropIndex("dbo.ApplicationRefreshTokens", new[] { "MembershipId" });
            DropIndex("dbo.ApplicationRefreshTokens", new[] { "ApplicationId" });
            DropIndex("dbo.Memberships", new[] { "RoleId" });
            DropTable("dbo.Services");
            DropTable("dbo.Admins");
            DropTable("dbo.OrderHistories");
            DropTable("dbo.ItemOrders");
            DropTable("dbo.Orders");
            DropTable("dbo.RequestBids");
            DropTable("dbo.ServiceTypes");
            DropTable("dbo.Items");
            DropTable("dbo.RequestHistories");
            DropTable("dbo.Customers");
            DropTable("dbo.Requests");
            DropTable("dbo.Vendors");
            DropTable("dbo.Roles");
            DropTable("dbo.Applications");
            DropTable("dbo.ApplicationRefreshTokens");
            DropTable("dbo.Memberships");
        }
    }
}
