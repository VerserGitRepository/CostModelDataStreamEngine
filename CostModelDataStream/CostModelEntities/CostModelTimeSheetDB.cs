
using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace CostModelDataStream.CostModelEntities
{
    public partial class CostModelTimeSheetDB : DbContext
    {
        public CostModelTimeSheetDB()
            : base("name=CostModelTimeSheetDB1")
        {
        }
        public virtual DbSet<ProjectDetails> ProjectDetails { get; set; }
        public virtual DbSet<ServiceCost> ServiceCosts { get; set; }
        public virtual DbSet<ServiceRevenue> ServiceRevenues { get; set; }
        public virtual DbSet<FilesProcessed> FilesProcessed { get; set; }
        public virtual DbSet<ProjectManagers> ProjectManagers { get; set; }
        public virtual DbSet<SalesManagers> SalesManagers { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Resources> Resources { get; set; }
        public virtual DbSet<OpportunityNumbers> OpportunityNumbers { get; set; }
        public virtual DbSet<ServiceActivities> ServiceActivities { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.OpportunityNumber)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.SiteAddress)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.Customer)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.Approver)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.ProjectManager)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.SalesManager)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.CustomerContactName)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectDetails>()
                .Property(e => e.VerserBranch)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.CostCategory)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.CostPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TravelCostPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.LabourCostPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.VariableCostPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.PMCostPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TechnicianHourlyRate)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TravelCostHoursPerunit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.LabourCostHoursPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.PMCostHoursPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.VariableCostPerUnitNA)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TotalCost)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.ProfitPerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TotalProfit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.ActualMarginOnOverHead)
                .IsUnicode(false);
          

            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.ServiceDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.PricePerUnit)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.Quantity)
                .IsUnicode(false);

            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.TotalPrice)
                .IsUnicode(false);

        }
    }
}
