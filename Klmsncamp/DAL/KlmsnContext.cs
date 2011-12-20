using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Web;
using System.Web.Security;
using Klmsncamp.Models;

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

        public DbSet<Location> Locations { get; set; }

        public DbSet<InventoryOwnership> InventoryOwnerships { get; set; }

        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<CorporateType> CorporateTypes { get; set; }

        public DbSet<CorporateAccount> CorporateAccounts { get; set; }

        public DbSet<RequestIssue> RequestIssues { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<Role> Roles { get; set; }

        public IEnumerable<RequestType> RequestIE { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<PaymentFile> PaymentFiles { get; set; }

        public DbSet<Personnel> Personnels { get; set; }

        public DbSet<LocationGroup> LocationGroups { get; set; }

        public DbSet<LogRequestIssue> LogRequestIssues { get; set; }

        public DbSet<LogRequestIssueDetail> LogRequestIssueDetails { get; set; }

        public DbSet<Project> Projects { get; set; }

        public ObjectContext KlmsnObjectContext
        {
            get { return ((IObjectContextAdapter)this).ObjectContext; }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            modelBuilder.Entity<RequestIssue>().HasRequired(a => a.RequestType).WithMany().HasForeignKey(u => u.RequestTypeID).WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestIssue>()
                        .HasRequired(a => a.Location)
                        .WithMany()
                        .HasForeignKey(u => u.LocationID).WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestIssue>()
                        .HasRequired(a => a.Workshop)
                        .WithMany()
                        .HasForeignKey(u => u.WorkshopID).WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestIssue>().HasRequired(a => a.ValidationState).WithMany().HasForeignKey(u => u.WorkshopID).WillCascadeOnDelete(false);

            modelBuilder.Entity<LogRequestIssue>().HasRequired(a => a.RequestIssue).WithMany().HasForeignKey(u => u.RequestIssueID).WillCascadeOnDelete(false);

            modelBuilder.Entity<Project>().HasRequired(a => a.cUser).WithMany().HasForeignKey(u => u.cUserID).WillCascadeOnDelete(false);
        }
    }
}