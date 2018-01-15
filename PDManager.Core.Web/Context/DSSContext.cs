using Microsoft.EntityFrameworkCore;
using PDManager.Core.Web.Entities;

namespace PDManager.Core.Web.Context
{
    /// <summary>
    /// DSS Context
    /// A small Context in order to store dss models
    /// This implementationj is a inmemory database only for testing
    /// </summary>
    public class DSSContext : DbContext
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public DSSContext(DbContextOptions<DSSContext> options)
      : base(options)
        { }


        /// <summary>
        /// DSS Models
        /// </summary>
        public DbSet<DSSModel> DSSModels { get; set; }

        /// <summary>
        /// Aggregation Models
        /// </summary>
        public DbSet<AggrModel> AggrModels { get; set; }

        /// <summary>
        /// Aggregation Models
        /// </summary>
        public DbSet<AlertModel> AlertModels { get; set; }



        /// <summary>
        /// On Configuration
        /// Here we set the storage provider
        /// Current implementation  use the InMemoryDatabase with a line of code
        ///    optionsBuilder.UseInMemoryDatabase("dssdb");
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("dssdb");
            
        }

        /// <summary>
        /// On Model Creating method
        /// Include here all configuration
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DSSModel>().HasKey(m => m.Id);
            modelBuilder.Entity<AggrModel>().HasKey(m => m.Id);
            modelBuilder.Entity<AlertModel>().HasKey(m => m.Id);
        }
    }
}