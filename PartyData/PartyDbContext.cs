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
        }
    }
}
