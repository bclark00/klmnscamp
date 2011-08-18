using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using Klmsncamp.Models;
using System.Web.Security;
using Telerik.Web.Mvc.UI;

namespace Klmsncamp.Models
{
    public class KlmsnContext : DbContext
    {
        public DbSet<ValidationState> ValidationStates { get; set; }
        public DbSet<Workshop> Workshops { get; set; }
        public DbSet<CustomPermission> CustomPermissions { get; set; }
        public DbSet<RequestType> RequestTypes { get; set; }
        public DbSet<RequestState> RequestStates { get; set; }
        public DbSet<Location> Locations {get; set;}
        public DbSet<InventoryOwnership> InventoryOwnerships { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<CorporateType> CorporateTypes { get; set; }
        public DbSet<CorporateAccount> CorporateAccounts { get; set; }
        public DbSet<RequestIssue> RequestIssues { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public IEnumerable<RequestType> RequestIE { get; set; }

        public ObjectContext KlmsnObjectContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<RequestIssue>()
                        .HasRequired(a => a.Location)
                        .WithMany()
                        .HasForeignKey(u => u.LocationID).WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestIssue>()
                        .HasRequired(a => a.Workshop)
                        .WithMany()
                        .HasForeignKey(u => u.WorkshopID).WillCascadeOnDelete(false);
 


        }


    }
}