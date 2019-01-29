using CopyLi.Data.Entities.Authentication;
using CopyLi.Data.Entities.Items;
using CopyLi.Data.Entities.Orders;
using CopyLi.Data.Entities.Requests;
using CopyLi.Data.Entities.Service;
using CopyLi.Data.Entities.Users.Admins;
using CopyLi.Data.Entities.Users.Customers;
using CopyLi.Data.Entities.Users.Vendors;
using System.Data.Entity;

namespace CopyLi.Data
{
    public class DatabaseContext : DbContext
    {
        static DatabaseContext()
        {
            Database.SetInitializer<DatabaseContext>(null);
        }

        public DatabaseContext() : base("name=defaultConnectionString")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Membership>();
            modelBuilder.Entity<Vendor>();
            modelBuilder.Entity<Role>();


            modelBuilder.Entity<Vendor>()
            .HasMany(c => c.Requests)
            .WithRequired()
            .WillCascadeOnDelete(false);



            modelBuilder.Entity<Customer>();




            modelBuilder.Entity<Request>()
              .HasRequired(i => i.Customer)
              .WithMany(pc => pc.Requests)
              .HasForeignKey(i => i.CustomerId)
              .WillCascadeOnDelete(false);



            modelBuilder.Entity<Request>()
            .HasRequired(i => i.Vendor)
            .WithMany(s => s.Requests)
            .HasForeignKey(s => s.VendorId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Request>()
               .HasOptional(s => s.RequestBid) // Mark Address property optional in Student entity
               .WithRequired(ad => ad.Request); // mark Student property as required in StudentAddress entity. Cannot save StudentAddress without Student


            modelBuilder.Entity<RequestHistory>()
           .HasRequired(i => i.Customer)
           .WithMany(pc => pc.RequestHistorires)
           .HasForeignKey(i => i.CustomerId)
           .WillCascadeOnDelete(false);



            modelBuilder.Entity<Order>()
            .HasMany(c => c.Items)
            .WithRequired()
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderHistory>();

            modelBuilder.Entity<Order>()
            .HasRequired(s => s.Vendor)
             .WithMany()
             .HasForeignKey(s => s.VendorId)
           .WillCascadeOnDelete(false);



            modelBuilder.Entity<Admin>();

            modelBuilder.Entity<RequestBid>();

            modelBuilder.Entity<Item>()
                .HasRequired(i => i.ServiceType)
                .WithMany(st => st.Items)
                .HasForeignKey(r => r.ServiceTypeId);

            modelBuilder.Entity<Services>();
            modelBuilder.Entity<ServiceType>();


        }
    }

}
