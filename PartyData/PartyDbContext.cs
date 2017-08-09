using Microsoft.EntityFrameworkCore;
using PartyData.Entities;

namespace PartyData
{
    public class PartyDbContext: DbContext
    {
        public DbSet<Party> Parties { get; set; }

        public PartyDbContext(DbContextOptions<PartyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Party>().ToTable("Party");
            modelBuilder.Entity<Party>().Property(x => x.PartyId);
            modelBuilder.Entity<Party>().Property(x => x.Name);
            modelBuilder.Entity<Party>().Property(x => x.DateCreated);
            modelBuilder.Entity<Party>().Property(x => x.DateLastModified);

            modelBuilder.Entity<Organisation>().ToTable("Organisation");
            modelBuilder.Entity<Organisation>().Property(x => x.OrganisationId);
            modelBuilder.Entity<Organisation>().Property(x => x.PartyId);
            modelBuilder.Entity<Organisation>().Property(x => x.OrganisationName);
            modelBuilder.Entity<Organisation>().Property(x => x.TradingName);

            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Person>().Property(x => x.PersonId);
            modelBuilder.Entity<Person>().Property(x => x.PartyId);
            modelBuilder.Entity<Person>().Property(x => x.FirstName);
            modelBuilder.Entity<Person>().Property(x => x.Surname);
            modelBuilder.Entity<Person>().Property(x => x.DateOfBirth);
            modelBuilder.Entity<Person>().Property(x => x.EmailAddress);
        }
    }
}
