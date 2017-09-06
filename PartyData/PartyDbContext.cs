using Microsoft.EntityFrameworkCore;
using PartyData.Entities;

namespace PartyData
{
    public class PartyDbContext: DbContext
    {
        public DbSet<Party> Parties { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Organisation> Organisations { get; set; }

        public DbSet<CustomService> CustomServices { get; set; }

        public DbSet<PartyCustomServiceRegistration> PartyCustomServiceRegistrations { get; set; }

        public PartyDbContext(DbContextOptions<PartyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Party>().ToTable("Party").HasIndex(p => p.Name);
            modelBuilder.Entity<Organisation>().ToTable("Organisation");
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<CustomService>().ToTable("CustomService");
            modelBuilder.Entity<PartyCustomServiceRegistration>().ToTable("PartyCustomServiceRegistration");
        }
    }
}
