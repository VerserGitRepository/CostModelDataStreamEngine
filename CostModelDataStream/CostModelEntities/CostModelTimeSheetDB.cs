
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
        public virtual DbSet<Customers> Customers { get; set; }       
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
                ;

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TravelCostPerUnit);


            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.LabourCostPerUnit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.VariableCostPerUnit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.PMCostPerUnit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TechnicianHourlyRate);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TravelCostHoursPerunit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.LabourCostHoursPerUnit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.PMCostHoursPerUnit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.VariableCostPerUnitNA);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TotalCost);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.ProfitPerUnit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.TotalProfit);
               

            modelBuilder.Entity<ServiceCost>()
                .Property(e => e.ActualMarginOnOverHead);



            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.ServiceDescription);



            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.PricePerUnit);



            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.Quantity);



            modelBuilder.Entity<ServiceRevenue>()
                .Property(e => e.TotalPrice);
                

        }
    }
}
