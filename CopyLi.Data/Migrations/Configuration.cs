namespace CopyLi.Data.Migrations
{
    using CopyLi.Data.Entities.Authentication;
    using CopyLi.Data.Entities.Items;
    using CopyLi.Data.Entities.Requests;
    using CopyLi.Data.Entities.Service;
    using CopyLi.Data.Entities.Users.Admins;
    using CopyLi.Data.Entities.Users.Customers;
    using CopyLi.Data.Entities.Users.Vendors;
    using CopyLi.Utilites.Cryptography;
    using Framework.Data.EF;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CopyLi.Data.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        #region [SeedMethod]
        protected override void Seed(DatabaseContext context)
        {
            EncrptionService _encryptionService = new EncrptionService();

            using (var uow = new UnitOfWork(context))
            {
                #region [ Applications ]
                Application app1 = new Application()
                {
                    SecretSalt = "ABCD",
                    Secret = _encryptionService.CreatePasswordHash("p@$$w0rd", "ABCD"),
                    CreatedOn = DateTime.Now,
                    CreatedById = 0,
                    DisplayName = "Mobile",
                    IsActive = true,
                    Type = ApplicationType.Native,
                    TokenLifeTime = 999999
                };

                Application app2 = new Application()
                {
                    SecretSalt = "ABCD",
                    Secret = _encryptionService.CreatePasswordHash("p@$$w0rd", "ABCD"),
                    CreatedOn = DateTime.Now,
                    CreatedById = 0,
                    DisplayName = "Web",
                    IsActive = true,
                    Type = ApplicationType.Web,
                    TokenLifeTime = 999999
                };

                context.Set<Application>().AddOrUpdate(app1);
                context.Set<Application>().AddOrUpdate(app2);

                #endregion

                #region [ Customer ]

                var customerMembership = new Membership()
                {
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    PasswordSalt = "ABCD",
                    Password = _encryptionService.CreatePasswordHash("p@$$w0rd", "ABCD"),
                    UserName = "user1",
                    CreatedById = 0
                };
                var cutomer = new Customer()
                {
                    Membership = customerMembership,
                    Name = "Ali",
                    Email = "aa@yahoo.com"

                };
                context.Set<Customer>().AddOrUpdate(cutomer);
                #endregion

                #region [Vendor]

                var vendorMembership = new Membership()
                {
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    PasswordSalt = "ABCD",
                    Password = _encryptionService.CreatePasswordHash("p@$$w0rd", "ABCD"),
                    UserName = "user2",
                    CreatedById = 0
                };
                var vendor = new Vendor()
                {
                    Membership = vendorMembership,
                    Name = "ahmed",
                    Email = "aa@yahoo.com"

                };
                context.Set<Vendor>().AddOrUpdate(vendor);
                #endregion

                #region [Admin]
                var adminMembership = new Membership()
                {
                    CreatedOn = DateTime.Now,
                    IsActive = true,
                    PasswordSalt = "ABCD",
                    Password = _encryptionService.CreatePasswordHash("p@$$w0rd", "ABCD"),
                    UserName = "SuperAdmin",
                    CreatedById = 0
                };

                Admin admin = new Admin()
                {
                    Membership = adminMembership,
                    Name = "admin1",
                    Email = "admin@copyli.com"
                };
                context.Set<Admin>().AddOrUpdate(admin);
                #endregion

                //ICollection<Item> items = new List<Item>()
                //{
                //    new Item{Name="asd",Descrption="asdas",Data="Asdasd"}
                //};

                //context.Set<Item>().AddRange(items);

                //Request request = new Request()
                //{
                //    Items = items,
                //    CreatedOn = DateTime.Now,
                //    Customer = cutomer
                //};

                //context.Set<Request>().Add(request);


                //ICollection<ServiceType> servicetypes = new List<ServiceType>();



                //ServiceType serviceType1 = new ServiceType()
                //{
                //    Title = "service type one",
                //    Properties = "aaaa",
                //    BidProperties = "aasdasda",
                //    TitleAr = "asd",
                //    Items = items
                //};
                //servicetypes.Add(serviceType1);

                //context.Set<ServiceType>().AddRange(servicetypes);


                //Services service = new Services()
                //{
                //    ParentId = null,
                //    Title = "service 1",
                //    TitleAr = "service 1",
                //    ServiceTypes = servicetypes
                //};

                //context.Set<Services>().AddOrUpdate(service);

            }

        }

        #endregion
    }
}
